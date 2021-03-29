package com.senseyun.openapi.SSRuntimeEasyJava;

public class ErrorCode {
    //============================================================
    //                  Module
    //============================================================
    public final static long MODE_H5_RUNTIME                          = 0x01;    //  H5 Module
    public final static long MODE_IPC                                 = 0x02;    //  IPC Module
    public final static long MODE_SYSTEM                              = 0x04;    //  System Module
    public final static long MODE_SS                                  = 0x05;    //  Virbox License Service Module
    public final static long MODE_NETAGENT                            = 0x11;    //  NetAgent Module
    public final static long MODE_SSPROTECT                           = 0x12;    //  SSPROTECT Module
    public final static long MODE_LM_API                              = 0x13;    //  LM Module(Runtime, D2C, Control)
    public final static long MODE_LM_FIRM                             = 0x22;    //  LM Firmware Module
    public final static long MODE_LM_SES                              = 0x23;    //  LM SES Module
    public final static long MODE_LM_SERVICE                          = 0x24;    //  LM SERVICE Module
    public final static long MODE_LIC_TRANS                           = 0x28;    //  License Transform Module
    public final static long MODE_AUTH_SERVER                         = 0x29;    //  Auth Server Module
    public final static long MODE_CLOUD                               = 0x30;    //  Cloud Module
    public final static long MODE_SO                                  = 0x51;    //  Slock Module
    public final static long MODE_UM                                  = 0x60;    //  User Manager Module

    //============================================================
    //              General error
    //============================================================
    public final static long SS_OK                                     = 0x00000000;  //  Success
    public final static long SS_ERROR                                  = 0x00000001;  //  Generic error
    public final static long SS_ERROR_INVALID_PARAM                    = 0x00000002;  //  Illegal parameter
    public final static long SS_ERROR_MEMORY_FAIELD                    = 0x00000003;  //  Memory error
    public final static long SS_ERROR_INSUFFICIENT_BUFFER              = 0x00000004;  //  Buffer is insufficient
    public final static long SS_ERROR_NOT_FOUND                        = 0x00000005;  //  Can not find the object
    public final static long SS_ERROR_EXISTED                          = 0x00000006;  //  Object already exists
    public final static long SS_ERROR_DATA_BROKEN                      = 0x00000007;  //  Bad data
    public final static long SS_ERROR_INVALID_HANDLE                   = 0x00000008;  //  Invalid handle
    public final static long SS_ERROR_TIMEOUT                          = 0x00000009;  //  Operation time out
    public final static long SS_ERROR_TARGET_NOT_IN_USE                = 0x0000000A;  //  Object is not using
    public final static long SS_ERROR_DATA_CONFLICT                    = 0X0000000B;  //  Incompatible data exist at the same time
    public final static long SS_ERROR_INVALID_TYPE                     = 0x0000000C;  //  Invalid type
    public final static long SS_ERROR_INVALID_LENGTH                   = 0x0000000D;  //  Invalid length
    public final static long SS_ERROR_USER_MOD_CRASH                   = 0x0000000E;  //  User module conflict
    public final static long SS_ERROR_SERVER_IS_LOCAL                  = 0x0000000F;  //  The License Service is local
    public final static long SS_ERROR_UNSUPPORT                        = 0x00000010;  //  Unsupported operation
    public final static long SS_ERROR_PORT_IN_USE                      = 0x00000011;  //  Port is occupied
    public final static long SS_ERROR_NO_KEY                           = 0x00000013;  //  No secret key
    public final static long SS_ERROR_SERVICE_TYPE_NOT_SUPPORT         = 0x00000014;  //  Service type is surpporting the operation
    public final static long SS_ERROR_MULTICAST_ADDR_IN_USE            = 0x00000015;  //  Multicast address is occupied
    public final static long SS_ERROR_MULTICAST_PORT_IN_USE            = 0x00000016;  //  Multicast port is occupied
    public final static long SS_ERROR_MOD_FAIL_LIBSTRING               = 0x00000020;  //  Libstring error
    public final static long SS_ERROR_NET_ERROR                        = 0x00000040;  //  Failed to connect to server
    public final static long SS_ERROR_IPC_ERROR                        = 0x00000041;  //  IPC error
    public final static long SS_ERROR_INVALID_SESSION                  = 0x00000042;  //  Conversation failed
    public final static long SS_ERROR_GTR_MAX_SERVER_COUNT             = 0x00000043;  //  The number of support services has reached an upper limit
    public final static long SS_ERROR_MASTER_UNSUPPORT_PIN             = 0x00000044;  //  The master lock doesn't support PIN. Please apply for relapement the master lock
    public final static long SS_ERROR_MASTER_PIN_NOT_ACTIVE            = 0x00000045;  //  The PIN is not activated. Please modify the initial PIN
    public final static long SS_ERROR_MASTER_NO_SUCH_PIN               = 0x00000046;  //  The PIN doesn't exist. Please check the whether PIN index is correct
    public final static long SS_ERROR_MASTER_OUTDATED_VERSION          = 0x00000047;  //  The master lock version is low and the new lock must be replaced
    public final static long SS_ERROR_MASTER_PIN_WRONG                 = 0x00000048;  //  Error PIN
    public final static long SS_ERROR_MASTER_PIN_BLOCKED               = 0x00000049;  //  PIN is locked

