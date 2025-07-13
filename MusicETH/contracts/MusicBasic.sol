pragma solidity >=0.4.21 <0.7.0;
pragma experimental ABIEncoderV2;
import "./MusicAsset.sol";
import "./MusicAssetTransfer.sol";

contract MusicBasic {

    /** @dev stores medicine batch addresses || chain point addresses || medicine batch transfer by guid */
    mapping(bytes32 => address) private  contractAddresses;
    
    mapping(bytes32 => mapping(string => MusicAsset)) private  musicAssets;

    /** @dev stores which addresses are owned by Global administrators */
    mapping (address => bool) private  admins;
    
    /** @dev stores medicine batch transfers by particular medicine batch like using 3-dimensionality array [getKey(medicineBatchId)][layerIndex][transferIndex] */
     mapping(bytes32 => MusicAssetTransfer) private  newMussicAssetTransfers;
    

    /** @dev stores the address of the Global Administrator */
    address public globalAdmin;
    
    MusicAsset newMusicAsset;
    MusicAssetTransfer newMussicAssetTransfer;

    constructor() public {
        globalAdmin = msg.sender;
        admins[msg.sender] = true;
        newMusicAsset = new MusicAsset();
        newMussicAssetTransfer = new MusicAssetTransfer();
    }

     // ================Music Functions================
    function addMusicAsset(
        string memory _guid,
        string memory _name,
        string memory _album,
        string memory _publishingYear,
        string memory _key2,
        uint _ownerId,
        string memory _licenceLink,
        string memory _musicLink,
        uint8 _creatureType,
        uint8 _ownerType)
        public
        onlyAdmin
    {
        bytes32 key = getEncodeKey(_guid);
        
        string[6] memory stringInfo;
        stringInfo[0] = _guid;
        stringInfo[1] = _name;
        stringInfo[2] = _album;
        stringInfo[3] = _publishingYear;
        stringInfo[4] = _key2;

        string[2] memory stringLink;
        stringLink[0] = _licenceLink;
        stringLink[1] = _musicLink;
        
        uint[4] memory intId;
        intId[0] = _ownerId;
        intId[1] = _creatureType;
        intId[2] = _ownerType;

        newMusicAsset.addMusicInformation(
            stringInfo,
            stringLink,
            intId,
            msg.sender);

        contractAddresses[key] = address(newMusicAsset);
        
        musicAssets[getEncodeKeyWithUint(_ownerId)][_guid] = newMusicAsset;

    }

    function removeMusicAsset(string memory _guid) public onlyAdmin {
        bytes32 key = getEncodeKey(_guid);

        delete contractAddresses[key];
    }
    
    function getMusicAsset(uint _ownerId, string memory _guid)
        public
        view
        returns (string memory, string memory, string memory, uint, string memory)
    {
        MusicAsset result = musicAssets[getEncodeKeyWithUint(_ownerId)][_guid];
        return (result.name(),
            result.album(),
            result.publishingYear(),
            result.ownerId(),
            result.key2());
    }
    // ================End Music Functions================
    
    
    // ================Music Transaction Functions================
    function addTransactionMusic(
        string memory _guid,
        string memory _musicAssetId,
        string memory _fromOwnerId,
        string memory _toFanId,
        string memory _key2,
        uint8 _tranType,
        uint8 _fanType,
        uint _dateStart,
        uint _dateEnd,
        bool isPermanent,
        bool isConfirmed)
        public
    {
        
        bytes32 key = getEncodeKey(_guid);

        
        string[5] memory transactInfo;
        transactInfo[0] = _guid;
        transactInfo[1] = _musicAssetId;
        transactInfo[2] = _fromOwnerId;
        transactInfo[3] = _toFanId;
        transactInfo[4] = _key2;

        uint[4] memory transacInfoInt;
        transacInfoInt[0] = _tranType; //_tranType
        transacInfoInt[1] = _fanType;
        transacInfoInt[2] = _dateStart;
        transacInfoInt[3] = _dateEnd;
        
        bool[2] memory transactInfoCheck;
        transactInfoCheck[0] = isPermanent;
        transactInfoCheck[1] = isConfirmed; //isConfirmed;

        newMussicAssetTransfer.addMusicAssetTransfer(
            transactInfo,
            transacInfoInt,
            transactInfoCheck,
            msg.sender);

        contractAddresses[key] = address(newMussicAssetTransfer);
        
        newMussicAssetTransfers[key] = newMussicAssetTransfer;
    }
    
    function getMusicAssetTransfer(
        string memory _guid)
        public
        view
        returns (string memory, string memory, string memory, string memory, string memory)
    {
        bytes32 key = getEncodeKey(_guid);
        MusicAssetTransfer result = newMussicAssetTransfers[key];
        return (result.guid(),
            result.musicAssetId(),
            result.fromOwnerId(),
            result.toFanId(),
            result.key2());
    }

    function removeMusicAssetTransfer(string memory _guid) public onlyAdmin {
        bytes32 key = getEncodeKey(_guid);

        delete contractAddresses[key];
    }
    // ================End Music Transaction Functions================
     
     
    
    // ================Administrator Functions================
    function addAdmin(address _address) public onlyGlobalAdmin {
        admins[_address] = true;
    }

    function removeAdmin(address _address) public onlyGlobalAdmin {
        delete admins[_address];
    }

    /**
      * Get the address of a internal contract.
      *
      * @param _guid Unique identifier.
      * @return Contract address.
      */
    function getAddressByID(string memory _guid) public view returns (address) {
        return contractAddresses[getEncodeKey(_guid)];
    }

    /**
      * Derives an unique key from a identifier to be used in the global mapping contractAddresses.
      *
      * This is necessary due to tx identifiers are of type string
      * which cannot be used as dictionary keys.
      *
      * @param _guid The unique certificate identifier.
      * @return The key derived from certificate identifier.
      */
    function getEncodeKey(string memory _guid) public pure returns (bytes32) {
        return sha256(abi.encodePacked(_guid));
    }
    
    function getEncodeKeyWithUint(uint _guid) public pure returns (bytes32) {
        return sha256(abi.encodePacked(_guid));
    }

    // ================Modifiers================
    modifier onlyGlobalAdmin() {
        require(msg.sender == globalAdmin);
        _;
    }

    modifier onlyAdmin() {
        require(admins[msg.sender]);
        _;
    }
}