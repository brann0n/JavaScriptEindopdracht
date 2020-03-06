class UnoHosts {

    UnoGame = null;

    constructor(gameId) {
        this.hostHub = $.connection.hostHub;
        this.hostHub.client.setGameMode = function (mode) {
            console.log(mode);
            $('#playerInfo').text($('#playerInfo').text() + " status: " + mode);
        };

        this.hostHub.client.getPlayerObjects = function () {
            var players = this.UnoGame.Players;
            this.hostHub.server.uploadPlayerObjects(players);
        };

        this.hostHub.client.endSession = function (reason) {
            console.log(reason);
        };

        this.hostHub.client.startGame = function (playerList) {
            this.UnoGame = new Uno(["p1", "p2"]); //TODO: get the player list from current server session.
            var CardCount = this.UnoGame.getCardsInStockPile();
            $('#numberOfCards').text(CardCount + " Cards");
        };

        this.hubReady = $.connection.hub.start();        
        this.gameId = gameId;
    }

    initGame() {
        this.hostHub.server.initGame(this.gameId); //server side version of game id.
    }

    startGame() {
        this.hostHub.server.startGame(this.gameId);        
    }

}