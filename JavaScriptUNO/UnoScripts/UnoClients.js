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
            var cardCounter = 0;

            playerObject.cards.sort(function (a, b) {
                var nameA = a.name.toUpperCase(); // ignore upper and lowercase
                var nameB = b.name.toUpperCase(); // ignore upper and lowercase
                if (nameA < nameB) {
                    return -1;
                }
                if (nameA > nameB) {
                    return 1;
                }

                // names must be equal
                return 0;
            });
            $('.cards-bar').empty();
            for (var card of playerObject.cards) {
                var imagePath = prefix + card.imageLocation;
                var imageDivWrapper = document.createElement("div");
                imageDivWrapper.className = "cards-item-wrapper";
                var imageDiv = document.createElement("div");
                imageDiv.className = "cards-item";
                imageDiv.style.background = "url('" + imagePath + "')";
                imageDiv.dataset.card = card.name;
                imageDiv.style.zIndex = 1000 + cardCounter++;
                imageDiv.onclick = function (item) {
                    var clientObject = item.target;
                    var cardName = clientObject.dataset.card;
                    console.log(cardName);
                    client.clientHub.server.postCard(cardName);
                };

                imageDivWrapper.append(imageDiv);
                $('.cards-bar').append(imageDivWrapper);
            }
        };

        this.hubReady = $.connection.hub.start();     
    }   

    connectToHost(gameId) {
        this.clientHub.server.subscribeToHost(gameId); //local id of the game to subscribe to
    }

}