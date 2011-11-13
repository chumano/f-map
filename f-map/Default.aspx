<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>{--- F-MAP ---}</title>
<!-- ** CSS ** -->
        <!-- base library -->
        <link rel="stylesheet" type="text/css" href="css/ext-all.css" />

<!-- ** Javascript ** -->
        <script type="text/javascript" src="js/globalvar.js"></script>
        <script type="text/javascript" src="js/flib.js"></script>
        <script type="text/javascript" src="js/connect.js"></script>
        
        <!---------- ExtJS library: base/adapter ----------->
        <script type="text/javascript" src="js/ext-base.js"></script>
        <script type="text/javascript" src="js/ext-all-debug.js"></script>

         <%-----------OpenLayers--------------%>
        <script type='text/javascript' src='openlayers/OpenLayers.js'></script>
        
        <script type="text/javascript" src="openlayers/mapinfo.js"></script>
        <script type="text/javascript" src="js/ui.js"></script>
      
        <script type='text/javascript' src='openlayers/map.js'></script>

</head>
<body onload="init()">
    <!-- use class="x-hide-display" to prevent a brief flicker of the content -->
    <div id="west" class="x-hide-display" >
        <p>Hi. I'm the west panel.</p>
    </div>
    <div id="center" >
        <div id="map" style='width: 100%; height:634px;'></div>
        <div id="wrapper">
            <div id="location" style="position:fixed; bottom:0px; right:0px; z-index:1001"></div>
            <div id="scale" style="position:fixed; bottom:0px; left:307px; z-index:1001"></div>
        </div>
    </div>
    <div id="props-panel" class="x-hide-display" style="width:200px;height:200px;overflow:hidden;">
    </div>


    

<div style="position:fixed; top:20px; right:20px; z-index:1001">
<input type="button" id="show-btn" value="menu" />
</div>

<div id="winmenu" class="x-hidden"></div>
<div id="wininfo" class="x-hidden"></div>

<div id="winwards" class="x-hidden"></div>

</body>
</html>