    //============================================================
    //          LM Module(0x13): (runtime, control, develop)
    //============================================================
    public final static long SS_ERROR_D2C_NO_PACKAGE                   = 0x13000000;  //  There is no signed content in D2C package
    public final static long SS_ERROR_DEVELOPER_CERT_ALREADY_EXIST     = 0x13000001;  //  Developer certificate already exists
    public final static long SS_ERROR_PARSE_CERT                       = 0x13000003;  //  Parse certificate error
    public final static long SS_ERROR_D2C_PACKAGE_TOO_LARGE            = 0x13000004;  //  D2C package is too large
    public final static long SS_ERROR_RESPONSE                         = 0x13000005;  //  Data response error
    public final static long SS_ERROR_SEND_LM_REMOTE_REQUEST           = 0x13000006;  //  Send LM remote request failed
    public final static long SS_ERROR_RUNTIME_NOT_INITIALIZE           = 0x13000007;  //  No call the init function of Runtime
    public final static long SS_ERROR_BAD_CONNECT                      = 0x13000008;  //  Get connection failed
    public final static long SS_ERROR_RUNTIME_VERSION                  = 0x13000009;  //  Version dose not match
    public final static long SS_ERROR_LIC_NOT_FOUND                    = 0x13000020;  //  Can not find the license
    public final static long SS_ERROR_AUTH_ACCEPT_FAILED               = 0x13000021;  //  Authentication error
    public final static long SS_ERROR_AUTH_HANDLE_FAILED               = 0x13000022;  //  Authentication failed
    public final static long SS_ERROR_DECODE_BUFFER                    = 0x13000023;  //  Decode error
    public final static long SS_ERROR_USER_DATA_TOO_SMALL              = 0x13000024;  //  User data field is small
    public final static long SS_ERROR_INVALID_LM_REQUEST               = 0x13000025;  //  Invalid LM request
    public final static long SS_ERROR_INVALID_SHORTCODE                = 0x13000026;  //  Invalid shortcode
    public final static long SS_ERROR_INVALID_D2C_PACKAGE              = 0x13000027;  //  D2C package is wrong
    public final static long SS_ERROR_CLOUD_RESPONSE                   = 0x13000028;  //  Clout lock return data error
    public final static long SS_ERROR_USER_DATA_TOO_LARGE              = 0x13000029;  //  The data of write of read is too large
    public final static long SS_ERROR_INVALID_MEMORY_ID                = 0x1300002A;  //  Invalid Memory ID
    public final static long SS_ERROR_INVALID_MEMORY_OFFSET            = 0x1300002B;  //  Invalid Memory offset
    public final static long SS_ERROR_INVALID_CLOUD_SERVER             = 0x1300002C;  //  Invalid cloud lock servers
    public final static long SS_ERROR_UNCALIBRATED_TIMESTAMP           = 0x1300002D;  //  No calibrate the timestamp
    public final static long SS_ERROR_GENERATE_GUID                    = 0x1300002F;  //  Build GUID error
    public final static long SS_ERROR_NO_LOGGED_USER                   = 0x13000030;  //  No logged user
    public final static long SS_ERROR_USER_AUTH_SERVER_NOT_RUNNING     = 0x13000031;  //  User auth server not running
    public final static long SS_ERROR_UNSUPPORTED_SNIPPET_CODE         = 0x13000033;  //  Unsupported code snippet
    public final static long SS_ERROR_INVALID_SNIPPET_CODE             = 0x13000034;  //  Invalid code
    public final static long SS_ERROR_EXECUTE_SNIPPET_CODE             = 0x13000035;  //  Execute snippet code failed
    public final static long SS_ERROR_SNIPPET_EXECUTE_LOGIN            = 0x13000036;  //  Login snippet code server failed
    public final static long SS_ERROR_LICENSE_MODULE_NOT_EXISTS        = 0x13000037;  //  License module not exist
    public final static long SS_ERROR_DEVELOPER_PASSWORD               = 0x13000038;  //  Error API password
    public final static long SS_ERROR_CALLBACK_VERSION                 = 0x13000039;  //  Error initialize version for callback
    public final static long SS_ERROR_INFO_RELOGIN                     = 0x1300003A;  //  Please relogin.
    public final static long SS_ERROR_LICENSE_VERIFY                   = 0x1300003B;  //  License data verify failed
    public final static long SS_ERROR_REFRESH_TOKEN_TIMEOUT            = 0x1300003C;  //  Refresh token timeout
    public final static long SS_ERROR_TOKEN_VERIFY_FAIL                = 0x1300003D;  //  Token validation failed
    public final static long SS_ERROR_GET_TOKEN_FAIL                   = 0x1300003E;  //  Get token failed
    public final static long SS_ERROR_NEED_WAIT                        = 0x13000044;  //  Inner error
    public final static long SS_ERROR_LICENSE_NEED_TO_ACTIVATE         = 0x13000051;  //  License need to activate online
    public final static long SS_ERROR_DATA_NOT_END                     = 0x13000052;  //  Internal error, data is not finished

