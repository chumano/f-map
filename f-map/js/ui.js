Ext.onReady(function () {
    Ext.state.Manager.setProvider(new Ext.state.CookieProvider());

    // information window
    infoWin = new Ext.Window({
        layout: 'fit',
        title: 'Thông tin',
        width: 300,
        height: 400,
        draggable: true,
        closeAction: 'hide',
        plain: true,
        border: false,
        header: false,
        resizable: true,
        autoScroll: true,
        x: 2,
        y: 89
    });

    // Search address
    comboAddress = createAutoCompleteAddressCombobox(
        600,
        15, 5, 15, 205,  // top, right, bottom, left
        'Nhập địa chỉ bạn muốn tìm'
    );

    btnSearchAddress = new Ext.Button({
        text: 'Tìm kiếm',
        height: 22,
        margins: {
            top: 15,
            right: 5,
            bottom: 15,
            left: 5
        },
        handler: function () { checkAddress(comboAddress, 0); },
        icon: "images/icon_search.png"
    });
    ///////////////////////////////////////////////////////////////////////////////////////////////

    // Find route
    comboAddressStart = createAutoCompleteAddressCombobox(
        300,
        15, 5, 15, 100,  // top, right, bottom, left
        'Địa chỉ bắt đầu'
    );

    comboAddressStart.on('select', function (combo, value) {
        checkAddress(combo, 1);
    });

    comboAddressEnd = createAutoCompleteAddressCombobox(
        300,
        15, 5, 15, 10,  // top, right, bottom, left
        'Địa chỉ kết thúc'
    );

    comboAddressEnd.on('select', function (combo, value) {
        checkAddress(combo, 2);
    });

    btnFindRoute = new Ext.Button({
        text: 'Tìm đường',
        height: 22,
        margins: {
            top: 15,
            right: 5,
            bottom: 15,
            left: 10
        },
        handler: function () {
            googleFindRoute();
            map.zoomToExtent(global_bounds);
        },
        icon: "images/icon_search.png"
    });
    ///////////////////////////////////////////////////////////////////////////////////////////////

    // District info
    comboDistricts = new Ext.form.ComboBox({
        width: 600,
        typeAhead: true,
        maxHeight: 200,
        triggerAction: 'all',
        lazyRender: true,
        mode: 'local',
        id: 'combo-district',
        valueNotFoundText: 'Không có',
        margins: {
            top: 15,
            right: 5,
            bottom: 15,
            left: 205
        },
        store: new Ext.data.ArrayStore({
            fields: ['id', 'district'],
            data: districts
        }),
        valueField: 'id',
        displayField: 'district',
        listeners: {
            select: {
                fn: function (combo, value) {
                    //get map
                    //?action=GetMap&map_id=1
                    changeMapRequest(combo.getValue());
                }
            }
        }
    });
    comboDistricts.setValue(0);
    ///////////////////////////////////////////////////////////////////////////////////////////////

    // Tabs
    var logoPanel = new Ext.Panel({
        html: '<img src="images/F-Map.png">',
        bodyStyle: 'background-color:transparent', 
        border: false,
        margins: {
            top: 8,
            right: 0,
            bottom: 0,
            left: 5
        }, 
    });

    tabSearchAddress = new Ext.Panel({
        title: 'Tìm địa chỉ',
        height: 100,
        layout: 'hbox',
        items: [logoPanel, comboAddress, btnSearchAddress],
        bodyStyle: "background-color:#d0ddf1 !important"
    });
    //info tab
    var logoPanel1 = new Ext.Panel({
        html: '<img src="images/F-Map.png">',
        bodyStyle: 'background-color:transparent', 
        border: false,
        margins: {
            top: 8,
            right: 0,
            bottom: 0,
            left: 5
        }
    });

    tabInfo = new Ext.Panel({
        title: 'Quản lý',
        items: [logoPanel1, comboDistricts],
        layout: 'hbox',
        bodyStyle: "background-color:#d0ddf1 !important"
    });

    //route tab
    var logoPanel2 = new Ext.Panel({
        html: '<img src="images/F-Map.png">',
        bodyStyle: 'background-color:transparent', 
        border: false,
        margins: {
            top: 8,
            right: 0,
            bottom: 0,
            left: 5
        }, 
    });
    tabRoute = new Ext.Panel({
        title: 'Tìm đường đi',
        layout: 'hbox',
        items: [logoPanel2, comboAddressStart, comboAddressEnd, btnFindRoute],
        bodyStyle: "background-color:#d0ddf1 !important"
    });

    tabPanel = new Ext.TabPanel({
        border: false, // already wrapped so don't add another border
        activeTab: 0, // second tab initially active
        // tabPosition: 'bottom',
        items: [tabSearchAddress, tabRoute, tabInfo]
    });

    nowTab = tabSearchAddress;
    tabPanel.on('tabchange', function (tabPanel, tab) {
        tabPanel.doLayout();
        onTabChange(tabPanel, tab);
    });
    ///////////////////////////////////////////////////////////////////////////////////////////////

    // Main view port
    var viewport = new Ext.Viewport({
        layout: {
            type: 'border',
            padding: 5
        },
        defaults: {
            split: true
        },
        items: [
            {
                region: 'north',
                layout: 'hbox',
                height: 82,
                boxMaxHeight: 82,
                boxMinHeight: 82,
                bodyStyle: "background-color:#d0ddf1 !important",
                items: tabPanel
            },
            new Ext.Panel({
                region: 'center', // a center region is ALWAYS required for border layout
                // deferredRender: false,
                items: [{
                    contentEl: 'center'
                }]
            })
        ]
    });
    ///////////////////////////////////////////////////////////////////////////////////////////////
});

