<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Test.aspx.cs" Inherits="Test" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en" lang="en"> 
<head> 
<meta http-equiv="content-type" content="text/html;charset=utf-8" /> 
<script type="text/javascript" src="http://openlayers.org/api/OpenLayers.js"></script>
<style type="text/css">
#map {
    width: 600px;
    height: 450px;
    border: 1px solid black;
}
div.olControlPanel {
    top: 0px;
    left: 50px;
    position: absolute;
}
.olControlPanel div {
    display: block;
    width:  22px;
    height: 22px;
    border: thin solid black;
    margin-top: 10px;
    background-color: white
}
.helpButtonItemInactive {
    background-image: url("http://maps.peterrobins.co.uk/files/help.png");
}
.olControlMousePosition {
    position: absolute;
    right: 10px;
    top: 0px;
    height: 15px;
    font-size: 8pt;
    background-color: white
}
#utm_mouse {
    top: 15px;
}
div.editPanel {
    top: 100px;
    left: 50px;
    position: absolute;
}
    .editPanel div {
      background-image: url("http://maps.peterrobins.co.uk/files/edit_sprite.png");
      background-repeat: no-repeat;
      width:  22px;
      height: 22px;
      border: thin solid black;
      margin-top: 10px;
    }
.olControlNavigationItemActive {background-position: 0px -207px; }
.olControlNavigationItemInactive {background-position: 0px -184px; }
.lineButtonItemActive {background-position: 0px -69px; }
.lineButtonItemInactive {background-position: 0px -46px; }
.pointButtonItemActive {background-position: 0px -23px; }
.pointButtonItemInactive {background-position: 0px 0px; }
.olControlModifyFeatureItemActive {background-position: 0px -161px; }
.olControlModifyFeatureItemInactive {background-position: 0px -138px; }
.olControlDeleteFeatureItemActive { background-position: 0px -253px; }
.olControlDeleteFeatureItemInactive { background-position: 0px -230px; }
.olControlSplitItemActive {background-position: 0px -347px; }
.olControlSplitItemInactive {background-position: 0px -322px; }
.editButtonItemActive {background-position: 0px -115px; }
.editButtonItemInactive {background-position: 0px -92px; }
.saveButtonItemActive {background-position: 0px -299px; }
.saveButtonItemInactive {background-position: 0px -276px;}
</style>
<script type="text/javascript">
    window.onload = function () {
        var options = { controls: [
            new OpenLayers.Control.Navigation(),
            new OpenLayers.Control.KeyboardDefaults(),
            new OpenLayers.Control.PanZoomBar(),
            new OpenLayers.Control.Scale(),
            new OpenLayers.Control.Attribution()
        ]
        };
        var map = new OpenLayers.Map('map', options);
        map.projection = "EPSG:900913";
        map.displayProjection = new OpenLayers.Projection("EPSG:4326");
        map.numZoomLevels = 18;
        map.maxExtent = new OpenLayers.Bounds(-20037508.3427892, -20037508.3427892, 20037508.3427892, 20037508.3427892);
        map.addLayer(new OpenLayers.Layer.OSM());
        var panel = new OpenLayers.Control.Panel();
        panel.addControls([
            new OpenLayers.Control.Button({
                displayClass: "helpButton", trigger: function () { alert('help') }, title: 'Help'
            })
        ]);
        map.addControl(panel);
        map.addControl(new OpenLayers.Control.MousePosition({ id: "ll_mouse", formatOutput: formatLonlats }));
        map.addControl(new OpenLayers.Control.MousePosition({ id: "utm_mouse", prefix: "Mercator ", displayProjection: map.baseLayer.projection, numDigits: 0 }));

        var wgs84 = new OpenLayers.Projection("EPSG:4326");
        var defStyle = { strokeColor: "blue", strokeOpacity: "0.7", strokeWidth: 2, fillColor: "blue", pointRadius: 3, cursor: "pointer" };
        var sty = OpenLayers.Util.applyDefaults(defStyle, OpenLayers.Feature.Vector.style["default"]);
        var sm = new OpenLayers.StyleMap({
            'default': sty,
            'select': { strokeColor: "red", fillColor: "red" }
        });
        var saveStrategy = new OpenLayers.Strategy.Save();
        saveStrategy.events.register('success', null, saveSuccess);
        saveStrategy.events.register('fail', null, saveFail);
        var vectorLayer = new OpenLayers.Layer.Vector("Line Vectors", {
            styleMap: sm,
            eventListeners: {
                "featuresadded": dataLoaded
            },
            strategies: [
                new OpenLayers.Strategy.Fixed(),
                saveStrategy
            ],
            protocol: new OpenLayers.Protocol.HTTP({
                url: "http://maps.peterrobins.co.uk/cgi-bin/fs/workspace/",
                format: new OpenLayers.Format.GeoJSON({
                    ignoreExtraDims: true,
                    internalProjection: map.baseLayer.projection,
                    externalProjection: wgs84
                })
            })
        });
        map.addLayer(vectorLayer);

        var navControl = new OpenLayers.Control.Navigation({ title: 'Pan/Zoom' });
        var editPanel = new OpenLayers.Control.Panel({ displayClass: 'editPanel' });
        editPanel.addControls([
            new OpenLayers.Control.DrawFeature(vectorLayer, OpenLayers.Handler.Point, { displayClass: 'pointButton', title: 'Add point', handlerOptions: { style: sty} }),
            new OpenLayers.Control.DrawFeature(vectorLayer, OpenLayers.Handler.Path, { displayClass: 'lineButton', title: 'Draw line', handlerOptions: { style: sty} }),
            new OpenLayers.Control.ModifyFeature(vectorLayer, { title: 'Edit feature' }),
            new DeleteFeature(vectorLayer, { title: 'Delete Feature' }),
            new OpenLayers.Control.Split({ layer: vectorLayer, deferDelete: true, title: 'Split line' }),
            new OpenLayers.Control.Button({ displayClass: 'saveButton', trigger: function () { saveStrategy.save() }, title: 'Save changes' }),
            navControl
        ]);
        editPanel.defaultControl = navControl;
        map.addControl(editPanel);

        function saveSuccess(event) {
            alert('Changes saved');
        }
        function saveFail(event) {
            alert('Error! Changes not saved');
        }
        function dataLoaded(event) {
            this.map.zoomToExtent(event.object.getDataExtent());
        }

        function formatLonlats(lonLat) {
            var lat = lonLat.lat;
            var long = lonLat.lon;
            var ns = OpenLayers.Util.getFormattedLonLat(lat);
            var ew = OpenLayers.Util.getFormattedLonLat(long, 'lon');
            return ns + ', ' + ew + ' (' + (Math.round(lat * 10000) / 10000) + ', ' + (Math.round(long * 10000) / 10000) + ')';
        }
    }

    DeleteFeature = OpenLayers.Class(OpenLayers.Control, {
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
                this.layer.events.triggerEvent("afterfeaturemodified", { feature: feature });
                feature.renderIntent = "select";
                this.layer.drawFeature(feature);
            }
        },
        setMap: function (map) {
            this.handler.setMap(map);
            OpenLayers.Control.prototype.setMap.apply(this, arguments);
        },
        CLASS_NAME: "OpenLayers.Control.DeleteFeature"
    })
</script>
</head>
<body>
<div id="map"></div>
</body>
</html>