    //============================================================
    //              IPC Module (0x02)
    //============================================================
    public final static long SS_ERROR_BAD_ADDR                         = 0x02000000;  //  Address error
    public final static long SS_ERROR_BAD_NAME                         = 0x02000001;  //  Name error
    public final static long SS_ERROR_IPC_FAILED                       = 0x02000002;  //  IPC receive and sent error
    public final static long SS_ERROR_IPC_CONNECT_FAILED               = 0x02000003;  //  Connect failed
    public final static long SS_ERROR_IPC_AUTH_INITIALIZE              = 0x02000004;  //  Auth failed
    public final static long SS_ERROR_IPC_QUERY_STATE                  = 0x02000005;  //  Query SS state failed
    public final static long SS_ERROR_SERVICE_NOT_RUNNING              = 0x02000006;  //  SS is not running
    public final static long SS_ERROR_IPC_DISCONNECT_FAILED            = 0x02000007;  //  Disconnect failed
    public final static long SS_ERROR_IPC_BUILD_SESSION_KEY            = 0x02000008;  //  Session key negotiation failed
    public final static long SS_ERROR_REQUEST_OUTPUT_BUFFER_TOO_LARGE  = 0x02000009;  //  The buffer size is too large of requested
    public final static long SS_ERROR_IPC_AUTH_ENCODE                  = 0x0200000A;  //  Auth encode error
    public final static long SS_ERROR_IPC_AUTH_DECODE                  = 0x0200000B;  //  Auth decode error
    public final static long SS_ERROR_IPC_INIT_FAILED                  = 0x0200000C;  //  IPC initialization failed, you can try to repair using the LSP repair tool
    public final static long SS_ERROR_IPC_EXCHANGE_CERT                = 0x0200000D;  //  Exchange certificate failed. Generally speaking, certificate upgrade compatibility error

    //============================================================
    //              Net Agent Module (0x11)
    //============================================================
    
    
    
    
    //============================================================
    //              Security Module (0x12)
    //============================================================
    
    public final static long SS_ERROR_INIT_ANTIDEBUG                    = 0x12000005;
    public final static long SS_ERROR_DEBUG_FOUNDED                     = 0x12000006;
    
    
    
    //============================================================
    //              LM Service (0x24)
    //============================================================
    public final static long ERROR_LM_SVC_UNINTIALIZED                 = 0x24000001;  //  Uninitialize the service table item
    public final static long ERROR_LM_SVC_INITIALIZING                 = 0x24000002;  //  Initializing the service table
    public final static long ERROR_LM_SVC_INVALID_SESSION_INFO_SIZE    = 0x24000003;  //  The size of input to session is wrong
    public final static long ERROR_LM_SVC_KEEP_ALIVE_FAILED            = 0x24000004;  //  Unknown reason of keep alive operation failure
    public final static long ERROR_LM_SVC_LICENSE_NOT_FOUND            = 0x24000005;  //  Can not find the specific license in the buffer
    public final static long ERROR_LM_SVC_SESSION_ALREADY_LOGOUT       = 0x24000006;  //  Session has quit
    public final static long ERROR_LM_SVC_SESSION_ID_NOT_FOUND         = 0x24000007;  //  Session id is not existent
    public final static long ERROR_LM_SVC_DEBUGGED                     = 0x24000008;  //  Find be debugged
    public final static long ERROR_LM_SVC_INVALID_DESCRIPTION          = 0x24000009;  //  Invalid license describe information
    public final static long ERROR_LM_SVC_HANDLE_NOT_FOUND             = 0x2400000A;  //  Can not find the specific handle
    public final static long ERROR_LM_SVC_CACHE_OVERFLOW               = 0x2400000B;  //  Cache buffer is full
    public final static long ERROR_LM_SVC_SESSION_OVERFLOW             = 0x2400000C;  //  Session buffer is full
    public final static long ERROR_LM_SVC_INVALID_SESSION              = 0x2400000D;  //  Invalid session
    public final static long ERROR_LM_SVC_SESSION_ALREADY_DELETED      = 0x2400000E;  //  Session has been deleted
    public final static long ERROR_LM_SVC_LICENCE_EXPIRED              = 0x2400000F;  //  License is out of date
    public final static long ERROR_LM_SVC_SESSION_TIME_OUT             = 0x24000010;  //  Session time out
    public final static long ERROR_LM_SVC_NOT_ENOUGH_BUFF              = 0x24000011;  //  Buffer is not enough
    public final static long ERROR_LM_SVC_DESC_NOT_FOUND               = 0x24000012;  //  Can not find device handle
    public final static long ERROR_LM_INVALID_PARAMETER                = 0x24000013;  //  LM service parameter error
    public final static long ERROR_LM_INVALID_LOCK_TYPE                = 0x24000014;  //    Unsupported lock type
    public final static long ERROR_LM_REMOTE_LOGIN_DENIED              = 0x24000015;  //  License can not remote login
    public final static long ERROR_LM_SVC_SESSION_INVALID_AUTHCODE     = 0x24000016;  //  Session auth failed
    public final static long ERROR_LM_SVC_ACCOUNT_NOT_BOUND            = 0x24000017;  //  Unbound user
    public final static long ERROR_LM_USER_NOT_EXISTS                  = 0x24000018;  //  Can not found account
    
    //============================================================
    //              LM Native (0x21)
    //============================================================
    public final static long SS_ERROR_UNSUPPORTED_ALGORITHM            = 0x21000000;  //  Unsupported algo type
    public final static long SS_ERROR_INVAILD_HLC_HANDLE               = 0x21000001;  //  Invalid HLC handle
    public final static long SS_ERROR_HLC_CHECK                        = 0x21000002;  //  HLC check failed
    public final static long SS_ERROR_LM_CHECK_READ                    = 0x21000003;  //  Read flag check failed
    public final static long SS_ERROR_LM_CHECK_LICENSE                 = 0x21000004;  //  Output buffer license ID is not match
    public final static long SS_ERROR_LM_CHECKSUM                      = 0x21000005;  //  Output buffer calibrate failed
    public final static long SS_ERROR_HLC_BUFFER_LEN                   = 0x21000006;  //  HLC data encrypt is large than buffer
    public final static long SS_ERROR_L2CWF_LEN                        = 0x21000007;  //  Invalid encrypt length
    public final static long SS_ERROR_INVAILD_MAX_ENCRYPT_LENGTH       = 0x21000008;  //  Invalid encrypt max length
    public final static long SS_ERROR_INVAILD_ENUM_CRYPT_TYPE          = 0x21000009;  //  Unsupported encryption type
    public final static long SS_ERROR_NATIVE_INSUFFICIENT_BUFFER       = 0x2100000A;  //  Buffer shortage
    public final static long SS_ERROR_NATIVE_LIST_FILE_FAILED          = 0x2100000B;  //  Enum files from dongle failed
    public final static long SS_ERROR_INVALID_C2H_REQUEST              = 0x2100000C;  //  Invalid request from cloud to dongle

