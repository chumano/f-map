/*!
* Ext JS Library 3.3.1
* Copyright(c) 2006-2010 Sencha Inc.
* licensing@sencha.com
* http://www.sencha.com/license
*/

Ext.BLANK_IMAGE_URL = 'images/default/s.gif';

Ext.onReady(function () {
    menuWin = new Ext.Window({
        applyTo: 'winmenu',
        layout: 'fit',
        width: 'auto',
        height: 'auto',
        closeAction: 'hide',
        plain: true,
        border: false,

        header: false,
        resizable: false,
        x: 800,
        y: 0
        , items: [new Ext.form.ComboBox({
            typeAhead: true,
            triggerAction: 'all',
            lazyRender: true,
            mode: 'local',
            id: 'combo-district',
            valueNotFoundText: 'chumano',
            store: new Ext.data.ArrayStore({
                fields: ['cid', 'district'],
                data: districts
            }),
            valueField: 'cid',
            displayField: 'district',
            listeners: { select: {
                fn: function (combo, value) {
                    //get map


                    //update combo-ward
                    var comboWard = Ext.getCmp('combo-ward');
                    comboWard.clearValue();

                    if (combo.getValue() == '0') {
                        // alert("AAA");
                        var comboWard = Ext.getCmp('combo-ward');
                        comboWard.hide();

                    }
                    else {
                        comboWard.show();
                        if (combo.getValue() != 1) {
                            comboWard.store = null;
                            alert("Chưa có dữ liệu");
                            return;
                        }

                        var actions = '[{"name":"action","value":"GetWards"},{"name":"district_id","value":' + combo.getValue() + '}]';
                        getInfo(actions,
                            function (reponsewards) {
                                comboWard.store = new Ext.data.ArrayStore({
                                    fields: ['id', 'ward'],
                                    data: str2Arr(reponsewards)
                                });
                            }
                        );

                        //comboCity.store.filter('cid', combo.getValue());
                    }
                }
            }
            }
        }),
            new Ext.form.ComboBox({
                typeAhead: true,
                triggerAction: 'all',
                lazyRender: true,
                mode: 'local',
                id: 'combo-ward',
                hidden: true,
                store: new Ext.data.ArrayStore({
                    fields: ['id', 'ward'],
                    data: wards
                }),
                valueField: 'id',
                displayField: 'ward',
                listeners: { select: {
                    fn: function (combo, value) {
                        // alert("aaa");
                        //action=GetInfo&bbox=105.922404,10.756404,105.968596,10.802596&x=234&y=430
                        //&layer_name=sde:QUAN1_RG_HCXA&width=550&height=550

                        //[{"name":"Mã hành chính","value":"7.6026755E7"},{"name":"Tên","value":"Phường Cô Giang"},{"name":"Số hộ","value":"4185"}]
                        var bbox = '"105.922404,10.756404,105.968596,10.802596"';
                        var x = "234", y = "430";
                        var layer = '"sde:QUAN1_RG_HCXA"';
                        var w = 550, h = 550;
                        var actions = '[{"name":"action","value":"GetInfo"}'
                                    + ',{"name":"bbox","value":' + bbox + '}'
                                    + ',{"name":"x","value":' + x + '}'
                                    + ',{"name":"y","value":' + y + '}'
                                    + ',{"name":"layer_name","value":' + layer + '}'
                                    + ',{"name":"width","value":' + w + '}'
                                    + ',{"name":"height","value":' + h + '}'
                                    + ']';
                        getInfo(actions,
                            function (reponsewards) {
                                alert(reponsewards);
                            }
                        );
                    }
                }
                }
            })

        ]
    });

    var comboCountry = Ext.getCmp('combo-district');
    comboCountry.setValue('0');
    //win.show(this);
    var button = Ext.get('show-btn');

    button.on('click', function () {
        // create the window on the first click and reuse on subsequent clicks
        if (menuWin.hidden) {
            menuWin.show();
        }
        else {
            menuWin.hide();
        }

    });

    labelInfo = new Ext.form.Label({
        text: 'chumano'
    });
    infoWin = new Ext.Window({
        applyTo: 'wininfo',
        layout: 'fit',
        width: 200,
        height: 100,
        closeAction: 'hide',
        plain: true,
        border: false,
        header: false,
        resizable: false,

        x: 100,
        y: 200
    });
    //    infoWin.show();


    buttonObject = new Ext.Button({ text: 'Search', height: 40, handler: searchAddress });
    textField = new Ext.form.TextField({ width: 380, height: 40 });
    searchWin = new Ext.Window({
        applyTo: 'search_window',
        height: 'auto',
        width: 460,
        bodyStyle: 'padding: 5px',
        layout: 'hbox',
        labelWidth: 50,
        defaultType: 'field',
        items: [textField,buttonObject],
        x: 100,
        y: 50
    });
    searchWin.show();
});

function searchAddress() {
    keyword = change2Str(textField.getValue());
    var actions = '[{"name":"action","value":"SearchAddress"},'
                    + '{"name":"keyword","value":' + keyword + '}]';
    getInfo(actions, function (response) {
        // TODO parse address response
        var diffLng = 0;
        var diffLat = 0;
        var parsedJSON = eval('(' + response + ')');
        var polygonCenterList = "";
        var points = [];
        for (i = 0; i < parsedJSON.length && i < 10; ++i) {
            lng = parsedJSON[i].X - 0.74630393;
            lat = parsedJSON[i].Y - (-0.00523917);

            diffLng += lng - 105.94731;
            diffLat += lat - 10.77824;

            polygonCenterList += lng + ", " + lat + "<br />";

            points.push(new OpenLayers.Feature.Vector(new OpenLayers.Geometry.Point(lng, lat), null, null));

            // addPoint(lng, lat);
        }

        addPoints(points);

        document.getElementById("test").innerHTML = polygonCenterList;
        // addPoint(105.94731, 10.77824);
    });
}

function addPoints(points) {
    vectorLayer.addFeatures(points);
}

function addPoint(lng, lat) {
    var point = new OpenLayers.Geometry.Point(lng, lat);
    vectorLayer.addFeatures([new OpenLayers.Feature.Vector(point, null, null)]);
}