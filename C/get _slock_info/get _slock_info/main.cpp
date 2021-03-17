/***************************************************************************************
Copyright (c) Beijing Sensesheild Technology ctd 
Creat: tac - shenyy
date: 2021/3/17
readme: 本代码演示了利用control api 单独获取软许可 id 、获取软件许可信息
1 打开 ipc
2 获得本地软许可描述
3 将描述转为json,用 getarrayItem 获得单个描述的Json， 在将其转为字符串
4 用许可描述 和 ipc 调用相关接口获取 许可信息并打印
结果：
1 ：同一许可来源同一开发商的的同一类型的许可，信息能一起获取
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
        //取出单个描述进行查询
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