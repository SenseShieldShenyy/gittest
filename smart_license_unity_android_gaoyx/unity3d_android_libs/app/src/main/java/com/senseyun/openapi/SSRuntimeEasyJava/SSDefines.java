package com.senseyun.openapi.SSRuntimeEasyJava;

public interface SSDefines {
    int LED_COLOR_BLUE = 0;         //LED: Blue
    int LED_COLOR_RED = 1;          //LED: Red

    int LED_STATE_CLOSE = 0;        //LED: Off
    int LED_STATE_OPEN = 1;         //LED: On
    int LED_STATE_SHRINK = 2;       //LED: Flashing

    //============================================================
    //              Message type of Callback function
    //============================================================
    int SS_ANTI_INFORMATION		    = 0x0101;  //  Message type:Information
    int SS_ANTI_WARNING			    = 0x0102;  //  Message type:Warning. The function parameter wParam represents a warning type
    int SS_ANTI_EXCEPTION		    = 0x0103;  //  Message type:Exception
    int SS_ANTI_IDLE			    = 0x0104;  //  Message type:Reserve
    
    
    int SS_MSG_SERVICE_START        = 0x0200;  //  Message type:Service Start
    int SS_MSG_SERVICE_STOP         = 0x0201;  //  Message type:Service Stop
    int SS_MSG_LOCK_AVAILABLE       = 0x0202;  //  Message type:Lock Available. The function parameter wParam represents a lock sn
    int SS_MSG_LOCK_UNAVAILABLE     = 0x0203;  //  Message type:Lock Unavailable. The function parameter wParam represents a lock sn
    
    
    //============================================================
    //              The Callback Function Type -Warning -SS_ANTI_WARNING: wparam 
    //============================================================
    int SS_ANTI_PATCH_INJECT		= 0x0201;  //  Detects a patch is injected into the target program, it is generally referred to as the patch dynamic library of pirated programs existed in the current process space.
    int SS_ANTI_MODULE_INVALID		= 0x0202;  //  The system module is abnormal, and there may be hijack behavior, such as some dynamic library(hid. DLL, LPK. DLL) located in the system directory, which is loaded from the wrong directory.
    int SS_ANTI_ATTACH_FOUND		= 0x0203;  //  Detects the behavior of the program being attached to the debugger, such as additional debugging through Ollydbg and Windbg.
    int SS_ANTI_THREAD_INVALID		= 0x0204;  //  An invalid thread. Undefined, reserved.
    int SS_ANTI_THREAD_ERROR		= 0x0205;  //  Detects that the anti-debug thread is running in an abnormal state and can be attacked by a malicious thread, such as a thread being manually suspended.
    int SS_ANTI_CRC_ERROR			= 0x0206;  //  The failure of CRC verification of the module data detected. There may be malicious analysis behavior, breakpoint or tampering of the module in the process.
    int SS_ANTI_DEBUGGER_FOUND		= 0x0207;  //  The debugger exists in the software running environment, such as Ollydbg and Windbg.
    
    
    int SLM_FIXTIME_RAND_LENGTH     = 0x08;    //  The clock calibrates the seed length of the random number.
    
    int SLM_CALLBACK_VERSION02      = 0x02;    //  The callback function version. See 'SS_CALL_BACK' 
    
    
    int  SLM_MEM_MAX_SIZE           = 2048;  //  The max buffer size of Secured Extended Memory. Uint:byte
    int SLM_MAX_INPUT_SIZE          = 1758;  //  The max input size of Code Transplantation. Unit:byte
    int SLM_MAX_OUTPUT_SIZE         = 1758;  //  The max output size of Code Transplantation. Unit:byte
    int SLM_MAX_USER_CRYPT_SIZE     = 1520;  //  The max buffer size of License Encryption and decryption. Unit:byte
                                                    
    int SLM_MAX_USER_DATA_SIZE      = 2048;  //  The max buffer size of User Authentication data. Unit:byte
    int SLM_MAX_WRITE_SIZE          = 1904;  //  The max written buffer size of User Data. Unit:byte

    String SLM_VERIFY_DEVICE_PREFIX   = "SENSELOCK";  //  For request of data prefix of the private key signature of the hardware lock device, please refer to 'slm_sign_by_device'

