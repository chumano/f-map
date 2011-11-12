/*!
* Ext JS Library 3.3.1
* Copyright(c) 2006-2010 Sencha Inc.
* licensing@sencha.com
* http://www.sencha.com/license
*/

Ext.BLANK_IMAGE_URL = 'images/default/s.gif';


LCombo.countries = [
     ['0', 'Toàn Thành']
    , ['1', 'Quận 1']
    , ['2', 'Quận 2']
    , ['3', 'Quận 3']
    , ['4', 'Quận 4']
    , ['5', 'Quận 5']
    , ['6', 'Quận 6']
    , ['7', 'Quận 7']
    , ['8', 'Quận 8']
    , ['9', 'Quận 9']
    , ['10', 'Quận 10']
    , ['11', 'Quận 11']
    , ['12', 'Quận 12']
    , ['TD', 'Quận Thủ Đức']
    , ['TB', 'Quận Tân Bình']
];

LCombo.cities = [
     [1, '0', 'New York']
    , [2, '0', 'Cleveland']
    , [3, '1', 'Austin']
    , [4, '2', 'Los Angeles']
    , [5, '3', 'Berlin']
    , [6, '4', 'Bonn']
    , [7, '5', 'Paris']
    , [8, '6', 'Nice']
    , [9, '7', 'London']
    , [10, '8', 'Glasgow']
    , [11, '9', 'Liverpool']
];

Ext.onReady(function () {
    var win = new Ext.Window({
        applyTo: 'hello-win',
        layout: 'fit',
        width: 'auto',
        height: 'auto',
        closeAction: 'hide',
        plain: true,
        border: false,

        header: false,
        resizable: false,
        x: 100,
        y: 0

        //        items: new Ext.TabPanel({
        //            applyTo: 'hello-tabs',
        //            autoTabs: true,
        //            activeTab: 1,
        //            deferredRender: false,
        //            border: false
        //        }),

        //        buttons: [{
        //            text: 'Submit',
        //            disabled: true
        //        }, {
        //            text: 'Close',
        //            handler: function () {
        //                win.hide();
        //            }
        //        }]

        , items: [new Ext.form.ComboBox({
            typeAhead: true,
            triggerAction: 'all',
            lazyRender: true,
            mode: 'local',
            id: 'combo-country',
            valueNotFoundText: 'chumano',
            store: new Ext.data.ArrayStore({
                fields: ['cid', 'country'],
                data: LCombo.countries
            }),
            valueField: 'cid',
            displayField: 'country',
            listeners: { select: {
                fn: function (combo, value) {
                    var comboCity = Ext.getCmp('combo-city');
                    if (combo.getValue() == '0') {
                       // alert("AAA");
                        comboCity.hide();
                        
                    }
                    else {
                        comboCity.show();
                        comboCity.clearValue();
                        comboCity.store.filter('cid', combo.getValue());
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
                id: 'combo-city',
                store: new Ext.data.ArrayStore({
                    fields: ['id', 'cid', 'city'],
                    data: LCombo.cities
                }),
                valueField: 'id',
                displayField: 'city',
                listeners: { select: {
                    fn: function (combo, value) {
                        alert("aaa");
                    }
                }
                }
            })

        ]
    });

    var comboCountry = Ext.getCmp('combo-country');
    comboCountry.setValue('a');
    //win.show(this);
    var button = Ext.get('show-btn');

    button.on('click', function () {
        // create the window on the first click and reuse on subsequent clicks
        if (win.hidden) {
            win.show();

        }
        else {
            win.hide();
        }

    });
});