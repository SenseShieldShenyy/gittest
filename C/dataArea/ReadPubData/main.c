/*****************************************************************************
Copyright (C )2020 Beijing Senseshield Technology Co ,.Ltd
Author :senseshield 2020-11
Description : 读取公开区 数据区的数据
History : 
[作者 日期 修改内容]
shenyy 2020/11/18 creat
********************************************************************************/
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include "common.h"
#include "ss_lm_runtime.h"  //runtime 库使用注意事项详情见"readme.txt"
#include "SlcCrypt.h"
#include "cJSON.h"
#include "vld.h"

/** 用户数据区最大*/
#ifndef MAX_USER_DATA_SIZE
    #define MAX_USER_DATA_SIZE          64 * 1024
#endif // MAX_USER_DATA_SIZE


unsigned char PubKey[152] = {
    0x01, 0x00, 0x00, 0x00, 0x11, 0x00, 0x02, 0x00, 0x00, 0x04, 0x8C, 0x00, 0x30, 0x81, 0x89, 0x02, 
    0x81, 0x81, 0x00, 0xBE, 0x90, 0xFF, 0x68, 0x50, 0xEF, 0xD7, 0x9C, 0x77, 0x8C, 0xD0, 0xC5, 0x53, 
    0x4B, 0xC7, 0xF2, 0x7C, 0x6E, 0x3A, 0xEC, 0x66, 0x25, 0x51, 0x43, 0x3E, 0x8A, 0xD7, 0x41, 0xA7, 
    0x33, 0xE9, 0x2A, 0xE0, 0x75, 0xFC, 0x6E, 0xAC, 0x28, 0x2A, 0x32, 0x10, 0x47, 0x03, 0x8E, 0xE8, 
    0x25, 0xFF, 0xF6, 0x4C, 0x55, 0xAF, 0x15, 0x30, 0x28, 0x03, 0xA4, 0x4F, 0x9E, 0x74, 0x0C, 0xD1, 
    0x35, 0x28, 0xA3, 0xF8, 0x2B, 0x12, 0x93, 0x75, 0x9E, 0xC8, 0x28, 0xDB, 0x81, 0x6E, 0xF8, 0x65, 
    0xA8, 0x40, 0x47, 0xE6, 0x31, 0xC9, 0x07, 0xB6, 0x9B, 0x57, 0x53, 0x5C, 0xC6, 0x52, 0x0A, 0x48, 
    0xBE, 0x81, 0x26, 0xD0, 0xD6, 0xC6, 0xBB, 0x39, 0xE6, 0x7D, 0xF3, 0xF9, 0x08, 0x28, 0xF4, 0x73, 
    0x70, 0x81, 0x4F, 0x80, 0xA7, 0xBE, 0x35, 0x35, 0x0D, 0x44, 0x16, 0x7C, 0x6A, 0x71, 0x32, 0xA4, 
    0x49, 0xE0, 0x07, 0x02, 0x03, 0x01, 0x00, 0x01
};
void mem_free(void *ptr)
{
    if (ptr)
    {
        free(ptr);
        ptr = NULL;
    }
}
int main()
{
    SS_UINT32 ret                              = SS_OK;
    ST_INIT_PARAM st_init_param                = {0};
    ST_LOGIN_PARAM login_param                 = {0};
    SLM_HANDLE_INDEX slm_handle                = 0;
    SS_UINT32 ulRAWLen                         = 0;
    SS_UINT32 ulPUBLen                         = 0;
    SS_UINT32 uSignLen                         = 0;
    SS_UINT32 ulPUBLen1                        = 0;
    SS_UINT32 uSignLen1                        = 0;
    SS_UINT32 ulROWLen                         = 0;  
    SS_CHAR *lic_info                          = NULL;
    SS_BYTE *pData                             = NULL;
    SS_BYTE *pData1                            = NULL;
    SS_BYTE *pSign                             = NULL;
    SS_BYTE *pSign1                            = NULL;
    SS_CHAR *pchar_Devdescs                    = NULL;
    cJSON *pjson_Devdescs                      = NULL;
    SS_UINT32 index_device                     = 0;
    SS_UINT32 dev_num                          = 0;
    cJSON *pjson_Array                         = NULL;
    cJSON *pjson_Item                          = NULL;
    SS_UINT32 iOffset                          = 0;
    SS_CHAR sn[2][50];
    FILE                                       *fp;
    char *OpenFileName                         ="testopen.exe";

    // 初始化接口调用，参数初始化
    st_init_param.version = SLM_CALLBACK_VERSION02;
    st_init_param.pfn = NULL;
    st_init_param.flag = SLM_INIT_FLAG_NOTIFY;
    memcpy( st_init_param.password, g_api_password, sizeof(g_api_password) );
    ret = slm_init( &(st_init_param) );
    if(SS_OK == ret)
    {
        printf("slm_init ok\n");
    }
    else if(SS_ERROR_DEVELOPER_PASSWORD == ret)
    {
        printf("slm_init error : 0x%08X(SS_ERROR_DEVELOPER_PASSWORD), Please login to the Virbox Developer Center(https://developer.lm.virbox.com), get the API password, and replace the 'g_api_password' variable content.\n", ret);
        goto CLEAR;
    }
    else
    {
        printf("slm_init error : 0x%08X\n", ret);
        goto CLEAR;
    }
    ret = slm_enum_device( &pchar_Devdescs );
    //返回设备sn
    pjson_Devdescs = cJSON_Parse(pchar_Devdescs);
    if (!pjson_Devdescs)
    {
        printf("cJSON_Parse error\n");
        goto CLEAR;
    }
    dev_num    = cJSON_GetArraySize( pjson_Devdescs );
    printf("device count : %d\n", dev_num );
    for( index_device = 0; index_device < dev_num; index_device++ )
    {
        printf("device %d[device-desc]:\n", index_device);
        pjson_Array = cJSON_GetArrayItem( pjson_Devdescs, index_device );
        pjson_Item = cJSON_GetObjectItem( pjson_Array, "sn" );
        if(pjson_Item)
        {
            printf("    sn : %s\n",pjson_Item->valuestring);
            strcpy(sn[index_device],pjson_Item->valuestring);
        }
    }
    // 安全登录许可 //登录第一个锁获取公开区数据
    login_param.license_id                       = 21;
    login_param.size                             = sizeof(ST_LOGIN_PARAM);
    login_param.timeout                          = 600;
    login_param.login_flag                       = SLM_LOGIN_FLAG_LOCKSN;
    login_param.login_mode                       = SLM_LOGIN_MODE_LOCAL_DONGLE;
    hexstr_to_bytes(sn[0], SLM_LOCK_SN_LENGTH * 2, login_param.sn);
    ret = slm_login( &login_param, STRUCT, &(slm_handle), NULL );
    if(SS_OK != ret)
    {
        printf("slm_login error : 0x%08X\n", ret);
        goto CLEAR;
    }
    else
    {
        printf("slm_login ok\n");
    }
    ret = slm_user_data_getsize(slm_handle, PUB, &ulPUBLen);  
    ret = slm_user_data_getsize(slm_handle, ROM, &uSignLen);
    if (ret == SS_OK && ulPUBLen > 0)
    {        
        pData = (SS_BYTE *)calloc(sizeof(SS_BYTE), ulPUBLen); 
        pSign = (SS_BYTE *)calloc(sizeof(SS_BYTE), uSignLen);
        ret = slm_user_data_read(slm_handle, PUB, pData, 0, ulPUBLen);        
        ret = slm_user_data_read(slm_handle, ROM, pSign, 0, uSignLen);
        if(SS_OK != ret)
        {
            printf("slm_user_data_read[PUB] error : 0x%08X\n", ret);
            goto CLEAR;
        } 
        ret = SlcRsaVerify(SLC_PAD_MODE_PKCS_1_V1_5, SLC_HASH_ALGO_SHA256, PubKey, pData, ulPUBLen, pSign, uSignLen);
        if(SS_OK == ret)
        {
            printf("verify success\n");
        }
        else
        {
            printf("verify error : 0x%08X\n", ret);
            goto CLEAR;
        }   
    }
    //  许可登出  
    ret = slm_logout(slm_handle);
    if (SS_OK == ret) 
    {
        printf("slm_logout ok.\n");
    }
    else
    {
        printf("slm_logout error : 0x%08X", ret);
    }    
    //登录第二个加密锁获得公开区的数据
    hexstr_to_bytes(sn[1], SLM_LOCK_SN_LENGTH * 2, login_param.sn);
    ret = slm_login( &login_param, STRUCT, &(slm_handle), NULL );
    if(SS_OK != ret)
    {
        printf("slm_login error : 0x%08X\n", ret);
        goto CLEAR;
    }
    else
    {
        printf("slm_login ok\n");
    }
    ret = slm_user_data_getsize(slm_handle, PUB, &ulPUBLen1); 
    ret = slm_user_data_getsize(slm_handle, ROM, &uSignLen1);
    if (ret == SS_OK && ulPUBLen > 0)
    {        
        pData1 = (SS_BYTE *)calloc(sizeof(SS_BYTE), ulPUBLen1); 
        pSign1 = (SS_BYTE *)calloc(sizeof(SS_BYTE), uSignLen1);

        ret = slm_user_data_read(slm_handle, PUB, pData1, 0, ulPUBLen1);
        ret = slm_user_data_read(slm_handle, ROM, pSign1, 0, uSignLen1);
        if(SS_OK != ret)
        {
            printf("slm_user_data_read[PUB] error : 0x%08X\n", ret);
            goto CLEAR;
        }
        ret = SlcRsaVerify(SLC_PAD_MODE_PKCS_1_V1_5, SLC_HASH_ALGO_SHA256, PubKey, pData1, ulPUBLen1, pSign1, uSignLen1);
        if(SS_OK == ret)
        {
            printf("verify success\n");
        }
        else
        {
            printf("verify error : 0x%08X\n", ret);
            goto CLEAR;
        }
        if ( ( fp = fopen(OpenFileName,"wb")) == NULL)
        {
            printf("open file error!\n");
            goto CLEAR;
        }
        //根据数据区的大小判断文件写入顺序
        if(ulPUBLen >ulPUBLen1)
        {
            fwrite(pData, 1,ulPUBLen,fp);
            fwrite(pData1, 1,ulPUBLen1,fp);
        }
        else
        {
            fwrite(pData1, 1,ulPUBLen1,fp);
            fwrite(pData, 1,ulPUBLen,fp);
        }
        fclose(fp);
    }
CLEAR:
     ret = slm_logout(slm_handle);
    if (SS_OK == ret) 
    {
        printf("slm_logout ok.\n");
    }
    else
    {
        printf("slm_logout error : 0x%08X", ret);
    }
    mem_free(pData);
    mem_free(pData1);
    mem_free(pSign);
    mem_free(pSign1);
    mem_free(pchar_Devdescs);
    cJSON_Delete(pjson_Devdescs);
    //反初始化函数(slm_cleanup)，与slm_init对
    slm_cleanup();
    system("testopen.exe");
    printf("\npress any key exit process.\n");
    getchar();
    return 0;
}
