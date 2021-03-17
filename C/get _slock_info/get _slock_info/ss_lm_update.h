
#ifndef _SS_LM_UPDATE_H
#define _SS_LM_UPDATE_H

#include "ss_define.h"

#ifdef __cplusplus
extern "C"{
#endif
/*!
*   @brief      将D2C包进行升级，D2C包由d2cAPI生成
*   @param[in]  d2c_pkg     d2c文件数据
*   @param[out] error_msg   错误信息，需要调用slm_free释放
*   @return     成功返回SS_OK，失败返回错误码
*   @remarks:   D2C包内可能包含多条数据，因此返回的错误码信息也是json数组结构，需要分别解析
*   @code
*     [
*       {"pkg_order":1, "pkg_desc":"package decription.", "status": 0},
*       {"pkg_order":2, "pkg_desc":"package decription.", "status": 0}
*     ]
*   @endcode
*/
SS_UINT32 SSAPI slm_update(char* d2c_pkg, char** error_msg);

/*!
*   @brief      将D2C包进行升级，需指定加密锁唯一序列号，D2C包由d2cAPI生成
*   @param[in]  lock_sn     锁号（唯一序列号,十六进制字符串）
*   @param[in]  d2c_pkg     d2c文件数据
*   @param[out] error_msg   错误信息，不使用需要调用slm_free释放
*   @return     成功返回SS_OK，失败返回错误码
*   @remarks:  D2C包内可能包含多条数据，因此返回的错误码信息也是json数组结构，需要分别解析
*   @code
*     [
*       {"pkg_order":1, "pkg_desc":"package decription.", "status": 0},
*       {"pkg_order":2, "pkg_desc":"package decription.", "status": 0}
*     ]
*   @endcode
*   @see    slm_update
*/
SS_UINT32 SSAPI slm_update_ex(SS_BYTE* lock_sn, char* d2c_pkg, char** error_msg);

/*!
*   @brief      锁内短码升级，需指定加密锁唯一序列号，需锁内可执行算法配合（仅支持硬件锁）
*   @param[in]  lock_sn     锁号（唯一序列号,十六进制字符串）
*   @param[in]  inside_file 锁内短码文件名
*   @return     成功返回SS_OK，失败返回错误码
*   @remarks    VirboxLM 提供的通过短码激活功能在硬件锁内生成一条 Virbox许可，该功能需要通过可执行算法的配合，简单的步骤描述为：
*               第一步、在锁内下载支持短码激活的使能文件slac_enable.evd，文件内容任意，大小最好不超过256字节；
*               第二步、编写锁内可执行算法，调用深思提供的h5ses.lib和h5ses_lm.lib库，实现短码转换的过程；
*               第三步、通过执行锁内可执行算法，即调用 slm_execute_static ， 并传入短码内容，促使锁内可执行算法在锁内将短码转化为
*                       硬件锁可识别的中间文件（文件名开发者设定）。
*               第四步、通过调用slm_d2c_update_inside 接口，将锁内生成的中间文件转化为正式的 Virbox许可。
*
*               特别说明：短码激活方案使用较为复杂，需要写锁内代码的技术要求，若开发者需要使用此功能，可联系深思技术服务进行支持。
*   @code
*       {
*           SS_UINT32 status = SS_OK;
*           SS_BYTE   data_buf[1024] = { 0 };
*           SS_UINT32 data_size = 0;
*           char *sn = "9733c80100070205106100030015000c";
*
*           memcpy(data_buf, "1234567890", 10);
*
*           status = slm_execute_static(slm_handle, "test.evx", NULL, 0, data_buf, 10, &data_size);
*           if(status != SS_OK)
*           {
*               // todo: deal error code
*           }
*
*           status = slm_d2c_update_inside(sn, "test.evd");
*           if(status != SS_OK)
*           {
*               // todo: deal error code
*           }
*       }
*   @endcode
*   @see    slm_execute_static
*/
SS_UINT32 SSAPI slm_d2c_update_inside(
    IN char*     lock_sn,
    IN char*        inside_file
    );

/*!
*   @brief      解析D2C包的信息
*	@param[in]	d2c_stream	d2c数据流
*	@param[out]	d2c_buffer  接收包描述的缓冲流
*	@param[in]	max_buf_len d2c_buffer的大小
*	@param[out]	out_buf_len 获取的包描述的大小
*	@return		成功返回SS_OK，失败返回相应的错误码
*	@remark     此接口用于解析D2C数据流的内容，主要获取描述信息，包内容，包数量等。
*   @code
*       {
*           "serial": "0102030405060708", 
*           "pkg":[
*                   {"pkg_order": 1, "pkg_desc":"add_pkg"},
*                   {"pkg_order":2, "pkg_desc": "delete_pkg"}
*                 ]
*       }
*   @endcode
*/
SS_UINT32 SSAPI package_parse( const char* d2c_stream, char* d2c_buffer, int max_buf_len, int* out_buf_len);

#ifdef __cplusplus
}
#endif


#endif
