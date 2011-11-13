Ext.onReady(function () {
    Ext.state.Manager.setProvider(new Ext.state.CookieProvider());

    var comboDistricts = new Ext.form.ComboBox({
            height:5,
            typeAhead: true,
            triggerAction: 'all',
            lazyRender: true,
            mode: 'local',
            id: 'combo-district',
            valueNotFoundText: 'Unknown',
            margins: {
                top: 10,
                right: 0,
                bottom: 10,
                left: 10
            },
            store: new Ext.data.ArrayStore({
                fields: ['cid', 'district'],
                data: districts
            }),
            valueField: 'cid',
            displayField: 'district',
            listeners: { select: {
                fn: function (combo, value) {
                    //get map
                    //?action=GetMap&map_id=1

                    // moveTo(105.95344, 10.78479, 3);

                    changeMapRequest(combo.getValue());
                    
                }
            }
            }
     });
     
     comboDistricts.setValue(0);

    buttonObject = new Ext.Button({ 
                        text: 'Tìm kiếm', 
                        height: 32, 
                        margins: {
                            top: 5,
                            right: 5,
                            bottom: 10,
                            left: 5
                        }, 
                        handler: searchAddress,
                        icon: "images/icon_search.png"
                   });
    textField = new Ext.form.TextField({ 
                        width: 600, 
                        height: 32, 
                        margins: {
                            top: 5,
                            right: 5,
                            bottom: 10,
                            left: 305
                        }
                    });

    // Tabs
    tabSearchAddress =  new Ext.Panel ({
        title: 'Tìm địa chỉ',
        html: '<p>Kết quả tìm kiếm</p>',
        height: 550,
        autoScroll: true
    });
    tabInfo = new Ext.Panel ({
        title: 'Thông tin',
        html: ''
    });
    tabPanel = new Ext.TabPanel({
                        border: false, // already wrapped so don't add another border
                        activeTab: 0, // second tab initially active
                        // tabPosition: 'bottom',
                        items: [tabSearchAddress, tabInfo]
                    });

    var viewport = new Ext.Viewport({
        layout: {
            type: 'border',
            padding: 5
        },
        defaults: {
            split: true
        },
        items: [

        // create instance immediately
            /*
            new Ext.BoxComponent({
                region: 'north',
                height: 32, // give north and south regions a height
                autoEl: {
                    tag: 'div',
                    html: '<p>north - generally for menus, toolbars and/or advertisements</p>'
                }
            }), 
            */
            new Ext.Panel({
                region: 'north', // a center region is ALWAYS required for border layout
                layout: 'hbox',
                height: 42,
                boxMaxHeight: 42,
                boxMinHeight: 42,
                bodyStyle: "background-color:#133783 !important",
                items: [
                    textField,
                    buttonObject,
                    comboDistricts
                ]
            }),
            {
                region: 'west',
                id: 'west-panel', // see Ext.getCmp() below
                // title: 'Kết quả tìm kiếm',
                split: true,
                width: 300,
                boxMinWidth: 300,
                boxMaxWidth: 300,
                // collapsible: true,
                // margins: '0 0 0 5',
                items: tabPanel
            },
            new Ext.Panel({
                region: 'center', // a center region is ALWAYS required for border layout
                // deferredRender: false,
                items: [{
                    contentEl: 'center',
                }]
            })
        ]
    });
    // get a reference to the HTML element with id "hideit" and add a click listener to it 
    Ext.get("hideit").on('click', function () {
        // get a reference to the Panel that was created wit0h id = 'west-panel' 
        var w = Ext.getCmp('west-panel');
        // expand or collapse that Panel based on its collapsed property state
        w.collapsed ? w.expand() : w.collapse();
    });
});

function searchAddress() {
    if (textField.getValue().trim() == '')
    {
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
                                     new OpenLayers.Size(100,100),
                                     "<h2>"+"The title" + "</h2>" +
                                     "description",
                                     null, true, null);

            popup.hide();
            map.addPopup(popup);

            var point  = new OpenLayers.LonLat(lng, lat)
            var marker = new OpenLayers.Marker(point);

            addMarker(marker, lng, lat, parsedJSON[i].SoNha, parsedJSON[i].TenDuong);
            // END TEST

            // addPoint(lng, lat);
            searchResults = searchResults 
                + '<span style="cursor:pointer;" onclick="moveTo(' + lng + ',' + lat + ');"><i>' 
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
                                     new OpenLayers.Size(100,100),
                                     "<h2>"+ sonha + "</h2>" + tenduong,
                                     null, true, null);

                    map.addPopup(this.popup);
                    this.popup.show();
                } else {
                    this.popup.toggle();
                }
                currentPopup = this.popup;

                // moveTo(lng, lat);

                OpenLayers.Event.stop(evt);
            });

    markersLayer.addMarker(marker);
}

function moveTo(lng, lat) {
    map.setCenter(new OpenLayers.LonLat(lng, lat), defaultCenterZoom);
}

function moveTo(lng, lat, zoom) {
    map.setCenter(new OpenLayers.LonLat(lng, lat), zoom);
}