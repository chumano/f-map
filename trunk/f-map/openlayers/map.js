// TODO:
// pink tile avoidance
OpenLayers.IMAGE_RELOAD_ATTEMPTS = 5;
// make OL compute scale according to WMS spec
OpenLayers.DOTS_PER_INCH = 25.4 / 0.28;

function init() {
    var bounds = new OpenLayers.Bounds(105.93, 10.758, 105.961, 10.801);
    var options = {
        controls: [],
        numZoomLevels: 10,
        maxResolution: 0.0009,//0.00016796875,
        projection: "EPSG:4326",
        units: 'degrees'
    };
	
    // Default map is 0 which means all districts
    mapid = 0;
	
    // ?action=GetMap&map_id=1
    var actions = '[{"name":"action","value":"GetMap"},{"name":"map_id","value":' + mapid + '}]';
    getInfo(actions,
        function (response) {
            var jSon;
            var myObject = "jSon=" + response;
            eval(myObject);

            //lay bound de tao options
            bounds = new OpenLayers.Bounds(jSon.bound.MinX, jSon.bound.MinY, jSon.bound.MaxX, jSon.bound.MaxY);

            options.maxExtent = bounds;

            //tao map
            map = new OpenLayers.Map('map', options);

            // Build up layers
            // basic layers 
            var layerNames = '', styleNames = '';
            for (var i = 0; i < jSon.layers.length; i++) {
                if (i != 0) { layerNames += ','; styleNames += ','; };
                layerNames += jSon.layers[i].Layer;
                styleNames += jSon.layers[i].StyleName;
            }

            layers = new OpenLayers.Layer.WMS(
                    "Geoserver layers",
					hostURL,
                    {
                        LAYERS: layerNames, //'sde:QUAN1_RG',
                        STYLES: styleNames, //'Quan1_Style',
                        format: format,
                        tiled: true,
                        transparent: true,
                        tilesOrigin: map.maxExtent.left + ',' + map.maxExtent.bottom
                    },
                    {
                        buffer: 2,
                        displayOutsideMaxExtent: true,
                        isBaseLayer: true
                    }
            );

            // Google layer
            gmap = new OpenLayers.Layer.Google(
                "Google Streets", // the default
                {
                numZoomLevels: 20,
                projection: new OpenLayers.Projection("EPSG:900913"),
                displayProjection: new OpenLayers.Projection("EPSG:4326")
            },
				{
				    isBaseLayer: false
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

            // draggable layer
            draggableFeatureLayer = new OpenLayers.Layer.Vector('DraggableFeature Layer');

            // marker Layer
            markersLayer = new OpenLayers.Layer.Markers("Markers");

            // add all layers
            map.addLayers([layers, vectorLayer, markersLayer, draggableFeatureLayer]);

            var dragFeature = new OpenLayers.Control.DragFeature(draggableFeatureLayer);
            dragFeature.onDrag = function (feature, pixel) {
                googleFindRoute();
            };

            dragFeature.onComplete = function (feature, pixel) {
                googleFindRoute();
                map.zoomToExtent(global_bounds);
            };

            map.addControl(dragFeature);
            dragFeature.activate();

            // Switcher Control
            // map.addControl(new OpenLayers.Control.LayerSwitcher());

            // add controls to map
            map.addControl(new OpenLayers.Control.PanZoomBar());
            map.addControl(new OpenLayers.Control.Navigation());
            map.addControl(new OpenLayers.Control.ScaleLine());
            map.addControl(new OpenLayers.Control.OverviewMap());
            map.addControl(new OpenLayers.Control.Scale($('scale')));
            map.addControl(new OpenLayers.Control.MousePosition({ element: $('location') }));

            // TODO add edit map panel
            /*
            var DeleteFeature = OpenLayers.Class(OpenLayers.Control, {
            initialize: function (layer, options) {
            OpenLayers.Control.prototype.initialize.apply(this, [options]);
            this.layer = layer;
            this.handler = new OpenLayers.Handler.Feature(
            this, layer, { click: this.clickFeature }
            );
            },
            clickFeature: function (feature) {
            // if feature doesn't have a fid, destroy it
            if (feature.fid == undefined) {
            this.layer.destroyFeatures([feature]);
            } else {
            feature.state = OpenLayers.State.DELETE;
            this.layer.events.triggerEvent("afterfeaturemodified",
            { feature: feature });
            feature.renderIntent = "select";
            this.layer.drawFeature(feature);
            }
            },
            setMap: function (map) {
            this.handler.setMap(map);
            OpenLayers.Control.prototype.setMap.apply(this, arguments);
            },
            CLASS_NAME: "OpenLayers.Control.DeleteFeature"
            });
            var saveStrategy = new OpenLayers.Strategy.Save();
            saveStrategy.activate();

            wfs = new OpenLayers.Layer.Vector("Editable Features", {
            strategies: [new OpenLayers.Strategy.BBOX(), saveStrategy],
            projection: new OpenLayers.Projection("EPSG:4326"),
            protocol: new OpenLayers.Protocol.WFS({
            version: "1.1.0",
            srsName: "EPSG:4326",
            url: "http://demo.opengeo.org/geoserver/wfs",
            featureNS: "http://opengeo.org",
            featureType: "restricted",
            geometryName: "the_geom",
            schema: "http://demo.opengeo.org/geoserver/wfs/DescribeFeatureType?version=1.1.0&typename=og:restricted"
            })
            });

            map.addLayers([wfs]);

            var panel = new OpenLayers.Control.Panel({
            displayClass: 'customEditingToolbar',
            allowDepress: true
            });

            var draw = new OpenLayers.Control.DrawFeature(
            wfs, OpenLayers.Handler.Polygon,
            {
            title: "Draw Feature",
            displayClass: "olControlDrawFeaturePolygon",
            multi: true
            }
            );

            var edit = new OpenLayers.Control.ModifyFeature(wfs, {
            title: "Modify Feature",
            displayClass: "olControlModifyFeature"
            });

            var del = new DeleteFeature(wfs, { title: "Delete Feature" });

            var save = new OpenLayers.Control.Button({
            title: "Save Changes",
            trigger: function () {
            if (edit.feature) {
            edit.selectControl.unselectAll();
            }
            saveStrategy.save();
            },
            displayClass: "olControlSaveFeatures"
            });

            panel.addControls([save, del, edit, draw]);
            map.addControl(panel);
            */

            //---------------------------------------------------------------------------------------
            map.zoomToExtent(bounds);

            // handle map click event
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

            // map.events.register('zoomend', this, function (event) {
            // var x = map.getZoom();
            // tabPanel.setActiveTab(1);
            // tabInfo.update("Zoom: "+x);
            // if (x > 15) {
            // map.zoomTo(15);
            // }
            // });

            // get all districts
            getMapView();
        }
    );
}

// TODO: change to getInfo(actions)
// get all street names
function getAllStreet() {
    url = 'Server.aspx?action=GetAllStreet';
    req = getAjax();
    req.onreadystatechange = function () {
        if (req.readyState == 4 && req.status == 200) {
            var jSon;
            var myObject = "jSon=" + req.responseText;
            eval(myObject);

            allAddress = [];
            var dataStore = [];
            for (var i = 0; i < jSon.length; i++) {
                var addr = [i, jSon[i].StreetName + ", " + jSon[i].WardName + ", " + jSon[i].DistrictName];
                dataStore.push(addr);
                allAddress.push([jSon[i].NoName, jSon[i].IDWard]);
            }
            comboAddress.store = new Ext.data.SimpleStore({
                fields: ['id', 'address'],
                data: dataStore
            });
            comboAddressStart.store = new Ext.data.SimpleStore({
                fields: ['id', 'address'],
                data: dataStore
            });
            comboAddressEnd.store = new Ext.data.SimpleStore({
                fields: ['id', 'address'],
                data: dataStore
            })
        }
    }

    req.open('GET', url, true);
    req.send(null);
}

// get all districts 
// TODO: change to getInfo(actions)
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
                var arr = [row.ID, row.Name];
                var num = i + 1;
                var arr = [row.ID, row.Name,row.NoName];
                districts.push(arr);
            }

            var dataStore = new Ext.data.ArrayStore({
                fields: ['id', 'district','icon'],
                data: districts
            });

            comboDistricts.store = dataStore;

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
