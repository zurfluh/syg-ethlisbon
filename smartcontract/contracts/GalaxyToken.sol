pragma solidity >=0.6.0 <=0.8.4;
pragma experimental ABIEncoderV2;

import "@openzeppelin/contracts/token/ERC1155/ERC1155.sol";
import "@openzeppelin/contracts/utils/math/SafeMath.sol";
import "@openzeppelin/contracts/access/Ownable.sol";
import "@openzeppelin/contracts/token/ERC1155/IERC1155Receiver.sol";

contract GalaxyToken is ERC1155, Ownable{
    using SafeMath for uint256;

    // token nonce
    uint256 internal nonce;

    // The top bit is a flag to tell if this is a NFI.
    uint256 internal constant TYPE_NF_BIT = 1 << 255;

    /// Store the type in the upper 128 bits..
    uint256 internal constant TOKEN_TYPE = uint256(type(uint128).max) << 128;

    /// ..and the non-fungible index in the lower 128
    uint256 internal constant NF_INDEX = type(uint128).max;

    // mapping of nft to owner
    mapping(uint256 => address) private _nfOwners;

    // mapping for operator role
    mapping(uint256 => mapping(address => bool)) public operators;

    mapping(address => mapping(uint256 => uint256)) public holderAmount;

    mapping(address => mapping(uint256 => uint256)) public claimableAmount;

    mapping(uint256 => bool) internal unlockedId;

    mapping(uint256 => string) public tokenMetadata;

    mapping(uint256 => uint256) public totalSupply;

    // selectors for receiver callbacks
    bytes4 constant public ERC1155_RECEIVED       = 0xf23a6e61;
    bytes4 constant public ERC1155_BATCH_RECEIVED = 0xbc197c81;

    constructor() ERC1155("GalaxyToken") { }

    /***********************************|
  |             EVENTS                |
  |__________________________________*/

    event TokenCreation(address indexed, uint256);
    event NfTokenMint(address indexed, uint256 indexed);
    event OwnerCredited(uint256 indexed, address indexed , uint256);

    /***********************************|
  |             Modifiers             |
  |__________________________________*/

    modifier isAlreadyOwned(uint256 id) {
        require(_nfOwners[id] != address(0), "NFT is already owned by others");
        _;
    }

    modifier isUnlocked(uint256 _type) {
        require(
            unlockedId[_type] != true,
            "Company did not create this token yet"
        );
        _;
    }


    modifier isOwnerOrOperator(uint256 id) {
        uint256 _type = getNonFungibleTokenType(id);
        require(
            owner() == msg.sender || operators[_type][msg.sender] == true || address(this) == msg.sender,
            "You cannot perform this action."
        );
        _;
    }

    /**
     * @dev See {IERC1155-isApprovedForAll}.
     */
    function isApprovedForAll(address account, address operator) public view override returns (bool) {
        return ERC1155.isApprovedForAll(account, operator) || msg.sender == owner();
    }

    /// @dev returns true if address is contract.
    function isContract(address _addr) private view returns (bool) {
        uint32 size;
        assembly {
            size := extcodesize(_addr)
        }
        return (size > 0);
    }

    /// @dev Returns true if token is non-fungible
    function isNonFungible(uint256 id) public pure returns (bool) {
        return id & TYPE_NF_BIT == TYPE_NF_BIT;
    }

    /// @dev Returns true if token is fungible
    function isFungible(uint256 id) public pure returns (bool) {
        return id & TYPE_NF_BIT == 0;
    }

    function getNfOwner(uint256 id) public view returns (address) {
        return _nfOwners[id];
    }

    /// Returns index of non-fungible token
    function getNonFungibleIndex(uint256 id) public pure returns (uint256) {
        return id & NF_INDEX;
    }

    /// Returns base type of non-fungible token
    function getNonFungibleTokenType(uint256 id) public pure returns (uint256) {
        return id & TOKEN_TYPE;
    }

    function addOperator(address account, uint256 _type)
        external
        onlyOwner
        returns (bool)
    {
        operators[_type][account] = true;
        return operators[_type][account];
    }

    function removeOperator(address account, uint256 _type)
        external
        onlyOwner
        returns (bool)
    {
        operators[_type][account] = false;
        return !operators[_type][account];
    }

    function transferNfOwner(uint256 id, address to)
        private
        isOwnerOrOperator(id)
    {
        _nfOwners[id] = to;
    }

    function createTokenType(bool isNF)
        external
        onlyOwner
        returns (uint256 _type)
    {
        // Store the type in the upper 128 bits
        _type = (++nonce << 128);

        // Set a flag if this is an NFT.
        if (isNF) _type = _type | TYPE_NF_BIT;
    }

    function nonFungibleMint(
        address account,
        uint256 _type,
        string memory tokenURI
    ) external returns(uint256 id) {
        require(
            isNonFungible(_type),
            "TRIED_TO_MINT_FUNGIBLE_FOR_NON_FUNGIBLE_TOKEN"
        );

        uint256 index = ++totalSupply[_type];

        id = _type | index;

        transferNfOwner(id, account);

        tokenMetadata[id] = tokenURI;

        emit NfTokenMint(account, id);
    }
    
    function onERC1155Received(
        address operator,
        address from,
        uint256 id,
        uint256 value,
        bytes calldata data
    ) external returns (bytes4){
        return ERC1155_RECEIVED;
    }

    function onERC1155BatchReceived(
        address operator,
        address from,
        uint256[] calldata ids,
        uint256[] calldata values,
        bytes calldata data
    ) external returns (bytes4){
        return ERC1155_BATCH_RECEIVED;
    }

    function fungibleMint(
        address account,
        uint256 _type,
        uint256 amount,
        bytes memory data
    ) public isOwnerOrOperator(_type) returns (bool) {
        require(
            isFungible(_type),
            "TRIED_TO_MINT_FUNGIBLE_FOR_NON_FUNGIBLE_TOKEN"
        );
        super._mint(account, _type, amount, data);
        totalSupply[_type] += amount;
        holderAmount[account][_type] += amount;
        return true;
    }

    function fungibleBurn(
        address account,
        uint256 _type,
        uint256 amount
    ) public isOwnerOrOperator(_type) returns (bool) {
        require(
            isFungible(_type),
            "TRIED_TO_BURN_FUNGIBLE_FOR_NON_FUNGIBLE_TOKEN"
        );
        super._burn(account, _type, amount);
        totalSupply[_type] -= amount;
        holderAmount[account][_type] -= amount;
        return true;
    }

    function safeTransferFrom(
        address from,
        address to,
        uint256 id,
        uint256 amount,
        bytes memory data
    ) public virtual override(ERC1155) {
        require(
            from == _msgSender() || isApprovedForAll(from, _msgSender()),
            "ERC1155: caller is not owner nor approved"
        );

        if (isFungible(id)) {
            require(holderAmount[from][id] >= amount, "insufficient funds");
            super._safeTransferFrom(from, to, id, amount, data);
            holderAmount[from][id] -= amount;
            holderAmount[to][id] += amount;
            return;
        } else {
            require(getNfOwner(id) == from, "Wrong NFT owner");
            transferNfOwner(id, to);
        }
    }

    function provideDividend(uint256 _type, address account, uint256 amount) public onlyOwner {
        require(
            isFungible(_type),
            "TRIED_TO_STAKE_FUNGIBLE_FOR_NON_FUNGIBLE_TOKEN"
        );
        
        claimableAmount[account][_type] += amount;

        emit OwnerCredited(_type, account, amount);
    }

    function dividendClaim(uint256 _type, address account) public returns (uint256 _amount)  {
        require(
            isFungible(_type),
            "TRIED_TO_BURN_FUNGIBLE_FOR_NON_FUNGIBLE_TOKEN"
        );
        _amount = claimableAmount[account][_type];
        fungibleMint(account,_type,_amount,"");
        claimableAmount[account][_type] = 0;
    }   
}
