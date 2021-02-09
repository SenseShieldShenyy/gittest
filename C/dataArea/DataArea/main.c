/************************************************************************************
Copyright(c)2016 Beijing SenseSheild Technology co.,Ltd
Author : shenyy
Description: 许可签发以及数据区的基本使用方法。
History : 
[作者 日期 修改内容]
shenyy 2020/11/17 creat
***********************************************************************************/
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <assert.h>
#include <time.h>

#include "d2c.h"
#include "ss_error.h"
#include "ss_lm_runtime.h"
#include "SlcCrypt.h"

#include "common.h"
#include "cJSON.h"
#include "base64.h"
#include "ss_help.h"
#include "vld.h"

/** 用户数据区最大*/
#ifndef MAX_USER_DATA_SIZE
    #define MAX_USER_DATA_SIZE          64 * 1024
#endif // MAX_USER_DATA_SIZE
#define TEMP_BUFFER_SIZE                256
#define LOCK_SN     "9733c801000702079da70011000b0013"
#define P7BDATA     "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"
#define GuoPan      "9733c801000702079da70019000f002d"

//签发许可
SS_UINT32 make_license(SS_UINT32 choose,  char *LOCK_SN_WHO);
//组织许可json数据
SS_UINT32 add_license_d2c(D2C_HANDLE hD2c,SS_UINT32 licID);
//获得加密锁的序列号和加密锁P7B证书
SS_UINT32 getP7bAndSNFromDB(SS_CHAR *pSN, SS_BYTE *pCertP7b, SS_UINT32 *pCertLen, char *LOCK_SN_WHO);
//读取二进制文件数据
SS_UINT32 read_files(char *FileName, char *InBuffer,int offet);
//释放内存
void mem_free(void *ptr)
{
    if(ptr)
    {
        free(ptr);
        ptr = NULL;
    }
}