    //============================================================
    //              LM Firmware (0x22)
    //============================================================
    public final static long SS_ERROR_FIRM_INVALID_FILE_NAME               = 0x22000001;  // Invalid file name                                    
    public final static long SS_ERROR_FIRM_CHECK_BUFF_FAILED               = 0x22000002;  // Data check failed
    public final static long SS_ERROR_FIRM_INVALID_BUFF_LEN                = 0x22000003;  // Input data length error
    public final static long SS_ERROR_FIRM_INVALID_PARAM                   = 0x22000004;  // Parameter error
    public final static long SS_ERROR_FIRM_INVALID_SESSION_INFO            = 0x22000005;  // session infomation error
    public final static long SS_ERROR_FIRM_INVALID_FILE_SIZE               = 0x22000006;  // Create file length is wrong
    public final static long SS_ERROR_FIRM_WRITE_FILE_FAILED               = 0x22000007;  // Read file data error
    public final static long SS_ERROR_FIRM_INVALID_LICENCE_HEADER          = 0x22000008;  // License information head is wrong
    public final static long SS_ERROR_FIRM_INVALID_LICENCE_SIZE            = 0x22000009;  // License data is wrong
    public final static long SS_ERROR_FIRM_INVALID_LICENCE_INDEX           = 0x2200000A;  // Exceed the max license number                                 
    public final static long SS_ERROR_FIRM_LIC_NOT_FOUND                   = 0x2200000B;  // Can not find specific license
    public final static long SS_ERROR_FIRM_MEM_STATUS_INVALID              = 0x2200000C;  // The data of memory state is not init
    public final static long SS_ERROR_FIRM_INVALID_LIC_ID                  = 0x2200000D;  // Invalid license ID
    public final static long SS_ERROR_FIRM_LICENCE_ALL_DISABLED            = 0x2200000E;  // All licenses are disabled
    public final static long SS_ERROR_FIRM_CUR_LICENCE_DISABLED            = 0x2200000F;  // Current license is disabled
    public final static long SS_ERROR_FIRM_LICENCE_INVALID                 = 0x22000010;  // Current license is not available
    public final static long SS_ERROR_FIRM_LIC_STILL_UNAVALIABLE           = 0x22000011;  // License is not available
    public final static long SS_ERROR_FIRM_LIC_TERMINATED                  = 0x22000012;  // License expires
    public final static long SS_ERROR_FIRM_LIC_RUNTIME_TIME_OUT            = 0x22000013;  // Running time use up
    public final static long SS_ERROR_FIRM_LIC_COUNTER_IS_ZERO             = 0x22000014;  // Counter use up
    public final static long SS_ERROR_FIRM_LIC_MAX_CONNECTION              = 0x22000015;  // Reach the limit concurrent number
    public final static long SS_ERROR_FIRM_INVALID_LOGIN_COUNTER           = 0x22000016;  // Login number is wrong
    public final static long SS_ERROR_FIRM_REACHED_MAX_SESSION             = 0x22000017;  // The maximum number of sessions has been reached within lock
    public final static long SS_ERROR_FIRM_INVALID_TIME_INFO               = 0x22000018;  // Communication time information error
    public final static long SS_ERROR_FIRM_SESSION_SIZE_DISMATCH           = 0x22000019;  // session size error
    public final static long SS_ERROR_FIRM_NOT_ENOUGH_SHAREMEMORY          = 0x2200001A;  // There is not enough shared memory
    public final static long SS_ERROR_FIRM_INVALID_OPCODE                  = 0x2200001B;  // The operate code is available
    public final static long SS_ERROR_FIRM_INVALID_DATA_LEN                = 0x2200001C;  // The length of data file is wrong
    public final static long SS_ERROR_FIRM_DATA_FILE_NOT_FOUND             = 0x2200001E;  // Can not find the specific license data
    public final static long SS_ERROR_FIRM_INVALID_PKG_TYPE                = 0x2200001F;  // The type of remote update package is wrong
    public final static long SS_ERROR_FIRM_INVALID_TIME_STAMP              = 0x22000020;  // Timestamp is wrong in the update package
    public final static long SS_ERROR_FIRM_INVALID_UPD_LIC_ID              = 0x22000021;  // The number of remote update license is wrong
    public final static long SS_ERROR_FIRM_LIC_ALREADY_EXIST               = 0x22000022;  // The added license is already exists
    public final static long SS_ERROR_FIRM_LICENCE_SIZE_LIMITTED           = 0x22000023;  // License number limits
    public final static long SS_ERROR_FIRM_INVALID_DATA_FILE_OFFSET        = 0x22000024;  // Invalid offset of license data
    public final static long SS_ERROR_FIRM_ZERO_INDEX_LIC_DESTROY          = 0x22000025;  // Bad No. 0 license
    public final static long SS_ERROR_FIRM_LIC_ALREADY_DISABLED            = 0x22000026;  // License is disable
    public final static long SS_ERROR_FIRM_INVALID_UPD_OPCODE              = 0x22000027;  // Invalid operate code of remote update
    public final static long SS_ERROR_FIRM_LIC_ALREADY_ENABLED             = 0x22000028;  // License is enable
    public final static long SS_ERROR_FIRM_INVALID_PKG_SIZE                = 0x22000029;  // The length of remote update package is wrong
    public final static long SS_ERROR_FIRM_LIC_COUNT_RETURN                = 0x2200002A;  // Return the wrong license number
    public final static long SS_ERROR_FIRM_INVALID_OPERATION               = 0x2200002B;  // Execute the wrong operate
    public final static long SS_ERROR_FIRM_SESSION_ALREADY_LOGOUT          = 0x2200002C;  // Session logout
    public final static long SS_ERROR_FIRM_EXCHANGE_KEY_TIMEOUT            = 0x2200002D;  // Exchange key time out
    public final static long SS_ERROR_FIRM_INVALID_EXCHANGE_KEY_MAGIC      = 0x2200002E;  // The wrong key exchange magic number
    public final static long SS_ERROR_FIRM_INVALID_AUTH_CODE               = 0x2200002F;  // Authentication data error
    public final static long SS_ERROR_FIRM_CONVERT_INDEX_TO_FILE           = 0x22000030;  // Exchange lic number to file name failed
    public final static long SS_ERROR_FIRM_INVALID_USER_DATA_TYPE          = 0x22000031;  // The type of user defined field is wrong
    public final static long SS_ERROR_FIRM_INVALID_DATA_FILE_SIZE          = 0x22000032;  // User defined data field is too large
    public final static long SS_ERROR_FIRM_INVALID_CCRNT_OPR_TYPE          = 0x22000033;  // Concurrent number operation type is wrong
    public final static long SS_ERROR_FIRM_ALL_LIC_TERMINATED              = 0x22000034;  // All licensed time expires
    public final static long SS_ERROR_FIRM_INVALID_CCRNT_VALUE             = 0x22000035;  // Concurrent number is wrong
    public final static long SS_ERROR_FIRM_INVALID_UPD_FILE                = 0x22000036;  // Delete history file is not available
    public final static long SS_ERROR_FIRM_UPD_RECORD_FULL                 = 0x22000037;  // Update record to achieve maximum
    public final static long SS_ERROR_FIRM_UPDATE_FAILED                   = 0x22000038;  // Remote update failed
    public final static long SS_ERROR_FIRM_LICENSE_BEING_WRITTING          = 0x22000039;  // License is writing
    public final static long SS_ERROR_FIRM_INVALID_PKG_FIELD_TYPE          = 0x2200003A;  // Update package sub type is wrong
    public final static long SS_ERROR_FIRM_LOAT_FSM_SALT                   = 0x2200003B;  // Load salt file error
    public final static long SS_ERROR_FIRM_DATA_LENGTH_ALIGNMENT           = 0x2200003C;  // The length of the encryption and decryption data is not aligned
    public final static long SS_ERROR_FIRM_DATA_CRYPTION                   = 0x2200003D;  // Encryption and decryption data is wrong
    public final static long SS_ERROR_FIRM_SHORTCODE_UPDATE_NOT_SUPPORTED  = 0x2200003E;  // Unsupported short code update
    public final static long SS_ERROR_FIRM_INVALID_SHORTCODE               = 0x2200003F;  // The short code is not available
    public final static long SS_ERROR_FIRM_LIC_USR_DATA_NOT_EXIST          = 0x22000040;  // User defined data is not existent
    public final static long SS_ERROR_FIRM_RCD_FILE_NOT_INITIALIZED        = 0x22000041;  // Delete history file is not initialized
    public final static long SS_ERROR_FIRM_AUTH_FILE_NOT_FOUND             = 0x22000042;  // Can not find authentication file
    public final static long SS_ERROR_FIRM_SESSION_OVERFLOW                = 0x22000043;  // Session num overflow
    public final static long SS_ERROR_FIRM_TIME_OVERFLOW                   = 0x22000044;  // Time information overflow
    public final static long SS_ERROR_FIRM_REACH_FILE_LIS_END              = 0x22000045;  // Enum reach the last file
    public final static long SS_ERROR_FIRM_ANTI_MECHANISM_ACTIVED          = 0x22000046;  // Punishment num active lock lm
    public final static long SS_ERROR_FIRM_NO_BLOCK                        = 0x22000047;  // Get block failed
    public final static long SS_ERROR_FIRM_NOT_ENDED                       = 0x22000048;  // Data transmit error
    public final static long SS_ERROR_FIRM_LIC_ALREADY_ACTIVE              = 0x22000049;  // license already active
    public final static long SS_ERROR_FIRM_FILE_NOT_FOUND                  = 0x22000050;  // File not find
    public final static long SS_ERROR_FIRM_UNKNOW_USER_DATA_TYPE           = 0x22000051;  // Unknown user data type
    public final static long SS_ERROR_FIRM_INVALID_TF_CODE                 = 0x22000052;  // Error transmit operation code
    public final static long SS_ERROR_FIRM_UNMATCH_GUID                    = 0x22000053;  // Mismatch GUID
    public final static long SS_ERROR_FIRM_UNABLE_TRANSFER                 = 0x22000054;  // License cann't transmit
    public final static long SS_ERROR_FIRM_INVALID_TRANSCODE               = 0x22000055;  // Unrecognise random code
    public final static long SS_ERROR_FIRM_ACCOUNT_NAME_NOT_FOUND          = 0x22000056;  // User not find
    public final static long SS_ERROR_FIRM_ACCOUNT_ID_NOT_FOUND            = 0x22000057;  // User id not find
    public final static long SS_ERROR_FIRM_INVALID_XKEY_STEP               = 0x22000058;  // Error kay exchange process
    public final static long SS_ERROR_FIRM_INVLAID_DEVELOPER_ID            = 0x22000059;  // Invalid developer ID
    public final static long SS_ERROR_FIRM_CA_TYPE                         = 0x2200005A;  // CA type error
    public final static long SS_ERROR_FIRM_LIC_TRANSFER_FAILURE            = 0x2200005B;  // license transmit failed
    public final static long SS_ERROR_FIRM_TF_PACKAGE_VERSION              = 0x2200005C;  // Error package version
    public final static long SS_ERROR_FIRM_BEYOND_PKG_ITEM_SIZE            = 0x2200005D;  // License number of update package is too large
    public final static long SS_ERROR_FIRM_UNBOUND_ACCOUNT_INFO            = 0x2200005E;  // The account is not bound to User Lock
    public final static long SS_ERROR_FIRM_DEVICE_LOCKED                   = 0x2200005F;  // The User Lock is locked, please contact the vendor to unlock it
    public final static long SS_ERROR_FIRM_INVALID_LOCK_PASSWORD           = 0x22000060;  // Locked password is incorrect, you may be the victim of pirated software
    public final static long SS_ERROR_FIRM_NOT_EXCHANGE_KEY                = 0x22000061;  // No key exchange was preformed
    public final static long SS_ERROR_FIRM_INVALID_SHORTCODE_SWAP_FILE     = 0x22000062;  // Invalid SLAC file in User Lock
    public final static long SS_ERROR_FIRM_SHORTCODE_UPDATE_USER_DATA      = 0x22000063;  // Upgrade user data area error by SLAC
    public final static long SS_ERROR_FIRM_CTRL_HMAC_VERSION               = 0x22000064;  // Incorrect D2C package signature version, please connect the vendor to update User Lock
    public final static long SS_ERROR_FIRM_CTRL_HMAC_MAGIC                 = 0x22000065;  // Incorrect D2C package data, please connect the vendor to solve it

