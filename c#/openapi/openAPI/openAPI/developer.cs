using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openAPI
{
    class developer
    {
        private string appid = "";
        private string secret = "";

        public developer(string appid, string secret)
        {
            this.appid = appid;
            this.secret = secret;
        }

        public string Appid
        {
            get { return appid; }
        }

        public string Secret
        {
            get { return secret; }
        }
    }
}