unsigned char PriKey[621] = {
    0x01, 0x00, 0x00, 0x00, 0x11, 0x00, 0x03, 0x00, 0x00, 0x04, 0x61, 0x02, 0x30, 0x82, 0x02, 0x5D, 
    0x02, 0x01, 0x00, 0x02, 0x81, 0x81, 0x00, 0xBE, 0x90, 0xFF, 0x68, 0x50, 0xEF, 0xD7, 0x9C, 0x77, 
    0x8C, 0xD0, 0xC5, 0x53, 0x4B, 0xC7, 0xF2, 0x7C, 0x6E, 0x3A, 0xEC, 0x66, 0x25, 0x51, 0x43, 0x3E, 
    0x8A, 0xD7, 0x41, 0xA7, 0x33, 0xE9, 0x2A, 0xE0, 0x75, 0xFC, 0x6E, 0xAC, 0x28, 0x2A, 0x32, 0x10, 
    0x47, 0x03, 0x8E, 0xE8, 0x25, 0xFF, 0xF6, 0x4C, 0x55, 0xAF, 0x15, 0x30, 0x28, 0x03, 0xA4, 0x4F, 
    0x9E, 0x74, 0x0C, 0xD1, 0x35, 0x28, 0xA3, 0xF8, 0x2B, 0x12, 0x93, 0x75, 0x9E, 0xC8, 0x28, 0xDB, 
    0x81, 0x6E, 0xF8, 0x65, 0xA8, 0x40, 0x47, 0xE6, 0x31, 0xC9, 0x07, 0xB6, 0x9B, 0x57, 0x53, 0x5C, 
    0xC6, 0x52, 0x0A, 0x48, 0xBE, 0x81, 0x26, 0xD0, 0xD6, 0xC6, 0xBB, 0x39, 0xE6, 0x7D, 0xF3, 0xF9, 
    0x08, 0x28, 0xF4, 0x73, 0x70, 0x81, 0x4F, 0x80, 0xA7, 0xBE, 0x35, 0x35, 0x0D, 0x44, 0x16, 0x7C, 
    0x6A, 0x71, 0x32, 0xA4, 0x49, 0xE0, 0x07, 0x02, 0x03, 0x01, 0x00, 0x01, 0x02, 0x81, 0x80, 0x32, 
    0xBA, 0xF9, 0xEB, 0x24, 0xC4, 0xBC, 0x92, 0xC0, 0x36, 0xA1, 0xEB, 0x2D, 0xE1, 0xFA, 0x20, 0x00, 
    0xE4, 0xFD, 0x55, 0xAA, 0x59, 0x9B, 0xD8, 0xF9, 0x60, 0xCF, 0xAE, 0x00, 0x1B, 0x6F, 0x22, 0x85, 
    0x6F, 0x93, 0x5C, 0x49, 0x03, 0x46, 0x3C, 0x5E, 0x9D, 0xF1, 0x02, 0x0D, 0xA5, 0xF4, 0x0E, 0x76, 
    0xC2, 0xC6, 0xA1, 0xAE, 0xD8, 0xB0, 0x23, 0x81, 0x38, 0x8E, 0xEF, 0x4C, 0x90, 0x48, 0xB9, 0x6C, 
    0x3B, 0xA6, 0x1E, 0x1A, 0x55, 0x4A, 0x92, 0x39, 0xA5, 0xFB, 0x30, 0x2D, 0x48, 0xB6, 0x74, 0xFD, 
    0x53, 0xED, 0xA9, 0x26, 0x2E, 0x9D, 0xF8, 0xC0, 0x6B, 0xDA, 0x13, 0x27, 0x6D, 0x2E, 0x3F, 0x72, 
    0x14, 0x7B, 0x30, 0xA0, 0x93, 0xD5, 0xFA, 0x94, 0x61, 0xC5, 0x46, 0xA3, 0x5B, 0x42, 0x41, 0x5B, 
    0xFF, 0xCF, 0x40, 0x09, 0x98, 0x22, 0xE7, 0x98, 0x00, 0x36, 0xF5, 0x04, 0x23, 0x64, 0xC9, 0x02, 
    0x41, 0x00, 0xE1, 0x53, 0x27, 0x3C, 0x3E, 0xA5, 0x41, 0x54, 0x4B, 0x96, 0xE5, 0x0A, 0x66, 0x9A, 
    0x8A, 0xE2, 0x28, 0xCF, 0x62, 0xF3, 0x0A, 0xA5, 0x18, 0x94, 0xCB, 0x26, 0xF3, 0x45, 0x5B, 0xC0, 
    0xBA, 0xF4, 0x91, 0x8B, 0xA8, 0xD7, 0xF5, 0xD5, 0x1E, 0xF5, 0x54, 0x6F, 0x3E, 0x5C, 0xE6, 0x9B, 
    0x40, 0xA9, 0x35, 0x4A, 0x1F, 0x78, 0xDD, 0x78, 0xF9, 0x33, 0xC4, 0xC8, 0xFE, 0xD5, 0x46, 0x61, 
    0x74, 0x9F, 0x02, 0x41, 0x00, 0xD8, 0x82, 0x78, 0x84, 0x74, 0x22, 0x97, 0x78, 0x41, 0x78, 0x0F, 
    0x20, 0x1B, 0x41, 0xEB, 0xA2, 0x81, 0x6F, 0xF1, 0x47, 0x73, 0x91, 0x68, 0x32, 0xE2, 0xFE, 0x16, 
    0x55, 0x71, 0xC0, 0x68, 0x48, 0xD3, 0x28, 0x9D, 0x94, 0xE9, 0xC6, 0x83, 0x84, 0xA2, 0xAB, 0x83, 
    0x3B, 0xD3, 0x17, 0x46, 0x24, 0x9F, 0x1E, 0x81, 0x87, 0xCE, 0x61, 0x51, 0x29, 0x97, 0x63, 0x7B, 
    0x40, 0xE0, 0x80, 0xB3, 0x99, 0x02, 0x41, 0x00, 0x8A, 0x09, 0x9C, 0x0E, 0xBF, 0x3E, 0x14, 0x10, 
    0xA5, 0x22, 0x32, 0xFC, 0xB5, 0x30, 0xD6, 0x06, 0x89, 0x03, 0xCB, 0xD5, 0xA2, 0xDE, 0xD3, 0x79, 
    0x4C, 0x1F, 0x77, 0x87, 0x35, 0x17, 0x94, 0x31, 0x01, 0xFD, 0x32, 0x19, 0xE3, 0x63, 0x85, 0xCC, 
    0xBB, 0xC6, 0x4E, 0xC9, 0x31, 0x09, 0x49, 0x8A, 0x9F, 0xB7, 0xE2, 0x21, 0xF4, 0x64, 0x09, 0x1E, 
    0xDA, 0xDE, 0x5B, 0xA7, 0xA2, 0xAF, 0x4F, 0x77, 0x02, 0x41, 0x00, 0xD8, 0x0C, 0x11, 0xA0, 0xF2, 
    0x96, 0x96, 0x19, 0x6D, 0x13, 0x15, 0xDC, 0xCE, 0xF6, 0x4F, 0xE1, 0x40, 0x52, 0x69, 0x2D, 0x08, 
    0x98, 0x9F, 0xA6, 0xAF, 0xB6, 0x26, 0xA9, 0x2A, 0xB2, 0x7A, 0x1D, 0xB9, 0x80, 0x3D, 0x07, 0x1C, 
    0xE4, 0x77, 0xD6, 0xC1, 0xD0, 0x6E, 0x4B, 0x23, 0x50, 0x85, 0x31, 0x04, 0x0B, 0x17, 0xEC, 0x61, 
    0xB7, 0xE0, 0x9A, 0xA5, 0x33, 0xA3, 0x09, 0x51, 0x3D, 0x7C, 0x79, 0x02, 0x40, 0x54, 0x07, 0xD0, 
    0xE6, 0x72, 0xCD, 0xCA, 0x4C, 0x30, 0x4C, 0x4E, 0x6B, 0xF8, 0xFD, 0x22, 0x18, 0x16, 0xE8, 0xE0, 
    0xF8, 0x28, 0x36, 0x12, 0x72, 0x0A, 0x8A, 0xBA, 0x75, 0x44, 0x9C, 0xF5, 0x20, 0x06, 0xF4, 0x41, 
    0xB0, 0x5E, 0xD0, 0x2F, 0x9A, 0x2C, 0xBB, 0x95, 0x72, 0xF1, 0xD4, 0x91, 0x40, 0x47, 0xDB, 0xA3, 
    0xF6, 0x4E, 0x9A, 0xC1, 0xC5, 0x89, 0x32, 0x8B, 0x7E, 0xE7, 0x58, 0x4B, 0x8D
};

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

