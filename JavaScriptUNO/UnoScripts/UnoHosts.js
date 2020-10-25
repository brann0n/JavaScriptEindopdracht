﻿class UnoHosts {

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
			//set the player list:
			$('#player-list').empty();
			var count = 0;
            for (var player of gameObject.Players) {
                let playername = (player.name === null)
                    ?'Player #' + ++count + ' ' + player.id.substring(0, 8)
                    : 'Player #' + ++count + ' ' + player.name;

                if (gameObject.CurrentPlayer === player.id) {                  
                    $('#player-list').append('<div class="player-list-item player-list-item-selected">' + playername + '</div>');
				}
				else {
                    $('#player-list').append('<div class="player-list-item">' + playername + '</div>');
				}
            }

            //overwrite current objects
            this.UnoGame.Players = gameObject.Players;
            this.UnoGame.Deck = gameObject.Deck;
            this.UnoGame.StockPile = gameObject.StockPile;
			this.UnoGame.CurrentPlayer = gameObject.CurrentPlayer;

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
            //check if the current player is allowed to play
            if (this.UnoGame.CurrentPlayer === playerId) {
                if (this.UnoGame.playCard(card, playerId)) {
                    //card was played and should be displayed or whatever
                    this.updateTopCard();
                    this.cardPlayed(true);
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
            if (this.UnoGame.CurrentPlayer === playerId) {
                //draw a card specified by the amount
                this.UnoGame.dealCardAmountToPlayer(playerId, 1);

                this.cardPlayed(true);
            }
            else {
                this.cardPlayed(false);
            }
        };

        this.hostHub.pushGame = function () {
            this.server.pushGame(this.UnoGame);
		};

		this.hostHub.cardPlayed = function (cardSuccess) {
			this.server.confirmCardGame(this.UnoGame, cardSuccess);
        };

        this.hostHub.updateTopCard = function () {
            var card = this.UnoGame.getTopCardFromPlayingStack();
            //todo: loop over all the cards and make a 'messy' deck
            $('.cardPlaying').css("background-image", "url(../../Content/UnoImages/" + card.imageLocation + ")");
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
        //show the drawn card
        this.hostHub.updateTopCard();
    } 
}