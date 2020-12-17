class UnoSessions {
	constructor() {
		this.sessionHub = $.connection.sessionHub;
		this.sessionHub.client.setSessions = function (dataObject) {
			//debug
			console.log("OBSOLETE: ", dataObject);
		};

		this.sessionHub.client.redirectToGame = function (gameId) {
			window.location.href = "/Host/Index/" + gameId;
		};

		this.sessionHub.client.redirectToClientGame = function (clientId) {
			setTimeout(function () {
				window.location.href = "/Client/Index/" + clientId;
			}, 1500);
        }

		this.hubReady = $.connection.hub.start();
	}

	callRequestUpdate() {
		this.sessionHub.server.getSessions();
	}

	createGameSession(name, gamesize) {
		this.sessionHub.server.createSession(name, gamesize);
	}

	joinGameWithPassword(password) {
		this.sessionHub.server.createClientSessionFromPassword(password).done(function (returnMessage) {
			//todo: set the return message somewhere.
			console.log("Message returned from server: ", returnMessage);
			$('#alertText').text(returnMessage);
			$('#alertText').show();
		});
    }
}