int main()
{
    ST_INIT_PARAM st_init_param           = {0};
    SS_CHAR sn[2][50];
    SS_CHAR *pchar_Devdescs               = NULL;
    cJSON *pjson_Devdescs                 = NULL;
    SS_UINT32 index_device                = 0;
    SS_UINT32 dev_num                     = 0;
    cJSON *pjson_Array                    = NULL;
    cJSON *pjson_Item                     = NULL;
    SS_UINT32 index                       = 0;
    SS_UINT32 ret                         = 0;
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
    //将返回的设备描述解析成JSON
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
        //getDevDesc(pjson_Devdescs, index_device);
        pjson_Array = cJSON_GetArrayItem( pjson_Devdescs, index_device );
        pjson_Item = cJSON_GetObjectItem( pjson_Array, "sn" );
        if(pjson_Item)
        {
            printf("    sn : %s\n",pjson_Item->valuestring);
            strcpy(sn[index_device],pjson_Item->valuestring);

        }
    }
    getchar();
    ret = make_license(LOCAL_MAKE, sn[0]);
    if(SS_OK == ret)
    {
        printf("Make License SHEN success\n");
    }
    printf("插入加密锁2 按下回车");
    getchar();
    
    ret = make_license(LOCAL_MAKE, sn[1]);
    if(SS_OK == ret)
    {
        printf("Make License GuoPan success\n");
    }

CLEAR:
    //反初始化函数(slm_cleanup)，与slm_init对
    cJSON_Delete(pjson_Devdescs);
    mem_free(pchar_Devdescs);
    slm_cleanup();
    printf("\npress any key exit process.\n");
    getchar();
    return 0;
}