    public final static long SS_ERROR_FIRM_GEN_HWFP                        = 0x22001001;  // Offline license can not bind
    public final static long SS_ERROR_FIRM_WRONG_VERSION                   = 0x22001002;  // Error protocol version
    public final static long SS_ERROR_FIRM_INVALID_PACKAGE                 = 0x22001003;  // Bad data
    public final static long SS_ERROR_FIRM_UNSUPPORTED_PACKAGE             = 0x22001004;  // Unsupported data packets
    public final static long SS_ERROR_FIRM_ILLEGAL_PACKAGE                 = 0x22001005;  // Illegal data packets
    public final static long SS_ERROR_FIRM_EXCEPTION                       = 0x22001006;  // Inner error
    public final static long SS_ERROR_FIRM_VERIFY_D2C                      = 0x22001007;  // D2C verification failed
    public final static long SS_ERROR_FIRM_HWFP_MISMATCHED                 = 0x22001008;  // Mismatched binding packets
    public final static long SS_ERROR_FIRM_LICDATA_ERROR                   = 0x22001009;  // License data error
    public final static long SS_ERROR_FIRM_DEVPCERTS_NOT_FOUND             = 0X2200100A;  // Can not found developer certificate
    public final static long SS_ERROR_FIRM_WRONG_CERTS                     = 0x2200100B;  // Error certificate
    public final static long SS_ERROR_FIRM_VERIFY_DEVPSIGN                 = 0x2200100C;  // Developer signature failed
    public final static long SS_ERROR_FIRM_INVALID_VCLOCK                  = 0x2200100D;  // Clock error
    public final static long SS_ERROR_FIRM_SLOCK_CORRUPT                   = 0x2200100E;  // Illegal license data
    public final static long SS_ERROR_FIRM_FORMAT_SLOCK                    = 0x2200100F;  // Format failed
    public final static long SS_ERROR_FIRM_BAD_CONFIG                      = 0x22001010;  // The configuration file does not exist or is corrupted
    public final static long SS_ERROR_FIRM_BAD_OFFLINE_ADJUST_TIME         = 0x22001011;  // Invalid offline calibration time               


