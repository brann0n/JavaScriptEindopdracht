﻿class UnoClients {
    AudioPathPrefix = "../../Content/sounds/";

    CardAudioPlayer = [
        new Audio(this.AudioPathPrefix + "cardsound-1.mp3"),
        new Audio(this.AudioPathPrefix + "cardsound-2.mp3"),
        new Audio(this.AudioPathPrefix + "cardsound-3.mp3"),
        new Audio(this.AudioPathPrefix + "cardsound-4.mp3"),
        new Audio(this.AudioPathPrefix + "cardsound-5.mp3")
    ];

    constructor() {
        this.clientHub = $.connection.clientHub;

        this.clientHub.client.setGameMode = function (mode) {
            $('.cards-bar').html("<h3>" + mode + "</h3>");
        };

        this.clientHub.client.endSession = function (reason) {
            console.log(reason);
            $('.gameBackground').empty();
            $('.gameBackground').html('<h1>' + reason + '</h1>');
        };

        this.clientHub.client.doRefresh = function (gameObject) {
            //called everytime a change happens either server side or client side.
            var prefix = gameObject.PathPrefix;
            var currConnId = $.connection.hub.id;
            var playerObject = gameObject.Players.find(x => x.connid === currConnId);
            var cardCounter = 0;

            //sort the cards by color and their ranking
            playerObject.cards.sort(function (a, b) {
                var nameA = a.name.toUpperCase(); // ignore upper and lowercase
                var nameB = b.name.toUpperCase(); // ignore upper and lowercase
                if (nameA < nameB) {
                    return -1;
                }
                if (nameA > nameB) {
                    return 1;
                }

                // names are equal, dont do anything
                return 0;
            });

            //remove all cards and redraw them
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
                    window.navigator.vibrate([100, 100]);
                    client.playRandomCardSound();
                    //this code is executed after a click on a card, item contains the html object that was clicked.
                    var clientObject = item.target;
                    var cardName = clientObject.dataset.card;
                    $(clientObject).fadeOut('fast').animate({
                        'bottom': '100%'
                    }, {
                        duration: 'fast', queue: false, complete: function () {
                            client.clientHub.server.postCard(cardName);
                        }
                    });
                };

                imageDivWrapper.append(imageDiv);
                $('.cards-bar').append(imageDivWrapper);
            }
        };


        this.clientHub.client.displayMessage = function (message) {
            console.log(message);
        };

        this.clientHub.client.setCurrentPlayer = function (playerObject) {
            var currConnId = $.connection.hub.id;
            var nameMessage = "Waiting for player " + playerObject.name + "!";

            //check if the given object is the current player
            if (currConnId === playerObject.connid) {
                nameMessage = "It's your turn!";
                window.navigator.vibrate([200, 200, 200]);
            }

            console.log("current player: ", nameMessage);
            $('#gameTitle').text(nameMessage);
        };

        this.clientHub.client.displayColorWheel = function (effects) {
            $(".popup-overlay, .popup-content").addClass("active");

            $("#hiddenEffects").data("effects", effects);
        };

        this.clientHub.client.stopGame = function () {
            location.replace("/");
        };

        this.hubReady = $.connection.hub.start();
        if (screen.availHeight > screen.availWidth) {
            alert("Please rotate your device into landscape mode for the best experience!");
        }
    }

    connectToHost(gameId, clientId) {
        //get the playername from the localstorage, this object was set on the session page

        let playerNameString = localStorage.getItem('playername');

        this.clientHub.server.subscribeToHost(gameId, clientId, playerNameString); //local id of the game to subscribe to
    }

    drawCardFromDeck() {
        this.clientHub.server.drawCardFromDeck();
        this.playRandomCardSound();
    }

    sendPickedColorToHost(buttonObject) {
        if (buttonObject !== null) {
            let color = $(buttonObject).data("color");
            let effects = $("#hiddenEffects").data("effects");
            this.clientHub.server.sendColorToHost(color, effects);
        }
        else {
            this.clientHub.server.sendColorToHost("error", null);
        }
    }

    uno() {
        this.clientHub.server.reportUno();
    }


    playRandomCardSound() {
        let min = Math.ceil(0);
        let max = Math.floor(4);
        let randomNumber = Math.floor(Math.random() * (max - min + 1)) + min;
        this.CardAudioPlayer[randomNumber].play();
    }
}