var map;
var tiled;
var pureCoverage = false;

// pink tile avoidance
OpenLayers.IMAGE_RELOAD_ATTEMPTS = 5;
// make OL compute scale according to WMS spec
OpenLayers.DOTS_PER_INCH = 25.4 / 0.28;

function init() {

    // if this is just a coverage or a group of them, disable a few items,
    // and default to jpeg format
    format = 'image/png';

    var bounds = new OpenLayers.Bounds(
                    105.93, 10.758,
                    105.961, 10.801
                );
    var options = {
        controls: [],
        maxExtent: bounds,
        numZoomLevels: 6,
        maxResolution: 0.00016796875,
        projection: "EPSG:4326",
        units: 'degrees'
    };
    map = new OpenLayers.Map('map', options);

    // setup tiled layer
    tiled = new OpenLayers.Layer.WMS(
                    "Geoserver layers", "http://localhost:8080/geoserver/wms",
                    {
                        LAYERS: 'sde:QUAN1_RG_HCXA',
                        STYLES: 'Quan1_Style',
                        format: format,
                        tiled: !pureCoverage,
                        tilesOrigin: map.maxExtent.left + ',' + map.maxExtent.bottom
                    },
                    {
                        buffer: 0,
                        displayOutsideMaxExtent: true,
                        isBaseLayer: true
                    }
           );

   map.addLayers([tiled]);

    // build up all controls
    map.addControl(new OpenLayers.Control.PanZoomBar({
        position: new OpenLayers.Pixel(2, 15)
    }));
    map.addControl(new OpenLayers.Control.Navigation());
    map.addControl(new OpenLayers.Control.Scale($('scale')));
    map.addControl(new OpenLayers.Control.MousePosition({ element: $('location') }));
    map.zoomToExtent(bounds);


}
