// pink tile avoidance
OpenLayers.IMAGE_RELOAD_ATTEMPTS = 5;
// make OL compute scale according to WMS spec
OpenLayers.DOTS_PER_INCH = 25.4 / 0.28;

function init() {


    var bounds = new OpenLayers.Bounds(
                    105.93, 10.758,
                    105.961, 10.801
                );
    var options = {
        controls: [],
        //maxExtent: bounds,
        numZoomLevels: 10,
        maxResolution: 0.0009,//0.00016796875,
        projection: "EPSG:4326",
        units: 'degrees'
    };
    //Default map is Quan 1 : mapid=1
    mapid = 0;
    //?action=GetMap&map_id=1
    var actions = '[{"name":"action","value":"GetMap"},{"name":"map_id","value":' + mapid + '}]';
    getInfo(actions,
        function (response) {
            var jSon;
            var myObject = "jSon=" + response;
            eval(myObject);
            //lay bound de tao options
            bounds = new OpenLayers.Bounds(
                    jSon.bound.MinX, jSon.bound.MinY, jSon.bound.MaxX, jSon.bound.MaxY
                );

            options.maxExtent = bounds;

            //tao map
            map = new OpenLayers.Map('map', options);

            //---------------------Build up cac layers--------------------------------
            // basic layers 
            var layerNames = '', styleNames = '';
            for (var i = 0; i < jSon.layers.length; i++) {
                if (i != 0) { layerNames += ','; styleNames += ','; };
                layerNames += jSon.layers[i].Layer;
                styleNames += jSon.layers[i].StyleName;
            }

            layers = new OpenLayers.Layer.WMS(
                    "Geoserver layers", hostURL,
                    {
                        LAYERS: layerNames, //'sde:QUAN1_RG',
                        STYLES: styleNames, //'Quan1_Style',
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

            //google layer
            gmap = new OpenLayers.Layer.Google(
                "Google Streets", // the default
                {numZoomLevels: 20,
                projection: "EPSG:900913"

            }
            );

            // layer for points
            var renderer = OpenLayers.Util.getParameters(window.location.href).renderer;
            renderer = (renderer) ? [renderer] : OpenLayers.Layer.Vector.prototype.renderers;
            var style_green = {
                strokeColor: "#00FF00",
                strokeWidth: 3,
                strokeDashstyle: "dashdot",
                pointRadius: 6,
                pointerEvents: "visiblePainted"
            };
            vectorLayer = new OpenLayers.Layer.Vector("Point Layer");

            // marker Layer
            markersLayer = new OpenLayers.Layer.Markers("Markers");

            // Selected Layer
            selectedLayer = new OpenLayers.Layer.Vector("Selection", { styleMap:
                new OpenLayers.Style(OpenLayers.Feature.Vector.style["select"])
            });
            var hover = new OpenLayers.Layer.Vector("Hover");

            // -----add all Layers----
            map.addLayers([layers, vectorLayer, selectedLayer, hover, markersLayer]);

            //-------------------------------build up all controls---------------------------------- 
            //Switcher Control
            map.addControl(new OpenLayers.Control.LayerSwitcher());

            //Draw feature Control-0
            map.addControl(new OpenLayers.Control.DrawFeature(vectorLayer, OpenLayers.Handler.Point));

            //PanZoom Control-1
            map.addControl(new OpenLayers.Control.PanZoomBar({
                position: new OpenLayers.Pixel(2, 15)
            }));
            //Navigation Control-2
            map.addControl(new OpenLayers.Control.Navigation());

            //Scale Control-3
            map.addControl(new OpenLayers.Control.Scale($('scale')));

            //Mouse position Control-4
            map.addControl(new OpenLayers.Control.MousePosition({ element: $('location') }));

            //---------------------------------------------------------------------------------------
            map.zoomToExtent(bounds);
            //map.zoomToMaxExtent();

            //add handler
            map.events.register('click', map, function (e) {
                //BBOX: map.getExtent().toBBOX(),
                //X: e.xy.x,
                //Y: e.xy.y,
                //QUERY_LAYERS: map.layers[0].params.LAYERS,
                //Layers: 'sde:QUAN1_RG',
                //WIDTH: map.size.w,
                //HEIGHT: map.size.h,
                //xoa het features hien tai
                if (mapid == 0) return;
                vectorLayer.destroyFeatures();

                var bbox = change2Str(map.getExtent().toBBOX());
                var x = e.xy.x, y = e.xy.y;
                var layer = change2Str(currentLayerName); //change2Str('sde:QUAN1_RG'); //hardcode
                var w = map.size.w, h = map.size.h;
                var actions = '[{"name":"action","value":"GetInfo"}'
                                    + ',{"name":"bbox","value":' + bbox + '}'
                                    + ',{"name":"x","value":' + x + '}'
                                    + ',{"name":"y","value":' + y + '}'
                                    + ',{"name":"layer_name","value":' + layer + '}'
                                    + ',{"name":"width","value":' + w + '}'
                                    + ',{"name":"height","value":' + h + '}'
                                    + ']';
                getInfoWhenClickOnMap(actions, processFeatureInfo);

            });

//            map.events.register('zoomend', this, function (event) {
//                var x = map.getZoom();
//                tabPanel.setActiveTab(1);
//                tabInfo.update("Zoom: "+x);
//                if (x > 15) {
//                    map.zoomTo(15);
//                }
//            });

            /////////////////////////////////////////////////////////////
            getMapView();
        }
    );


}


function getAllStreet() {
    url = 'Server.aspx?action=GetAllStreet';
    req = getAjax();
    req.onreadystatechange = function () {
        if (req.readyState == 4 && req.status == 200) {
            var jSon
            var myObject = "jSon=" + req.responseText;
            eval(myObject);

            allAddress = [];
            var dataStore = [];
            for (var i = 0; i < jSon.length; i++) {
                var addr = [i, jSon[i].StreetName + ", " + jSon[i].WardName + ", " + jSon[i].DistrictName] ;
                dataStore.push(addr);
                allAddress.push([jSon[i].NoName, jSon[i].IDWard]);
            }
            comboAddress.store = new Ext.data.SimpleStore({
                fields: ['id', 'address'],
                data: dataStore
            });
            // comboAddress.store.filter('address', "a");
        }

    }

    req.open('GET', url, true);
    req.send(null);
}

function getMapView() {
    url = 'Server.aspx?action=GetMapView';
    req = getAjax();
    req.onreadystatechange = function () {
        if (req.readyState == 4 && req.status == 200) {
            var jSon;
            var myObject = "jSon=" + req.responseText;
            eval(myObject);

            districts = [];
            for (var i = 0; i < jSon.length; i++) {
                var row = jSon[i];
                var num = i + 1;
                var arr = [row.ID, row.Name,row.NoName];
                districts.push(arr);

            }

            var dataStore = new Ext.data.ArrayStore({
                fields: ['id', 'district','icon'],
                data: districts
            });


            comboDistricts.store = dataStore;
            //alert("here");

            var tpl='<tpl for="."><div class="x-combo-list-item">'
            + '<table><tbody><tr>'
            + '<td>'
            + '<div><img src="images/district_icons/{icon}.png"/></div></td>'
            + '<td>{district}</td>'
            + '</tr></tbody></table>'
            + '</div></tpl>';
            comboDistricts.tpl = tpl;

            ///////////////////////////////
            getAllStreet();
        }

    }

    req.open('GET', url, true);
    req.send(null);
}
