#ifndef _H_SS_HELP_H_
#define _H_SS_HELP_H_

#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#include "d2c.h"
#include "ss_error.h"
#include "ss_define.h"
#include "ss_lm_control.h"

#include "common.h"
#include "cJSON.h"
#include "ss_x509.h"

#pragma warning(disable : 4996)


#define CERT_SIZE 2048
#define LOCAL_MAKE	1			//	本地有锁，使用加密锁序列号和证书合成证书链
#define REMOTE_MAKE 2			//	本地无锁，使用数据库中序列号和证书链

#define FIND_DEVICE_INDEX          0                       // 获取查找到的第一把加密锁
#define FIND_DEVICE_DEVELOPER_ID   "0300000000000009"      // 查找加密锁的开发者ID，用于过滤非本厂商的精锐5加密锁。注意：开发者使用前需要修改此值（可通过 Virbox 开发者网站获取），否则无法找到加密锁

#ifdef __cplusplus
extern "C" {
#endif

/*
*	name:	getUserDevSN	获取当前用户锁的序列号
*	param1:	[out]deviceSN	当前用户锁的序列号
*	return:	error(1),ok(0)
*/
int getUserDevSN(char* deviceSN);

/*
*	name:	getUserDevP7b	获取用户锁的p7b证书
*	param1:	[out]pCertP7b	p7b接收buffer
*	param2:	[out]pCertLen	p7b长度
*	return: error(1),ok(0)
*/
int getUserDevP7b(unsigned char *pCertP7b, unsigned int *pCertLen);

int masterPINVerify(MASTER_HANDLE hD2CHandle, char *pPIN, int nPinLen);
int masterPINDeauth(MASTER_HANDLE hD2CHandle);

#ifdef __cplusplus
}
#endif

#endif // _H_SS_HELP_H_

