/*
  Copyright (c) 2009 Dave Gamble
 
  Permission is hereby granted, free of charge, to any person obtaining a copy
  of this software and associated documentation files (the "Software"), to deal
  in the Software without restriction, including without limitation the rights
  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
  copies of the Software, and to permit persons to whom the Software is
  furnished to do so, subject to the following conditions:
 
  The above copyright notice and this permission notice shall be included in
  all copies or substantial portions of the Software.
 
  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
  THE SOFTWARE.
*/

#include <stdio.h>
#include <stdlib.h>
#include "cJSON.h"

/* Parse text to JSON, then render back to text, and print! */
void doit(char *text)
{
    char *out;cJSON *json;
    
    json=cJSON_Parse(text);
    if (!json) {printf("Error before: [%s]\n",cJSON_GetErrorPtr());}
    else
    {
        out=cJSON_Print(json);
        cJSON_Delete(json);
        printf("%s\n",out);
        free(out);
    }
}

/* Read a file, parse, render back, etc. */
void dofile(char *filename)
{
    FILE *f;long len;char *data;
    
    f=fopen(filename,"rb");fseek(f,0,SEEK_END);len=ftell(f);fseek(f,0,SEEK_SET);
    data=(char*)malloc(len+1);fread(data,1,len,f);fclose(f);
    doit(data);
    free(data);
}

/* Used by some code below as an example datatype. */
struct record {const char *precision;double lat,lon;const char *address,*city,*state,*zip,*country; };

/* Create a bunch of objects as demonstration. */
void create_objects()
{
    cJSON *root,*fmt,*img,*thm,*fld;char *out;int i;    /* declare a few. */
    /* Our "days of the week" array: */
    const char *strings[7]={"Sunday","Monday","Tuesday","Wednesday","Thursday","Friday","Saturday"};
    /* Our matrix: */
    int numbers[3][3]={{0,-1,0},{1,0,0},{0,0,1}};
    /* Our "gallery" item: */
    int ids[4]={116,943,234,38793};
    /* Our array of "records": */
    struct record fields[2]={
        {"zip",37.7668,-1.223959e+2,"","SAN FRANCISCO","CA","94107","US"},
        {"zip",37.371991,-1.22026e+2,"","SUNNYVALE","CA","94085","US"}};

    /* Here we construct some JSON standards, from the JSON site. */
    
    /* Our "Video" datatype: */
    root=cJSON_CreateObject();    
    cJSON_AddItemToObject(root, "name", cJSON_CreateString("Jack (\"Bee\") Nimble"));
    cJSON_AddItemToObject(root, "format", fmt=cJSON_CreateObject());
    cJSON_AddStringToObject(fmt,"type",        "rect");
    cJSON_AddNumberToObject(fmt,"width",        1920);
    cJSON_AddNumberToObject(fmt,"height",        1080);
    cJSON_AddFalseToObject (fmt,"interlace");
    cJSON_AddNumberToObject(fmt,"frame rate",    24);
    
    out=cJSON_Print(root);    cJSON_Delete(root);    printf("%s\n",out);    free(out);    /* Print to text, Delete the cJSON, print it, release the string. */

    /* Our "days of the week" array: */
    root=cJSON_CreateStringArray(strings,7);

    out=cJSON_Print(root);    cJSON_Delete(root);    printf("%s\n",out);    free(out);

    /* Our matrix: */
    root=cJSON_CreateArray();
    for (i=0;i<3;i++) cJSON_AddItemToArray(root,cJSON_CreateIntArray(numbers[i],3));

/*    cJSON_ReplaceItemInArray(root,1,cJSON_CreateString("Replacement")); */
    
    out=cJSON_Print(root);    cJSON_Delete(root);    printf("%s\n",out);    free(out);


    /* Our "gallery" item: */
    root=cJSON_CreateObject();
    cJSON_AddItemToObject(root, "Image", img=cJSON_CreateObject());
    cJSON_AddNumberToObject(img,"Width",800);
    cJSON_AddNumberToObject(img,"Height",600);
    cJSON_AddStringToObject(img,"Title","View from 15th Floor");
    cJSON_AddItemToObject(img, "Thumbnail", thm=cJSON_CreateObject());
    cJSON_AddStringToObject(thm, "Url", "http:/*www.example.com/image/481989943");
    cJSON_AddNumberToObject(thm,"Height",125);
    cJSON_AddStringToObject(thm,"Width","100");
    cJSON_AddItemToObject(img,"IDs", cJSON_CreateIntArray(ids,4));

    out=cJSON_Print(root);    cJSON_Delete(root);    printf("%s\n",out);    free(out);

    /* Our array of "records": */

    root=cJSON_CreateArray();
    for (i=0;i<2;i++)
    {
        cJSON_AddItemToArray(root,fld=cJSON_CreateObject());
        cJSON_AddStringToObject(fld, "precision", fields[i].precision);
        cJSON_AddNumberToObject(fld, "Latitude", fields[i].lat);
        cJSON_AddNumberToObject(fld, "Longitude", fields[i].lon);
        cJSON_AddStringToObject(fld, "Address", fields[i].address);
        cJSON_AddStringToObject(fld, "City", fields[i].city);
        cJSON_AddStringToObject(fld, "State", fields[i].state);
        cJSON_AddStringToObject(fld, "Zip", fields[i].zip);
        cJSON_AddStringToObject(fld, "Country", fields[i].country);
    }
    
/*    cJSON_ReplaceItemInObject(cJSON_GetArrayItem(root,1),"City",cJSON_CreateIntArray(ids,4)); */
    
    out=cJSON_Print(root);    cJSON_Delete(root);    printf("%s\n",out);    free(out);

}

