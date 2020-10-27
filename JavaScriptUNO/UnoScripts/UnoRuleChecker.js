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
        //force string types...
        var t = "" + topCard;
        var p = "" + playedCard;
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
        var type = "";
        if (topCard.startsWith(color)) {
            type = topCard.substring(color.length);
            if (playedCard.startsWith(color) || playedCard.includes(type) || playedCard.startsWith(this.wild)) {
                //either the color or the type matches, or the card is a wildcard and can be played anyway
                return true;
            }
        }
        return false;
	}

    checkWildCardAndType(topCard, playedCard) {
        if (topCard.startsWith(this.wild)) {
            //needs the picked color from the previous player
            if (playedCard.startsWith(this.previousPickedColor) || playedCard.startsWith(this.wild)) {
                //either the player had a card with the correct color, or its another wild card
                return true;
            }
        }
        return false;
	}

    checkSpecials(card) {




        return {};
    }

    setPickedColor(color) {
        previousPickedColor = color;
	}
}