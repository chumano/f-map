<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>.:: F-MAP ::.</title>
    <!-- base library -->
    <link rel="stylesheet" type="text/css" href="css/ext-all.css" />
    <link rel='stylesheet' type="text/css" href='css/control_style.css' />
    <!-- ** Javascript ** -->
    <!---------------Utils--------------->
    <script type="text/javascript" src="utils/jscoord-1.1.1.js"></script>
    <script type="text/javascript" src="http://maps.google.com/maps/api/js?v=3.5&amp;sensor=false"></script>
    <!------------------------------------------>
    <script type="text/javascript" src="js/globalvar.js"></script>
    <script type="text/javascript" src="js/flib.js"></script>
    <script type="text/javascript" src="js/connect.js"></script>
    <script type="text/javascript" src="js/autocomplete_address_combobox.js"></script>
    <!---------- ExtJS library: base/adapter ----------->
    <script type="text/javascript" src="js/ext-base.js"></script>
    <script type="text/javascript" src="js/ext-all-debug.js"></script>
    <%-----------OpenLayers--------------%>
    <script type='text/javascript' src='openlayers/draw.js'></script>
    <script type='text/javascript' src='js/googleUtils.js'></script>
    <script type='text/javascript' src='openlayers/OpenLayers.js'></script>
    <script type="text/javascript" src="openlayers/mapinfo.js"></script>
    <script type="text/javascript" src="js/ui.js"></script>
    <script type='text/javascript' src='openlayers/map.js'></script>
    
    <script type="text/javascript">
        function test() {
            var url = serverURL + "?action=Test";
            req = getAjax();

            req.onreadystatechange = function () {
                if (req.readyState == 4 && req.status == 200) {
                    alert(req.responseText);
                }

            }
            req.open('GET', url, true);
            req.send(null);
        }

        function GoogleMap() {
            var p1 = new OpenLayers.Geometry.Point(106.61547, 10.80296);
            var p2 = new OpenLayers.Geometry.Point(106.70262, 10.71830);
            var p3 = new OpenLayers.Geometry.Point(106.72717, 10.77373);
            var start = new google.maps.LatLng(10.80296, 106.61547);
            var end = new google.maps.LatLng(10.71830, 106.70262);
            var points = [];
            points.push(p1);
            points.push(p2); points.push(p3);
            drawLineFromPoints(points);

            findRoute(start, end);
        }

        function initAll() {

            init();

            //getMapView();

        }
    </script>

    <!-- ** CSS ** -->
    <style type="text/css">
        

    </style>
</head>
<body onload="initAll()">
    <!-- use class="x-hide-display" to prevent a brief flicker of the content -->
    <!--------- WEST ------------->
    <div id="west" class="x-hide-display">
        <p>Hi. I'm the west panel.</p>
    </div>

    <!--------- NORTH ------------->
    <div id="north">
    </div>

    <!--------- CENTER ------------->
    <div id="center">
        <div id="map" style="width: 100%; height: 595px; background-color:#000">
        </div>
        <div id="wrapper">
            <div id="location" style="position: fixed; bottom: 0px; right: 0px; z-index: 1001">
            </div>
            <div id="scale" style="position: fixed; bottom: 0px; left: 307px; z-index: 1001">
           
            </div>
        </div>
    </div>

    <!----------- FLOATING ------------->
    <div style="position: fixed; top: 20px; right: 20px; z-index: 1001">
        <input type="button" id="googleButton" onclick="GoogleMap()" value="Google" />
        <input type="button" id="show-btn" onclick="test()" value="Test" />
    </div>

    <div id="winmenu" class="x-hidden">
    </div>
    <div id="wininfo" class="x-hidden">
    </div>
    <div id="winwards" class="x-hidden">
    </div>

    <div id="panZoomCont" style="position:fixed;top:125px;right:0px"></div> 

</body>
</html>