function onTabChange(tabPanel, tab) {
	if (nowTab == tabSearchAddress) {
		//co the co marker Current
		if (currentMarker) currentMarker.destroy();
		currentMarker = null;
		if (nowTab == tabRoute) {
		} else { //tabInfo

		}
	} else if (nowTab == tabRoute) {
		//co the co marker start and end
		if (startMarker) startMarker.destroy();
		if (endMarker) endMarker.destroy();
		startMarker = null;
		endMarker = null;
		vectorLayer.destroyFeatures();

		if (nowTab == tabSearchAddress) {


		} else { //tabInfo

		}
	} else {
		if (mapid != 0) {
			//doi lai map ToanThanh
			comboDistricts.setValue(0);
			changeMapRequest(0);
		}
	}

	nowTab = tabPanel.activeTab;
}

function searchAddress() {
    if (textField.getValue().trim() == '') {
        return;
    }

    keyword = change2Str(textField.getValue());

    //mask = new Ext.LoadMask(Ext.getBody(), { msg: "Đang tìm kiếm..." });
    //mask.show();

    var actions = '[{"name":"action","value":"SearchAddress"},'
                    + '{"name":"keyword","value":' + keyword + '}]';
    getInfo(actions, function (response) {
        markersLayer.destroy();
        markersLayer = new OpenLayers.Layer.Markers("Markers");
        map.addLayers([markersLayer]);

        if (currentPopup != null) {
            currentPopup.hide();
        }

        // TODO parse address response
        var diffLng = 0;
        var diffLat = 0;
        var parsedJSON = eval('(' + response + ')');
        var polygonCenterList = "";
        var searchResults = "";
        var points = [];
        markers = [];
        for (i = 0; i < parsedJSON.length && i < 10; ++i) {
            lng = parsedJSON[i].X - 0.74630393;
            lat = parsedJSON[i].Y - (-0.00523917);

            diffLng += lng - 105.94731;
            diffLat += lat - 10.77824;

            polygonCenterList += lng + ", " + lat + "<br />";

            points.push(new OpenLayers.Feature.Vector(new OpenLayers.Geometry.Point(lng, lat), null, null));

            // TODO test marker


            /*
            popup = new OpenLayers.Popup.Anchored("Example",
            new OpenLayers.LonLat(lng, lat),
            new OpenLayers.Size(200, 50),
            "Welcome to F-Map");
            */
            popup = new OpenLayers.Popup.FramedCloud("featurePopup",
                                     new OpenLayers.LonLat(lng, lat),
                                     new OpenLayers.Size(100, 100),
                                     "<h2>" + "The title" + "</h2>" +
                                     "description",
                                     null, true, null);

            popup.hide();
            map.addPopup(popup);

            var point = new OpenLayers.LonLat(lng, lat)
            var marker = new OpenLayers.Marker(point);

            addMarker(marker, lng, lat, parsedJSON[i].SoNha, parsedJSON[i].TenDuong);
            // END TEST

            // addPoint(lng, lat);
            searchResults = searchResults
                + '<span style="cursor:pointer;" onclick="zoom2PointD(' + lng + ',' + lat + ');"><i>'
                + "<b>Số nhà </b>" + parsedJSON[i].SoNha + ", <b>Tên đường </b>" + parsedJSON[i].TenDuong + "<br/>"
                + '</i></span>';
        }

        // addPoints(points);
        tabPanel.setActiveTab(0);
        tabSearchAddress.update(searchResults);

        //mask.hide();

        // document.getElementById("test").innerHTML = polygonCenterList;
    });
}

function addPoints(points) {
    vectorLayer.addFeatures(points);
}

function addMarker(marker, lng, lat, sonha, tenduong) {
    marker.events.register("click", marker, function (evt) {
        if (currentPopup != null) {
            currentPopup.hide();
        }

        if (this.popup == null) {
            // this.popup = this.createPopup(this.closeBox);

            this.popup = new OpenLayers.Popup.FramedCloud("address",
                                     new OpenLayers.LonLat(lng, lat),
                                     new OpenLayers.Size(100, 100),
                                     "<h2>" + sonha + "</h2>" + tenduong,
                                     null, true, null);

            map.addPopup(this.popup);
            this.popup.show();
        } else {
            this.popup.toggle();
        }
        currentPopup = this.popup;

        // zoom2Point(lng, lat);

        //OpenLayers.Event.stop(evt);
    });

    markersLayer.addMarker(marker);
}

