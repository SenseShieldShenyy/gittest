using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace opeapiShow
{
    //接口类
    class openAPI
    {
        //通用地址
        private string httpsUrl = "https://openapi.senseyun.com";
        private string auth = "https://openapi.senseyun.com/v2/sv/";
        /*各功能地址*/
        private string addUser = "addUser";                          //添加普通用户
        private string addShadowUser = "addShadowUser";              //添加影子账户
        private string modifyUserPasswd = "modifyUserPasswd";        //修改影子账户密码
        private string deleteUser = "deleteUser";                    //删除影子账户
        private string findUsers = "findUsers";                      //查找用户
        private string modifyUser = "modifyUser";                    //修改影子账户信息
        private string addProduct = "addProduct";                    //添加产品
        private string modifyProduct = "modifyProduct";              //修改产品
        private string getProductInfo = "getProductInfo";            //获取产品信息
        private string productList = "productList";                  //模糊搜索产品信息
        private string deleteProduct = "deleteProduct";              //删除指定产品授权
        private string addTemplate = "addTemplate";                  //添加模板
        private string modifyTemplate = "modifyTemplate";            //修改模板
        private string templateList = "templateList";                //按照产品号枚举模板
        private string findTemplate = "findTemplate";                //按照模板编号查找模板信息
        private string deleteTemplate = "deleteTemplate";            //删除指定编号的模板
        private string issueLicenseByPloy = "issueLicenseByPloy";    //可修改许可策略的发布许可
        private string issueLicense = "issueLicense";                //不可修改许可策略的发布许可
        private string deleteLicense = "deleteLicense";              //删除许可 
        private string licenseList = "licenseList";                  //搜索许可
        private string softLockbindList = "softLockbindList";        //获取软锁的绑定机器信息
        private string softLockUnbind = "softLockUnbind";            //解绑软锁

        public string HttpsUrl
        {
            get
            {
                return httpsUrl;
            }
        }
        public string AddUser
        {
            get
            {
                return auth + addUser;
            }
        }

        public string AddShadowUser
        {
            get
            {
                return auth + addShadowUser;
            }
        }

        public string ModifyUserPasswd
        {
            get
            {
                return auth + modifyUserPasswd;
            }
        }

        public string DeleteUser
        {
            get
            {
                return auth + deleteUser;
            }
        }

        public string FindUsers
        {
            get
            {
                return auth + findUsers;
            }
        }

        public string ModifyUser
        {
            get
            {
                return auth + modifyUser;
            }
        }

        public string AddProduct
        {
            get
            {
                return auth + addProduct;
            }
        }

        public string ModifyProduct
        {
            get
            {
                return auth + modifyProduct;
            }
        }

        public string GetProductInfo
        {
            get
            {
                return auth + getProductInfo;
            }
        }

        public string ProductList
        {
            get
            {
                return auth + productList;
            }
        }

        public string DeleteProduct
        {
            get
            {
                return auth + deleteProduct;
            }
        }

        public string AddTemplate
        {
            get
            {
                return auth + addTemplate;
            }
        }

        public string ModifyTemplate
        {
            get
            {
                return auth + modifyTemplate;
            }
        }

        public string TemplateList
        {
            get
            {
                return auth + templateList;
            }
        }

        public string FindTemplate
        {
            get
            {
                return auth + findTemplate;
            }
        }

        public string DeleteTemplate
        {
            get
            {
                return auth + deleteTemplate;
            }
        }

        public string IssueLicenseByPloy
        {
            get
            {
                return auth + issueLicenseByPloy;
            }
        }

        public string IssueLicense
        {
            get
            {
                return auth + issueLicense;
            }
        }

        public string DeleteLicense
        {
            get
            {
                return auth + deleteLicense;
            }
        }

        public string LicenseList
        {
            get
            {
                return auth + licenseList;
            }
        }

        public string SoftLockbindList
        {
            get
            {
                return auth + softLockbindList;
            }
        }

        public string SoftLockUnbind
        {
            get
            {
                return auth + softLockUnbind;
            }
        }
    }
}
