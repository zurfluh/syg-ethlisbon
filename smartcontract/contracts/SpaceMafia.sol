pragma solidity ^0.8.0;
pragma experimental ABIEncoderV2;

import "@openzeppelin/contracts/utils/math/SafeMath.sol";
import "@openzeppelin/contracts/access/Ownable.sol";

import "./GalaxyToken.sol";

contract SpaceMafia is Ownable {

    using SafeMath for uint256;

    uint256 constant APR_TIME_PERIOD = 1 seconds; 
    uint256 constant ROCKET_COST = 1 ether; 
    uint256 constant MINIMUM_MISSION_COST = 1 ether;
    uint256 constant BLOCK_TO_FINALIZATION = 20;

    // ERC1155 Token interface
    GalaxyToken public galaxyToken;

    // Planet type id
    uint256 public planetType;
    // Rocket type id
    uint256 public rocketType;
    // Mafia ERC20 Token
    uint256 public mafiaToken;

    // Count the number of attacks
    uint256 public nukeCount;

    mapping(uint256 => uint256) stakedEth; // This also represents the APR
    mapping(uint256 => uint256) lastStakedTime;


    struct Nuke { 
        uint256 totalStake;
        uint256 rocketId;
        uint256 targetPlanet;
        uint256 finalityBlock;
        bool complete;
    }
    // Staked value for each attack
    mapping(uint256 => Nuke) nukes;

    constructor(address _galaxyToken)  {
        galaxyToken = GalaxyToken(_galaxyToken);
        mafiaToken = galaxyToken.createTokenType(false);
        planetType = galaxyToken.createTokenType(true);
        rocketType = galaxyToken.createTokenType(true);
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
        address _owner = galaxyToken.getNfOwner(_tokenId);
        // Update dividends
        galaxyToken.provideDividend(mafiaToken, _owner, _getPendingClaimableAmount(_tokenId));
        stakedEth[_tokenId] = stakedEth[_tokenId].add(msg.value);
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
        // uint256 _power = keccak256(block.blockhash(block.number - 1),  msg.sig) % 10; //Request from chainlink
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
        require(galaxyToken.balanceOf(msg.sender, mafiaToken) >= _missionCost, "Sender does not have enough balance");
        nukes[nukeCount] =  Nuke(_missionCost, _rocketId, _planetId, block.number + BLOCK_TO_FINALIZATION, false);
        galaxyToken.safeTransferFrom(msg.sender, address(this), mafiaToken, _missionCost, "");
        return nukeCount++;
    }

    function defenseProbability(uint256 _nukeId) public view returns (uint256) {
        require(_nukeId<nukeCount, "Invalid nukeId");
        Nuke storage _n = nukes[_nukeId];
        require(!_n.complete, "Attack has already been resolved");
        uint256 _denominator = stakedEth[_n.targetPlanet].add(_n.totalStake).add(1 ether);
        return stakedEth[_n.targetPlanet].add(1 ether).mul(2**128).div(_denominator);
    }

    function completeAttack(uint256 _nukeId) public returns (bool) {
        uint256 _threshold = defenseProbability(_nukeId);
        Nuke storage _n = nukes[_nukeId];
        uint256 _finality = _n.finalityBlock;
        require(block.number > _finality, "Attack not finalizable yet");
        uint256 _random = uint256(keccak256(abi.encode(blockhash(_finality)))).div(2**128);
        // DEFENDER WINS
        if (_random <= _threshold) {
            address _recipient = galaxyToken.getNfOwner(_n.targetPlanet);
            galaxyToken.safeTransferFrom(address(this), _recipient, mafiaToken, _n.totalStake, "");
            // TODO: Need nonFungibleBurn for the rocket
            _n.complete=true;
            return false;
        }
        // ATTACKER WINS
        address _defender = galaxyToken.getNfOwner(_n.targetPlanet);
        address _attacker = galaxyToken.getNfOwner(_n.rocketId);
        galaxyToken.safeTransferFrom(address(this), _attacker, mafiaToken, _n.totalStake, "");
        galaxyToken.safeTransferFrom(_defender, _attacker, _n.targetPlanet, 0, "");
        _n.complete=true;
        return true;
    }

}