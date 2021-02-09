using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;


namespace D2cDemo
{
    /// <summary>
    /// Libd2c操作步骤：
    /// 注意：执行许可签发操作必须插入控制锁。
    /// 1.调用OpenMaster()打开控制锁
    /// 2.签发许可（添加、更新、删除等）
    ///   2.1 执行NewD2CLic()创建D2C句柄，必须执行成功（返回0）才能执行后续步骤。
    ///   2.2 组装许可内容（JSON字符串）
    ///   2.3 将许可内容通过AddD2CLic()方法，写入控制锁。
    ///       （步骤2.3允许多次操作，支持一次性签发多个许可）
    ///   2.4 2.3执行成功（返回0），调用GetD2C()获取签名加密后的d2c内容。
    ///   2.5 2.4执行成功（返回0），返回完整的d2c包（完整的JSON结构），
    ///       调用ControlAPI.UpdateLicToDevice()方法，将d2c内容写入指定用户锁。
    /// 3.调用CloseMaster()关闭控制锁
    /// </summary>
    class Libd2c
    {
        /// <summary>
        /// GUID长度
        /// </summary>
        private readonly static int GUID_LEN = 37;
        /// <summary>
        /// 错误码-成功 
        /// </summary>
        private readonly static int SS_OK = 0;
        public static ACCOUNT_TYPE accountType = ACCOUNT_TYPE.NONE;
        public static string accountID = string.Empty;
        public static byte[] cert = null;
        public static UInt32 certSize = 0;

