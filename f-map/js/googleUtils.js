//start = new google.maps.LatLng(lat,lng)
function findRoute(start, end) {
    

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

            tabPanel.setActiveTab(1);
            tabInfo.update(rslt[1]);
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

        if (i == myRoute.steps.length - 1) {
            googleP = myRoute.steps[i].end_point;
            openLayersP = new OpenLayers.Geometry.Point(googleP.lng(), googleP.lat());
            points.push(openLayersP);
        }

        //instructions
        instructions += myRoute.steps[i].instructions + '<br/>';
    }

    return [points,instructions];
}