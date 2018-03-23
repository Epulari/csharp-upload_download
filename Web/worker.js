/**
 * Created by Ep on 2018/3/22.
 */
onmessage=function(evt){
    var URL = evt.data;
    var xmlhttp = new createXMLHTTP();
    xmlhttp.open("POST", URL, false);
    xmlhttp.onreadystatechange = function () {
        if (xmlhttp.readyState == 4) {
            var res=xmlhttp.responseText;
            postMessage(res);
        }
    }
    xmlhttp.send(null);
};

function createXMLHTTP() {
    return new XMLHttpRequest();
}