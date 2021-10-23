pragma solidity ^0.8.0;
pragma experimental ABIEncoderV2;

import "@openzeppelin/contracts/utils/math/SafeMath.sol";
import "@openzeppelin/contracts/access/Ownable.sol";

import "./GalaxyToken.sol";

contract SpaceMafia is Ownable {

    using SafeMath for uint256;

    uint256 constant APR_TIME_PERIOD = 1 weeks; 

    // ERC1155 Token interface
    GalaxyToken public galaxyToken;

    // Planet type id
    uint256 public planetType;
    // Spaceship type id
    uint256 public spaceshipType;
    // Mafia ERC20 Token
    uint256 public mafiaToken;

    mapping(uint256 => uint256) stakedEth; // This also represents the APR
    mapping(uint256 => uint) lastStakedTime;

    constructor(address _galaxyToken)  {
        galaxyToken = GalaxyToken(_galaxyToken);
        mafiaToken = galaxyToken.createTokenType(false);
        planetType = galaxyToken.createTokenType(true);
        spaceshipType = galaxyToken.createTokenType(true);
    }

    modifier planetExist(uint256 _tokenId) {
        require(galaxyToken.getNfOwner(_tokenId) != address(0), "Planet does not exist");
        require(galaxyToken.getNonFungibleTokenType(_tokenId) == planetType, "Planet is not a planet");
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
    ) public payable planetExist(_tokenId) returns(bool){
        address _owner = galaxyToken.getNfOwner(_tokenId);
        // Update dividends
        galaxyToken.provideDividend(mafiaToken, _owner, _getPendingClaimableAmount(_tokenId));
        stakedEth[_tokenId] = stakedEth[_tokenId].add(msg.value);
        lastStakedTime[_tokenId] = block.timestamp;
        return true;
    }

    function claimableDividends(
        uint256 _tokenId
    ) public view planetExist(_tokenId) returns(uint256){
        address _owner = galaxyToken.getNfOwner(_tokenId);
        uint256 _settledAmount = galaxyToken.claimableAmount(_owner, mafiaToken);
        uint256 _pendingAmount = _getPendingClaimableAmount(_tokenId);
        return _settledAmount.add(_pendingAmount);
    }

    function claimDividends(
        uint256 _tokenId
    ) public planetExist(_tokenId) returns(uint256) {
        address _owner = galaxyToken.getNfOwner(_tokenId);
        galaxyToken.provideDividend(mafiaToken, _owner, _getPendingClaimableAmount(_tokenId));
        lastStakedTime[_tokenId] = block.timestamp;
        galaxyToken.dividendClaim(mafiaToken, _owner);
    }


}