int main (int argc, const char * argv[]) 
{
    /*构造*/
    cJSON* cjson_test = NULL;
    cJSON* cjson_address = NULL;
    cJSON* cjson_skill = NULL;
    char* str = NULL;
    /*解析*/
    cJSON* cjson_name = NULL;
    cJSON* cjson_age = NULL;
    cJSON* cjson_weight = NULL;
    cJSON* cjson_address1 = NULL;
    cJSON* cjson_address_country = NULL;
    cJSON* cjson_address_zipcode = NULL;
    cJSON* cjson_skill1 = NULL;
    cJSON* cjson_student = NULL;
    int    skill_array_size = 0, i = 0;
    cJSON* cjson_skill_item = NULL;
    

    /* 创建一个JSON数据对象(链表头结点) */
    cjson_test = cJSON_CreateObject();

    /* 添加一条字符串类型的JSON数据(添加一个链表节点) */
    cJSON_AddStringToObject(cjson_test, "name", "mculover666");

    /* 添加一条整数类型的JSON数据(添加一个链表节点) */
    cJSON_AddNumberToObject(cjson_test, "age", 22);

    /* 添加一条浮点类型的JSON数据(添加一个链表节点) */
    cJSON_AddNumberToObject(cjson_test, "weight", 55.5);

    /* 添加一个嵌套的JSON数据（添加一个链表节点） */
    cjson_address = cJSON_CreateObject();
    cJSON_AddStringToObject(cjson_address, "country", "China");
    cJSON_AddNumberToObject(cjson_address, "zip-code", 111111);
    cJSON_AddItemToObject(cjson_test, "address", cjson_address);

    /* 添加一个数组类型的JSON数据(添加一个链表节点) */
    cjson_skill = cJSON_CreateArray();
    cJSON_AddItemToArray(cjson_skill, cJSON_CreateString( "C" ));
    cJSON_AddItemToArray(cjson_skill, cJSON_CreateString( "Java" ));
    cJSON_AddItemToArray(cjson_skill, cJSON_CreateString( "Python" ));
    cJSON_AddItemToObject(cjson_test, "skill", cjson_skill);

    /* 添加一个值为 False 的布尔类型的JSON数据(添加一个链表节点) */
    cJSON_AddFalseToObject(cjson_test, "student");

    /* 打印JSON对象(整条链表)的所有数据 */
    str = cJSON_Print(cjson_test);
    printf("%s\n", str);

    /* 解析整段json*/
    /*pu通键值对部分*/
    cjson_name = cJSON_GetObjectItem(cjson_test, "name");
    cjson_age = cJSON_GetObjectItem(cjson_test, "age");
    cjson_weight = cJSON_GetObjectItem(cjson_test, "weight");

    printf("name: %s\n", cjson_name->valuestring);
    printf("age:%d\n", cjson_age->valueint);
    printf("weight:%.1f\n", cjson_weight->valuedouble);
    /*嵌套的json部分*/
    cjson_address1 = cJSON_GetObjectItem(cjson_test, "address");
    cjson_address_country = cJSON_GetObjectItem(cjson_address1, "country");
    cjson_address_zipcode = cJSON_GetObjectItem(cjson_address1, "zip-code");

    /*json 数组部分*/
    cjson_skill1 = cJSON_GetObjectItem(cjson_test, "skill");
    skill_array_size = cJSON_GetArraySize(cjson_skill1);
    printf("skill:[");
    for(i = 0; i < skill_array_size; i++)
    {
        cjson_skill_item = cJSON_GetArrayItem(cjson_skill1, i);
        printf("%s,", cjson_skill_item->valuestring);
    }
    printf("\b]\n");
     /* 解析布尔型数据 */
    cjson_student = cJSON_GetObjectItem(cjson_test, "student");
    if(cjson_student->valueint == 0)
    {
        printf("student: false\n");
    }
    else
    {
        printf("student:error\n");
    }
    getchar();
    return 0;

}
    /* a bunch of json: */
