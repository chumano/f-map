<%@ Control Language="C#" AutoEventWireup="true" Inherits="DotNetNuke.Modules.AJAXToolkit"
    CodeFile="View.ascx.cs" %>
<head>
    <!--
        <script type="text/javascript" src="OpenLayers.js"></script>
        <script type="text/javascript" src="MyJScript.js"></script>
    -->

        <script type="text/javascript">
            var map, drawControls;
            function init() {
                //alert("chumano");
                map = new OpenLayers.Map('map');

                var rule = new OpenLayers.Rule({
                /*
                  filter: new OpenLayers.Filter.Comparison({
                      type: OpenLayers.Filter.Comparison.EQUAL_TO,
                      property: "MAHC",
                      value: 76026743
                  }),
                  */
                      symbolizer: { fillColor: "#ff0000"}
                  });

                  var defaultStyle = new OpenLayers.Style({
                      fillColor: "#ff0000"
                  });

                  var selectStyle = new OpenLayers.Style({
                      fillColor: "#00ff00"
                    });

                  var styleMap = new OpenLayers.StyleMap({ 'default': defaultStyle,
                      'select': selectStyle
                  });

                // var style = new OpenLayers.Style();
                // style.addRules([rule]);

                  // var styleMap = new OpenLayers.StyleMap({'default': style});


                var wmsLayer = new OpenLayers.Layer.WMS(
                    "OpenLayers WMS",
                    "http://localhost:8080/geoserver/wms",
                    { layers: 'sde:SDE.SDE.QUAN1_RG_HCXA', styles: 'quan1_hcxa' }
                );

                map.addControl(new OpenLayers.Control.MousePosition({ element: $('location') }));

                map.addLayer(wmsLayer);

                map.setCenter(new OpenLayers.Bounds(105.93, 10.758, 105.962, 10.798).getCenterLonLat(), 14);

                /*
                  var bounds = new OpenLayers.Bounds(
                    105.93, 10.758,
                    105.962, 10.798
                );
                  var options = {
                      controls: [],
                      maxExtent: bounds,
                      maxResolution: 0.00015625,
                      projection: "EPSG:4326",
                      units: 'degrees'
                  };
                  map = new OpenLayers.Map('map', options);

                  // setup tiled layer
                  tiled = new OpenLayers.Layer.WMS(
                    "thesis:SDE.SDE.QUAN1_RG_HCXA - Tiled", "http://localhost:8080/geoserver/wms",
                    {
                        LAYERS: 'thesis:SDE.SDE.QUAN1_RG_HCXA',
                        STYLES: 'quan1_hcxa',
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
                    "thesis:SDE.SDE.QUAN1_RG_HCXA - Untiled", "http://localhost:8080/geoserver/wms",
                    {
                        LAYERS: 'thesis:SDE.SDE.QUAN1_RG_HCXA',
                        STYLES: 'quan1_hcxa',
                        format: format
                    },
                    { singleTile: true, ratio: 1, isBaseLayer: true }
                );

                    map.addLayers([untiled, tiled]);

                    // build up all controls
                    map.addControl(new OpenLayers.Control.PanZoomBar({
                        position: new OpenLayers.Pixel(2, 15)
                    }));
                    map.addControl(new OpenLayers.Control.Navigation());
                    map.addControl(new OpenLayers.Control.Scale($('scale')));
                    map.addControl(new OpenLayers.Control.MousePosition({ element: $('location') }));
                    map.zoomToExtent(bounds);
                    */

            }
        </script>
</head>
<body onload="init()">
<div>
    <h2>
        Calling Web Methods</h2>
    <table>
        <tr align="left">
            <td>
                Method that does not return a value:</td>
            <td>
                <!-- Getting no retun value from 
                            the Web service. -->
                <button id="Button1" onclick="GetNoReturn()">
                    No Return</button>
            </td>
        </tr>
        <tr align="left">
            <td>
                Method that returns a value:</td>
            <td>
                <!-- Getting a retun value from 
                            the Web service. -->
                <button id="Button2" onclick="GetTime(); return false;">
                    Server Time</button>
            </td>
        </tr>
        <tr align="left">
            <td>
                Method that takes parameters:</td>
            <td>
                <!-- Passing simple parameter types to 
                            the Web service. -->
                <button id="Button3" onclick="Add(20, 30); return false;">
                    Add</button>
            </td>
        </tr>
        <tr align="left">
            <td>
                Method that returns XML data:</td>
            <td>
                <!-- Get Xml. -->
                <button id="Button4" onclick="GetXmlDocument(); return false;">
                    Get Xml</button>
            </td>
        </tr>
        <tr align="left">
            <td>
                Method that uses GET:</td>
            <td>
                <!-- Making a GET Web request. -->
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
<asp:TextBox ID="TextBox1" runat="server" Width="328px"></asp:TextBox>
<asp:Button ID="Button6" runat="server" Text="Button" />
<p>
    &nbsp;</p>
<div id="map"  style='width: 100%; height: 500px;'>
</div>
</body>