    int SLM_VERIFY_DATA_SIZE        = 41 ;  //  Request the data size of the private key signature of the hardware lock device, please refer to 'slm_sign_by_device'
    int SLM_LOCK_SN_LENGTH          = 16 ;  //  The length of the Chip Serial Number of Hardware Lock device. Uint:byte
    int SLM_DEVELOPER_ID_SIZE       = 8  ;  //  The length of Developer ID. Uint:byte
    int SLM_MAX_SERVER_NAME         = 32 ;  //  The max buffer size of Remote Host Name. Uint:byte
    int SLM_MAX_ACCESS_TOKEN_LENGTH = 64 ;  //  The max buffer size of Access Token. Uint:byte
    int SLM_MAX_CLOUD_SERVER_LENGTH = 100;  //  The max buffer size of Cloud Host Name. Uint:byte
    int SLM_SNIPPET_SEED_LENGTH     = 32 ;  //  The length of Code Snippet. Uint:byte
    int SLM_DEV_PASSWORD_LENGTH     = 16 ;  //  The length of API Password. Uint:byte

    int SLM_CLOUD_MAX_USER_GUID_SIZE 	       = 128;   //  The max buffer size of User Guid. Uint:byte
                                                               
    int SLM_FILE_TYPE_BINARY                   = 0;     //  File type in Hardware Lock:Binary
    int SLM_FILE_TYPE_EXECUTIVE                = 1;     //  File type in Hardware Lock:Executive
    int SLM_FILE_TYPE_KEY                      = 2;     //  File type in Hardware Lock:Key
                                                              
    int SLM_FILE_PRIVILEGE_FLAG_READ           = 0x01;  //  Hardware Lock internal file attributes:Developer Read
    int SLM_FILE_PRIVILEGE_FLAG_WRITE          = 0x02;  //  Hardware Lock internal file attributes:Developer Write
    int SLM_FILE_PRIVILEGE_FLAG_USE            = 0x04;  //  Hardware Lock internal file attributes:Developer Use
    int SLM_FILE_PRIVILEGE_FLAG_UPDATE         = 0x08;  //  Hardware Lock internal file attributes:Developer Update

    int SLM_FILE_PRIVILEGE_FLAG_ENTRY_READ     = 0x10;  //  Hardware Lock internal file attributes:Admin Read
    int SLM_FILE_PRIVILEGE_FLAG_ENTRY_WRITE    = 0x20;  //  Hardware Lock internal file attributes:Admin Write
    int SLM_FILE_PRIVILEGE_FLAG_ENTRY_USE      = 0x40;  //  Hardware Lock internal file attributes:Admin Use
    int SLM_FILE_PRIVILEGE_FLAG_ENTRY_UPDATE   = 0x80;  //  Hardware Lock internal file attributes:Admin Update

    int SLM_LOGIN_MODE_AUTO             = 0x0000;  //  License Login Mode:Auto
    int SLM_LOGIN_MODE_LOCAL            = 0x0001;  //  License Login Mode:Local Dongle
    int SLM_LOGIN_MODE_REMOTE           = 0x0002;  //  License Login Mode:Remote Dongle
    int SLM_LOGIN_MODE_CLOUD            = 0x0004;  //  License Login Mode:Cloud
    int SLM_LOGIN_MODE_SLOCK            = 0x0008;  //  License Login Mode:Soft Based License
                                                        
    int SLM_LOGIN_FLAG_FIND_ALL         = 0x0001;  //  License Login Control: find all locks, and if a number of duplicate names are found, do not log in, provide a multi choice, or find a qualified lock to log in (not recommended)
    int SLM_LOGIN_FLAG_VERSION          = 0x0004;  //  License Login Control:Specify License Version
    int SLM_LOGIN_FLAG_LOCKSN           = 0x0008;  //  License Login Control:Specify Chip Serial Number
    int SLM_LOGIN_FLAG_SERVER           = 0x0010;  //  License Login Control:Specify Host Name
    int SLM_LOGIN_FLAG_SNIPPET          = 0x0020;  //  License Login Control:Specify Code Snippet

    int LANGUAGE_CHINESE_ASCII              = 0x0001;  //  Language of error code: Chinese
    int LANGUAGE_ENGLISH_ASCII              = 0x0002;  //  Language of error code: English
    int LANGUAGE_TRADITIONAL_CHINESE_ASCII  = 0x0003;  //  Language of error code: Traditional Chinese

    int SLM_INIT_FLAG_NOTIFY          = 0x01;  //  The switch to accept the message of Callback function.
}
