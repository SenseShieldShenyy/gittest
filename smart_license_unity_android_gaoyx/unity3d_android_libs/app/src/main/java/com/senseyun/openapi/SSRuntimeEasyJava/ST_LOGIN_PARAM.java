package com.senseyun.openapi.SSRuntimeEasyJava;

public class ST_LOGIN_PARAM {
  
    public int       size;  // Size of structure(required)
 
    public int       license_id;   // License ID(required)
  
    public int       timeout;  // The timeout period for the license session (in seconds), and 0 for the default value: 600 seconds.
  
    public int       login_mode;  // icense login mode: local lock License, network License, cloud license, soft license. If you fill 0, you use SLM_LOGIN_MODE_AUTO.
  
    public int       login_flag;  // License sign for login, non, not set this parameter. special purpose only, refer to SLM_LOGIN_FLAG_XXX
  
    public byte[]         sn = new byte[SSDefines.SLM_LOCK_SN_LENGTH];  // Chip Serial Number(optional)
  
    public char[]         server = new char[SSDefines.SLM_MAX_SERVER_NAME];  // Remote Host IP(optional)
  
    public char[]         access_token = new char[SSDefines.SLM_MAX_ACCESS_TOKEN_LENGTH];  // User Token(optional)
   
    public char[]         cloud_server = new char[SSDefines.SLM_MAX_CLOUD_SERVER_LENGTH]; // Cloud Host URL(optional)
    
    public byte[]         snippet_seed = new byte[SSDefines.SLM_SNIPPET_SEED_LENGTH];// Code Snippet Seed(optional)
    
    public byte[]         user_guid = new byte[SSDefines.SLM_CLOUD_MAX_USER_GUID_SIZE];// User Guid(optional)
    
    public int getSize()
    {
   	
    	return (4 * 5  + SSDefines.SLM_LOCK_SN_LENGTH + SSDefines.SLM_MAX_SERVER_NAME + SSDefines.SLM_MAX_ACCESS_TOKEN_LENGTH +
    			SSDefines.SLM_MAX_CLOUD_SERVER_LENGTH + SSDefines.SLM_SNIPPET_SEED_LENGTH + SSDefines.SLM_CLOUD_MAX_USER_GUID_SIZE);
    }
}
