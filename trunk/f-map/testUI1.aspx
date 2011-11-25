<%@ Page Language="C#" AutoEventWireup="true" CodeFile="testUI1.aspx.cs" Inherits="testUI1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
  <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>

  <link rel="stylesheet" type="text/css" href="css/ext-all.css"/>
  <script type="text/javascript" src="js/ext-base.js"></script>
  <script type="text/javascript" src="js/ext-all-debug.js"></script>
  <script type='text/javascript' src='openlayers/OpenLayers.js'></script>
  <script type='text/javascript' src='openlayers/map1.js'></script>


  <title id="page-title">TestUI1</title>
 
  <!-- Layout Mandatory Styles -->
  <style type="text/css">
    #ct-wrap {
      text-align:center;
    }
    #ct {
      text-align:left;
      margin:auto;
    }
    #north, #navlinks, #south {
      float:none;
      clear:both;
    }
    #west {
      float:left;
      clear:left;
    }
    #center {
      float:left;
    }
  </style>
 
  <!-- Mandatory Dimensions -->
  <style type="text/css">
    #ct {
      width:100%;
    }
    #west {
      width:300px;
    }
    #center {
      width: 100%;
    }
  </style>
 
  <!-- This Example Customizations -->
  <style type="text/css">
    body {
      font-size:13px;
      vertical-align:middle;
      margin:0;
      padding:0;
    }
    #north {
      height:40px;
      color:white;
      background:#203d8e;
    }
    #navlinks {
      height: 24px;
      color:blue;
      background:#ffc800;
    }
    #west, #center {
      
    }
    #west {
      color:white;
      position:fixed; top:62px; left:0;
      z-index:1001;
    }
    #south {
      height: 30px;
      background:#ffc800;
    }
    #center {
      background:#e8e8e8;
    }
  </style>
 
  <!-- source area styles -->
  <style type="text/css">
    pre {
      border:1px solid silver;
      border-left-width: 8px;
      background:#f4f4f4;
      font-size:11px;
    }
  </style>

  <script type="text/javascript">
<!--
      var viewportwidth;
      var viewportheight;
      
//-->
</script>

  <script type="text/javascript">
      function initUI() {
          resize();
          initMap();
      }
        
      function resize() {
          if (typeof window.innerWidth != 'undefined') {
              viewportwidth = window.innerWidth,
          viewportheight = window.innerHeight
          }

          // IE6 in standards compliant mode (i.e. with a valid doctype as the first line in the document)
          else if (typeof document.documentElement != 'undefined'
                && typeof document.documentElement.clientWidth != 'undefined'
                && document.documentElement.clientWidth != 0) {
              viewportwidth = document.documentElement.clientWidth,
            viewportheight = document.documentElement.clientHeight
          }

          // older versions of IE
          else {
              viewportwidth = document.getElementsByTagName('body')[0].clientWidth,
            viewportheight = document.getElementsByTagName('body')[0].clientHeight
          }

          var s = document.getElementById('south');
          s.innerHTML = 'Your viewport width is ' + viewportwidth + 'x' + viewportheight;
          var c = document.getElementById('center')
          var height = viewportheight - 94;
          c.style.height = height +'px';
          var lw = document.getElementById('l_west');
          var rw = document.getElementById('r_west');
          lw.style.height = height + 'px';
          rw.style.height = height + 'px';

          //map.updateSize();
      }

      window.onload = initUI;
      window.onresize = resize;

      function hideWest() {
          var w = document.getElementById('l_west');
          var c = document.getElementById('showWest');
          if (w.style.display != 'none') {
              w.style.display = 'none';
              c.style.display = '';
          }
      }

      function showWest() {
          var w = document.getElementById('l_west');
          var c = document.getElementById('showWest');
          if (w.style.display == 'none') {
              w.style.display = '';
              c.style.display = 'none';
          }
      }
  </script>
</head>


<body>
<div id="ct-wrap">
  <div id="ct">
    <div id="north">north</div>
    <div id="navlinks">navlinks</div>
    <div id="center" align="center"></div>
    <div id="south">south</div>
  </div>
</div>

<div id="west">
    <table><tr>
    <td><div id="l_west" style="width:200px; background-color:Navy">
        <div align="center">
        ---L-WEST---
        <img alt="AAA" src="images/icon_search1.png" onclick="hideWest()" align="right"></img>
        </div>
        <hr />
        Content
    </div></td>

    <td><div id="r_west" style="width:100px;top:62px;" align="center">
        <br />
        R-WEST
    </div></td>
    </tr></table>
</div>

<div id="showWest" style="position:fixed;top:62px; left:0;display:none; z-index:1001">
    <img alt="AAA" src="images/icon_search1.png" onclick="showWest()"></img>
</div>


</body>
</html>
<!-- eof -->
