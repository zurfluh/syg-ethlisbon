pragma solidity ^0.8.0;
pragma experimental ABIEncoderV2;

import "./open-zeppelin/utils/math/SafeMath.sol";
import "./open-zeppelin/access/Ownable.sol";

import "./GalaxyToken.sol";

contract SpaceMafia is Ownable {

    using SafeMath for uint256;

    // ERC1155 Token interface
    GalaxyToken public galaxyToken;

    // Planet type id
    uint256 public planetType;
    // Spaceship type id
    uint256 public spaceshipType;
    // Mafia ERC20 Token
    uint256 public mafiaToken;

    constructor(address _galaxyToken)  {
        galaxyToken = GalaxyToken(_galaxyToken);
        mafiaToken = galaxyToken.createTokenType(false);
        planetType = galaxyToken.createTokenType(true);
        spaceshipType = galaxyToken.createTokenType(true);
    }
}