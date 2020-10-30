class Uno {

	//108 cards in uno: 4 colors, each consisting of one 0 card, two 1 cards, two 2s, 3s, 4s, 5s, 6s, 7s, 8s and 9s; 
	//two Draw Two cards; two Skip cards; and two Reverse cards.In addition there are four Wild cards and four Wild Draw Four cards
	PathPrefix = "../../Content/UnoImages/";
	FullDeck = [
		//red
		{ name: "red0", imageLocation: "Red_0.png", amount: 1 },
		{ name: "red1", imageLocation: "Red_1.png", amount: 2 },
		{ name: "red2", imageLocation: "Red_2.png", amount: 2 },
		{ name: "red3", imageLocation: "Red_3.png", amount: 2 },
		{ name: "red4", imageLocation: "Red_4.png", amount: 2 },
		{ name: "red5", imageLocation: "Red_5.png", amount: 2 },
		{ name: "red6", imageLocation: "Red_6.png", amount: 2 },
		{ name: "red7", imageLocation: "Red_7.png", amount: 2 },
		{ name: "red8", imageLocation: "Red_8.png", amount: 2 },
		{ name: "red9", imageLocation: "Red_9.png", amount: 2 },
		{ name: "redDraw", imageLocation: "Red_Draw.png", amount: 2 },
		{ name: "redReverse", imageLocation: "Red_Reverse.png", amount: 2 },
		{ name: "redSkip", imageLocation: "Red_Skip.png", amount: 2 },
		//blue
		{ name: "blue0", imageLocation: "Blue_0.png", amount: 1 },
		{ name: "blue1", imageLocation: "Blue_1.png", amount: 2 },
		{ name: "blue2", imageLocation: "Blue_2.png", amount: 2 },
		{ name: "blue3", imageLocation: "Blue_3.png", amount: 2 },
		{ name: "blue4", imageLocation: "Blue_4.png", amount: 2 },
		{ name: "blue5", imageLocation: "Blue_5.png", amount: 2 },
		{ name: "blue6", imageLocation: "Blue_6.png", amount: 2 },
		{ name: "blue7", imageLocation: "Blue_7.png", amount: 2 },
		{ name: "blue8", imageLocation: "Blue_8.png", amount: 2 },
		{ name: "blue9", imageLocation: "Blue_9.png", amount: 2 },
		{ name: "blueDraw", imageLocation: "Blue_Draw.png", amount: 2 },
		{ name: "blueReverse", imageLocation: "Blue_Reverse.png", amount: 2 },
		{ name: "blueSkip", imageLocation: "Blue_Skip.png", amount: 2 },
		//green
		{ name: "green0", imageLocation: "Green_0.png", amount: 1 },
		{ name: "green1", imageLocation: "Green_1.png", amount: 2 },
		{ name: "green2", imageLocation: "Green_2.png", amount: 2 },
		{ name: "green3", imageLocation: "Green_3.png", amount: 2 },
		{ name: "green4", imageLocation: "Green_4.png", amount: 2 },
		{ name: "green5", imageLocation: "Green_5.png", amount: 2 },
		{ name: "green6", imageLocation: "Green_6.png", amount: 2 },
		{ name: "green7", imageLocation: "Green_7.png", amount: 2 },
		{ name: "green8", imageLocation: "Green_8.png", amount: 2 },
		{ name: "green9", imageLocation: "Green_9.png", amount: 2 },
		{ name: "greenDraw", imageLocation: "Green_Draw.png", amount: 2 },
		{ name: "greenReverse", imageLocation: "Green_Reverse.png", amount: 2 },
		{ name: "greenSkip", imageLocation: "Green_Skip.png", amount: 2 },
		//yellow
		{ name: "yellow0", imageLocation: "Yellow_0.png", amount: 1 },
		{ name: "yellow1", imageLocation: "Yellow_1.png", amount: 2 },
		{ name: "yellow2", imageLocation: "Yellow_2.png", amount: 2 },
		{ name: "yellow3", imageLocation: "Yellow_3.png", amount: 2 },
		{ name: "yellow4", imageLocation: "Yellow_4.png", amount: 2 },
		{ name: "yellow5", imageLocation: "Yellow_5.png", amount: 2 },
		{ name: "yellow6", imageLocation: "Yellow_6.png", amount: 2 },
		{ name: "yellow7", imageLocation: "Yellow_7.png", amount: 2 },
		{ name: "yellow8", imageLocation: "Yellow_8.png", amount: 2 },
		{ name: "yellow9", imageLocation: "Yellow_9.png", amount: 2 },
		{ name: "yellowDraw", imageLocation: "Yellow_Draw.png", amount: 2 },
		{ name: "yellowReverse", imageLocation: "Yellow_Reverse.png", amount: 2 },
		{ name: "yellowSkip", imageLocation: "Yellow_Skip.png", amount: 2 },
		//wildcards
		{ name: "wild", imageLocation: "Wild.png", amount: 4 },
		{ name: "wildDraw", imageLocation: "Wild_Draw.png", amount: 4 }
	];

	StockPile = [];
	Deck = [];
	Players = [];
	Rules = new UnoRuleChecker();
	DirectionClockwise = true;
	//newPlayerObjects contains an array of newly created playerObjects with their respective clientId's in it.
	//these id's relate to an array server side that then relates to client id's to send requests to
	constructor(newPlayerObjects) {
		//set the stockPile to the predefined Deck.
		this.StockPile = this.FullDeck;
		this.Deck = [];
		this.Players = newPlayerObjects;
	}

	getCardsInStockPile() {
		var card;
		var stock = this.StockPile;
		var count = 0;
		for (card of stock) {
			count += card.amount;
		}

		return count;
	}

	takeCardFromStock() {
		var random = Math.round(Math.random() * this.getCardsInStockPile());
		console.log("getting card:", random);
		var card;
		var stock = this.StockPile;
		var count = 0;
		for (card of stock) {
			var prevCount = count;
			count += card.amount;
			if (prevCount === random || random > prevCount && random <= count) {
				if (card.amount !== 0) {
					card.amount--;
					return { imageLocation: card.imageLocation, name: card.name, amount: 1 };
				}
			}

		}
		return null;
	}

	dealCardsToPlayers() {
		for (var player of this.Players) {
			//deal 7 cards to each player
			for (var i = 0; i < 7; i++) {
				var card = this.takeCardFromStock();
				player.cards.push(card);
			}
		}
	}

	//handles all the logic for playing a card, it checks if its your turn, then it checks if you own that card, if the card is playable, and then displays it.
	playCard(card, playerId) {
		//check if this is the first card on the pile
		var player = this.Players.find(function (element) {
			return element.id === playerId;
		});

		var fndCard = player.cards.find(function (element) {
			return element.name === card.name;
		});

		var playerIndex = this.Players.findIndex(obj => obj.id === playerId);
		var cardsIndex = this.Players[playerIndex].cards.findIndex(obj => obj.name === card.name);

		//check if the card was actually found
		if (fndCard.name === card.name) {
			if (this.Deck.length === 0) {
				this.Deck.push(card);
				return { played: true, effects: null };
			}
			else {
				//check if the card is allowed to be played
				if (this.Rules.check(this.Deck[this.Deck.length - 1].name, card.name)) {
					//the card is confirmed, place it on the deck
					this.Deck.push(card);

					if (this.Players[playerIndex].cards[cardsIndex].amount === 1) {
						this.Players[playerIndex].cards.splice(cardsIndex, 1);
					}
					else {
						this.Players[playerIndex].cards[cardsIndex].amount--;
					}

					//check if the current top card has any special functions (skip, turn around, take 2, take 4)
					var specialEffect = this.Rules.checkSpecials(card.name);

					return { played: true, effects: specialEffect };
				}
			}
		}

		return { played: false, effects: null };
	}

	//deals cards to all player objects, then puts one card on the deck
	dealFirstRoundToPlayers() {
		for (var player of this.Players) {
			//deal 7 cards to each player
			for (var i = 0; i < 7; i++) {
				var card = this.takeCardFromStock();
				player.cards.push(card);
			}
		}

		var card1 = this.takeCardFromStock();
		this.Deck.push(card1);
	}

	//deals cards to the provided player, with the specified amount
	dealCardAmountToPlayer(playerId, amount) {
		//get the player index from the player Array
		var playerIndex = this.Players.findIndex(obj => obj.id === playerId);

		var player = this.Players[playerIndex];
		//deal 7 cards to each player
		for (var i = 0; i < amount; i++) {
			var card = this.takeCardFromStock();
			player.cards.push(card);
		}
	}

	//gets the current card that should be displayed
	getTopCardFromPlayingStack() {
		console.log("deck size: ", this.Deck.length);
		return this.Deck[this.Deck.length - 1];
	}
}