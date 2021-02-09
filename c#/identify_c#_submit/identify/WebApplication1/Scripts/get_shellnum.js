var request_session = document.getElementById("shellnum_required");
var checkparh = request_session.getAttribute("checkpath");
var guid = request_session.getAttribute("guid");

var shell_num = undefined;
var sense_web = SenseWeb();
sense_web.port = 9080;
sense_web.request_schema = "http://";


function verify_param() {
    if (shell_num === undefined)
        return;
    form = {
        "shell_num": shell_num,
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

function get_shellnum(){
    sense_web.get_local_device_p7b(
        function success(data, statusText) {
            console.log("获取证书链：");
            var json = JSON.parse(data);
            console.log("certlist: " + data);
            if (json["code"] === 0) {
                var detail = JSON.parse(json["data"])[0];
                shell_num = detail["shell_num"];
                alert(shell_num);
            }
            else {
                alert("获取锁号出错： " + json["code"]);
                return;
            }

            verify_param();
        }
    );
}