//生成许可
SS_UINT32 make_license(SS_UINT32 choose, char *LOCK_SN_WHO)
{
    SS_UINT32 i = 21;
    SS_UINT32 ret = 0;
    SS_CHAR strDevSn[SLM_LOCK_SN_LENGTH*2+1]    = {0};
    SS_BYTE deviceSn[SLM_LOCK_SN_LENGTH]        = {0};
    SS_BYTE p7b_cert[CERT_SIZE*3]                = {0};
    SS_UINT32 p7b_size                            = CERT_SIZE*3;
    MASTER_HANDLE hmaster                        = NULL;
    SS_CHAR* d2c                                = NULL;
    SS_UINT32 d2c_len                            = 0;
    D2C_HANDLE hD2c                        = NULL;
    SS_CHAR d2c_filename[256]                   = {0};
    int offset                                  = 0;
        
    /************************************************************************/
    /*                 1、获取p7b证书用于签发许可(两种方式制作p7b)             */
    /************************************************************************/
    if( choose == LOCAL_MAKE )
    {
        ret = getUserDevP7b(p7b_cert, &p7b_size);
        getUserDevSN(strDevSn);
        hexstr_to_bytes(strDevSn, SLM_LOCK_SN_LENGTH, deviceSn);
    }
    else
    {
        ret = getP7bAndSNFromDB(strDevSn, p7b_cert, &p7b_size, LOCK_SN_WHO);
        hexstr_to_bytes(strDevSn, SLM_LOCK_SN_LENGTH, deviceSn);
    }    
    if (ret != 0)
    {
        printf("get_p7b error :0x%08X \n", ret);
        return 1;
    }
    /************************************************************************/
    /*                 2、打开控制锁并创建D2C对象            */
    /************************************************************************/
    ret = master_open(&hmaster);
    if (ret != 0)
    {
        printf("master_open error :0x%08X \n", ret);
        return 1;
    }

    // 新开发锁必须验证PIN码，否则无法使用
    // 详情见 ../common/common.h 文件中对 PIN_DEFAULT_PWD 的注释。
    ret = masterPINVerify(hmaster, PIN_DEFAULT_PWD, strlen(PIN_DEFAULT_PWD));
    if (ret != 0)
    {
        printf("masterPINVerify error :0x%08X \n", ret);
        return 1;
    }

    //获得d2c句柄
    ret = d2c_lic_new(hmaster, &hD2c, ACCOUNT_TYPE_NONE, deviceSn, sizeof(deviceSn), p7b_cert, p7b_size);
    if (ret != 0)
    {
        printf("d2c_lic_new error :0x%08X \n", ret);
        master_close(hmaster);
        return 1;
    }
    /************************************************************************/
    /*                 3、添加许可信息到d2c句柄并获取签发完成的许可  (支持添加多条许可信息)          */
    /************************************************************************/
    //写入锁的数据先后，测试时候先固定一个锁先写
    if (0 != strcmp(LOCK_SN_WHO,LOCK_SN))
    {
        offset = 65535;
    }
    ret    = add_license_d2c(hD2c, i, offset);
    //offset = MAX_USER_DATA_SIZE; 
    if (ret != 0)
    {
        printf("add_license %d error :0x%08X \n", i, ret);
        master_close(hmaster);
        return 1;
    }


    //获取签发完的许可，获取buffer传入空，可获取到许可大小
    ret = d2c_get(hD2c, NULL, 0, &d2c_len);
    if (ret != 0 && d2c_len == 0)
    {
        printf("d2c_get error :0x%08X \n", ret);
    }
    else
    {
        d2c = (SS_CHAR*)malloc(d2c_len);
        ret = d2c_get(hD2c, d2c, d2c_len, &d2c_len);
        if (ret == SS_OK)
        {
            // 写入本地文件，升级至锁内需要通过 slm_update() API接口，或者使用用户许可工具的加密锁数据升级工具。
            strcpy(d2c_filename, strDevSn);
            if( choose == LOCAL_MAKE )
                strcat(d2c_filename, "_local_license.d2c");
            else
                strcat(d2c_filename, "_remote_license.d2c");
            write_file(d2c_filename, d2c, d2c_len);
            printf("生成D2C升级包成功：\n\t%s\n" ,d2c_filename);
        }
        else
        {
            printf("d2c_get buffer error :0x%08X\n", ret);
        }
        free(d2c);
    }
    masterPINDeauth(hmaster);
    //mem_free(hmaster);
    master_close(hmaster);
    //mem_free(hmaster);
    d2c_delete(hD2c);
    return ret;
}