    //============================================================
    //              License Transform Module Module (0x28)
    //============================================================
    public final static long SS_ERROR_LIC_TRANS_NO_SN_DESC              = 0x28000001;  // Can not found device description
    public final static long SS_ERROR_LIC_TRANS_INVALID_DATA            = 0x28000002;  // Error data
    
    //============================================================
    //              Auth Server Module (0x29)
    //============================================================
    
    public final static long SS_ERROR_AUTH_SERVER_INVALID_TOKEN          = 0x29000001;  // Invalid access token
    public final static long SS_ERROR_AUTH_SERVER_REFRESH_TOKEN          = 0x29000002;  // Refresh token failed
    public final static long SS_ERROR_AUTH_SERVER_LOGIN_CANCELED         = 0x29000003;  // Registration cancellation
    public final static long SS_ERROR_AUTH_SERVER_GET_ALL_USER_INFO_FAIL = 0x29000004;  // Failed to obtain all user information
    
    //============================================================
    //              Cloud Module (0x30)
    //============================================================
    public final static long SS_CLOUD_OK                                 = 0x30000000;  //  Success
    public final static long SS_ERROR_CLOUD_INVALID_PARAMETER            = 0x30000001;  //  Parameter error
    public final static long SS_ERROR_CLOUD_QUERY_UESR_INFO              = 0x30000002;  //  Cann't find user information
    public final static long SS_ERROR_CLOUD_INVALID_LICENSE_SESSION      = 0x30000003;  //  License not login or timeout
    public final static long SS_ERROR_CLOUD_DATA_EXPIRED                 = 0x30000004;  //  Data out of date
    public final static long SS_ERROR_CLOUD_VERIFY_TIMESTAMP_SIGNATURE   = 0x30000005;  //  Timestamp signature verify failed
    public final static long SS_ERROR_CLOUD_AUTH_FAILED                  = 0x30000006;  //  p2p authentication failed
    public final static long SS_ERROR_CLOUD_NOT_BOUND                    = 0x30000007;  //  Algorithm doesn't exist or unbind
    public final static long SS_ERROR_CLOUD_EXECUTE_FAILED               = 0x30000008;  //  Algorithm execute failed
    public final static long SS_ERROR_CLOUD_INVALID_TOKEN                = 0x30000010;  //  Illegae access token
    public final static long SS_ERROR_CLOUD_LICENSE_ALREADY_LOGIN        = 0x30000011;  //  LIcense Alreay Login
    public final static long SS_ERROR_CLOUD_LICENSE_EXPIRED              = 0x30000012;  //  LIcense Overtime
    public final static long SS_ERROR_CLOUD_SESSION_KICKED               = 0x30000013;  //  License session is kicked
    public final static long SS_ERROR_CLOUD_INVALID_SESSSION             = 0x30001002;  //  SessionId Losed
    public final static long SS_ERROR_CLOUD_SESSION_TIMEOUT              = 0x30001004;  //  Key Not Exist Or Timed Out
    public final static long SS_ERROR_CLOUD_PARSE_PARAM                  = 0x30001007;  //  Incorrect Format.Parmeter Parse Error
    public final static long SS_ERROR_CLOUD_LICENSE_LOGIN_SUCCESS        = 0x31001000;  //  License login succeed
    public final static long SS_ERROR_CLOUD_LICENSE_NOT_EXISTS           = 0x31001001;  //  License not exist
    public final static long SS_ERROR_CLOUD_LICENSE_NOT_ACTIVE           = 0x31001002;  //  License not active
    public final static long SS_ERROR_CLOUD_LICENSE_EXPIRED2             = 0x31001003;  //  License expired
    public final static long SS_ERROR_CLOUD_LICENSE_COUNTER_IS_ZERO      = 0x31001004;  //  License no usage count
    public final static long SS_ERROR_CLOUD_LICENSE_RUNTIME_TIME_OUT     = 0x31001005;  //  License no usage time
    public final static long SS_ERROR_CLOUD_LICENSE_MAX_CONNECTION       = 0x31001006;  //  License concurrent limit
    public final static long SS_ERROR_CLOUD_LICENSE_LOCKED               = 0x31001007;  //  License locked
    public final static long SS_ERROR_CLOUD_LICENSE_DATA_NOT_EXISTS      = 0x31001008;  //  No license data
    public final static long SS_ERROR_CLOUD_LICENSE_STILL_UNAVAILABLE    = 0x31001010;  //  License not to use time
    public final static long SS_ERROR_CLOUD_ZERO_LICENSE_NOT_EXISTS      = 0x31001011;  //  License 0 not exist
    public final static long SS_ERROR_CLOUD_VERIFY_LICENSE               = 0x31001012;  //  License verify failed
    public final static long SS_ERROR_CLOUD_EXECUTE_FILE_NOT_EXISTS      = 0x31002000;  //  Algorithm not exist
    public final static long SS_ERROR_CLOUD_LICENSE_NOT_BOUND            = 0x31003001;  //  Algorithm already bind

