var request_session = document.getElementById("lockinfo_required");
var random_value = request_session.getAttribute("random_value");
var guid = request_session.getAttribute("guid");
var schema = request_session.getAttribute("schema");
var need_pub = request_session.getAttribute("need_pub");
var licid = request_session.getAttribute("licid");
var checkparh = request_session.getAttribute("checkpath");

need_pub = parseInt(need_pub, 10);
var sense_web = SenseWeb();
sense_web.port = 9080;
sense_web.request_schema = schema;

var license_id = parseInt(licid, 0);
var local_desc = undefined;
var lock_sn = undefined;
var shell_num = undefined;
var verify_data = undefined;
var pub_data_size = undefined;
var pub_data = undefined;

function verify_param() {
    if (shell_num === undefined || lock_sn === undefined || verify_data === undefined)
        return;
    if (need_pub && (pub_data_size === undefined || pub_data === undefined))
        return;
    form = {
        "shell_num": shell_num,
        "lock_sn": lock_sn,
        "verify_data": verify_data,
    };
    if (need_pub) {
        form["pub_data"] = pub_data;
        form["pub_data_size"] = pub_data_size;
    }
    var authform = $("<form></form>");
    authform.attr("method", "post");
    authform.attr("action", "/" + checkparh + "?guid=" + guid);

    $.each(form, function (key, value) {
        var input = $("<input>");
        input.attr("type", "hidden");
        input.attr("name", key);
        input.val(value);
        $(document.body).append(authform);
        authform.append(input);
    }
    );
    $(document.body).append(authform);
    authform.submit();
}



function getlockcertchain() {
    sense_web.get_local_device_p7b(
        function success(data, statusText) {
            console.log("获取证书链：");
            var json = JSON.parse(data);
            console.log("certlist: " + data);
            if (json["code"] === 0) {
                var detail = JSON.parse(json["data"])[0];
                shell_num = detail["shell_num"];
                lock_sn = detail["lock_sn"];
            }
            else {
                alert("获取加密锁证书链出错： " + json["code"]);
                return;
            }

            verify_param();
        }
    );
}

function getlocksign(signdata) {
    sense_web.slm_ctrl_sign_by_device(local_desc, signdata,
        function success(data, statusText) {
            var json = JSON.parse(data);
            console.log("获取签名数据: " + data);

            if (json["code"] === 0) {
                verify_data = json["data"];
            }
            else {
                alert("获取加密锁签名出错：" + json["code"]);
                return;
            }

            verify_param();
        }
    );
}

function getlock_pubdata() {
    sense_web.slm_get_pub_size(local_desc, license_id,
        function (data, statusText) {
            var json = JSON.parse(data);
            console.log("获取公开区数据大小: " + data);

            if (json["code"] === 0) {
                pub_data_size = json["data"];
            }
            else {
                alert("获取公开区数据大小出错：" + json["code"]);
                return;
            }
        }
    );

    sense_web.slm_read_pub_data(local_desc, license_id, 0, pub_data_size,
        function (data, statusText) {
            var json = JSON.parse(data);
            console.log("获取公开区数据: " + data);

            if (json["code"] === 0) {
                pub_data = json["data"];
            }
            else {
                alert("获取公开区数据出错：" + json["code"]);
                return;
            }
        }
    );

    verify_param();
}


function getlockinfo() {

    sense_web.get_local_desc(
        function success(data, statusText) {
            console.log("获取本地锁描述: ");
            var json = JSON.parse(data);

            //获取第一个锁描述用于下面的测试
            if (json["code"] === 0) {
                local_desc = JSON.parse(json["data"]);
                if (local_desc.length === 0) {
                    alert("未发现加密锁");
                    return;
                }
                else if (local_desc.length !== 1) {
                    alert("发现多把加密锁，请拔出其中一把后重试");
                    return;
                }

                local_desc = JSON.stringify(JSON.parse(json["data"])[0]);

                //获取证书链
                getlockcertchain();

                //获取加密锁签名数据，需要先使用base64进行编码
                var signdata = window.btoa("SENSELOCK" + random_value);
                getlocksign(signdata);

                //获取许可公开区数据, 如果需要获取加密锁中授权的公开区数据时，需要去掉注释。
                if (need_pub)
                    getlock_pubdata();
            }
        }
    );
}