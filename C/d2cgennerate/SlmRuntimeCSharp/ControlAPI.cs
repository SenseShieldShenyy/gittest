using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using cs_libd2c_demo;

namespace D2cDemo
{
    /// <summary>
    /// ControlAPI操作步骤：
    /// 1.调用OpenClient()，获取ipc句柄
    /// 2.使用GetAllDescription()获取所有设备描述（JsonArray）
    ///     2.1 遍历设备描述，根据设备描述查询设备信息（调用GetDeviceInfo()）
    /// 3.关闭ipc句柄
    /// </summary>
    /// 
    /** 设备证书类型*/
    public enum CERT_TYPE : uint
    {
        /** 证书类型：根证书  */
        CERT_TYPE_ROOT_CA = 0,

        /** 证书类型：设备子CA  */
        CERT_TYPE_DEVICE_CA = 1,

        /** 证书类型：设备证书  */
        CERT_TYPE_DEVICE_CERT = 2,

        /** 证书类型：深思设备证书  */
        CERT_TYPE_SENSE_DEVICE_CERT = 3,
    }

    class ControlAPI
    {
        /// <summary>
        /// 参数格式枚举
        /// </summary>
        public enum INFO_FORMAT_TYPE
        {
            JSON = 2,       /** JSON格式  */
            STRUCT = 3,     /** 结构体格式  */
            STRING_KV = 4,  /** 字符串模式,遵行Key=value  */
            CIPHER = 5,     /** 加密二进制格式*/
        }

