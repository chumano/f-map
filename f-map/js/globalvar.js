﻿//-------Map----------
var defaultCenterZoom = 5;
var format = 'image/png';
var hostURL = 'http://localhost:8080/geoserver/wms';

var map;
var mapid; //Xac dinh map nao duoc hien
var currentLayerName; //sde:Quan1_RG

var gmap; //google layer
var layers; // main layers
var vectorLayer; // Layer chua cac features
var markersLayer; // Layer chua cac markers
var selectedLayer; // Layer chua cac Features duoc chon

var selectControl;
var mask;

var currentPopup;
//--------------------
var allAddress;
var districts;  //danh sach cac mapview hien tai -- json

//--------UI----------
var labelInfo;
var searchWin;
var textField;

var tabPanel;
var tabSearchAddress;
var tabInfo;

var comboDistricts;
var comboAddress;

var menuWin;
var infoWin;
var wardsWin;