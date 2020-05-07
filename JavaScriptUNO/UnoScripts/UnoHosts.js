class UnoHosts {

    constructor(gameId) {
        this.hostHub = $.connection.hostHub;
        this.gameMode = "Awaiting game initiation...";
        this.hostHub.client.setGameMode = function (mode) {
            console.log(mode);
            this.gameMode = mode;           
        };

        this.hostHub.client.getPlayerObjects = function () {
            var players = this.hostHub.UnoGame.Players;
            this.server.uploadPlayerObjects(players);
        };

        this.hostHub.client.endSession = function (reason) {
            console.log(reason);
        };

        this.hostHub.client.updatePlayerCount = function (pCount, pMax) {
            $('#playerInfo').text("Players: " + pCount + "/" + pMax);
        };

        this.hostHub.client.doRefresh = function (gameObject) {
            //called everytime a change happens either server side or client side.
            console.log(gameObject);
            //assign the 3 objects over
            this.UnoGame.Players = gameObject.Players;
            this.UnoGame.Deck = gameObject.Deck;
            this.UnoGame.StockPile = gameObject.StockPile;

            var CardCount = this.UnoGame.getCardsInStockPile();
            $('#numberOfCards').text(CardCount + " Cards");
        };

        this.hostHub.client.startGame = function (playerList) {
            this.UnoGame = new Uno(playerList); 
            this.pushGame();
            var CardCount = this.UnoGame.getCardsInStockPile();
            $('#numberOfCards').text(CardCount + " Cards");
		};

		this.hostHub.client.playCard = function (playerId, card) {
			if (this.UnoGame.playCard(card, playerId)) {
				//card was played and should be displayed and whatever
				console.log("played card: ", card);
				console.log(this.UnoGame.Players);
				console.log(this.UnoGame.Deck);
				this.pushGame();
			}
			else {
				console.log("could not play card: ", card);
				//card was not allowed
			}			
		};

        this.hostHub.pushGame = function () {
            this.server.pushGame(this.UnoGame);
        };

        this.hubReady = $.connection.hub.start();        
        this.gameId = gameId;
    }

    initGame() {
        this.hostHub.server.initGame(this.gameId); //server side version of game id.
    }

    startGame() {
        this.hostHub.server.startGame();
    }

    dealCardsToPlayers() {
		this.hostHub.UnoGame.dealFirstRoundToPlayers();
        this.hostHub.server.pushGame(this.hostHub.UnoGame);
    }
}