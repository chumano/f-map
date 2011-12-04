// perform Ajax HTTP GET request 
// actions: a string in JSON format which contains parameters of the request
// func: callback to execute when get response
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

// handle FeatureInfo response
function getInfoWhenClickOnMap(actions, func) {
    var url = serverURL + para2Str(actions);
    req = getAjax();

    req.onreadystatechange = function () {
        if (req.readyState == 4 && req.status == 200) {
            func(req.responseText);
        }

    }
    req.open('GET', url, true);
    req.send(null);
}

function processFeatureInfo(response) {
    // alert(response);
    var jSon;
    var myObject = "jSon=" + response;
    eval(myObject);

    var str = '';
    for (var i = 0; i < jSon[0].Attrs.length &&jSon[0].Attrs[i].Name!='SHAPE'; i++) {
        str += jSon[0].Attrs[i].Name + " : " + jSon[0].Attrs[i].Value + '<br/>';
    }

    //update info - SHOWDIV
    infoWin.show();
    infoWin.body.update(str);

    //UTM2LatLng
    //    var utm1 = new UTMRef(X1, Y1, "N", 48);
    //    var ll1 = utm1.toLatLng();
    //    var location1 = new OpenLayers.Geometry.Point(ll1.lat, ll1.lng);

    //LatLng2UTM
    //    var latlng1 = new LatLng(lat, lng);
    //    var utm1 = latlng1.toUTMRef();
    //    var X1 = utm1.easting;
    //    var Y1 = utm1.northing;

    //get coordinates
    if (jSon[0].Attrs[jSon[0].Attrs.length - 1].Name == 'SHAPE') {
        var coors = jSon[0].Attrs[jSon[0].Attrs.length - 1].Value.split(' ');
        var pointList = [];
        for (var j = 0; j < coors.length; j++) {
            var xy = coors[j].split(',');
            var utm1 = new UTMRef(xy[0], xy[1], "N", 48);
            var ll1 = utm1.toLatLng();
            var point = new OpenLayers.Geometry.Point(ll1.lng, ll1.lat);

            pointList.push(point);
        }
        pointList.push(pointList[0]);

        var linearRing = new OpenLayers.Geometry.LinearRing(pointList);
        var polygonFeature = new OpenLayers.Feature.Vector(
                new OpenLayers.Geometry.Polygon([linearRing]), null, null);

        //var vectorLayer1 = new OpenLayers.Layer.Vector("Simple Geometry");

        //var point1 = new OpenLayers.Geometry.Point(106.69210, 10.78148);
        //alert(pointList[0].x + " - " + pointList[0].y);
        //var pointFeature = new OpenLayers.Feature.Vector(point1, null, null);

        //map.addLayer(vectorLayer1);
        vectorLayer.addFeatures([polygonFeature]);
        //alert("here");
    }
}

function showInfo(reponsewards) {
    //[{"name":"Mã hành chính","value":"7.6026755E7"},{"name":"Tên","value":"Phường Cô Giang"},{"name":"Số hộ","value":"4185"}]
    //alert(reponsewards);
    var jSon;
    var myObject = "jSon=" + reponsewards;
    eval(myObject);

    var str = '';
    for (var i = 0; i < jSon.length; i++) {
        if (i != 0) str += '<br/>';
        str += jSon[i].name + ' : '
                    + '<b>' + jSon[i].value + '</b>';

    }

    tabPanel.setActiveTab(1);
    tabInfo.update(str);
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

// change map layers when switch map view
// response: response string
function changeMap(response) {
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
    //var baseLayer = map.baseLayer;
    layers.destroy();

    layers = new OpenLayers.Layer.WMS(
                    "Geoserver layers", hostURL,
                    {
                        LAYERS: layerNames,
                        STYLES: styleNames,
                        format: format,
                        tiled: true,
                        transparent: true,
                        tilesOrigin: map.maxExtent.left + ',' + map.maxExtent.bottom
                    },
                    {
                        buffer: 0,
                        displayOutsideMaxExtent: true,
                        isBaseLayer: true
                    }
           );

    currentLayerName = layerNames;
    map.addLayer(layers);
    //map.setBaseLayer(newlayers);
    //alert(map.getNumLayers());

    map.zoomToExtent(bounds);

    //////////////////////////////////////////////////////////////////
    //update infor
    //updateInfo();
    //if (mapid == 0) return; //hardcode

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

            //tabPanel.setActiveTab(1);
            //tabInfo.update(str);
        }
    );


}

// send request to change current map
// mid: id of map view
function changeMapRequest(mid) {
    //clear FeatureVector
    vectorLayer.destroyFeatures();
   //ShowDIV
   // tabPanel.setActiveTab(1);
   // tabInfo.update('');
    //
    mapid = mid;
    var actions = '[{"name":"action","value":"GetMap"},{"name":"map_id","value":' + mapid + '}]';
    getInfo(actions, changeMap);
}

// Creating object of XMLHTTP
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