//    char text1[]="{\n\"name\": \"Jack (\\\"Bee\\\") Nimble\", \n\"format\": {\"type\":       \"rect\", \n\"width\":      1920, \n\"height\":     1080, \n\"interlace\":  false,\"frame rate\": 24\n}\n}";    
//    char text2[]="[\"Sunday\", \"Monday\", \"Tuesday\", \"Wednesday\", \"Thursday\", \"Friday\", \"Saturday\"]";
//    char text3[]="[\n    [0, -1, 0],\n    [1, 0, 0],\n    [0, 0, 1]\n    ]\n";
//    char text4[]="{\n        \"Image\": {\n            \"Width\":  800,\n            \"Height\": 600,\n            \"Title\":  \"View from 15th Floor\",\n            \"Thumbnail\": {\n                \"Url\":    \"http:/*www.example.com/image/481989943\",\n                \"Height\": 125,\n                \"Width\":  \"100\"\n            },\n            \"IDs\": [116, 943, 234, 38793]\n        }\n    }";
//    char text5[]="[\n     {\n     \"precision\": \"zip\",\n     \"Latitude\":  37.7668,\n     \"Longitude\": -122.3959,\n     \"Address\":   \"\",\n     \"City\":      \"SAN FRANCISCO\",\n     \"State\":     \"CA\",\n     \"Zip\":       \"94107\",\n     \"Country\":   \"US\"\n     },\n     {\n     \"precision\": \"zip\",\n     \"Latitude\":  37.371991,\n     \"Longitude\": -122.026020,\n     \"Address\":   \"\",\n     \"City\":      \"SUNNYVALE\",\n     \"State\":     \"CA\",\n     \"Zip\":       \"94085\",\n     \"Country\":   \"US\"\n     }\n     ]";
//
//    /* Process each json textblock by parsing, then rebuilding: */
//    doit(text1);
//    doit(text2);    
//    doit(text3);
//    doit(text4);
//    doit(text5);
//
//    /* Parse standard testfiles: */
///*    dofile("../../tests/test1"); */
///*    dofile("../../tests/test2"); */
///*    dofile("../../tests/test3"); */
///*    dofile("../../tests/test4"); */
///*    dofile("../../tests/test5"); */
//
//    /* Now some samplecode for building objects concisely: */
//    create_objects();
//    getchar();