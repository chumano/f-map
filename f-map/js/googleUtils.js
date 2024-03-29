﻿//start = new google.maps.LatLng(lat,lng)
function findRoute(start, end) {
//    mask = new Ext.LoadMask(Ext.getBody(), { msg: "Đang tải..." });
//    mask.show();

    var request = {
        origin: start,
        destination: end,
        travelMode: google.maps.DirectionsTravelMode.DRIVING 
    };

    // Route the directions and pass the response to a
    // function to create markers for each step.
    var directionsService = new google.maps.DirectionsService();
    directionsService.route(request, function (response, status) {
        if (status == google.maps.DirectionsStatus.OK) {
            //directionsDisplay.setDirections(response);
            var rslt = parseRouteResponse2Points(response);

            drawLineFromPoints(rslt[0]);
//            mask.hide();
//            tabPanel.setActiveTab(1);
//            tabInfo.update(rslt[1]);
        }
    });
}

function parseRouteResponse2Points(directionResult) {
    var myRoute = directionResult.routes[0].legs[0];
    //    DirectionsStep is an object literal with the following fields:
    //instructions:     contains instructions for this step within a text string.
    //distance:         contains the distance covered by this step until the next step, as a Distance object. (See the description in DirectionsLeg above.) This field may be undefined if the distance is unknown.
    //duration:         contains the typical time required to perform the step, until the next step, as a Duration object. (See the description in DirectionsLeg above.) This field may be undefined if the duration is unknown.
    //start_location:   contains the geocoded LatLng of the starting point of this step.
    //end_location:     contains the LatLng of the ending point of this step.
    var points = [];
    var instructions = '';
    for (var i = 0; i < myRoute.steps.length; i++) {
        var googleP = myRoute.steps[i].start_point;
        var openLayersP = new OpenLayers.Geometry.Point(googleP.lng(), googleP.lat());
        points.push(openLayersP);

        //decode
        var decodePoints = google.maps.geometry.encoding.decodePath(myRoute.steps[i].polyline.points);
        for (var j = 0; j < decodePoints.length; j++) {
            googleP = decodePoints[j];
            openLayersP = new OpenLayers.Geometry.Point(googleP.lng(), googleP.lat());
            points.push(openLayersP);
        }

        //dua end vao cuoi
        if (i == myRoute.steps.length - 1) {
            googleP = myRoute.steps[i].end_point;
            openLayersP = new OpenLayers.Geometry.Point(googleP.lng(), googleP.lat());
            points.push(openLayersP);
        }

        //instructions
        instructions += myRoute.steps[i].instructions + '<br/>';

        

    }

    var NE = directionResult.routes[0].bounds.getNorthEast();
    var SW = directionResult.routes[0].bounds.getSouthWest();
    var minX = SW.lng(); var minY = SW.lat();
    var maxX = NE.lng(); var maxY = NE.lat();
    var bounds = new OpenLayers.Bounds(minX, minY, maxX, maxY);
    global_bounds = bounds;
    //map.zoomToExtent(bounds);

    return [points,instructions];
}

function googleFindRoute() {
    if (waiting)
        return;
    vectorLayer.removeAllFeatures();
//    if (startMarker && endMarker) {
//        var start = new google.maps.LatLng(startMarker.lonlat.lat, startMarker.lonlat.lon); //lat, lng
//        var end = new google.maps.LatLng(endMarker.lonlat.lat, endMarker.lonlat.lon);
//        findRoute(start, end);
//    }
    if (startPoint && endPoint) {
        var start = new google.maps.LatLng(startPoint.geometry.y, startPoint.geometry.x); //lat, lng
        var end = new google.maps.LatLng(endPoint.geometry.y, endPoint.geometry.x);
        waiting = true;
        findRoute(start, end);
    }
    else {
        alert("Bạn phải nhập điểm đầu và điểm cuối");
    }
}
