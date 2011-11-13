var tabPanel;
var tabSearchAddress;
var tabInfo;

Ext.onReady(function () {

    // NOTE: This is an example showing simple state management. During development,
    // it is generally best to disable state management as dynamically-generated ids
    // can change across page loads, leading to unpredictable results.  The developer
    // should ensure that stable state ids are set for stateful components in real apps.
    Ext.state.Manager.setProvider(new Ext.state.CookieProvider());

    buttonObject = new Ext.Button({ 
                        text: 'Search', 
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
                    textField,
                    buttonObject
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
                items: tabPanel
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

function searchAddress() {
    if (textField.getValue().trim() == '')
    {
        return;
    }

    keyword = change2Str(textField.getValue());

    mask = new Ext.LoadMask(Ext.getBody(), { msg: "Đang tìm kiếm..." });
    mask.show();

    var actions = '[{"name":"action","value":"SearchAddress"},'
                    + '{"name":"keyword","value":' + keyword + '}]';
    getInfo(actions, function (response) {
        // TODO parse address response
        var diffLng = 0;
        var diffLat = 0;
        var parsedJSON = eval('(' + response + ')');
        var polygonCenterList = "";
        var searchResults = "";
        var points = [];
        for (i = 0; i < parsedJSON.length; ++i) {
            lng = parsedJSON[i].X - 0.74630393;
            lat = parsedJSON[i].Y - (-0.00523917);

            diffLng += lng - 105.94731;
            diffLat += lat - 10.77824;

            polygonCenterList += lng + ", " + lat + "<br />";

            // points.push(new OpenLayers.Feature.Vector(new OpenLayers.Geometry.Point(lng, lat), null, null));

            // addPoint(lng, lat);
            searchResults = searchResults + "<b>Số nhà </b>" + parsedJSON[i].SoNha + ", <b>Tên đường </b>" + parsedJSON[i].TenDuong + "<br/>"
        }

        // addPoints(points);
        tabSearchAddress.update(searchResults);

        mask.hide();

        // document.getElementById("test").innerHTML = polygonCenterList;
    });
}