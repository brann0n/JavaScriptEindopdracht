class UnoHosts {

    constructor(gameId) {
        this.hostHub = $.connection.hostHub;
        this.gameMode = "AWAITING_GAME_INIT";

        this.hostHub.client.setGameMode = function (mode) {
            this.gameMode = mode;
            switch (mode) {
                case "AWAITING_PLAYERS":
                    console.log("waiting for other players to join");
                    break;
                case "RESUMING_GAME":
                    //do some stuff to make it look like the game continues
                    this.UnoGame = new Uno(null);
                    console.log("resuming game");
                    break;
            }
        };

        this.hostHub.client.getPlayerObjects = function () {
            var players = this.hostHub.UnoGame.Players;
            this.server.uploadPlayerObjects(players);
        };

        this.hostHub.client.endSession = function (reason) {
            console.log(reason);
            $('.gameBackground').empty();
            $('.gameBackground').html('<h1 id="winToast">' + reason + '</h1>');
        };

        this.hostHub.client.updatePlayerCount = function (pCount, pMax) {
            $('#playerInfo').text("Players: " + pCount + " / " + pMax);
        };

        this.hostHub.client.doRefresh = function (gameObject) {
            //set the player list:
            $('#player-list').empty();
            var count = 0;
            for (var player of gameObject.Players) {               
                if (player.connid !== "") {
                    let playername = (player.name === null)
                        ? '👤' + ' Unknown (' + player.id.substring(0, 8) + ')'
                        : '👤' + ' ' + player.name;

                    if (gameObject.CurrentPlayer === player.id) {
                        $('#player-list').append('<div id="currentPlayerObject" class="player-list-item">' + playername + '</div>');
                        
                    }
                    else {
                        $('#player-list').append('<div class="player-list-item">' + playername + '</div>');
                    }
                }
                else {
                    console.log("skipped empty player with id", player.id);
                }
            }
            $('#currentPlayerObject').toggleClass('player-list-item-selected');
            //overwrite current objects
            if (this.UnoGame) {
                //console.log("GameObject: ", gameObject);
                //console.log("this.UnoGame: ", this.UnoGame);
                this.UnoGame.Players = gameObject.Players;
                this.UnoGame.Deck = gameObject.Deck;
                this.UnoGame.StockPile = gameObject.StockPile;
                this.UnoGame.CurrentPlayer = gameObject.CurrentPlayer;
                this.UnoGame.DirectionClockwise = gameObject.DirectionClockwise;

                var CardCount = this.UnoGame.getCardsInStockPile();
                $('#numberOfCards').text(CardCount + " Cards");
            }
        };

        this.hostHub.client.startGame = function (playerList) {
            this.UnoGame = new Uno(playerList);
            this.UnoGame.dealFirstRoundToPlayers();
            this.server.pushGame(this.UnoGame)
                .done(function () {
                    //show the drawn card
                    this.updateTopCard();
                });
            //this.server.pushGame(this.UnoGame)
            //	.done(function () {
            //		this.UnoGame.dealFirstRoundToPlayers();
            //		this.server.pushGame(this.UnoGame);
            //		//show the drawn card
            //		this.updateTopCard();
            //	});
        };

        this.hostHub.client.playCard = function (playerId, card) {
            //check if the current player is allowed to play
            if (this.UnoGame.CurrentPlayer === playerId) {
                let gameHandler = this.UnoGame.playCard(card, playerId);
                if (gameHandler.played) {
                    //card was played and should be displayed
                    this.updateTopCard();

                    //handle the special cards now
                    if (gameHandler.effects === null) {
                        //there are no effects, just update and continue:
                        this.cardPlayed(true);
                    }
                    else {
                        this.handleEffects(gameHandler.effects);
                    }

                    //reset the color identifiction label
                    $("#colorName").text("");
                }
                else {
                    console.log("could not play card: ", card);
                    //card was not allowed
                    this.cardPlayed(false);
                }
            }
            else {
                //illegal move!
                this.cardPlayed(false);
            }
        };

        this.hostHub.client.drawCard = function (playerId) {
            //check if the current player is allowed to play
            console.log(this.UnoGame.CurrentPlayer, playerId);
            if (this.UnoGame.CurrentPlayer === playerId) {
                //draw a card specified by the amount
                this.UnoGame.dealCardAmountToPlayer(playerId, 1);

                this.cardPlayed(true);
            }
            else {
                this.cardPlayed(false);
            }
        };

        this.hostHub.client.drawCardFromSpecial = function (playerId, amount) {
            this.UnoGame.dealCardAmountToPlayer(playerId, amount);
            this.pushGame();
        };

        this.hostHub.client.setCurrentPlayer = function (playerObject) {
            console.log("current player: ", playerObject.name);
        };

        this.hostHub.client.setPickedColor = function (playerObject, colorName, effects) {
            if (playerObject.id === this.UnoGame.CurrentPlayer) {
                this.UnoGame.Rules.setPickedColor(colorName);

                //callback to server to advance round with the effects object.
                this.server.handleSpecialAfterColorPick(this.UnoGame, effects);

                $("#colorName").text(colorName);
            }
            else {
                console.log("non playing player sent the colorchange request, someone is hacking :)");
            }
        };

        this.hostHub.client.gameWon = function (playerId) {
            this.server.processGameWon(playerId);
        };

        this.hostHub.client.showWinnerAndEndGame = function (playerObject) {
            $('.gameBackground').empty();
            $('.gameBackground').html('<h1 id="winToast">' + playerObject.name + ' has won the game!</h1>');
        };

        this.hostHub.client.displayMessage = function (message) {
            $('#messageBox').fadeOut(function () {
                $(this).text(message).fadeIn();
            });
        };

        this.hostHub.pushGame = function () {
            //possible bug fix: do not push game if current player == null
            if (this.UnoGame.CurrentPlayer === null) {
                console.log("Tried game push while object empty: ", this.UnoGame);
            }
            else {
                this.server.pushGame(this.UnoGame);
            }
        };

        this.hostHub.cardPlayed = function (cardSuccess) {
            this.server.confirmCardGame(this.UnoGame, cardSuccess);
        };

        this.hostHub.handleEffects = function (effects) {
            //HandleSpecialCard
            this.server.handleSpecialCard(this.UnoGame, effects);
        };

        this.hostHub.updateTopCard = function () {
            var card = this.UnoGame.getTopCardFromPlayingStack();
            //todo: loop over all the cards and make a 'messy' deck by slightly translating and rotating
            $('.cardPlaying').css("background-image", "url(../../Content/UnoImages/" + card.imageLocation + ")");
        };

        this.hubReady = $.connection.hub.start();
        $.connection.hub.logging = true;
        this.gameId = gameId;
    }

    initGame() {
        this.hostHub.server.initGame(this.gameId)//server side version of game id.
            .done(function (gamepassword) {
                console.log(gamepassword);
            }); 
    }

    startGame() {
        this.hostHub.server.startGame();
    }

    endGame() {
        this.hostHub.server.endGame().done(function () {
            location.replace("/");
        });
    }

    sendMessageToClients(message) {
        this.hostHub.server.relayMessage(message);
    }
}