SS_UINT32 add_license_d2c(D2C_HANDLE hD2c,SS_UINT32 licID, int offset)
{
    SS_UINT32 ret                                        = 0;
    SS_CHAR lic_guid[D2C_GUID_LENGTH]                    = {0};
    cJSON *root                                          = NULL;
    cJSON *data                                          = NULL;
    SS_CHAR temp[MAX_USER_DATA_SIZE * 2 + 1]             = {0};
    SS_CHAR temp_signed[512]                             = {0};
    int FileLen                                          = 0;
    SLC_ULONG SignDateLen                                = 0;
    char *FileName                                       = "PE_Tetris.exe";
    char *InBuffer                                       = NULL; 
    char *InBuffer1                                      = NULL;
    char *print                                          = NULL;

    root = cJSON_CreateObject();
    // 添加许可
    cJSON_AddStringToObject(root, "op", "addlic");
    // 强制写入（标识）
    cJSON_AddBoolToObject(root, "force", cJSON_True);
    // 许可ID
    cJSON_AddNumberToObject(root, "license_id", licID);

    /************************************************************************/
    /*                  许可限制条件                                        */
    /************************************************************************/
    // 使用次数（1000次）
    cJSON_AddStringToObject(root, "counter", "=1000");
    /************************************************************************/
    /*                        数据区                                        */
    /************************************************************************/
    // 公开区
    InBuffer = (char *)malloc(sizeof(char) * MAX_USER_DATA_SIZE);
    FileLen = read_files(FileName, InBuffer, offset);
    data = cJSON_CreateObject();
    bytes_to_hexstr(InBuffer, FileLen, temp);
    cJSON_AddStringToObject(data, "data", temp);
    cJSON_AddNumberToObject(data, "offset", 0);
    cJSON_AddNumberToObject(data, "resize",FileLen);
    cJSON_AddItemToObject(root, "pub", data);
   
    // 只读区
    InBuffer1 = (char *)malloc(sizeof(char) * TEMP_BUFFER_SIZE);
    ret = SlcRsaSign(SLC_PAD_MODE_PKCS_1_V1_5,SLC_HASH_ALGO_SHA256, PriKey, (unsigned char *) InBuffer,FileLen ,(unsigned char *) InBuffer1,TEMP_BUFFER_SIZE,&SignDateLen);
    ret = SlcRsaVerify(SLC_PAD_MODE_PKCS_1_V1_5, SLC_HASH_ALGO_SHA256, PubKey, (unsigned char *) InBuffer, FileLen , (unsigned char *) InBuffer1, SignDateLen);
    if (ret == SS_OK)
    {
        printf("ok\n");
    }
    bytes_to_hexstr(InBuffer1, SignDateLen, temp_signed);
    data = cJSON_CreateObject();
    cJSON_AddStringToObject(data, "data", temp_signed);
    cJSON_AddNumberToObject(data, "offset", 0);
    cJSON_AddNumberToObject(data, "resize", SignDateLen);
    cJSON_AddItemToObject(root, "rom", data);
    print = cJSON_Print(root);
    printf("%s\n", print);
    mem_free(print);
    print = cJSON_PrintUnformatted(root);
    ret = d2c_add_lic(hD2c, print, "add license sample", lic_guid);
    mem_free(print);
    cJSON_Delete(root);
    mem_free(InBuffer);
    mem_free(InBuffer1);
    return ret;
}
SS_UINT32 getP7bAndSNFromDB(SS_CHAR *pSN, SS_BYTE *pCertP7b, SS_UINT32 *pCertLen, char *LOCK_SN_WHO)
{
    SS_UINT32 ret = 0;
    SS_CHAR SNTem[56] = {0};
    SS_BYTE *pDecodeP7bTemp = NULL;

    //获取加密锁信息的数据库中的信息，可向深思销售申请所需信息(LOCK_SN 和 P7BDATA)
    pDecodeP7bTemp = base64Decode(P7BDATA, pCertLen, 0);
    memcpy(pCertP7b, pDecodeP7bTemp, *pCertLen) ;
    base64Free(pDecodeP7bTemp);

    // 验证证书链的是否正确
    ss_x509_init();
    ret = ss_verify_p7b(pCertP7b, *pCertLen);
    ss_x509_cleanup();
    if (ret)
    {
        printf("ss_verify_p7b error :0x%08X\n", ret);
        return 1;
    }
    strcpy(SNTem, LOCK_SN_WHO);
    memcpy(pSN, SNTem, strlen(SNTem));    
    return 0;
}
SS_UINT32 read_files(char *FileName, char *InBuffer, int offset)
{

    int                                     Len = 0;
    FILE                                    *fp;
    int                                     i = 0;
    int                                     j = 0;
    if ( ( fp = fopen(FileName,"rb")) == NULL)
    {
        printf("open file error!\n");
        return 0;
    }
    if (offset)
    { 
        fseek(fp, 0, SEEK_END);
        i   = ftell(fp);
        fseek(fp, offset, SEEK_SET);
        j   = ftell(fp);
        Len = i - j;

    }
    else
    {
        Len = MAX_USER_DATA_SIZE - 1;
        rewind(fp);
    }
    fread(InBuffer, Len, 1, fp);
    fclose(fp);
    return Len;
}