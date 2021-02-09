var request_session = document.getElementById("pub_data_required");
var checkparh = request_session.getAttribute("checkpath");
var guid = request_session.getAttribute("guid");
var licid = request_session.getAttribute("licid");

var sense_web = SenseWeb();
sense_web.port = 9080;
sense_web.request_schema = "http://";

var license_id = parseInt("20210115", 0);
var pub_data_size = undefined;
var pub_data = undefined;
var local_desc = undefined;

function verify_param() {
    
    if (pub_data_size === undefined || pub_data === undefined)
        return;
    form = {
        "pub_data": pub_data,
        "pub_data_size": pub_data_size,
    };
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



function getlock_pubdata() {
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
            }
        }
    );
    sense_web.slm_get_pub_size(local_desc, license_id,
        function (data, statusText) {
            var json = JSON.parse(data);
            console.log("获取公开区数据大小: " + data);

            if (json["code"] === 0) {
                pub_data_size = json["data"];
                alert("pub_data_size" + ":" + pub_data_size);
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
            console.log("获取公开区数据: "+":" + data);

            if (json["code"] === 0) {
                pub_data = json["data"];
                alert("pub_data" +":"+ pub_data);
            }
            else {
                alert("获取公开区数据出错：" + json["code"]);
                return;
            }
            verify_param();
        }
    );
   
}