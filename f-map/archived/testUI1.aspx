<%@ Page Language="C#" AutoEventWireup="true" CodeFile="testUI1.aspx.cs" Inherits="testUI1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link rel="stylesheet" type="text/css" href="css/ext-all.css" />
    <title id="page-title">TestUI1</title>
    <script type="text/javascript" src="js/jQuery-1.4.2.min.js"></script>
    <style type="text/css">
        body
        {
            font-size: 13px;
            vertical-align: middle;
            margin: 0;
            padding: 0;
        }
        
        #northDiv
        {
            height: 100px;
            color: white;
            background: #203d8e;
            z-index: 2000;
        }
        .shadow
        {
            -moz-box-shadow: 0 0 30px 5px #999;
            -webkit-box-shadow: 0 0 30px 5px #999;
        }
        
        #infoDiv
        {
            height: 24px;
            color: blue;
            opacity: 0.4;
            background-color: #fff;
        }
        
        #floatDiv
        {
            background-color: #eeffbb;
            position: relative;
            margin-left: 0px;
            z-index: 1001;
            width: 200px;
        }
        
        #mapDiv
        {
            width: 100%;
            position: relative;
            background-color: #ffe;
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

            var s = document.getElementById('mapDiv');
            s.innerHTML = 'Your viewport width is ' + viewportwidth + 'x' + viewportheight;
            var c = document.getElementById('centerDiv')
            var height = viewportheight - 100;
            c.style.height = height + 'px';
            var lw = document.getElementById('floatDiv');
            var rw = document.getElementById('mapDiv');
            lw.style.height = height + 'px';
            rw.style.height = height + 'px';

            //map.updateSize();
        }
        window.onresize = resize;
        window.onload = initUI;
        
        var showFloatDiv = true;
        function show_hideFloatDiv() {
            if (showFloatDiv) {
                hideWest();
                showFloatDiv = false;
            }
            else {
                showWest();
                showFloatDiv = true;
            }
        };

        function hideWest() {
            var w = document.getElementById('floatDiv');
            $("#floatDiv").animate({
                marginLeft: '-190px'
            }, 'slow', function () {
                // Animation complete.
            });
        }

        function showWest() {
            var w = document.getElementById('floatDiv');
            $("#floatDiv").animate({ marginLeft: '0px' }, 'slow');
        }
    </script>
</head>
<body>
    <div id="ct-wrap">
        <div id="ct">
            <div id="northDiv" class="shadow">
                north
                <div id="infoDiv">
                    info
                </div>
            </div>
            <div id="centerDiv">
                <table cellspacing="0" border="0">
                    <tr>
                        <td>
                            <div id="floatDiv">
                                ---L-WEST---
                                <img alt="AAA" src="images/icon_search1.png" onclick="show_hideFloatDiv()" align="right" />
                            </div>
                        </td>
                        <td style="width: 100%">
                            <div id="mapDiv">
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <div id="showWest" style="position: fixed; top: 62px; left: 0; display: none; z-index: 1001">
        <img alt="AAA" src="images/icon_search1.png" onclick="showWest()"></img>
    </div>
</body>
</html>
<!-- eof -->
