<%@ Control Language="C#" AutoEventWireup="true" Inherits="DotNetNuke.Modules.AJAXToolkit"
    CodeFile="View.ascx.cs" %>
<head>
    <!--
        <script type="text/javascript" src="OpenLayers.js"></script>
        <script type="text/javascript" src="MyJScript.js"></script>
    -->
    <script type="text/javascript">
        <script defer="defer" type="text/javascript">
            var map;
            var untiled;
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
                if (pureCoverage) {
                    document.getElementById('filterType').disabled = true;
                    document.getElementById('filter').disabled = true;
                    document.getElementById('antialiasSelector').disabled = true;
                    document.getElementById('updateFilterButton').disabled = true;
                    document.getElementById('resetFilterButton').disabled = true;
                    document.getElementById('jpeg').selected = true;
                    format = "image/jpeg";
                }

                var bounds = new OpenLayers.Bounds(
                    105.93, 10.758,
                    105.961, 10.801
                );
                var options = {
                    controls: [],
                    maxExtent: bounds,
                    maxResolution: 0.00016796875,
                    projection: "EPSG:4326",
                    units: 'degrees'
                };
                map = new OpenLayers.Map('map', options);

                // setup tiled layer
                tiled = new OpenLayers.Layer.WMS(
                    "sde:SDE.DBO.QUAN1_RG_HCXA - Tiled", "http://localhost:8080/geoserver/wms",
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

                // setup single tiled layer
                untiled = new OpenLayers.Layer.WMS(
                    "sde:SDE.DBO.QUAN1_RG_HCXA - Untiled", "http://localhost:8080/geoserver/wms",
                    {
                        LAYERS: 'sde:QUAN1_RG_HCXA',
                        STYLES: 'Quan1_Style',
                        format: format
                    },
                    { singleTile: true, ratio: 1, isBaseLayer: true }
                );

               // map.addLayers([untiled, tiled]);
                map.addLayers([untiled]);
                // build up all controls
                map.addControl(new OpenLayers.Control.PanZoomBar({
                    position: new OpenLayers.Pixel(2, 15)
                }));
                map.addControl(new OpenLayers.Control.Navigation());
                map.addControl(new OpenLayers.Control.Scale($('scale')));
                map.addControl(new OpenLayers.Control.MousePosition({ element: $('location') }));
                map.zoomToExtent(bounds);

                // wire up the option button
                var options = document.getElementById("options");
                options.onclick = toggleControlPanel;

                // support GetFeatureInfo
                map.events.register('click', map, function (e) {
                    document.getElementById('nodelist').innerHTML = "Loading... please wait...";
                    var params = {
                        REQUEST: "GetFeatureInfo",
                        EXCEPTIONS: "application/vnd.ogc.se_xml",
                        BBOX: map.getExtent().toBBOX(),
                        SERVICE: "WMS",
                        VERSION: "1.1.1",
                        X: e.xy.x,
                        Y: e.xy.y,
                        INFO_FORMAT: 'text/html',
                        QUERY_LAYERS: map.layers[0].params.LAYERS,
                        FEATURE_COUNT: 50,
                        Layers: 'sde:SDE.DBO.QUAN1_RG_HCXA',
                        WIDTH: map.size.w,
                        HEIGHT: map.size.h,
                        format: format,
                        styles: map.layers[0].params.STYLES,
                        srs: map.layers[0].params.SRS
                    };
                    // merge filters
                    if (map.layers[0].params.CQL_FILTER != null) {
                        params.cql_filter = map.layers[0].params.CQL_FILTER;
                    }
                    if (map.layers[0].params.FILTER != null) {
                        params.filter = map.layers[0].params.FILTER;
                    }
                    if (map.layers[0].params.FEATUREID) {
                        params.featureid = map.layers[0].params.FEATUREID;
                    }
                    OpenLayers.loadURL("http://localhost:8080/geoserver/wms", params, this, setHTML, setHTML);
                    OpenLayers.Event.stop(e);
                });
            }

            // sets the HTML provided into the nodelist element
            function setHTML(response) {
                document.getElementById('nodelist').innerHTML = response.responseText;
            };

            // shows/hide the control panel
            function toggleControlPanel(event) {
                var toolbar = document.getElementById("toolbar");
                if (toolbar.style.display == "none") {
                    toolbar.style.display = "block";
                }
                else {
                    toolbar.style.display = "none";
                }
                event.stopPropagation();
                map.updateSize()
            }

            // Tiling mode, can be 'tiled' or 'untiled'
            function setTileMode(tilingMode) {
                if (tilingMode == 'tiled') {
                    untiled.setVisibility(false);
                    tiled.setVisibility(true);
                    map.setBaseLayer(tiled);
                }
                else {
                    untiled.setVisibility(true);
                    tiled.setVisibility(false);
                    map.setBaseLayer(untiled);
                }
            }

            // Transition effect, can be null or 'resize'
            function setTransitionMode(transitionEffect) {
                if (transitionEffect === 'resize') {
                    tiled.transitionEffect = transitionEffect;
                    untiled.transitionEffect = transitionEffect;
                }
                else {
                    tiled.transitionEffect = null;
                    untiled.transitionEffect = null;
                }
            }

            // changes the current tile format
            function setImageFormat(mime) {
                // we may be switching format on setup
                if (tiled == null)
                    return;

                tiled.mergeNewParams({
                    format: mime
                });
                untiled.mergeNewParams({
                    format: mime
                });
                /*
                var paletteSelector = document.getElementById('paletteSelector')
                if (mime == 'image/jpeg') {
                paletteSelector.selectedIndex = 0;
                setPalette('');
                paletteSelector.disabled = true;
                }
                else {
                paletteSelector.disabled = false;
                }
                */
            }

            // sets the chosen style
            function setStyle(style) {
                // we may be switching style on setup
                if (tiled == null)
                    return;

                tiled.mergeNewParams({
                    styles: style
                });
                untiled.mergeNewParams({
                    styles: style
                });
            }

            function setAntialiasMode(mode) {
                tiled.mergeNewParams({
                    format_options: 'antialias:' + mode
                });
                untiled.mergeNewParams({
                    format_options: 'antialias:' + mode
                });
            }

            function setPalette(mode) {
                if (mode == '') {
                    tiled.mergeNewParams({
                        palette: null
                    });
                    untiled.mergeNewParams({
                        palette: null
                    });
                }
                else {
                    tiled.mergeNewParams({
                        palette: mode
                    });
                    untiled.mergeNewParams({
                        palette: mode
                    });
                }
            }

            function setWidth(size) {
                var mapDiv = document.getElementById('map');
                var wrapper = document.getElementById('wrapper');

                if (size == "auto") {
                    // reset back to the default value
                    mapDiv.style.width = null;
                    wrapper.style.width = null;
                }
                else {
                    mapDiv.style.width = size + "px";
                    wrapper.style.width = size + "px";
                }
                // notify OL that we changed the size of the map div
                map.updateSize();
            }

            function setHeight(size) {
                var mapDiv = document.getElementById('map');

                if (size == "auto") {
                    // reset back to the default value
                    mapDiv.style.height = null;
                }
                else {
                    mapDiv.style.height = size + "px";
                }
                // notify OL that we changed the size of the map div
                map.updateSize();
            }

            function updateFilter() {
                if (pureCoverage)
                    return;

                var filterType = document.getElementById('filterType').value;
                var filter = document.getElementById('filter').value;

                // by default, reset all filters
                var filterParams = {
                    filter: null,
                    cql_filter: null,
                    featureId: null
                };
                if (OpenLayers.String.trim(filter) != "") {
                    if (filterType == "cql")
                        filterParams["cql_filter"] = filter;
                    if (filterType == "ogc")
                        filterParams["filter"] = filter;
                    if (filterType == "fid")
                        filterParams["featureId"] = filter;
                }
                // merge the new filter definitions
                mergeNewParams(filterParams);
            }

            function resetFilter() {
                if (pureCoverage)
                    return;

                document.getElementById('filter').value = "";
                updateFilter();
            }

            function mergeNewParams(params) {
                tiled.mergeNewParams(params);
                untiled.mergeNewParams(params);
            }

        </script>
