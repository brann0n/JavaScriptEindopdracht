class UnoRuleChecker {
    constructor() {

    }

    check(topCard, playedCard) {
        var red = "red";
        var green = "green";
        var blue = "blue";
        var yellow = "yellow";
        var t = "" + topCard;
        var p = "" + playedCard;
        var type = "";
        //red
        if (t.startsWith(red)) {
            //get the remainder of the card type
            type = t.substring(red.length - 1);
            if (p.startsWith(red)) {
                //colors match
                return true;
            }
            else {
                if (p.includes(type)) {
                    //card types match
                    return true;
                }
            }
        }
        //green
        else if (t.startsWith(green)) {
            //get the remainder of the card type
            type = t.substring(green.length - 1);
            if (p.startsWith(green)) {
                //colors match
                return true;
            }
            else {
				if (p.includes(type)) {
                    //card types match
                    return true;
                }
            }
        }
        //blue
        else if (t.startsWith(blue)) {
            //get the remainder of the card type
            type = t.substring(blue.length - 1);
            if (p.startsWith(blue)) {
                //colors match
                return true;
            }
            else {
				if (p.includes(type)) {
                    //card types match
                    return true;
                }
            }
        }
        //yellow
		else if (t.startsWith(yellow)) {
			console.log("checking yellow");
            //get the remainder of the card type
            type = t.substring(yellow.length - 1);
            if (p.startsWith(yellow)) {
                //colors match
                return true;
            }
            else {
				if (p.includes(type)) {
                    //card types match
                    return true;
                }
            }

        }
        else {
            //card is a wildcard
			return true;
        }

        return false;
    }
}