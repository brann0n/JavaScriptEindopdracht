class GameSessionCard {

	//changed contructor to object :)
	constructor(sessionObject) {
		this.gameName = sessionObject["GameName"];
		this.gameId = sessionObject["GameId"];
		this.pCount = sessionObject["PlayerCount"];
		this.pTotal = sessionObject["PlayerTotal"];
		this.gameStarted = sessionObject["GameStarted"];

		//join button, public available for event subscription
		this.joinButton = document.createElement("button");
		this.joinButton.innerText = "Join";
		this.joinButton.dataset.gameName = this.gameName;
		this.joinButton.dataset.gameId = this.gameId;

		//spectate button, public available for event subscription
		this.spectateButton = document.createElement("button");
		this.spectateButton.innerText = "Spectate";
		this.spectateButton.dataset.gameName = this.gameName;
		this.spectateButton.dataset.gameId = this.gameId;

		//depending on the game state, enable or disable the spectate and join buttons:
		if (this.gameStarted === true) {
			this.joinButton.classList = "action-btn btn btn-default disabled";
			this.spectateButton.classList = "action-btn btn btn-default";
		}
		else {
			this.joinButton.classList = "action-btn btn btn-default";
			this.spectateButton.classList = "action-btn btn btn-default disabled";
		}
	}

	//Creates the object that can be placed in the HTML document.
	createDOMElement() {
		let gameCard = document.createElement("div");
		gameCard.classList = "game-item";

		let gameTitle = document.createElement("h4");
		gameTitle.classList = "game-name";
		gameTitle.innerText = "Game: " + this.gameName;

		let gameController = document.createElement("div");
		gameController.classList = "game-controller";

		let playerTag = document.createElement("div");
		playerTag.classList = "player-tag";
		playerTag.innerText = "Players: [" + this.pCount + " / " + this.pTotal + "]";

		//add controll items to the controller
		gameController.appendChild(playerTag);
		gameController.appendChild(this.joinButton);
		//gameController.appendChild(this.spectateButton);

		//add the elements to the main block
		gameCard.appendChild(gameTitle);
		gameCard.appendChild(gameController);

		return gameCard;
	}
}