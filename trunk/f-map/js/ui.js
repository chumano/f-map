Ext.onReady(function () {

    // NOTE: This is an example showing simple state management. During development,
    // it is generally best to disable state management as dynamically-generated ids
    // can change across page loads, leading to unpredictable results.  The developer
    // should ensure that stable state ids are set for stateful components in real apps.
    Ext.state.Manager.setProvider(new Ext.state.CookieProvider());

    var viewport = new Ext.Viewport({
        layout: 'border',
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
                items: [
                    new Ext.form.TextField({ 
                        width: 600, 
                        height: 32, 
                        margins: {
                            top: 5,
                            right: 5,
                            bottom: 10,
                            left: 305
                        },
                    }),
                    new Ext.Button({ 
                        text: 'Search', 
                        height: 32, 
                        margins: {
                            top: 5,
                            right: 5,
                            bottom: 10,
                            left: 5
                        } 
                        /*, handler: searchAddress*/ 
                   })
                ]
            })
            ,
            /*
            {
                region: 'east',
                title: 'Phường',
                collapsible: true,
                split: true,
                width: 225, // give east and west regions a width
                minSize: 175,
                maxSize: 400,
                // margins: '0 5 0 0',
                layout: 'fit', // specify layout manager for items
                items:            // this TabPanel is wrapped by another Panel so the title will be applied
                new Ext.TabPanel({
                    border: false, // already wrapped so don't add another border
                    activeTab: 1, // second tab initially active
                    tabPosition: 'bottom',
                    items: [{
                        html: '<p>A TabPanel component can be a region.</p>',
                        title: 'A Tab',
                        autoScroll: true
                    }, new Ext.grid.PropertyGrid({
                        title: 'Property Grid',
                        closable: true,
                        source: {
                            "(name)": "Properties Grid",
                            "grouping": false,
                            "autoFitColumns": true,
                            "productionQuality": false,
                            "created": new Date(Date.parse('10/15/2006')),
                            "tested": false,
                            "version": 0.01,
                            "borderWidth": 1
                        }
                    })]
                })
            }, 
            */
            {
                region: 'west',
                id: 'west-panel', // see Ext.getCmp() below
                // title: 'Kết quả tìm kiếm',
                split: true,
                width: 300,
                minSize: 300,
                maxSize: 400,
                // collapsible: true,
                // margins: '0 0 0 5',
                items: 
                    new Ext.TabPanel({
                        border: false, // already wrapped so don't add another border
                        activeTab: 0, // second tab initially active
                        // tabPosition: 'bottom',
                        items: [
                            {
                                html: '<p>Kết quả tìm kiếm</p>',
                                title: 'Tìm địa chỉ',
                                autoScroll: true
                            }, 
                            {
                                html: '<p>xxx</p>',
                                title: 'Thông tin',
                                autoScroll: true
                            }
                        ]
                    })
            },
        // in this instance the TabPanel is not wrapped by another panel
        // since no title is needed, this Panel is added directly
            // as a Container
            new Ext.Panel({
                region: 'center', // a center region is ALWAYS required for border layout
                // deferredRender: false,
                items: [{
                    contentEl: 'center1',
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