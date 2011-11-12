/*!
* Ext JS Library 3.3.1
* Copyright(c) 2006-2010 Sencha Inc.
* licensing@sencha.com
* http://www.sencha.com/license
*/

Ext.BLANK_IMAGE_URL = 'images/default/s.gif';


Ext.onReady(function () {
    var win = new Ext.Window({
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
                data: districts
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
                hidden: true,
                store: new Ext.data.ArrayStore({
                    fields: ['id', 'cid', 'city'],
                    data: wards
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
    comboCountry.setValue('0');
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