<%@ Control Language="C#" AutoEventWireup="true" Inherits="DotNetNuke.Modules.AJAXToolkit"
    CodeFile="View.ascx.cs" %>
<head>
    <!--
        <script type="text/javascript" src="OpenLayers.js"></script>
        <script type="text/javascript" src="MyJScript.js"></script>
    -->
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

<div id="map"  style='width: 100%; height: 800px;'>
</div>

 <div id="nodelist">
            <em>Click on the map to get feature info</em>
        </div>
</body>

