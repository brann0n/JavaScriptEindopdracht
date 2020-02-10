class GameSessionCard {

    //constructor(gameName, playerCount, totalPlayers) {
    //    this.gameName = gameName;
    //    this.pCount = playerCount;
    //    this.pTotal = totalPlayers;
    //}

    //changed contructor to object :)
    constructor(sessionObject) {
        this.gameName = sessionObject["GameName"];
        this.pCount = sessionObject["PlayerCount"];
        this.pTotal = sessionObject["PlayerTotal"];
        this.joinButton = document.createElement("button");
        this.joinButton.classList = "action-btn btn btn-default";
        this.joinButton.innerText = "Join";
        this.joinButton.dataset.gameName = this.gameName;

        this.spectateButton = document.createElement("button");
        this.spectateButton.classList = "action-btn btn btn-default";
        this.spectateButton.innerText = "Spectate";
        this.spectateButton.dataset.gameName = this.gameName;
    }

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
        gameController.appendChild(this.spectateButton);

        //add the elements to the main block
        gameCard.appendChild(gameTitle);
        gameCard.appendChild(gameController);

        return gameCard;
    }
}