        /*!
        *   @brief 客户端打开SS IPC句柄
        *   @param[out] ipc 返回ipc句柄
        *   @return SS_UINT32错误码 
        */
        [DllImport("slm_control.dll", EntryPoint = "slm_client_open", CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 slm_client_open(ref IntPtr ipc);

        /*!
        *   @brief 关闭客户端IPC句柄
        *   @param[in] ipc 输入ipc句柄
        *   @return SS_UINT32错误码 
        */
        [DllImport("slm_control.dll", EntryPoint = "slm_client_close", CallingConvention = CallingConvention.StdCall)]
        private static extern UInt32 slm_client_close(IntPtr ipc);

        /*!
        *   @brief 获取所有设备描述
        *   @param[in]  ipc     IPC句柄
        *   @param[in]  format_type  参数格式
        *   @param[out] desc    设备描述
        *   @return 成功返回SS_OK，失败返回相应错误码
        */
        [DllImport("slm_control.dll", EntryPoint = "slm_get_all_description", CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 slm_get_all_description(
            IntPtr      ipc,
            INFO_FORMAT_TYPE format_type,
            ref IntPtr  desc
            );
        /*!
*   @brief      获取指定设备描述下的所有许可ID
*   @param[in]  ipc     IPC句柄
*   @param[in]  type    参数类型（JSON)
*   @param[in]  desc   设备描述，通过slm_get_all_descritpion等函数获得的设备描述中，取其中需要的描述内容进行查询
*   @param[out] result  许可ID，json数组，需要调用slm_free释放
*   @return     成功返回SS_OK，失败返回相应的错误码
*   @remarks    当前接口可以简单的得到指定设备中的许可ID列表，便于统计指定设备中的许可个数。
*   @code
*     - 许可ID json结构
*       [1,2,3,4]
*   @endcode
*   @see    slm_client_open slm_get_all_description
*/

        [DllImport("slm_control.dll", EntryPoint = "slm_get_license_id", CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 slm_get_license_id(
            IntPtr ipc,
            INFO_FORMAT_TYPE type,
            [In, MarshalAs(UnmanagedType.LPStr)] string desc,
            ref IntPtr result
            );


        /*!
        *   @brief      获取加密锁的证书(仅支持本地加密锁)
        *   @param[in]  ipc         IPC句柄
        *   @param[in]  desc        设备描述，由slm_get_all_description得到
        *   @param[in]  cert_type   证书类型，参考 CERT_TYPE_XXXX  
        *   @param[out] cert        设备证书缓冲区
        *   @param[in]  cert_size   缓冲区大小
        *   @param[out] cert_len    返回的设备证书大小
        *   @return     成功返回SS_OK，失败返回相应的错误码
        *   @remarks    获取设备证书，此处的设备指硬件锁、云锁。
        *               如果 cert_type = CERT_TYPE_DEVICE_CERT，其功能与 slm_ctrl_get_device_cert 完全一致； 
        *               如果为其他类型，则仅支持硬件锁。
        *               通过此接口可以获取加密锁的根证书、设备子CA和设备，方便合成设备证书链。
        *   @see        slm_get_all_description slm_ctrl_get_device_cert
        */
        [DllImport("slm_control.dll", EntryPoint = "slm_ctrl_get_cert", CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 slm_ctrl_get_cert(
            IntPtr           ipc,
            [In, MarshalAs(UnmanagedType.LPStr)] string desc,
            CERT_TYPE        cert_type,
            [Out, MarshalAs(UnmanagedType.LPArray)] Byte[]   certs,
            UInt32 cert_size,
            ref UInt32 cert_len );




        /*!
        *   @brief 获得设备信息
        *   @param[in] ipc ipc句柄
        *   @param[in] desc  设备描述
        *   @param[in] result 接收数据的指针，需要调用slm_free释放
        *   @return SS_UINT32错误码 
        *   @remarks
        *   - JSON 字段说明：
        *       -# "clock"（数字）：锁内时间（UTC）
        *       -# "available_space"(数字）：可用空间
        *       -# "communication_protocol": 通迅协议
        *       -# "firmware_version":固件版本
        *       -# "manufacture_date":生产日期
        *       -# "slave_addr": 总线地址
        */
        [DllImport("slm_control.dll", EntryPoint = "slm_get_device_info", CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 slm_get_device_info(
            IntPtr      ipc,
            [In, MarshalAs(UnmanagedType.LPStr)] string desc,
            ref IntPtr  result
            );

        /*!
        *   @brief  将D2C包进行升级
        *   @param[in]  d2c_pkg     d2c文件数据
        *   @param[out] error_msg   错误信息，不使用需要调用slm_free释放
        *   @return 成功返回SS_OK，失败返回错误码
        *   @remarks:   error_msg的数据内容（JSON）：
        *   @code
        *   [
        *   {"pkg_order":1, "pkg_desc":"package decription.", "status": 0},
        *   {"pkg_order":2, "pkg_desc":"package decription.", "status": 0}
        *   ]
        *   @endcode
        *   @see    slm_update 
        */
        [DllImport("slm_control.dll", EntryPoint = "slm_update", CallingConvention = CallingConvention.StdCall)]
        public  static extern UInt32 slm_update(
            [In, MarshalAs(UnmanagedType.LPStr)] string d2c_pkg,
            ref IntPtr  error_msg
            );

        [DllImport("slm_control.dll", EntryPoint = "slm_update_ex", CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 slm_update_ex(
            [In, MarshalAs(UnmanagedType.LPStr)] string lock_sn,
    [In, MarshalAs(UnmanagedType.LPStr)] string d2c_pkg,
    ref IntPtr error_msg
    );

        /*!
        *   @brief  获取锁的设备证书
        *   @param[in]  ipc         ipc句柄
        *   @param[in]  desc        设备描述，由slm_get_all_server得到
        *   @param[out] device_cert 证备证书缓冲区
        *   @param[in]  buff_size   缓冲区大小
        *   @param[out] return_size 返回的设备证书大小
        *   @return 成功返回SS_OK，失败返回相应的错误码
        *   @remarks    该函数暂未实现
        *   @see    slm_login
        */
        [DllImport("slm_control.dll", EntryPoint = "slm_ctrl_get_device_cert", CallingConvention = CallingConvention.StdCall)]
        private static extern UInt32 slm_ctrl_get_device_cert(
            IntPtr          ipc,
            [In, MarshalAs(UnmanagedType.LPStr)] string desc,
            [Out, MarshalAs(UnmanagedType.LPArray)] Byte[] device_cert,
            UInt32          buff_size,
            ref UInt32      return_size
            );


        /// <summary>
        /// 获取用户锁证书链数据，签发设备许可时必填参数。
        /// </summary>
        /// <param name="usrDeviceCer">用户锁的证书数据</param>
        /// <returns>组合sense根证书后的完整证书链</returns>
        /// <remarks>引用COM组件CERTCLIENTLib和CERTENROLLLib</remarks>
        public static Byte[] GetUsrDeviceP7b(Byte[] usrDeviceCer)
        {
            X509Certificate2Collection p7b = new X509Certificate2Collection();

            X509Certificate2 rootx509 = new X509Certificate2(Certs.RootCert);
            X509Certificate2 deviceRootx509 = new X509Certificate2(Certs.DeviceRootCert);
            X509Certificate2 usrdevicex509 = new X509Certificate2(usrDeviceCer);

            p7b.Add(rootx509);
            p7b.Add(deviceRootx509);
            p7b.Add(usrdevicex509);

            Byte[] byptes = p7b.Export(X509ContentType.Pkcs7);
            return byptes;
        }
    }
}
