class UnoClients {

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
				console.log(card);
				if (card.amount > 0) {
					var imagePath = prefix + card.imageLocation;
					var imageDivWrapper = document.createElement("div");
					imageDivWrapper.className = "cards-item-wrapper";
					var imageDiv = document.createElement("div");
					imageDiv.className = "cards-item";
					imageDiv.style.background = "url('" + imagePath + "')";
					imageDiv.dataset.card = card.name;
					imageDiv.style.zIndex = 1000 + cardCounter++;
					imageDiv.onclick = function (item) {
						//this code is executed after a click on a card, item contains the html object that was clicked.
						var clientObject = item.target;
						var cardName = clientObject.dataset.card;
						console.log(cardName);
						$(clientObject).fadeOut('fast').animate({
							'bottom': '100%'
						}, {
							duration: 'fast', queue: false, complete: function () {
								console.log(cardName + "aaa");
								client.clientHub.server.postCard(cardName);
							}
						});
					};

					imageDivWrapper.append(imageDiv);
					$('.cards-bar').append(imageDivWrapper);
				}
			}
		};

		this.hubReady = $.connection.hub.start();
	}

	connectToHost(gameId, clientId) {
		//get the playername from the localstorage, this object was set on the session page

		let playerNameString = localStorage.getItem('playername');

		this.clientHub.server.subscribeToHost(gameId, clientId, playerNameString); //local id of the game to subscribe to
	}

	drawCardFromDeck() {
		this.clientHub.server.drawCardFromDeck();
	}
}