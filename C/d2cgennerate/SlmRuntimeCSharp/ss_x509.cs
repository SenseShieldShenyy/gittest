using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace cs_libd2c_demo
{
    

    public struct CERT_ST
    {
     
        [MarshalAs(UnmanagedType.ByValArray, SizeConst =ss_x509.x509_CERT_SIZE)]
        public Byte[] certbuf;
        public UInt32 certlen;
    }


    class ss_x509
    {
        public const int x509_CERT_SIZE = 2048;

        [DllImport("libx509.dll", EntryPoint = "ss_make_p7b", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 ss_make_p7b(
              [In, MarshalAs(UnmanagedType.LPArray)] CERT_ST[] cert_array,
              UInt32 certcount,
              [Out, MarshalAs(UnmanagedType.LPArray)] Byte[]  p7b_buf,
              ref UInt32 p7b_len
            );

    }
}
