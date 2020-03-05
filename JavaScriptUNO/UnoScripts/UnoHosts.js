class UnoHosts {

    UnoGame = null;

    constructor() {
        this.hostHub = $.connection.hostHub;
        this.hostHub.client.setGameMode = function (mode) {
            console.log(mode);
        };
        this.hubReady = $.connection.hub.start();        
    }

    startGame() {
        this.hostHub.server.startGame();
        this.UnoGame = new Uno(["p1", "p2"]);
        var CardCount = this.UnoGame.getCardsInStockPile();
        $('#numberOfCards').text(CardCount + " Cards");
    }


}