</head>
<body onload="init()">
<!--
<div>
    <h2>
        Calling Web Methods</h2>
    <table>
        <tr align="left">
            <td>
                Method that does not return a value:</td>
            <td>
                <button id="Button1" onclick="GetNoReturn()">
                    No Return</button>
            </td>
        </tr>
        <tr align="left">
            <td>
                Method that returns a value:</td>
            <td>
                <button id="Button2" onclick="GetTime(); return false;">
                    Server Time</button>
            </td>
        </tr>
        <tr align="left">
            <td>
                Method that takes parameters:</td>
            <td>
                <button id="Button3" onclick="Add(20, 30); return false;">
                    Add</button>
            </td>
        </tr>
        <tr align="left">
            <td>
                Method that returns XML data:</td>
            <td>
                <button id="Button4" onclick="GetXmlDocument(); return false;">
                    Get Xml</button>
            </td>
        </tr>
        <tr align="left">
            <td>
                Method that uses GET:</td>
            <td>
                <button id="Button5" onclick="MakeGetRequest(); return false;">
                    Make GET Request</button>
            </td>
        </tr>
    </table>
</div>
<hr />
<div>
    <span id="ResultId"></span>
</div>
-->
<asp:TextBox ID="TextBox1" runat="server" Width="328px"></asp:TextBox>
<asp:Button ID="Button6" runat="server" Text="Button" />
<p>
    &nbsp;</p>
<div id="map"  style='width: 100%; height: 500px;'>
</div>
</body>

