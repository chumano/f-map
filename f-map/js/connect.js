var serverURL = 'Server.aspx';

function getMap(actions) {
    var url = serverURL + para2Str(actions);
    req = getAjax();

    req.onreadystatechange = function () {

        if (req.readyState == 4 && req.status == 200) {
            alert(req.responseText);
        }

    }

    req.open('GET', url, true);
    req.send(null);
}

function getInfo(actions) {
    var url = serverURL + para2Str(actions);
    req = getAjax();

    req.onreadystatechange = function () {

        if (req.readyState == 4 && req.status == 200) {
            alert(req.responseText);
        }

    }
    req.open('GET', url, true);
    req.send(null);
}

function query(actions) {  
    var url =  serverURL + para2Str(actions);//?action=TenDuong&u=' + item;

    // url = 'http://localhost:8080/geoserver/wms?bbox=-130,24,-66,50&styles=population&Format=image/png&request=GetMap&layers=topp:states&width=550&height=250&srs=EPSG:4326';
    req = getAjax();

    req.onreadystatechange = function () {

        if (req.readyState == 4 && req.status == 200) {

            //obj = getElement("userlist");
            //obj.innerHTML = req.responseText;
            alert(req.responseText);

            // add canvas into map object
            map.addLayer(new Layer);

//            var jSon;
//            var myObject = "jSon=" + req.responseText;
//            eval(myObject);
//            var str = "";
//            for (var i = 0; i < jSon.TABLE[0].ROW.length; i++) {
//                str += jSon.TABLE[0].ROW[i].COL[0].DATA;
//                str += "<br/>";
//            }
//            //alert(myObject);
//            document.getElementById("div1").innerHTML = str;
//            show("div1");

        }

    }

    req.open('GET', url, true);
    req.send(null);
}

function getAjax() {
    var XmlHttp;

    //Creating object of XMLHTTP in IE
    try {
        XmlHttp = new ActiveXObject("Msxml2.XMLHTTP");
    }
    catch (e) {
        try {
            XmlHttp = new ActiveXObject("Microsoft.XMLHTTP");
        }
        catch (oc) {
            XmlHttp = null;
        }
    }
    //Creating object of XMLHTTP in Mozilla and Safari 
    if (!XmlHttp && typeof XMLHttpRequest != "undefined") {
        XmlHttp = new XMLHttpRequest();
    }
    return XmlHttp;
}

//actions is an array : [{"name":attrName1,"value":attrValue1},{"name":attrName2,"value":attrValue2}]
function para2Str(actions) {
    if (actions == '') return '';

    var jSon;
    var myObject = "jSon=" + actions;
    eval(myObject);

    var resultStr = '?';
    for (var i = 0; i < jSon.length; i++) {
        if (i != 0)
            resultStr += '&';

        resultStr += jSon[i].name + '=' + jSon[i].value;
    }

    return resultStr;
}
