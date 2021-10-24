pragma solidity 0.8.4;
pragma experimental ABIEncoderV2;

import "@openzeppelin/contracts/utils/math/SafeMath.sol";
import "@openzeppelin/contracts/access/Ownable.sol";

import "./GalaxyToken.sol";
import "./aave-helper/ILendingPoolAddressesProvider.sol";
import "./aave-helper/ILendingPool.sol";

contract SpaceMafia is Ownable {

    using SafeMath for uint256;

    uint256 constant APR_TIME_PERIOD = 1 seconds; 
    uint256 constant ROCKET_COST = 1 ether; 
    uint256 constant MINIMUM_MISSION_COST = 1 ether;
    uint256 constant BLOCK_TO_FINALIZATION = 20;

    bool initialized = false;

    // ERC1155 Token interface
    GalaxyToken public galaxyToken;

    // Planet type id
    uint256 public planetType;
    // Rocket type id
    uint256 public rocketType;
    // Mafia ERC20 Token
    uint256 public mafiaToken;

    address public lendingpool;


    // Count the number of attacks
    uint256 public nukeCount;

    mapping(uint256 => uint256) public stakedEth; // This also represents the APR
    mapping(uint256 => uint256) public lastStakedTime;


    // selectors for receiver callbacks
    bytes4 constant public ERC1155_RECEIVED       = 0xf23a6e61;
    bytes4 constant public ERC1155_BATCH_RECEIVED = 0xbc197c81;


    struct Nuke { 
        uint256 totalStake;
        uint256 rocketId;
        uint256 targetPlanet;
        uint256 finalityBlock;
        uint256 missionCost;
        bool complete;
    }
    // Staked value for each attack
    mapping(uint256 => Nuke) public nukes;

    constructor()  {
    }

    function initialize(address _galaxyToken) public onlyOwner {
        require(initialized==false, "Already initialized");
        galaxyToken = GalaxyToken(_galaxyToken);
        mafiaToken = galaxyToken.createTokenType(false);
        planetType = galaxyToken.createTokenType(true);
        rocketType = galaxyToken.createTokenType(true);
        initialized = true;
        lendingpool = ILendingPoolAddressesProvider(address(0x24a42fD28C976A61Df5D00D0599C34c4f90748c8)).getLendingPool();
    }

    function getPlanet(uint256 _id) public view returns (address, string memory){
        return (galaxyToken.getNfOwner(_id), galaxyToken.tokenMetadata(_id));
    }

    function getRocket(uint256 _id) public view returns (address, string memory){
        return (galaxyToken.getNfOwner(_id), galaxyToken.tokenMetadata(_id));
    }

    function getMafiaBalanceOf(address _account) public view returns (uint256){
        return galaxyToken.balanceOf(_account, mafiaToken);
    }

    function getPlanetsOf(address _account) public view returns (uint256[] memory _out){
        uint256 _planetSupply = galaxyToken.totalSupply(planetType);
        _out = new uint256[](_planetSupply);
        for (uint256 j = 0; j < _planetSupply; j++) { 
            uint256 _planetId = planetType.add(j+1);
            if (galaxyToken.getNfOwner(_planetId) == _account) {
                _out[j]=_planetId;
            }
        }
        return _out;
    }
    
    function getPlanetSupply() public view returns (uint256){
        return galaxyToken.totalSupply(planetType);
    }

    function getRocketSupply() public view returns (uint256){
        return galaxyToken.totalSupply(rocketType);
    }

    function getMafiaTokenSupply() public view returns (uint256){
        return galaxyToken.totalSupply(mafiaToken);
    }

    function transferPlanet(address _to, uint256 _id) public {
        return galaxyToken.safeTransferFrom(msg.sender, _to, _id, 1, "");
    }

    function transferRocket(address _to, uint256 _id) public {
        return galaxyToken.safeTransferFrom(msg.sender, _to, _id, 1, "");
    }
    
    function transferMafiaToken(address _to, uint256 _amount) public {
        return galaxyToken.safeTransferFrom(msg.sender, _to, mafiaToken, _amount, "");
    }

    function depositInAAVE(uint256 amount) public payable{
        ILendingPool(lendingpool).deposit(address(0xEeeeeEeeeEeEeeEeEeEeeEEEeeeeEeeeeeeeEEeE), amount, address(this), 0);
    }

    modifier planetExists(uint256 _tokenId) {
        require(galaxyToken.getNfOwner(_tokenId) != address(0), "Planet does not exist");
        require(galaxyToken.getNonFungibleTokenType(_tokenId) == planetType, "Planet is not a planet");
        _;
    }

    modifier rocketExists(uint256 _tokenId) {
        require(galaxyToken.getNfOwner(_tokenId) != address(0), "Rocket does not exist");
        require(galaxyToken.getNonFungibleTokenType(_tokenId) == rocketType, "Rocket is not a rocket");
        _;
    }

    modifier isNukeValid(uint256 _nukeId) {
        require(_nukeId<nukeCount, "Invalid nukeId");
        require(!nukes[_nukeId].complete, "Attack has already been resolved");
        _;
    }

    function getRandom(bytes32 _entropy) public pure returns (uint256) {
        return uint256(keccak256(abi.encode(_entropy))).div(2**128);
    }

    function getThreshold(uint256 _numerator, uint256 _denominator) public pure returns (uint256) {
        return _numerator.mul(2**128).div(_denominator);
    }

    function checkRandom(uint256 _random, uint256 _threshold) public pure returns (bool) {
        return _random <= _threshold;
    }

    function _getPendingClaimableAmount(uint256 _tokenId) internal view returns(uint256 _amount) {
        if (lastStakedTime[_tokenId] != 0) {
            uint256 _delta = block.timestamp - lastStakedTime[_tokenId];
            _amount = stakedEth[_tokenId].mul(_delta).div(APR_TIME_PERIOD);
        }
        return _amount;
    }

    function mintPlanet(
        address _account,
        string memory _tokenURI
    ) public onlyOwner returns(uint256 _id){
        _id = galaxyToken.nonFungibleMint(_account, planetType , _tokenURI);
    }

    function stakeEthOnPlanet(
        uint256 _tokenId
    ) public payable planetExists(_tokenId) returns(bool){
        require(msg.value > 0, "No staking amount is mentioned");
        address _owner = galaxyToken.getNfOwner(_tokenId);
        // Update dividends
        galaxyToken.provideDividend(mafiaToken, _owner, _getPendingClaimableAmount(_tokenId));
        stakedEth[_tokenId] = stakedEth[_tokenId].add(msg.value);
        depositInAAVE(msg.value);
        lastStakedTime[_tokenId] = block.timestamp;
        return true;
    }

    function claimableDividends(
        uint256 _tokenId
    ) public view planetExists(_tokenId) returns(uint256){
        address _owner = galaxyToken.getNfOwner(_tokenId);
        uint256 _settledAmount = galaxyToken.claimableAmount(_owner, mafiaToken);
        uint256 _pendingAmount = _getPendingClaimableAmount(_tokenId);
        return _settledAmount.add(_pendingAmount);
    }

    function claimDividends(
        uint256 _tokenId
    ) public planetExists(_tokenId) returns(uint256) {
        address _owner = galaxyToken.getNfOwner(_tokenId);
        galaxyToken.provideDividend(mafiaToken, _owner, _getPendingClaimableAmount(_tokenId));
        lastStakedTime[_tokenId] = block.timestamp;
        return galaxyToken.dividendClaim(mafiaToken, _owner);
    }

    function mintRocket(
        uint256 _planetId
    ) public returns(uint256 _id){
        address _owner = galaxyToken.getNfOwner(_planetId);
        require(galaxyToken.balanceOf(msg.sender, mafiaToken) >= ROCKET_COST, "Sender does not have enough balance");
        galaxyToken.fungibleBurn(msg.sender, mafiaToken, ROCKET_COST);
        _id = galaxyToken.nonFungibleMint(_owner, rocketType , string(abi.encodePacked("{planet:", _planetId, "}")));
    }

    function nuke(
        uint256 _planetId,
        uint256 _rocketId,
        uint256 _missionCost
    ) public planetExists(_planetId) rocketExists(_rocketId) returns (uint256) {
        address _attacker = galaxyToken.getNfOwner(_rocketId);
        require(_attacker == msg.sender, "Attacker is not sender");
        require(_missionCost >= MINIMUM_MISSION_COST, "Mission cost is too low");
        require(galaxyToken.balanceOf(msg.sender, mafiaToken) >= _missionCost, "Sender does not have enough MAFIA balance");
        nukes[nukeCount] =  Nuke(_missionCost, _rocketId, _planetId, block.number + BLOCK_TO_FINALIZATION, _missionCost, false);
        galaxyToken.safeTransferFrom(msg.sender, address(this), mafiaToken, _missionCost, "");
        return nukeCount++;
    }

    function defenseProbability(uint256 _nukeId) public isNukeValid(_nukeId) view returns (uint256) {
        require(_nukeId<nukeCount, "Invalid nukeId");
        Nuke storage _n = nukes[_nukeId];
        require(!_n.complete, "Attack has already been resolved");
        uint256 _numerator = stakedEth[_n.targetPlanet].add(1 ether);
        uint256 _denominator = stakedEth[_n.targetPlanet].add(_n.totalStake).add(1 ether);
        return getThreshold(_numerator, _denominator);
    }

    function onERC1155Received(
        address operator,
        address from,
        uint256 id,
        uint256 value,
        bytes calldata data
    ) pure external returns (bytes4){
        abi.encode(operator,from,id,value,data);
        return ERC1155_RECEIVED;
    }

    function onERC1155BatchReceived(
        address operator,
        address from,
        uint256[] calldata ids,
        uint256[] calldata values,
        bytes calldata data
    ) pure external returns (bytes4){
        abi.encode(operator,from,ids,values,data);
        return ERC1155_BATCH_RECEIVED;
    }

    function completeAttack(uint256 _nukeId) public isNukeValid(_nukeId) returns (bool) {
        uint256 _threshold = defenseProbability(_nukeId);
        Nuke storage _n = nukes[_nukeId];
        uint256 _finality = _n.finalityBlock;
        require(block.number >= _finality, "Attack not finalizable yet");
        uint256 _random = getRandom(blockhash(_finality));
        _n.complete=true;
        address _defender = galaxyToken.getNfOwner(_n.targetPlanet);
        address _attacker = galaxyToken.getNfOwner(_n.rocketId);
        // Rocket goes bust
        galaxyToken.safeTransferFrom(_attacker, address(0), _n.rocketId, 0, "");
        // DEFENDER WINS
        if (checkRandom(_random, _threshold)) {
            // Defender gets mission MAFIA
            galaxyToken.safeTransferFrom(address(this), _defender, mafiaToken, _n.totalStake, "");
            return false;
        }
        // ATTACKER WINS
        // Mission MAFIA goes back to attacker
        galaxyToken.safeTransferFrom(address(this), _attacker, mafiaToken, _n.totalStake, "");
        // Planet goes to attacker
        galaxyToken.safeTransferFrom(_defender, _attacker, _n.targetPlanet, 0, "");
        return true;
    }

    function hijack(
        uint256 _nukeId, 
        uint256 _hijackCost
    ) public isNukeValid(_nukeId) returns (bool) {
        address _hijacker = msg.sender;
        Nuke storage _n = nukes[_nukeId];
        address _attacker = galaxyToken.getNfOwner(_n.rocketId);
        require(_hijackCost >= MINIMUM_MISSION_COST, "Mission cost is too low");
        require(galaxyToken.balanceOf(_hijacker, mafiaToken) >= _hijackCost, "Sender does not have enough MAFIA balance");
        uint256 _finality = _n.finalityBlock;
        require(block.number < _finality, "Attack is already complete");
        uint256 _numerator = _hijackCost;
        uint256 _denominator = _hijackCost.add(_n.totalStake);
        uint256 _threshold = getThreshold(_numerator, _denominator);
        uint256 _random = getRandom(blockhash(block.number - 1)); //This is not safe, but hey.. that's what we got after 24h of no sleep... 
        // Add MAFIA to totalStake
        galaxyToken.safeTransferFrom(_hijacker, address(this), mafiaToken, _hijackCost, "");
        _n.totalStake = _n.totalStake + _hijackCost;
        // HIJACKER WINS
        if (checkRandom(_random, _threshold)) {
            // Rocket goes bust
            galaxyToken.safeTransferFrom(_attacker, address(0), _n.rocketId, 0, "");
            // Complete the attack
            _n.complete=true;
            // Hijacker wins stake
            galaxyToken.safeTransferFrom(address(this), _hijacker, mafiaToken, _n.totalStake, "");
            return true;
        }
        // NOTHING HAPPENS
        // Reset clock
        _n.finalityBlock = block.number + BLOCK_TO_FINALIZATION;
        return false;
    }

}