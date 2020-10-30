class UnoRuleChecker {

	constructor() {
		this.red = "red";
		this.green = "green";
		this.blue = "blue";
		this.yellow = "yellow";
		this.wild = "wild";
		this.previousPickedColor = "";
	}

	check(topCard, playedCard) {
		var t = this.getCardColorAndType(topCard);
		var p = this.getCardColorAndType(playedCard);
		if (this.checkColorAndType(t, p, this.red)
			|| this.checkColorAndType(t, p, this.green)
			|| this.checkColorAndType(t, p, this.blue)
			|| this.checkColorAndType(t, p, this.yellow)
			|| this.checkWildCardAndType(t, p)) {
			return true;
		}

		return false;
	}

	checkColorAndType(topCard, playedCard, color) {
		if (topCard.color === color) {
			if (playedCard.color === color || playedCard.type === topCard.type || playedCard.color === this.wild) {
				//either the color or the type matches, or the card is a wildcard and can be played anyway
				return true;
			}
		}
		return false;
	}

	checkWildCardAndType(topCard, playedCard) {
		if (topCard.color === this.wild) {
			//needs the picked color from the previous player
			if (playedCard.color === this.previousPickedColor || playedCard.color === this.wild) {
				//either the player had a card with the correct color, or its another wild card
				return true;
			}
			else if (this.previousPickedColor === "") {
				return true; //because that means the top card was a wild card, there is no set color, and a card was played, by game logic that means its the first card on the stack.
			}
		}
		return false;
	}

	getCardColorAndType(card) {
		var returnObject = { color: "", type: "" };
		if (card.startsWith(this.red)) {
			returnObject.color = this.red;
			returnObject.type = card.substring(returnObject.color.length);
		}
		if (card.startsWith(this.green)) {
			returnObject.color = this.green;
			returnObject.type = card.substring(returnObject.color.length);
		}
		if (card.startsWith(this.blue)) {
			returnObject.color = this.blue;
			returnObject.type = card.substring(returnObject.color.length);
		}
		if (card.startsWith(this.yellow)) {
			returnObject.color = this.yellow;
			returnObject.type = card.substring(returnObject.color.length);
		}
		if (card.startsWith(this.wild)) {
			returnObject.color = this.wild;
			returnObject.type = card.substring(returnObject.color.length);
		}

		return returnObject;
	}

	checkSpecials(card) {
		var cardObj = this.getCardColorAndType(card);
		var returnObject = { sendColorWheel: false, cardDrawAmount: 0, skipNextPerson: false, reverseOrder: false };
		switch (cardObj.type) {
			case "Skip":
				returnObject.skipNextPerson = true;
				break;
			case "Reverse":
				returnObject.reverseOrder = true;
				break;
			case "Draw":
				if (cardObj.color === this.wild) {
					returnObject.sendColorWheel = true;
					returnObject.cardDrawAmount = 4;
				}
				else {
					returnObject.cardDrawAmount = 2;
				}
				break;
			default:
				//its either a number or the only draw card
				if (cardObj.color === this.wild) {
					returnObject.sendColorWheel = true;
				}
				else {
					returnObject = null;
				}
				break;
		}

		return returnObject;
	}

	setPickedColor(color) {
		this.previousPickedColor = color;
	}
}