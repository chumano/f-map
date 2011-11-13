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
        numZoomLevels: 6,
        maxResolution: 0.00016796875,
        projection: "EPSG:4326",
        units: 'degrees'
    };
    //Default map is Quan 1 : mapid=1
    mapid = 1;
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

            // layer for points
            vectorLayer = new OpenLayers.Layer.Vector("Point Layer");
            var layerNames = '', styleNames = '';
            for (var i = 0; i < jSon.layers.length; i++) {
                if (i != 0) { layerNames += ','; styleNames += ','; };
                layerNames += jSon.layers[i].Layer;
                styleNames += jSon.layers[i].StyleName;
            }
            var layers = new OpenLayers.Layer.WMS(
                    "Geoserver layers", "http://localhost:8080/geoserver/wms",
                    {
                        LAYERS: layerNames, //'sde:QUAN1_RG_HCXA',
                        STYLES: styleNames, //'Quan1_Style',
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

            map.addLayers([layers, vectorLayer]);

            //add Control   
            map.addControl(new OpenLayers.Control.DrawFeature(vectorLayer, OpenLayers.Handler.Point));
            // build up all controls
            map.addControl(new OpenLayers.Control.PanZoomBar({
                position: new OpenLayers.Pixel(2, 15)
            }));
            map.addControl(new OpenLayers.Control.Navigation());
            map.addControl(new OpenLayers.Control.Scale($('scale')));
            map.addControl(new OpenLayers.Control.MousePosition({ element: $('location') }));
            map.zoomToExtent(bounds);

            //add handler
            map.events.register('click', map, function (e) {
                //BBOX: map.getExtent().toBBOX(),
                //X: e.xy.x,
                //Y: e.xy.y,
                //QUERY_LAYERS: map.layers[0].params.LAYERS,
                //Layers: 'sde:QUAN1_RG_HCXA',
                //WIDTH: map.size.w,
                //HEIGHT: map.size.h,

                var bbox = change2Str(map.getExtent().toBBOX());
                var x = e.xy.x, y = e.xy.y;
                var layer = change2Str('sde:QUAN1_RG_HCXA');
                var w = map.size.w, h = map.size.h;
                var actions = '[{"name":"action","value":"GetInfo"}'
                                    + ',{"name":"bbox","value":' + bbox + '}'
                                    + ',{"name":"x","value":' + x + '}'
                                    + ',{"name":"y","value":' + y + '}'
                                    + ',{"name":"layer_name","value":' + layer + '}'
                                    + ',{"name":"width","value":' + w + '}'
                                    + ',{"name":"height","value":' + h + '}'
                                    + ']';
                getInfo(actions, showInfo);

            });

           
        }
    );
    
    
}
