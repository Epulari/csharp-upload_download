/**
 * Created by Ep on 2018/3/22.
 */
$("#ctlBtnDown").unbind("click").bind("click", function () {
    DownloadFile();
});

function DownloadFile() {
    var loading = layer.load(1);
    var jsontext = encodeURI('{"functionname":"downloadfile"}');
    var URL = WebServiceAddress_download + jsontext;
    //创建同步对象
    var worker = new Worker("worker.js");
    worker.postMessage(URL);
    worker.onmessage = function (p1) {
        var res=p1.data;
        $("<a>",{
            id:'downloada',
            href: PID + res,
            text: "123",
            style: "display: none;"
        }).appendTo("#downloadfile");
        layer.close(loading);
        document.getElementById("downloada").click();
    };
}