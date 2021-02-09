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
#define LOCAL_MAKE	1			//	����������ʹ�ü��������кź�֤��ϳ�֤����
#define REMOTE_MAKE 2			//	����������ʹ�����ݿ������кź�֤����

#define FIND_DEVICE_INDEX          0                       // ��ȡ���ҵ��ĵ�һ�Ѽ�����
#define FIND_DEVICE_DEVELOPER_ID   "0300000000000009"      // ���Ҽ������Ŀ�����ID�����ڹ��˷Ǳ����̵ľ���5��������ע�⣺������ʹ��ǰ��Ҫ�޸Ĵ�ֵ����ͨ�� Virbox ��������վ��ȡ���������޷��ҵ�������

#ifdef __cplusplus
extern "C" {
#endif

/*
*	name:	getUserDevSN	��ȡ��ǰ�û��������к�
*	param1:	[out]deviceSN	��ǰ�û��������к�
*	return:	error(1),ok(0)
*/
int getUserDevSN(char* deviceSN);

/*
*	name:	getUserDevP7b	��ȡ�û�����p7b֤��
*	param1:	[out]pCertP7b	p7b����buffer
*	param2:	[out]pCertLen	p7b����
*	return: error(1),ok(0)
*/
int getUserDevP7b(unsigned char *pCertP7b, unsigned int *pCertLen);

int masterPINVerify(MASTER_HANDLE hD2CHandle, char *pPIN, int nPinLen);
int masterPINDeauth(MASTER_HANDLE hD2CHandle);

#ifdef __cplusplus
}
#endif

#endif // _H_SS_HELP_H_

