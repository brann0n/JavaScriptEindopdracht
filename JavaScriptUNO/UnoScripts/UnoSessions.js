$(function () {
    // Reference the auto-generated proxy for the hub.
    var sessionHub = $.connection.sessionHub;
    sessionHub.client.setSessions = function (dataObject) {
        //debug
        console.log(dataObject);

        //send the received object to the draw function.
        drawSessions(dataObject);
    };

    $.connection.hub.start().done(function () {       
        callRequestUpdate();
    });

    callRequestUpdate = function () {
        sessionHub.server.getSessions();
    };
});

function drawSessions(sessionList) {
    if (sessionList.length > 0) {
        $('#sessionContainer').empty();
        for (var i = 0; i < sessionList.length; i++) {
            var sessionCard = new GameSessionCard(sessionList[i]);
            sessionCard.joinButton.onclick = function (e) {
                //the code to perform when join is clicked
                console.log("Clicked join on: " + e.target.dataset.gameName);
            };

            sessionCard.spectateButton.onclick = function (e) {
                //the code to run when spectate is clicked
                console.log("Clicked spectate on: " + e.target.dataset.gameName);
            };

            $('#sessionContainer').append(sessionCard.createDOMElement());
        }       
        $('#sessionContainer').append("<div class='item-clear'></div>");
    }
    else {
        $('#sessionContainer').empty();
        $('#sessionContainer').append("<p>No sessions available</p>");
    }
}