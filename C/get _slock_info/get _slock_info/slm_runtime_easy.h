#ifndef _slm_runtime_easy_H_
#define _slm_runtime_easy_H_

#include "ss_lm_runtime.h"

#ifdef __cplusplus
extern "C" {
#endif

    /*!
    *   @brief   slm_get_last_error
    *   @return  返回最后的许可错误值
    */
    SS_UINT32 SSAPI slm_get_last_error(void);


    /*!
    *   @brief   app_msg_get_last_status
    *   @return  返回最后的系统消息APP状态值，包括反黑引擎和锁插拔消息和SS停止服务的通知
    */
    SS_UINT32 SSAPI app_msg_get_last_status(void);


    /*!
    *   @brief   app_msg_get_last_info
    *   @return  返回最后的系统消息APP状态描述字符串,不需要释放
    */
    const SS_CHAR* SSAPI app_msg_get_last_info(void);


    /*!
    *   @brief     SS反黑引擎初始化，验证开发商合法身份
    *   @param[in] szdeveloper_password     开发商密码HEX字符串
    *   @return    成功返回SS_OK，失败返回相应的错误码
    *   @see       slm_init_easy slm_cleanup_easy slm_is_debug_easy
    *   @remark    为保护APP检测调试器做准备
    */
    SS_UINT32 SSAPI slm_init_easy(const char *szdeveloper_password);

    /*!
    *   @brief     SS反黑引擎反初始化，断开跟SS连接，并且关闭反黑引擎的监视
    *   @return    成功返回SS_OK，失败返回相应的错误码
    *   @see       slm_init_easy slm_cleanup_easy slm_is_debug_easy
    */
    SS_UINT32 SSAPI slm_cleanup_easy(void);

    /*!
    *   @brief     检测APP是否被调试
    *   @return    成功返回SS_OK，失败返回相应的错误码
    *   @see       slm_init_easy slm_cleanup_easy slm_is_debug_easy
    */
    SS_UINT32 SSAPI slm_is_debug_easy(void);

    /*!
    *   @brief     安全登录许可,用STRUCT传递参数,并且检查时间（是否到期或者是否早于开始时间）、次数、并发数是否归零，
    *              如果有计数器，则永久性减少对应的计数器，对于网络锁则临时增加网络并发计数器。
    *   @param[in]  license_param     登录许可描述参数串，可以用来描述licenseid，或者指定特性等，参数格式请从ss_lm_runtime.h中查看
    *   @param[in]  param_format      许可描述字符串类型，目前仅支持STRUCT格式
    *   @return     SLM_HANDLE_INDEX    返回登录之后许可句柄index值,范围在1-256之间。
    *   @return     SS_UINT32错误码,非0代表失败
    *   @remark
    *   - 错误码通过slm_get_last_error 得到
    *   - slm_login函数详细说明：
    *       -# 会自动查找跨锁查找许可句柄。
    *       -# 在runtime库里面分配管理内存与进程线程信息。
    *       -# 对与调用者需要定期监控会话进程，如果进程死锁或者崩溃，自己释放对应的内存和其它资源。
    *       -# LM库属于客户定制自动编译，包含RSA 公钥、认证设备ID、开发商编号等一切认证手段。
    *       -# LM后续操作必须都要login之后才能有权限操作 比如读写、加解密等操作。
    *   - 功能参数说明，采用STRUCT参数登录，确保安全，参考相关SLM_LOGIN_PARAM结构
    *   @see slm_logout SS_UINT32 INFO_FORMAT_TYPE ST_LOGIN_TEMPLET
    */
    SLM_HANDLE_INDEX SSAPI slm_login_easy(
        IN  const ST_LOGIN_PARAM     *license_param,
        IN  INFO_FORMAT_TYPE    param_format
        );

    /*!
    *   @brief     许可登出，并且释放许可句柄等资源
    *   @param[in] slm_handle    许可句柄值
    *   @return    成功返回SS_OK，失败返回相应的错误码
    *   @see       slm_login
    */
    SS_UINT32 SSAPI slm_logout_easy(SLM_HANDLE_INDEX slm_handle);

    /*!
    *   @brief      slm_find_license_easy 查找所有许可
    *   @param[in]  license_id      许可ID
    *   @param[in]  format          参数格式
    *   @return     许可描述信息的字符串，格式由format指定，不再使用时调用slm_free_easy释放  
    *   @remark     如果成功，需要调用slm_free 释放license_desc
    */
    SS_CHAR* SSAPI slm_find_license_easy(
        IN  SS_UINT32           license_id,
        IN  INFO_FORMAT_TYPE    format
        );

    /*!
    *   @brief     slm_get_info_easy
    *   @param[in] slm_handle     许可句柄值
    *   @param[in] type           信息类型
    *   @param[in] format         信息格式可使用json
    *   @return    返回info结果,需要slm_free_easy释放  
    */
    SS_CHAR*  SSAPI slm_get_info_easy(
        IN SLM_HANDLE_INDEX slm_handle,
        IN INFO_TYPE        type,
        IN INFO_FORMAT_TYPE format
        );

    /*!
    *   @brief     保持登录会话心跳，避免变为“僵尸句柄”。
    *   @brief     僵尸句柄：如果进程死循环或者崩溃，slm_handle不会自动被释放，造成slm_runtime库中内存和资源被
    *   @param[in] slm_handle     许可句柄值
    *   @return    成功返回SS_OK，失败返回相应的错误码
    *   @see       slm_login_easy  
    */
    SS_UINT32 SSAPI slm_keep_alive_easy(SLM_HANDLE_INDEX slm_handle);

    /*!
    *   @brief     检查模块
    *   @param[in] slm_handle     许可句柄值
    *   @param[in] module_id      模块ID，范围由（1 ~ 64）
    *   @return    模块存在返回SS_OK，不存在返回SS_ERROR_LICENSE_MODULE_NOT_EXISTS, 否则返回其它错误码
    */
    SS_UINT32 SSAPI slm_check_module_easy(IN SLM_HANDLE_INDEX slm_handle, IN SS_UINT32 module_id);

    /*!
    *   @brief      许可加密，相同的许可ID相同的开发商加密结果相同
    *   @param[in]  slm_handle     许可句柄值
    *   @param[in]  inbuffer       加密输入缓冲区,需要16字节对齐
    *   @param[out] outbuffer      加密输出缓冲区,需要16字节对齐
    *   @param[in]  len            加密缓冲区大小，为16字节的整数倍。
    *   @return     成功返回SS_OK，失败返回相应的错误码
    *   @see        slm_decrypt_easy slm_login_easy
    */
    SS_UINT32 SSAPI slm_encrypt_easy(
        IN  SLM_HANDLE_INDEX    slm_handle,
        IN  SS_BYTE            *inbuffer,
        OUT SS_BYTE            *outbuffer,
        IN  SS_UINT32           len
        );

    /*!
    *   @brief      许可解密，相同的许可ID相同的开发商加密结果相同
    *   @param[in]  slm_handle     许可句柄值
    *   @param[in]  inbuffer       解密输入缓冲区,需要16字节对齐
    *   @param[out] outbuffer      解密输出缓冲区,需要16字节对齐
    *   @param[in]  len            加密缓冲区大小，为16字节的整数倍。
    *   @return     成功返回SS_OK，失败返回相应的错误码
    *   @see        slm_encrypt_easy slm_login_easy
    */
    SS_UINT32 SSAPI slm_decrypt_easy(
        IN  SLM_HANDLE_INDEX    slm_handle,
        IN  SS_BYTE            *inbuffer,
        OUT SS_BYTE            *outbuffer,
        IN  SS_UINT32           len
        );

    /*!
    *   @brief      获得许可的数据区大小
    *   @param[in]  slm_handle     许可句柄
    *   @param[in]  type           数据区类型，类型定义见 LIC_USER_DATA_TYPE
    *   @return     返回数据区的大小  
    *   @see        slm_user_data_read_easy slm_user_data_write_easy
    *   @remark     错误值通过slm_get_last_error获得
    */
    SS_UINT32 SSAPI slm_user_data_getsize_easy(
        IN SLM_HANDLE_INDEX     slm_handle,
        IN LIC_USER_DATA_TYPE   type
        );

    /*!
    *   @brief      读许可数据，可以读取RAW和ROM和公开区
    *   @param[in]  slm_handle      许可句柄值
    *   @param[in]  type            内存类型，有ROM(只读区)和RAW(读写区)和PUB(公开区)
    *   @param[out] readbuff        读取到的数据
    *   @param[in]  offset          读偏移
    *   @param[in]  len             读取数据大小 
    *   @return     成功返回SS_OK，失败返回相应的错误码
    *   @see        slm_user_data_getsize_easy slm_user_data_write_easy
    */
    SS_UINT32 SSAPI slm_user_data_read_easy(
        IN  SLM_HANDLE_INDEX    slm_handle,
        IN  LIC_USER_DATA_TYPE  type,
        OUT SS_BYTE             *readbuff,
        IN  SS_UINT32           offset,
        IN  SS_UINT32           len
        );

    /*!
    *   @brief     写许可的读写数据区 ,数据区操作之前请先确认内存区的大小，可以使用slm_user_data_getsize_easy获得
    *   @param[in] slm_handle       许可句柄值
    *   @param[in] writebuf         写缓存区
    *   @param[in] offset           写偏移
    *   @param[in] len              写长度（长度最大，见SLM_MAX_WRITE_SIZE)
    *   @return    成功返回SS_OK，失败返回相应的错误码
    *   @see       slm_user_data_getsize_easy slm_user_data_read_easy
    */
    SS_UINT32 SSAPI slm_user_data_write_easy(
        IN  SLM_HANDLE_INDEX    slm_handle,
        IN  SS_BYTE            *writebuf,
        IN  SS_UINT32           offset,
        IN  SS_UINT32           len
        );

    /*!
    *   @brief      获得指定许可的公开区数据区大小，需要登录0号许可
    *   @param[in]  slm_handle    0号许可句柄值
    *   @param[in]  license_id    指定许可ID
    *   @param[out] pmem_size     数据区大小（填地址）
    *   @return     返回数据区的大小
    *   @remark     错误值通过slm_get_last_error获得
    */
    SS_UINT32 SSAPI slm_pub_data_getsize_easy(
        IN  SLM_HANDLE_INDEX    slm_handle,
        IN  SS_UINT32           license_id
        );

    /*!
    *   @brief      读取许可公开区，需要登录0号许可
    *   @param[in]  slm_handle   0号许可句柄值
    *   @param[in]  license_id   指定许可ID
    *   @param[out] readbuf      读取缓存区
    *   @param[in]  offset       数据偏移
    *   @param[in]  len          缓存区大小
    *   @return     成功返回SS_OK，失败返回相应的错误码
    *   @remark     当用户不希望通过登录相应的许可，从而获取到公开区的时候，用户可用通过调用此接口进行获取，前提是需要登录0号许可
    *              ，然后将0号许可句柄和要查询公开区的许可ID传入参数，获取该许可的公开区数据。
    *   @code
    *      SS_UINT32 status = 0;
    *      SLM_HANDLE_INDEX handle_zero = 0;
    *      ST_LOGIN_PARAM login = {0};
    *      SS_BYTE   pub_data[2048] = { 0 };
    *      SS_UINT32 pub_size = 0;
    *      SS_UINT32 lic_find = 1; // 要找1号许可公开区
    *      
    *      login.size = sizeof(ST_LOGIN_PARAM);
    *      login.license_id = 0;
    *      
    *      handle_zero = slm_login_easy(&login, STRUCT);
    *      
    *      pub_size = slm_pub_data_getsize_easy(handle_zero, lic_find);
    *      status = pslm_get_last_error();
    *      if (SS_OK == status && pub_size != 0)
    *      {
    *          status = pslm_pub_data_read_easy(hslm_zero, lic_find, pub_data, 0, pub_size);
    *          if (SS_OK == status)
    *          {
    *              // read ok
    *          }
    *      }
    *   @endcode 
    *   @see        slm_user_data_pub_getsize
    */
    SS_UINT32 SSAPI slm_pub_data_read_easy(
        IN  SLM_HANDLE_INDEX    slm_handle,
        IN  SS_UINT32           license_id,
        OUT SS_BYTE            *readbuf,
        IN  SS_UINT32           offset,
        IN  SS_UINT32           len
        );

    /*!
    *   @brief      SS内存托管内存申请
    *   @param[in]  slm_handle      许可句柄值
    *   @param[in]  size            申请大小，最大 SLM_MEM_MAX_SIZE.
    *   @return     返回托管内存id(内存id从0开始计数，即0是有效内存id)
    *   @see        slm_mem_free_easy slm_mem_read_easy slm_mem_write_easy
    *   @remark     错误值通过slm_get_last_error获得
    */
    SS_UINT32 SSAPI slm_mem_alloc_easy(
        IN  SLM_HANDLE_INDEX    slm_handle,
        IN  SS_UINT32           size
        );

    /*!
    *   @brief     释放托管内存
    *   @param[in] slm_handle    许可句柄值
    *   @param[in] mem_id        托管内存id
    *   @return    成功返回SS_OK，失败返回相应的错误码
    *   @see slm_mem_alloc_easy slm_mem_read_easy slm_mem_write_easy
    */
    SS_UINT32 SSAPI slm_mem_free_easy(
        IN  SLM_HANDLE_INDEX    slm_handle,
        IN  SS_UINT32           mem_id
        );

    /*!
    *   @brief     SS内存托管读
    *   @param[in]  slm_handle    许可句柄值
    *   @param[in]  mem_id        托管内存id
    *   @param[in]  offset        偏移
    *   @param[in]  len           长度
    *   @param[out] readbuff      读取到的数据
    *   @return     成功返回读取数据的大小
    *   @see        slm_mem_alloc_easy slm_mem_free_easy slm_mem_write_easy
    *   @remark     错误值通过slm_get_last_error获得
    */
    SS_UINT32 SSAPI slm_mem_read_easy(
        IN  SLM_HANDLE_INDEX    slm_handle,
        IN  SS_UINT32           mem_id,
        IN  SS_UINT32           offset,
        IN  SS_UINT32           len,
        OUT SS_BYTE             *readbuff
        );

    /*!
    *   @brief      SS内存托管内存写入
    *   @param[in]  slm_handle    许可句柄值
    *   @param[in]  mem_id        托管内存id
    *   @param[in]  offset        偏移
    *   @param[in]  len           写入长度，最大值参考SLM_MEM_MAX_SIZE
    *   @param[in]  writebuff     缓存
    *   @return     返回实际写的长度
    *   @see        slm_mem_alloc_easy slm_mem_free_easy slm_mem_read_easy
    *   @remark     错误码通过slm_get_last_error 得到
    */
    SS_UINT32 SSAPI slm_mem_write_easy(
        IN  SLM_HANDLE_INDEX    slm_handle,
        IN  SS_UINT32           mem_id,
        IN  SS_UINT32           offset,
        IN  SS_UINT32           len,
        IN  SS_BYTE             *writebuff
        );

    /*!
    *   @brief      许可动态执行代码，由开发商API gen_dynamic_code生成
    *   @param[in]  slm_handle      许可句柄值
    *   @param[in]  exf_buffer      动态exf算法缓冲区
    *   @param[in]  exf_size        动态exf文件大小
    *   @param[in]  inbuf           输入缓冲区
    *   @param[in]  insize          输入长度
    *   @param[out] poutbuf         输出缓存区
    *   @param[in]  outsize         输出缓存长度
    *   @param[out] pretsize        实际返回缓存长度
    *   @return     成功返回SS_OK，失败返回相应的错误码
    *   @see        slm_login_easy slm_execute_static_easy
    */
    SS_UINT32 SSAPI slm_execute_dynamic_easy(
        IN  SLM_HANDLE_INDEX    slm_handle,
        IN  SS_BYTE            *exf_buffer,
        IN  SS_UINT32           exf_size,
        IN  SS_BYTE            *inbuf,
        IN  SS_UINT32           insize,
        OUT SS_BYTE            *poutbuf,
        IN  SS_UINT32           outsize,
        OUT SS_UINT32          *pretsize
        );

    /*!
    *   @brief      执行锁内算法
    *   @param[in]  slm_handle      许可句柄值
    *   @param[in]  exfname         锁内执行文件名
    *   @param[in]  inbuf           输入缓冲区
    *   @param[in]  insize          输入长度
    *   @param[out] poutbuf         输出缓存区
    *   @param[in]  outsize         输出缓存长度
    *   @param[out] pretsize        实际返回缓存长度
    *   @return     成功返回SS_OK，失败返回相应的错误码
    *   @see        slm_login_easy slm_execute_dynamic_easy
    */
    SS_UINT32 SSAPI slm_execute_static_easy(
        IN  SLM_HANDLE_INDEX    slm_handle,
        IN  const char         *exfname,
        IN  SS_BYTE            *inbuf,
        IN  SS_UINT32           insize,
        OUT SS_BYTE            *poutbuf,
        IN  SS_UINT32           outsize,
        OUT SS_UINT32          *pretsize
        );


    /*!
    *   @brief      获取锁的设备证书
    *   @param[in]  slm_handle           许可句柄
    *   @param[out] pdevice_cert         设备证书数据缓存
    *   @param[in]  buff_size            设备证书缓存大小
    *   @param[out] preturn_size         返回的设备证书大小
    *   @return     成功返回SS_OK，失败返回相应的错误码
    *   @see        slm_login_easy
    */

    SS_UINT32 SSAPI slm_get_device_cert_easy(
        IN  SLM_HANDLE_INDEX slm_handle,
        OUT SS_BYTE          *pdevice_cert,
        IN  SS_UINT32         buff_size,
        OUT SS_UINT32        *preturn_size
        );

    /*!
    *   @brief      通过证书类型，获取已登录许可的证书
    *   @param[in]  slm_handle  许可句柄
    *   @param[in]  cert_type   证书类型，参考 CERT_TYPE_XXX
    *   @param[out] cert        证备证书缓冲区
    *   @param[in]  cert_size   缓冲区大小
    *   @return     返回证书的实际长度
    *   @remarks    当前仅支持本地硬件锁，错误值通过slm_get_last_error获得
    *   @see    slm_login
    */
    SS_UINT32 SSAPI slm_get_cert_easy(
        IN  SLM_HANDLE_INDEX    slm_handle,
        IN  CERT_TYPE           cert_type,
        OUT SS_BYTE            *cert,
        IN  SS_UINT32           cert_size
        );

    /*!
    *   @brief      设备私钥签名   
    *   @param[in]  slm_handle           许可句柄
    *   @param[in]  psign_data           验证数据（必须以字符"SENSELOCK"开头，共9字节）
    *   @param[in]  sign_data_size       验证数据大小，大小必须为 SLM_VERIFY_DATA_SIZE(41)个字节
    *   @param[out] psignature           验证数据的签名结果
    *   @param[in]  max_size             验证数据的最大缓存大小
    *   @param[out] psignature_ret_size  签名结果大小
    *   @return     成功返回SS_OK，失败返回相应的错误码
    *   @see        slm_login_easy
    */
    SS_UINT32 SSAPI slm_sign_by_device_easy(
        IN  SLM_HANDLE_INDEX    slm_handle,
        IN  SS_BYTE             *psign_data,
        IN  SS_UINT32           sign_data_size,
        OUT SS_BYTE             *psignature,
        IN  SS_UINT32           max_size,
        OUT SS_UINT32           *psignature_ret_size
        );

    /*!
    *   @brief      闪烁指示灯
    *   @param[in]  slm_handle          许可句柄
    *   @param[in]  color               控制灯颜色，0表示蓝色LED，1表示红色LED
    *   @param[in]  state               控制灯状态，0代表关闭，1代表打开， 2代表闪烁
    *   @param[in]  interval            控制灯闪烁间隔（毫秒）
    *   @return     成功返回SS_OK，失败返回相应的错误码
    *   @see        slm_login_easy
    */
    SS_UINT32 SSAPI slm_led_control_easy(
        IN  SLM_HANDLE_INDEX    slm_handle,
        IN  SS_UINT32           color,
        IN  SS_UINT32           state,
        IN  SS_UINT32           interval
        );

    /*!
    *   @brief      获得runtime库和SS的版本信息.
    *   @param[out] api_version         API的版本（总是成功）
    *   @param[out] ss_version          SS服务的版本
    *   @return     成功返回SS_OK，失败则返回相应的错误码
    */
    SS_UINT32 SSAPI slm_get_version_easy(
        OUT SS_UINT32      *api_version,
        OUT SS_UINT32      *ss_version
        );

    /*!
    *   @brief     通过错误码获得错误信息
    *   @param[in] error_code        错误码
    *   @param[in] language_id       要返回字符串的语言，见 LANGUAGE_XXXX_ASCII
    *   @return    成功返回错误信息（不需要调用slm_free_easy释放），失败返回空指针NULL 
    *
    */
    const SS_CHAR * SSAPI slm_error_format_easy(
        IN SS_UINT32    error_code,
        IN SS_UINT32    language_id
        );


    /*!
    *   @brief      将D2C包进行升级
    *   @param[in]  d2c_pkg     d2c文件数据
    *   @return     错误信息，使用需要调用slm_free_easy释放
    *   @see        slm_update_ex_easy
    *   @remarks:   error_msg的数据内容（JSON）：
    *   @code
    *   [
    *   {"pkg_order":1, "pkg_desc":"package decription.", "status": 0},
    *   {"pkg_order":2, "pkg_desc":"package decription.", "status": 0}
    *   ]
    *   @endcode
    *   @remark 错误值通过slm_get_last_error获得，返回值调用slm_free_easy释放
    */
    SS_CHAR* SSAPI slm_update_easy(
        IN  SS_CHAR       *d2c_pkg
        );

    /*!
    *   @brief      将D2C包进行升级，需指定加密锁芯片号,可以并行操作
    *   @param[in]  szlock_sn     锁号HEX字符串
    *   @param[in]  d2c_pkg       d2c文件数据
    *   @return     error_msg，错误信息，使用需要调用slm_free_easy释放
    *   @remarks:   error_msg的数据内容（JSON）：
    *   @code
    *   [
    *   {"pkg_order":1, "pkg_desc":"package decription.", "status": 0},
    *   {"pkg_order":2, "pkg_desc":"package decription.", "status": 0}
    *   ]
    *   @endcode
    *   @see    slm_update  // TODO:
    */
    SS_CHAR* SSAPI slm_update_ex_easy(
        IN SS_CHAR     *szlock_sn,
        IN SS_CHAR     *d2c_pkg
        );

    /*!
    *   @brief      锁内短码升级，需指定加密锁唯一序列号
    *   @param[in]  lock_sn     锁号（唯一序列号）
    *   @param[in]  inside_file 锁内短码文件名
    *   @return     成功返回SS_OK，失败返回错误码
    *   @see        slm_execute_static_easy
    */
    SS_UINT32 SSAPI slm_d2c_update_inside_easy(
        IN char        *lock_sn,
        IN char        *inside_file
        );


    /*!
    *   @brief      将D2C包进行升级，需指定加密锁芯片号和D2C包文件路径。
    *   @param[in]  szlock_sn     锁号HEX字符串
    *   @param[in]  d2c_file      d2c文件路径
    *   @return     error_msg，错误信息，使用需要调用slm_free_easy释放
    *   @see    slm_update_ex, slm_update
    */
    SS_CHAR* SSAPI slm_update_file_easy(
        IN SS_CHAR      *szlock_sn,
        IN SS_CHAR      *d2c_file
        );

    /*!
    *   @brief       枚举本地锁信息
    *   @return      成功返回设备信息，需要调用slm_free_easy
    *   @remark      错误码需要调用slm_get_last_error 获取
    */
    SS_CHAR* SSAPI slm_enum_device_easy(void);

    /*!
    *   @brief       枚举指定设备下所有许可ID
    *   @param[in]   device_info   指定某个锁登陆
    *   @param[out]  license_ids   返回所有许可的ID数组，JSON格式
    *   @return      返回所有许可的ID数组，JSON格式,需要调用 slm_free_easy释放
    *   @remark      错误通过slm_get_last_error 返回
    *   @see slm_enum_device_easy slm_get_license_info_easy
    */
    SS_CHAR * SSAPI slm_enum_license_id_easy(
        IN const char   *device_info
        );

    /*!
    *   @brief       获取指定设备下指定许可的全部信息
    *   @param[in]   device_info   指定某个锁登陆
    *   @param[in]   license_id    指定许可ID
    *   @return      返回许可的信息JSON格式 等同于slm_get_info_easy的LICENSE_INFO，需要调用slm_free_easy释放  
    *   @remark      错误通过slm_get_last_error 返回
    *   @see slm_enum_device_easy slm_enum_license_id_easy
    */
    SS_CHAR * SSAPI slm_get_license_info_easy(
        IN const char  *device_info,
        IN SS_UINT32    license_id
        );

    /*!
    *   @brief     释放API生成的缓冲区
    *   @param[in] buffer     API生成的缓冲区
    */
    void SSAPI slm_free_easy(IN void *buffer);

    /*!
    *   @brief   枚举已登录的用户token
    *   @return  SS_CHAR* access_token 默认用户的token
    *   @remark  错误通过slm_get_last_error 返回;SS_OK，如果oauth后台进程未启动，则返回SS_ERROR_BAD_CONNECT，
    *            返回值调用slm_free_easy释放
    */
    SS_CHAR* SSAPI slm_get_cloud_token_easy(void);

    /*! 
    *   @brief       获取开发商ID
    *   @param[out]  szdeveloper_id     开发商id HEX字符串
    *   @return      返回十六进制字符串类型的开发商ID
    *   @remark      错误通过slm_get_last_error 返回
    */
    SS_UINT32 SSAPI slm_get_developer_id_easy(char *szdeveloper_id);


    /*!
    *  @brief      使用已登录的许可进行签名
    *  @param[in]  slm_handle      许可句柄
    *  @param[in]  sign_data       要签名的数据（最少16字节，最大64字节）
    *  @param[in]  sign_length,    要签名的数据长度
    *  @param[out] signature,      签名结果缓冲区
    *  @param[in]  max_buf_size,   签名结果缓冲区大小
    *  @return     成功返回签名结果实际长度
    *  @remark     错误码需要调用slm_get_laster_error返回
    *  @see        slm_license_verify_easy
    */
    SS_UINT32 SSAPI slm_license_sign_easy(
        IN  SLM_HANDLE_INDEX   slm_handle,
        IN  SS_BYTE           *sign_data,
        IN  SS_UINT32          sign_length,
        OUT SS_BYTE           *signature,
        IN  SS_UINT32          max_buf_size
        );

    /*!
    *  @brief      对许可签名后的数据进行验签
    *  @param[in]  sign_data        要签名的数据（最少16字节，最大64字节）
    *  @param[in]  sign_length,     要签名的数据长度
    *  @param[in]  signature        签名结果数据
    *  @param[in]  signature_length 签名结果长度
    *  @return     签名数据信息，json结构，不再使用时，需要调用slm_free释放
    *  @remark    -如果成功，需要调用slm_free_easy 释放sign_info  
    *             -错误码需要调用slm_get_laster_error返回
    *             -验签过程可以不登录许可
    *  @code
         //json参数
         {
         "type":2,                                   // 2表示云锁
         "developer_id":0000000000000000,            // 开发商ID
         "license_id":0,                             // 许可ID
         "guid":xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx,    // 用户guid（由类型决定）
         "sn":00000000000000000000000000000000,      // 锁号（由类型决定）
         "rand":0,                                   // 随机数
         }
    *  @see  slm_license_sign_easy slm_free_easy
    */
    SS_CHAR * SSAPI slm_license_verify_easy(
        IN  SS_BYTE      *sign_data,
        IN  SS_UINT32     sign_length,
        IN  SS_BYTE      *signature,
        IN  SS_UINT32     signature_length
        );

    /*!
    *   @brief      扩展配置接口
    *   @param[in]  config    配置参数（JSON）
    *               1.slm_login 扩展参数，指定许可登录云锁、软锁服务地址，注意服务地址URL长度不得超过 SLM_MAX_CLOUD_SERVER_LENGTH 宏定义长度。
    *                           {"api_slm_login":{"cloud_runtime_url":"http://rt.senseyun.com", "slock_url":"http://sl.senseyun.com"}}
    *   @return     成功返回SS_OK，失败返回相应错误码
    *   @remarks    不支持多线程调用，非线程安全，请保证配置在相关功能运行之前执行。
    */
    SS_UINT32 SSAPI slm_extensions_config_easy(
        IN const char *config
        );

#ifdef __cplusplus
}
#endif

#endif //_slm_runtime_easy_H_

