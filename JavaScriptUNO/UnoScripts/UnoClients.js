class UnoClients {

    constructor() {
        this.clientHub = $.connection.clientHub;

        this.clientHub.client.setGameMode = function (mode) {
            console.log(mode);
        };      

        this.clientHub.endSession = function (reason) {
            console.log(reason);
        };

        this.clientHub.client.doRefresh = function (gameObject) {
            //called everytime a change happens either server side or client side.
            var prefix = gameObject.PathPrefix;
            var currConnId = $.connection.hub.id;
            var playerObject = gameObject.Players.find(x => x.id === currConnId);
            //console.log(gameObject.Players.find(x => x.id === currConnId));
            
            for (var card of playerObject.cards) {
                var imagePath = prefix + card.imageLocation;
                var imageDiv = document.createElement("div");
                imageDiv.className = "cards-item";
                imageDiv.style.background = "url('" + imagePath + "')";
                imageDiv.dataset.card = card.name;
                $('.cards-bar').append(imageDiv);
            }
        };

        this.hubReady = $.connection.hub.start();     
    }   

    connectToHost(gameId) {
        this.clientHub.server.subscribeToHost(gameId); //local id of the game to subscribe to
    }

}