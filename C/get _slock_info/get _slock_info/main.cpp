/***************************************************************************************
Copyright (c) Beijing Sensesheild Technology ctd 
Creat: tac - shenyy
date: 2021/3/17
readme: ��������ʾ������control api ������ȡ����� id ����ȡ��������Ϣ
1 �� ipc
2 ��ñ������������
3 ������תΪjson,�� getarrayItem ��õ���������Json�� �ڽ���תΪ�ַ���
4 ��������� �� ipc ������ؽӿڻ�ȡ �����Ϣ����ӡ
�����
1 ��ͬһ�����Դͬһ�����̵ĵ�ͬһ���͵���ɣ���Ϣ��һ���ȡ
*/

#include "ss_lm_control.h"
#include "ss_define.h"
#include "ss_error.h"
#include "cJSON.h"
#include <stdio.h>
#include <string.h>
#include <stdlib.h>


int main(int argc, char* argv[])
{
    SS_UINT32 ret   =                           0;
    void *ipc       =                        NULL;
    char *desc      =                        NULL;
    char *dev_desc  =                        NULL;
    cJSON *root     =                        NULL;
    cJSON *array_items =                     NULL;
    cJSON *object_item =                     NULL;
    char * slock_type =                      NULL;
    char *dev_id =                           NULL;
    char *result =                           NULL;
    SS_UINT32 lens  =                           0;
    cJSON *id_root  =                        NULL;
    int i           =                           0;
    slm_ctrl_client_open(&ipc);
    slm_ctrl_get_offline_desc(ipc,&desc);
    printf("%s\n",desc);
    root = cJSON_Parse(desc);
    lens = cJSON_GetArraySize(root);

    for(i = 0; i < lens; i++)
    {
        array_items = cJSON_GetArrayItem(root,i);
        if(!array_items)
            continue;

        object_item = cJSON_GetObjectItem(array_items,"developer_id");
        if(!object_item)
            continue;
        dev_id = object_item->valuestring;
        printf("dev_id : %s\n",dev_id);

        object_item = cJSON_GetObjectItem(array_items,"slock_type");
        if(!object_item)
            continue;
        slock_type = object_item->valuestring;
        printf("slock_type : %s\n",slock_type);

        dev_desc = cJSON_PrintUnformatted(array_items);
        printf("%s\n",dev_desc);
        //ȡ�������������в�ѯ
        ret      = slm_ctrl_get_license_id(ipc,JSON,dev_desc,&result);
        printf("%s\n",result);
        ret      = slm_ctrl_read_brief_license_context(ipc,JSON,dev_desc,&result);
        printf("%s\n",result);
    }
    slm_ctrl_free(dev_desc);
    dev_desc = NULL;
    slm_ctrl_client_close(ipc);
    getchar();
    return 0;
}