function zoom2PointD(lng, lat) {
    //map.setCenter(new OpenLayers.LonLat(lng, lat), defaultCenterZoom);
    map.moveTo(new OpenLayers.LonLat(lng, lat), defaultCenterZoom);
}

function zoom2Point(lng, lat, zoom) {
    map.moveTo(new OpenLayers.LonLat(lng, lat), zoom);
}

function checkAddress(combobox, kind) {
    if (mapid != 0) {
        alert('Trở về MAP - Toàn Thành để thực hiện');
        return;
    }

    if (kind == 0) {
        if (currentMarker) currentMarker.destroy();
        currentMarker = null;
    } else if(kind == 1) {
//        if (startMarker) startMarker.destroy();
        vectorLayer.removeAllFeatures();
//        startMarker = null;
        if(startPoint!=null)
            draggableFeatureLayer.removeFeatures([startPoint]);
        startPoint = null;
    } else {
//        if (endMarker) endMarker.destroy();
        vectorLayer.removeAllFeatures();
//        endMarker = null;
        if(endPoint!=null)
            draggableFeatureLayer.removeFeatures([endPoint]);
        endPoint = null;
    } 

    //Clear overlay
    //while (markersLayer.markers.length > 0)
    //    markersLayer.removeMarker(markersLayer.markers[0]);
    ////////////////////
    var stt = combobox.getValue();
    var noname = allAddress[stt][0];
    var idward = allAddress[stt][1];


    var addressStr = combobox.getRawValue();
    var part = addressStr.split(' ');

    //part[0] is So Nha
    var actions = '[{"name":"action","value":"SearchAddress"},'
                    + '{"name":"NoAdd","value":' + part[0] + '},'
                    + '{"name":"StrNoName","value":"' + noname + '"},'
                    + '{"name":"IDWard","value":"' + idward + '"}'
                    + ']';
    //action=SearchAddress&NoAdd=123&StrNoName=y thai to&IDWard='p1
    getInfo(actions, function (response) {
        var jSon;
        var myObject = "jSon=" + response;
        eval(myObject);

        if (jSon.Found) {
            for (var i = 0; i < jSon.Points.length; i++) {
                var utm1 = new UTMRef(jSon.Points[i].X, jSon.Points[i].Y, "N", 48);
                var ll1 = utm1.toLatLng();
                var point = new OpenLayers.LonLat(ll1.lng, ll1.lat);

                var number = i + 1;
                var size = new OpenLayers.Size(32, 37);
                var offset = new OpenLayers.Pixel(-(size.w / 2), -size.h);

                var marker;
                if (kind == 0) {
                    var icon = new OpenLayers.Icon('./images/markers/number_' + number + '.png', size, offset);
                    marker = new OpenLayers.Marker(point, icon);
                    currentMarker = marker;
                } else if (kind == 1) {
//                    var icon = new OpenLayers.Icon('./images/markers/letter_b.png', size, offset);
//                    marker = new OpenLayers.Marker(point, icon);
//                    startMarker = marker;

                    var newstyle = {
                        graphicWidth: 32,
                        graphicHeight: 37,
                        graphicXOffset: -16,
                        graphicYOffset: -37,
                        externalGraphic: "./images/markers/letter_b.png"
                    };
                    startPoint = new OpenLayers.Feature.Vector(new OpenLayers.Geometry.Point(ll1.lng, ll1.lat), null, newstyle);
                    draggableFeatureLayer.addFeatures([startPoint]);
                } else {
//                    var icon = new OpenLayers.Icon('./images/markers/letter_e.png', size, offset);
//                    marker = new OpenLayers.Marker(point, icon);
//                    endMarker = marker;

                    var newstyle = {
                        graphicWidth: 32,
                        graphicHeight: 37,
                        graphicXOffset: -16,
                        graphicYOffset: -37,
                        externalGraphic: "./images/markers/letter_e.png"
                    };
                    endPoint = new OpenLayers.Feature.Vector(new OpenLayers.Geometry.Point(ll1.lng, ll1.lat), null, newstyle);
                    draggableFeatureLayer.addFeatures([endPoint]);
                }

                // addMarker(marker, ll1.lng, ll1.lat, i + 1, combobox.getRawValue());
                // markersLayer.addMarker(marker);
                // draggableFeatureLayer.addMarker(marker);

                //zoom2Point(ll1.lng, ll1.lat, defaultCenterZoom);
                zoom2PointD(ll1.lng, ll1.lat);
            }
        }
        else {
            alert("Không tìm thấy");
        }
    });
}
