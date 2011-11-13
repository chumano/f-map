var serverURL = 'Server.aspx';

function getInfo(actions, func) {
    mask = new Ext.LoadMask(Ext.getBody(), { msg: "Đang tải..." });
    mask.show();

    var url = serverURL + para2Str(actions);
    req = getAjax();

    req.onreadystatechange = function () {
        if (req.readyState == 4 && req.status == 200) {
            func(req.responseText);
            mask.hide();
        }

    }
    req.open('GET', url, true);
    req.send(null);
}

function updateInfo() {
    var comboWard = Ext.getCmp('combo-ward');
    comboWard.clearValue();

    if (mapid == 0) {//toan thanh
        comboWard.hide();

    }
    else {
        comboWard.show();
        if (mapid != 1) {
            comboWard.store = null;
            alert("Chưa có dữ liệu");
            return;
        }

        var actions = '[{"name":"action","value":"GetWards"},{"name":"district_id","value":' + mapid + '}]';
        getInfo(actions,
                            function (reponsewards) {
                                comboWard.store = new Ext.data.ArrayStore({
                                    fields: ['id', 'ward'],
                                    data: str2Arr(reponsewards)
                                });
                            }
                        );

        //comboCity.store.filter('cid', combo.getValue());
    }
}

function changeMap(response) {
    //test response

    ///////////
    var jSon;
    var myObject = "jSon=" + response;
    eval(myObject);
    //lay bound de tao options
    bounds = new OpenLayers.Bounds(
                    jSon.bound.MinX, jSon.bound.MinY, jSon.bound.MaxX, jSon.bound.MaxY
                );
    map.maxExtent = bounds;

    var layerNames = '', styleNames = '';
    for (var i = 0; i < jSon.layers.length; i++) {
        if (i != 0) { layerNames += ','; styleNames += ','; };
        layerNames += jSon.layers[i].Layer;
        styleNames += jSon.layers[i].StyleName;
    }
    var newlayers = new OpenLayers.Layer.WMS(
                    "Geoserver layers", "http://localhost:8080/geoserver/wms",
                    {
                        LAYERS: layerNames,
                        STYLES: styleNames,
                        format: format,
                        tiled: true,
                        tilesOrigin: map.maxExtent.left + ',' + map.maxExtent.bottom
                    },
                    {
                        buffer: 0,
                        displayOutsideMaxExtent: true,
                        isBaseLayer: true
                    }
           );

    var baseLayer = map.baseLayer;
    baseLayer.destroy();
    map.addLayer(newlayers);
    map.setBaseLayer(newlayers);
    //alert(map.getNumLayers());

    map.zoomToExtent(bounds);

    //////////////////////////////////////////////////////////////////
    //update infor
    //updateInfo();
    var actions = '[{"name":"action","value":"GetWards"},{"name":"district_id","value":' + mapid + '}]';
    getInfo(actions,
        function (reponse) {
            var jSon;
            var myObject = "jSon=" + reponse;
            eval(myObject);

            var str = '';
            for (var i = 0; i < jSon.length; i++) {
                if (i != 0) str += '<br/>';
                str += '<i>' + jSon[i] + '</i>';
            }

            tabPanel.setActiveTab(1);
            tabInfo.update(str);
        }
    );
}

function changeMapRequest(mid) {
    mapid = mid;
    var actions = '[{"name":"action","value":"GetMap"},{"name":"map_id","value":' + mapid + '}]';
    getInfo(actions, changeMap );
}


function query(actions) {  
    var url =  serverURL + para2Str(actions);//?action=TenDuong&u=' + item;
    //?action=GetWards&district_id=1
    //?action=GetMap&map_id=1
    //?action=GetInfo&bbox=105.922404,10.756404,105.968596,10.802596&x=234&y=430&layer_name=sde:QUAN1_RG_HCXA&width=550&height=550
    //?action=SearchAddress&keyword=102%20/11,Tr%C3%A2%CC%80n%20Quang%20Kha%CC%89i
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
