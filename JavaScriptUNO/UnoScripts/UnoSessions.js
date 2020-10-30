class UnoSessions {
	constructor() {
		this.sessionHub = $.connection.sessionHub;
		this.sessionHub.client.setSessions = function (dataObject) {
			//debug
			console.log(dataObject);

			//send the received object to the draw function.
			this.drawSessions(dataObject);
		};

		this.sessionHub.client.redirectToGame = function (gameId) {
			window.location.href = "/Host/Index/" + gameId;
		};

		this.sessionHub.drawSessions = function (sessionList) {
			if (sessionList.length > 0) {
				$('#sessionContainer').empty();
				for (var i = 0; i < sessionList.length; i++) {
					var sessionCard = new GameSessionCard(sessionList[i]);
					if (sessionList[i]["GameStarted"] === false) {
						sessionCard.joinButton.onclick = function (e) {
							//the code to perform when join is clicked
							console.log("Clicked join on: " + e.target.dataset.gameName, e.target.dataset.gameId);
							session.sessionHub.server.createClientSession(e.target.dataset.gameId).done(function (clientSessionId) {
								window.location.href = "/Client/Index/" + clientSessionId;
							});
						};
					}

					$('#sessionContainer').append(sessionCard.createDOMElement());
				}
				$('#sessionContainer').append("<div class='item-clear'></div>");
			}
			else {
				$('#sessionContainer').empty();
				$('#sessionContainer').append("<p>No sessions available</p>");
			}
		};

		this.hubReady = $.connection.hub.start();
	}

	callRequestUpdate() {
		this.sessionHub.server.getSessions();
	}

	createGameSession(name) {
		this.sessionHub.server.createSession(name);
	}
}