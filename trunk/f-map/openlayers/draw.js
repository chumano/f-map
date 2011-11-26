function drawLineFromPoints(points) {
//    points = new Array(
//                          new OpenLayers.Geometry.Point(lon1, lat1),
//                          new OpenLayers.Geometry.Point(lon2, lat2)
//                          );
   
    var line = new OpenLayers.Geometry.LineString(points);

    var style = { strokeColor: '#0000ff',
        strokeOpacity: 0.5,
        strokeWidth: 5
    };

    var lineFeature = new OpenLayers.Feature.Vector(line, null, style);
    vectorLayer.addFeatures([lineFeature]);

}