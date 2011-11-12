<%@ Page Language="C#" AutoEventWireup="true" CodeFile="index.aspx.cs" Inherits="index" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
    <head>
        <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
        <title id='title'>Map Web</title>

<!-- ** CSS ** -->
        <!-- base library -->
        <link rel="stylesheet" type="text/css" href="css/ext-all.css" />

<!-- ** Javascript ** -->
        <script type="text/javascript" src="js/flib.js"></script>
        <script type="text/javascript" src="js/globalvar.js"></script>
        <!---------- ExtJS library: base/adapter ----------->
        <script type="text/javascript" src="js/ext-base.js"></script>
        <script type="text/javascript" src="js/ext-all-debug.js"></script>
        
        <!-- connect -->
         <script type="text/javascript" src="js/connect.js"></script>

        <!--window-->
        <%--<script type="text/javascript" src="js/lcombobox.js"></script>--%>
         <script type="text/javascript" src="openlayers/mapinfo.js"></script>
         <script type="text/javascript" src="js/interface.js"></script>
        
        <%-----------OpenLayers--------------%>
        <script type='text/javascript' src='openlayers/OpenLayers.js'></script>
        <script type='text/javascript' src='openlayers/map.js'></script>
    </head>


<body onload='init()'>
<div id='map' style='width: 100%; height:628px;'></div>
<div id="wrapper">
    <div id="location" style="position:fixed; bottom:0px; right:0px; z-index:1001"></div>
    <div id="scale" style="position:fixed; bottom:0px; left:0px; z-index:1001"></div>
</div>

<div style="position:fixed; top:20px; right:20px; z-index:1001">
<input type="button" id="show-btn" value="menu" />
</div>

<div id="winmenu" class="x-hidden"></div>
<div id="wininfo" class="x-hidden"></div>

<div id="winwards" class="x-hidden"></div>

<div id="search_window" class="x-hidden"></div>
<div id="test"></div>

</body>
</html>
