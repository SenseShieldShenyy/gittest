// 深思精锐5 Web Server js接口


function SenseWeb() {

    var senseweb = new Object();

    senseweb.host = "localhost"
    senseweb.path = "/v1/api"
    senseweb.default_error_func = function (XMLHttpRequest, textStatus, errorThrown) {
        alert("连接加密锁失败! ");
    };

    senseweb.default_success_func = function (data, statusText) {
        alert("连接加密锁成功! 接收到数据：" + data);
    };

    //使用的同/异步模式
    senseweb.is_async = false

    //配置使用的端口号
    senseweb.port = 9080;


    //默认同步模式
    //兼容IE8的跨域问题<script src="https://cdn.bootcss.com/jquery-ajaxtransport-xdomainrequest/1.0.4/jQuery.XDomainRequest.js"></script>
    senseweb.jquery_ajax_get = function (request_url, attach_header, is_async, success_fun, error_fun) {
        if (attach_header === undefined)
            attach_header = {};
        if (is_async === undefined)
            is_async = false;

        jQuery.support.cors = true;
        $.ajax({
            type: "GET",
            url: request_url,
            cache: false,
            headers: attach_header,
            async: is_async,
            success: success_fun,
            error: error_fun
        });
    };

    //默认同步模式
    //兼容IE8的跨域问题<script src="https://cdn.bootcss.com/jquery-ajaxtransport-xdomainrequest/1.0.4/jQuery.XDomainRequest.js"></script>
    senseweb.jquery_ajax_post = function (request_url, data_map, attach_header, is_async, success_fun, error_fun) {
        if (attach_header === undefined)
            attach_header = { "Content-Type": "application/x-www-form-urlencoded" };
        if (is_async === undefined)
            is_async = false;


        jQuery.support.cors = true;
        $.ajax({
            type: "POST",
            url: request_url,
            cache: false,
            headers: attach_header,
            async: is_async,
            data: data_map,
            success: success_fun,
            error: error_fun
        });
    };


    //获取版本号
    senseweb.get_version = function (success_fun, error_fun) {
        if (success_fun == undefined)
            success_fun = senseweb.default_success_func;
        if (error_fun == undefined)
            error_fun = senseweb.default_error_func;

        var request_url = senseweb.request_schema + senseweb.host + ":" + senseweb.port + senseweb.path + "/wbsrv/getVersion"
        senseweb.jquery_ajax_get(request_url, undefined, senseweb.is_async, success_fun, error_fun);

    };

    //获取所有硬/网络锁描述
    senseweb.get_all_desc = function (success_fun, error_fun) {
        if (success_fun == undefined)
            success_fun = senseweb.default_success_func;
        if (error_fun == undefined)
            error_fun = senseweb.default_error_func;

        var request_url = senseweb.request_schema + senseweb.host + ":" + senseweb.port + senseweb.path + "/control/slm_get_all_description";

        senseweb.jquery_ajax_get(request_url, undefined, senseweb.is_async, success_fun, error_fun);
    };

    //获取本地锁描述
    senseweb.get_local_desc = function (success_fun, error_fun) {
        if (success_fun == undefined)
            success_fun = senseweb.default_success_func;
        if (error_fun == undefined)
            error_fun = senseweb.default_error_func;

        var request_url = senseweb.request_schema + senseweb.host + ":" + senseweb.port + senseweb.path + "/control/slm_get_local_description";
        senseweb.jquery_ajax_get(request_url, undefined, senseweb.is_async, success_fun, error_fun);
    };

    /*
     * 获取指定描述的所有许可
     * tar_desc   指定json格式描述的字符串，可以使用get_local_desc获取
     * */
    senseweb.slm_read_brief_license_context = function (desc, success_fun, error_fun) {
        if (success_fun == undefined)
            success_fun = senseweb.default_success_func;
        if (error_fun == undefined)
            error_fun = senseweb.default_error_func;

        var request_url = senseweb.request_schema + senseweb.host + ":" + senseweb.port + senseweb.path + "/control/slm_read_brief_license_context";
        senseweb.jquery_ajax_post(request_url, { desc: desc }, undefined, senseweb.is_async, success_fun, error_fun);
    };

    /*
     * 获取本地锁指定描述的证书
     * tar_desc   指定json格式描述的字符串，可以使用get_local_desc获取
     * */
    senseweb.slm_ctrl_get_device_cert = function (local_desc, success_fun, error_fun) {
        if (success_fun == undefined)
            success_fun = senseweb.default_success_func;
        if (error_fun == undefined)
            error_fun = senseweb.default_error_func;


        var request_url = senseweb.request_schema + senseweb.host + ":" + senseweb.port + senseweb.path + "/control/slm_ctrl_get_device_cert";
        senseweb.jquery_ajax_post(request_url, { desc: local_desc }, undefined, senseweb.is_async, success_fun, error_fun);
    };

    /*
    * 查询所有设备证书链，会包含锁sn, 外壳号， 证书链
    */
    senseweb.get_local_device_p7b = function (success_fun, error_fun) {
        if (success_fun == undefined)
            success_fun = senseweb.default_success_func;
        if (error_fun == undefined)
            error_fun = senseweb.default_error_func;


        var request_url = senseweb.request_schema + senseweb.host + ":" + senseweb.port + senseweb.path + "/ext/getLocalDeviceP7B";
        senseweb.jquery_ajax_get(request_url, undefined, senseweb.is_async, success_fun, error_fun);
    };

    //升级硬件加密锁d2c
    senseweb.slm_updated2c = function (d2c_str, success_fun, error_fun) {
        if (success_fun == undefined)
            success_fun = senseweb.default_success_func;
        if (error_fun == undefined)
            error_fun = senseweb.default_error_func;

        var request_url = senseweb.request_schema + senseweb.host + ":" + senseweb.port + senseweb.path + "/update/slm_update";
        senseweb.jquery_ajax_post(request_url, { "d2c": d2c_str }, undefined, senseweb.is_async, success_fun, error_fun);
    }

    //登录云账号， 并记录一个g_user_guid供后续测试使用
    senseweb.ss_cloud_user_login = function (username, password, success_fun, error_fun) {
        if (success_fun == undefined)
            success_fun = senseweb.default_success_func;
        if (error_fun == undefined)
            error_fun = senseweb.default_error_func;

        var request_url = senseweb.request_schema + senseweb.host + ":" + senseweb.port + senseweb.path + "/cloudUser/ss_cloud_user_login";
        var header = { "username": username, "password": password };
        senseweb.jquery_ajax_post(request_url, header, undefined, senseweb.is_async, success_fun, error_fun);
    };

    //使用登录时获取的user_guid登出云账号
    senseweb.ss_cloud_user_logout = function (user_guid, success_fun, error_fun) {
        if (success_fun == undefined)
            success_fun = senseweb.default_success_func;
        if (error_fun == undefined)
            error_fun = senseweb.default_error_func;

        var request_url = senseweb.request_schema + senseweb.host + ":" + senseweb.port + senseweb.path + "/cloudUser/ss_cloud_user_logout";
        senseweb.jquery_ajax_post(request_url, { "user_guid": user_guid }, undefined, senseweb.is_async, success_fun, error_fun);
    };

    /*
     * 使用登录时获取的user_guid获取所有云软锁描述
     */
    senseweb.get_cloud_desc = function (user_guid, success_fun, error_fun) {
        if (success_fun == undefined)
            success_fun = senseweb.default_success_func;
        if (error_fun == undefined)
            error_fun = senseweb.default_error_func;

        var request_url = senseweb.request_schema + senseweb.host + ":" + senseweb.port + senseweb.path + "/control/slm_get_cloud_description";
        senseweb.jquery_ajax_post(request_url, { "user_guid": user_guid }, undefined, senseweb.is_async, success_fun, error_fun);
    };

    /*
     * 使用指定的软锁描述进行在线绑定许可id, 该许可id必须属于该软锁描述，可以使用slm_read_brief_license_context确认是否包含该许可
     */
    senseweb.slm_bind_offline_license = function (slock_desc, lic_id, success_fun, error_fun) {
        if (success_fun == undefined)
            success_fun = senseweb.default_success_func;
        if (error_fun == undefined)
            error_fun = senseweb.default_error_func;

        var request_url = senseweb.request_schema + senseweb.host + ":" + senseweb.port + senseweb.path + "/control/slm_bind_offline_license";
        senseweb.jquery_ajax_post(request_url, { "desc": slock_desc, "license_id": lic_id }, undefined, senseweb.is_async, success_fun, error_fun);

    };


    // 获取所有已绑定的软锁信息
    senseweb.get_smart_binded_desc = function (success_fun, error_fun) {
        if (success_fun == undefined)
            success_fun = senseweb.default_success_func;
        if (error_fun == undefined)
            error_fun = senseweb.default_error_func;

        var request_url = senseweb.request_schema + senseweb.host + ":" + senseweb.port + senseweb.path + "/control/slm_get_offline_desc";
        senseweb.jquery_ajax_get(request_url, undefined, senseweb.is_async, success_fun, error_fun);
    };

    /*
     * 在线解绑软锁，
     * 如何确定某一个许可id与哪个软锁描述关联。
     *      0。 登录指定的账号 可以获取当前用户的user_guid  == g_user_guid.
     *      1。 使用slm_get_offline_desc 查询所有已绑定的软锁许可信息, 可以获取到每个已绑定软锁的 developer_id, user_guid 和 type
     *      2。 使用步骤1中确认出的软锁描述 + 需要解绑的id, 进行解绑
     */
    senseweb.slm_unbind_offline_license = function (slock_binded_desc, lic_id, success_fun, error_fun) {
        if (success_fun == undefined)
            success_fun = senseweb.default_success_func;
        if (error_fun == undefined)
            error_fun = senseweb.default_error_func;

        var request_url = senseweb.request_schema + senseweb.host + ":" + senseweb.port + senseweb.path + "/control/slm_unbind_offline_license";
        senseweb.jquery_ajax_post(request_url, { "desc": slock_binded_desc, "license_id": lic_id }, undefined, senseweb.is_async, success_fun, error_fun);
    };

    /*
     * 获取当前计算机c2d信息 
     * */
    senseweb.slm_offline_bind_c2d = function (success_fun, error_fun) {
        if (success_fun == undefined)
            success_fun = senseweb.default_success_func;
        if (error_fun == undefined)
            error_fun = senseweb.default_error_func;

        var request_url = senseweb.request_schema + senseweb.host + ":" + senseweb.port + senseweb.path + "/control/slm_offline_bind_c2d";
        senseweb.jquery_ajax_get(request_url, undefined, senseweb.is_async, success_fun, error_fun);
    };

    //使用许可id，c2d和软锁相关的desc 获取d2c数据包
    senseweb.slm_cloud_offline_get_d2c = function (slock_desc, c2d, lic_id, success_fun, error_fun) {
        if (success_fun == undefined)
            success_fun = senseweb.default_success_func;
        if (error_fun == undefined)
            error_fun = senseweb.default_error_func;


        var request_url = senseweb.request_schema + senseweb.host + ":" + senseweb.port + senseweb.path + "/control/slm_cloud_offline_get_d2c";
        var postmap = { "desc": slock_desc, "license_id": lic_id, "c2d": c2d }
        senseweb.jquery_ajax_post(request_url, postmap, undefined, senseweb.is_async, success_fun, error_fun);
    };

    //绑定d2c
    senseweb.slm_offline_bind_d2c = function (d2c_str, success_fun, error_fun) {
        if (success_fun == undefined)
            success_fun = senseweb.default_success_func;
        if (error_fun == undefined)
            error_fun = senseweb.default_error_func;

        var request_url = senseweb.request_schema + senseweb.host + ":" + senseweb.port + senseweb.path + "/control/slm_offline_bind_d2c";
        senseweb.jquery_ajax_post(request_url, { "d2c": d2c_str }, undefined, senseweb.is_async, success_fun, error_fun);
    };


    //使用硬件锁做数据签名，签名时函数内部使用了SENSELOCK前缀，所以真实的签名数据为: SENSELOCK{VERIFY_DATA}
    senseweb.slm_ctrl_sign_by_device = function (desc, verify_data, success_fun, error_fun) {
        if (success_fun == undefined)
            success_fun = senseweb.default_success_func;
        if (error_fun == undefined)
            error_fun = senseweb.default_error_func;


        var request_url = senseweb.request_schema + senseweb.host + ":" + senseweb.port + senseweb.path + "/control/slm_ctrl_sign_by_device";
        var postmap = { "desc": desc, "verify_data": verify_data }
        senseweb.jquery_ajax_post(request_url, postmap, undefined, senseweb.is_async, success_fun, error_fun);
    };

    //获取加密锁公开区数据大小
    senseweb.slm_get_pub_size = function (desc, license_id, success_fun, error_fun) {
        if (success_fun == undefined)
            success_fun = senseweb.default_success_func;
        if (error_fun == undefined)
            error_fun = senseweb.default_error_func;


        var request_url = senseweb.request_schema + senseweb.host + ":" + senseweb.port + senseweb.path + "/control/slm_get_pub_size";
        var postmap = { "desc": desc, "license_id": license_id }
        senseweb.jquery_ajax_post(request_url, postmap, undefined, senseweb.is_async, success_fun, error_fun);
    };

    //获取加密锁公开区数据内容
    senseweb.slm_read_pub_data = function (desc, license_id, offset, length, success_fun, error_fun) {
        if (success_fun == undefined)
            success_fun = senseweb.default_success_func;
        if (error_fun == undefined)
            error_fun = senseweb.default_error_func;


        var request_url = senseweb.request_schema + senseweb.host + ":" + senseweb.port + senseweb.path + "/control/slm_read_pub_data";
        var postmap = { "desc": desc, "license_id": license_id, "offset": offset, "length": length }
        senseweb.jquery_ajax_post(request_url, postmap, undefined, senseweb.is_async, success_fun, error_fun);
    };
    return senseweb;
}