    public final static long SS_ERROR_SO_REFUSE_ADJUST_TIME              = 0x51004003;  //  Refuse adjust time
    public final static long SS_ERROR_SO_BEFORE_START_TIME               = 0x51004004;  //  License before start time
    public final static long SS_ERROR_SO_EXPIRED                         = 0x51004005;  //  License expired
    public final static long SS_ERROR_SO_LICENSE_BIND_ERROR              = 0x51004006;  //  License bind error
    public final static long SS_ERROR_SO_LICENSE_BIND_FULL               = 0x51004007;  //  License bind machines is full
    public final static long SS_ERROR_SO_LICENSE_UNBOUND                 = 0x51004008;  //  License already bound
    public final static long SS_ERROR_SO_LICENSE_MAX_BIND_FULL           = 0x51004009;  //  License maximum bind machines is full
    public final static long SS_ERROR_SO_NOT_SUPPORTED_OFFLINE_BIND      = 0x51004010;  //  The license does not support offline bind
    public final static long SS_ERROR_SO_EXPIRED_C2D                     = 0x51004011;  //  The C2D packet generation time differ greatly from server time
    public final static long SS_ERROR_SO_INVALID_C2D                     = 0x51004012;  //  Invalid C2D packet

    public final static long SS_ERROR_LL_SERVER_INTERNAL_ERROR           = 0x61000001;  //  Inner Service Error
    public final static long SS_ERROR_LL_INVALID_PARAMETERS              = 0x61000002;  //  Parameter error
    public final static long SS_ERROR_LL_DEVICE_INFO_NOT_EXIST           = 0x61000003;  //  The hardware lock Info can't be found
    public final static long SS_ERROR_LL_DEVICE_LOSS_OR_LOCKED           = 0x61000004;  //  The hardware lock has been lost or locked
    public final static long SS_ERROR_LL_UNKOWN_ACTIVATION_MODE          = 0x61001001;  //  Unknown activation state
    public final static long SS_ERROR_LL_UNKOWN_DEVICE_STATE             = 0x61001002;  //  Unknown lock type
    public final static long SS_ERROR_LL_UNKOWN_D2C_USAGE                = 0x61001003;  //  Unknown D2C type
    public final static long SS_ERROR_LL_UNKOWN_D2C_WRITE_RESULT         = 0x61001004;  //  Unknown upgrade result of D2C
    public final static long SS_ERROR_LL_VERIFY_ERROR                    = 0x61001005;  //  Verify failed
    public final static long SS_ERROR_LL_DATABASE_OPT_ERROR              = 0x61001006;  //  Database operation failed
    public final static long SS_ERROR_LL_NO_NEED_TO_ACTIVATE             = 0x61001007;  //  The hardware lock needn't to be activated
    public final static long SS_ERROR_LL_DEVICE_ALREADY_LOCKED           = 0x61001008;  //  The hardware lock has been activated
    public final static long SS_ERROR_LL_D2C_INFO_NOT_EXIST              = 0x61001009;  //  D2C Info can't be found
    public final static long SS_ERROR_LL_ILLEGAL_DEVICE_STATE            = 0x6100100a;  //  Invalid hardware lock state
    public final static long SS_ERROR_LL_CANNOT_BE_ACTIVATED             = 0x6100100b;  //  The hardware lock can't be activated in the current state
    public final static long SS_ERROR_LL_NOT_SUPPORTED_PROTOCOL_VERSION  = 0x6100100c;  //  Unsupported protocols


