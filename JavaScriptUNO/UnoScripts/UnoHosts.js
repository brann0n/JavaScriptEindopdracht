class UnoHosts {
    constructor() {
        this.hostHub = $.connection.hostHub;
        this.hostHub.client.setGameMode = function (mode) {
            console.log(mode);
        };
        this.hubReady = $.connection.hub.start();        
    }

    startGame() {
        this.hostHub.server.startGame();
    }
}