        /// <summary>
        /// 账户类型，指定给特定的签发对象
        /// </summary>
        public enum ACCOUNT_TYPE
        {
            NONE   = 0,    // 签给非账户（离线USB锁设备，许可证不可转移）
            USB    = 1,    // 签给在线账户（USB锁号作为匿名账户，许可证可以转移）
            EMAIL  = 2,    // 签给邮箱账户（许可证可以转移）
            PHONE  = 3,    // 签给手机账户（许可证可以转移）
        }
        //D2C包的签发类型
        public enum SIGN_TYPE
        {
            SIGN_TYPE_CERT          = 1, //证书签发，给指定的用户锁
            SIGN_TYPE_SEED          = 2, //种子码签发，给非指定的用户锁
        }
        public enum CA_TYPE
        { 
            /** 系统级别CA */
              PKI_CA_TYPE_SYSTEM                          = 0,       
            /** 开发者CA */
              PKI_CA_TYPE_DEVELOPER                       = 1,       
            /** 硬件设备CA */
             PKI_CA_TYPE_DEVICE                           = 2,       
            /** 账户证书 */
              PKI_CA_TYPE_ACCOUNT                         = 3,       
            /** 根证书 */
              PKI_CA_TYPE_ROOT                            = 0x80,    
        }
        /*!
        *   @brief  打开签发设备（控制锁）
        *   @param[out] device_handle   设备句柄
        *   @return 成功返回SS_OK，失败返回错误码
        *   @see master_close d2c_new
        */
        [DllImport("libd2c.dll", EntryPoint = "master_open", CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 master_open(ref IntPtr deviceHandlePtr);

        /*!
        *   @brief  关闭句柄
        *   @param[in]  device_handle   设备句柄
        *   @return 成功返回SS_OK，失败返回错误码
        */
        [DllImport("libd2c.dll", EntryPoint = "master_close", CallingConvention = CallingConvention.StdCall)]
        private static extern UInt32 master_close(IntPtr deviceHandlePtr);

        /*!
        *   @brief 创建D2C句柄，用于签发许可
        *   @param[in]  device_handle   签发设备的句柄
        *   @param[out] d2c_handle      返回D2C的句柄
        *   @param[in]  account_type    账号类型 @see ACCOUNT_TYPE
        *   @param[in]  account_id      账户名（字符串）或锁号（16字节锁号）
        *   @param[in]  account_size    账户名长度
        *   @param[in]  cert            证书，如果是account type为ACCOUNT_TYPE_NONE，填入硬件锁设备证书链，账户许可填入云平台证书链
        *   @param[in]  cert_size       证书长度
        *   @return     SS_OK						成功
        *               SS_ERROR_PARSE_CERT		    解析设备证书失败.
        *   @see d2c_delete d2c_add_developer_cert d2c_add_pkg d2c_get
        */
        [DllImport("libd2c.dll", EntryPoint = "d2c_lic_new", CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 d2c_lic_new(
            IntPtr          device_handle,
            ref IntPtr      d2c_handle,
            ACCOUNT_TYPE    account_type,
            [In, MarshalAs(UnmanagedType.LPArray)] byte[] account_id,
            UInt32          account_size,
            [In, MarshalAs(UnmanagedType.LPArray)] byte[] cert,
            UInt32 cert_size
        );

        /*!
         *   @brief      通过证书类型，获取开发锁 CA 证书
         *   @param[in]  device_handle       开发锁设备句柄，通过调用 #master_open 得到
         *   @param[in]  ca_type             证书类型，详见 #PKI_CA_TYPE_DEVELOPER 等
         *   @param[in]  root_index          根证书索引序号
         *   @param[out] ca_cert             存放锁CA证书的缓冲区
         *   @param[in]  cert_bufsize        缓冲区大小
         *   @param[out] cert_size           CA 证书大小
         */
        [DllImport("libd2c.dll", EntryPoint = "master_get_ca_cert_ex", CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 master_get_ca_cert_ex(
            IntPtr                                           device_handle,
            CA_TYPE                                          ca_type,
            UInt32                                           root_index,
            [Out, MarshalAs(UnmanagedType.LPArray)] byte[]   ca_cert,
            UInt32                                           cert_bufsize,
            ref UInt32                                       cert_size
            );
        /*!
      *   @brief      创建D2C句柄，用于签发非许可类型升级包，例如签发文件
      *   @param[in]  device_handle   开发锁设备句柄，通过调用 #master_open 得到
      *   @param[out] d2c_file        返回 D2C 的句柄
      *   @param[in]  sign_type       签包类型，参考 #SIGN_TYPE
      *   @param[in]  param           如果是设备证书，则传入设备证书链，种子码签发填入 Virbox Root CA
      *   @param[in]  param_size      证书链的大小或种签子码签发填 Virbox根证书大小
      *   @return     成功返回S S_OK，失败返回相应的错误码
         */
        /*
        *   @param[in]  d2c_file        文件 D2C 句柄，由 #d2c_file_new 得到
        *   @param[in]  param           升级包json串
        *   @param[in]  opr_desc        包描述字符串
        *   @exception  没有异常
        *   @return     成功返回 SS_OK ，失败返回相应的错误码
         */
        //[DllImport("libd2c.dll", EntryPoint = "d2c_add_pkg", CallingConvention = CallingConvention.StdCall)]
        //public static extern UInt32 d2c_add_pkg(
        //    IntPtr                                              device_handle,
        //    [In, MarshalAs(UnmanagedType.LPArray)] string       param,
        //    [In, MarshalAs(UnmanagedType.LPStr)]  string         opr_desc
        //    );
        [DllImport("libd2c.dll", EntryPoint = "d2c_file_new", CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 d2c_file_new(
            IntPtr                                        device_handle,
            ref IntPtr                                    d2c_handle,
            SIGN_TYPE                                     sign_type,
            [In, MarshalAs(UnmanagedType.LPArray)] byte[] param,
            UInt32                                        param_size
            );

        /*!
    *   @brief      验证开发锁PIN码
    *   @param[in]  device_handle   签发设备的句柄
    *   @param[in]  pin_index       PIN码所在的索引位置，目前只支持 1 个PIN码，当前为0
    *   @param[in]  pin             当前使用中的PIN码
    *   @param[in]  pin_len         当前使用的PIN码长度
    *   @return     成功返回SS_OK，失败返回相应的错误码
    *   @remarks    PIN验证遵循以下规则：
    *               - 初始PIN码不能通过验证
    *               - 必须通过PIN码验证才能签发升级包
    */
        [DllImport("libd2c.dll", EntryPoint = "master_pin_verify", CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 master_pin_verify(
             IntPtr deviceHandlePtr,
             byte pin_index,
             [In, MarshalAs(UnmanagedType.LPArray)] byte[] pin,
             UInt32 pin_len
            );
        /*!
        *   @brief 根据描述，生成升级包，并添加升级包到D2C
        *   @param[in] h_d2c D2C句柄（由d2c_new函数得到）
        *   @param[in] param 升级包json串
        *   @param[in] opr_desc 包描述字符串
        *   @exception 没有异常
        *   @return 添加成功则返回0，否则返回错误码
        *   @remarks
        *   - 升级规则：
        *       -#  更新文件时，执行文件的offset必须为0
        *       -#  种子码方式，不能签发许可
        *   JSON 参数说明：
        *   @code
        *   文件操作：
        *   {
        *   "op":           "addfile" | "updatefile" | "delfile",   添加、更新或删除文件
        *   "filename":     "file_name.xxx",            文件名，长度需小于16字节
        *   "filetype":     "evx" | "evd" | "key"         文件类型，依次为可执行文件，数据文件，密钥文件（删除文件时不需要）
        *   "access":       number                      文件的访问权限（添加，更新，删除文件时都可以设置，默认为0x0F（开发商所有权限，Entry不可访问）
        *   "timestamp":    number(0 ~ 0xFFFFFFFF)      文件的版本（用于抗重放），如果JSON中没有此项，则使用当前时间戳生成一个版本号
        *   "filebuffer":   "0123456789ABCDEF"          文件内容HEX16字符串（删除文件时不需要）
        *   "fileoffset":   number,                     （可选，默认为0）文件偏移（删除文件时不需要）
        *   "bind_lic":     [1,2,3,4]                   可执行文件绑定的许可（删除文件时不需要）
        *   }
        *   设置种子码：
        *   {
        *   "op":   "setseed"
        *   "seed": "0102030405060708090A0B0C0D0E0F101112131415161718"  (24个字节：见D2C_SEED_LENGTH）
        *   "timestamp":  number      种子码的签发时间戳（用于防重放），如果JSON中没有此项，则使用当前时间戳生成一个版本号
        *   }
        *
        *   许可操作：
        *   {
        *   "op":           
        *   }
        *
        *   修订时钟：
        *   {
        *   "op" :          "fixtime",                  修复时钟
        *   "lock_time":      number,					用户锁的时间
        *   "rand":         "0102030405060708"          8个字节HEX16字符串
        *   "diff":          number,                    时间差(pc_time - lock_time)
        *   }
        *
        *   @endcode
        *   @see d2c_new d2c_add_developer_cert
        *
        */
        [DllImport("libd2c.dll", EntryPoint = "d2c_add_pkg", CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 d2c_add_pkg(
            IntPtr h_d2c,
            [In, MarshalAs(UnmanagedType.LPStr)] string param,
            [In, MarshalAs(UnmanagedType.LPStr)] string opr_desc
        );

        /*!
        *   @brief 根据描述，生成升级包，并添加升级包到D2C
        *   @param[in] h_d2c D2C句柄（由d2c_new函数得到）
        *   @param[in] param 升级包json串
        *   @param[in] opr_desc 包描述字符串
        *   @param[out] guid 许可GUID
        *   @exception 没有异常
        *   @return 添加成功则返回0，否则返回错误码
        *   @remarks
        *   - 升级规则：
        *       -#  更新文件时，执行文件的offset必须为0
        *       -#  种子码方式，不能签发许可
        *   JSON 参数说明：
        *   @code
        *   许可安装与激活：
        *   {
        *   "op":          "installlic" | "activelic" |"updatelic" | "dellic" | "lockalllic" | "unlockalllic" | "delalllic",
        *                   依次为安装、激活、添加、更新、删除、锁定所有许可、解锁所有许可、删除所有许可
        *   "license_id":  number,              许可ID（数字，范围从 1 ~ 4294967295）（lockalllic, delalllic, unlockalllic不需要）
        *   "force":       bool,                是否强制升级，升级的策略为：
        *                                       [addlic]无则添加，有则覆盖
        *                                       [dellic]有则删除，无则成功
        *   "start_time" : "op number"  起始时间(UTC秒)            （可选）
        *   "end_time" :   "op number"  终止时间(UTC秒)            （可选）
        *   "counter" :    "op number"  使用次数(UTC秒)            （可选）
        *   "span" : 	   "op number"  时间跨度(UTC秒)            （可选）
        *   "concurrent" : "op number"  最大并发数(0~65535)        （可选）
        *   "concurrent_type":  "process" | "win_user_session" : 以进程/Windows Session限制并发数 (可选）
        *   "module":      [0,1,2,3,4,5...]，    （数组）模块区，数组里可以表示模块，范围从0到63
        *
        *   "timestamp":    number,(0~0xFFFFFFFF)       （可选）许可的签发时间戳（用于防重放，签发许）
        *   "serial":       number,(0~0xFFFF)           （可选）许可的签发流水号（与时间戳结合，用于防重放）
        *   "rom": {"data":"HEX字符串", "offset":number,"resize":number}           只读数据区（可选）
        *   "raw": {"data":"HEX字符串", "offset":number,"resize":number}           读写数据区（可选）
        *   "pub": {"data":"HEX字符串", "offset":number,"resize":number}           公开数据区（可选）
        *   "data"表示数据内容，resize如果存在，表示要重置数据区的大小 ，其value表示重置的大小
        *   注： 除"lockalllic", "unlockalllic", "delalllic"外，其它操作均需要设置"license_id"字段
        *   }
        *   @endcode
        *   @see d2c_new d2c_add_developer_cert
        *
        */
        [DllImport("libd2c.dll", EntryPoint = "d2c_add_lic", CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 d2c_add_lic(
            IntPtr          h_lic_d2c,
            [In, MarshalAs(UnmanagedType.LPStr)] string param,
            [In, MarshalAs(UnmanagedType.LPStr)] string opr_desc,
            [In, MarshalAs(UnmanagedType.LPArray, SizeConst = 37)] byte[] guid
        );

        /*!
        *   @brief 从D2C句柄中获取D2C流，可保存为 .d2c 文件用于升级
        *   @param[in]  h_d2c D2C句柄（由d2c_new函数得到）
        *   @param[out] d2c_buf 得到的D2C数据流，该数据流为字符串格式
        *       如果为0，则out_len返回需要的长度
        *   @param[in]  max_buf_len d2c_buf的缓冲区大小
        *   @param[out] out_len 得到的D2C数据流长度
        *   @return 添加成功则返回0，否则返回错误码
        *   @remarks
        *   @code
        *   {
        *       ["pkg_type":"type", "pkg_data":"base64", "pkg_desc":"desc"]
        *       ["pkg_type":"type", "pkg_data":"base64", "pkg_desc":"desc"]
        *   }
        *   @endcode
        *   @see d2c_new d2c_add_developer_cert
        */
        [DllImport("libd2c.dll", EntryPoint = "d2c_get", CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 d2c_get(
            IntPtr      h_d2c,
            [Out, MarshalAs(UnmanagedType.LPArray)] byte[] d2c_buf,
            UInt32      max_buf_len,
            ref UInt32  out_len
        );

       

    }

}