    //============================================================
    //              User Manager Module (0x60)
    //============================================================
    public final static long SS_UM_OK                                      = 0x00000000;    //  Success
    public final static long SS_UM_ERROR                                   = 0x00000001;    //  Generic error
    public final static long SS_ERROR_UM_PARAMETER_ERROR                   = 0x60000002;  //  Parameter error
    public final static long SS_ERROR_UM_CAPTCHA_INVALID                   = 0x60000003;  //  Verify Code expired
    public final static long SS_ERROR_UM_CAPTCHA_ERROR                     = 0x60000004;  //  Verify Code error
    public final static long SS_ERROR_UM_CAPTCHA_IS_NULL                   = 0x60000005;  //  Please input verify code
    public final static long SS_ERROR_UM_USER_NO_ACTIVE                    = 0x60000006;  //  User not activated
    public final static long SS_ERROR_UM_RETRY_TOO_MORE                    = 0x60000007;  //  User operation is frequent, Please try again later
    public final static long SS_ERROR_UM_USER_OR_PWD_ERROR                 = 0x60000008;  //  Error incorrect username or password
    public final static long SS_ERROR_UM_OAUTH_CONFIG_ERROR                = 0x60000009;  //  OAuth User center configuration error
    public final static long SS_ERROR_UM_GRANT_TYPE_ERROR                  = 0x6000000A;  //  Unsupported authorization type
    public final static long SS_ERROR_UM_SCOPE_INVALID                     = 0x6000000B;  //  Invalid scope
    public final static long SS_ERROR_UM_SERVER_STOP                       = 0x6000000C;  //  Service stop
    public final static long SS_ERROR_UM_IPC_TIMEOUT                       = 0x6000000D;  //  Operation timeout
    public final static long SS_ERROR_UM_TRANS_ERROR                       = 0x6000000E;  //  Internal data transfer error
    public final static long SS_ERROR_UM_CLOUD_INVALID_TOKEN               = 0x6000000F;  //  Invalid Token
    public final static long SS_ERROR_UM_ACCOUNT_HAVE_BEEN_LOGOUT          = 0x60000010;  //  No user information for this guid
    public final static long SS_ERROR_UM_NET_ERROR                         = 0x60000011;  //  Network error
    public final static long SS_ERROR_UM_COULDNT_RESOLVE_HOST              = 0x60000012;  //  Host unreachable
    public final static long SS_ERROR_UM_MEMORY_ERROR                      = 0x60000013;  //  Internal memory error
    public final static long SS_ERROR_UM_USERLIST_AND_AUTH_CFG_ERROR       = 0x60000014;  //  Configuration file error
    public final static long SS_ERROR_UM_NEED_RELOGON                      = 0x60000015;  //  Need relogon
    public final static long SS_ERROR_UM_VERIFY_TOKEN_TIMEOUT              = 0x60000016;  //  Verify token failed
    public final static long SS_ERROR_UM_REFRESH_TOKEN_TIMEOUT             = 0x60000017;  //  Refresh token failed
    
    
    
	public final static long H5_ERROR_SUCCESS                            = 0;
	// Macro
	public static long MAKE_ERROR(long mode, long errcode){
		return (long)(((mode) << 24) | (errcode));
	}
	
	public static long MAKE_COMMON_ERROR(long mode, long errcode){
		return (long)(((mode) << 24) | (errcode));
	}
	public static long MAKE_H5_RUNTIME(long errorcode){
		return (long)(((errorcode)==H5_ERROR_SUCCESS) ? 0 : (MAKE_COMMON_ERROR(MODE_H5_RUNTIME,(errorcode))));
	}
	public static long MAKE_NETAGENT(long errorcode){
		return MAKE_COMMON_ERROR(MODE_NETAGENT,(errorcode));
	}
	public static long MAKE_SSPROTECT(long errorcode){
		return MAKE_COMMON_ERROR(MODE_NETAGENT,(errorcode));
	}
	public static long MAKE_LM_FIRM_ERROR(long errorcode){
		return MAKE_COMMON_ERROR(MODE_LM_FIRM,(errorcode));
	}
	public static long MAKE_LM_SES_ERROR(long errorcode){
        return MAKE_COMMON_ERROR(MODE_LM_SES,(errorcode));
	}
	public static long GET_ERROR_MODULE(long errorcode){
		return (long)((errorcode) >> 24);
	}
}
