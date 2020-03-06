class UnoClients {

    constructor() {
        this.clientHub = $.connection.clientHub;

        this.clientHub.client.setGameMode = function (mode) {
            console.log(mode);
        };      

        this.clientHub.endGame = function (reason) {
            console.log(reason);
        };

        this.hubReady = $.connection.hub.start();
    }   

    connectToHost(gameId) {
        this.clientHub.server.subscribeToHost(gameId); //local id of the game to subscribe to
    }

}