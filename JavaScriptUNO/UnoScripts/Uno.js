class Uno {

    //108 cards in uno: 4 colors, each consisting of one 0 card, two 1 cards, two 2s, 3s, 4s, 5s, 6s, 7s, 8s and 9s; 
    //two Draw Two cards; two Skip cards; and two Reverse cards.In addition there are four Wild cards and four Wild Draw Four cards
    PathPrefix = "../../Content/UnoImages/";
    FullDeck = [
        //red
        { name: "red0", imageLocation: "Red_0.png" },
        { name: "red1", imageLocation: "Red_1.png" },
        { name: "red2", imageLocation: "Red_2.png" },
        { name: "red3", imageLocation: "Red_3.png" },
        { name: "red4", imageLocation: "Red_4.png" },
        { name: "red5", imageLocation: "Red_5.png" },
        { name: "red6", imageLocation: "Red_6.png" },
        { name: "red7", imageLocation: "Red_7.png" },
        { name: "red8", imageLocation: "Red_8.png" },
        { name: "red9", imageLocation: "Red_9.png" },
        { name: "redDraw", imageLocation: "Red_Draw.png" },
        { name: "redReverse", imageLocation: "Red_Reverse.png" },
        { name: "redSkip", imageLocation: "Red_Skip.png" },
        { name: "red1", imageLocation: "Red_1.png" },
        { name: "red2", imageLocation: "Red_2.png" },
        { name: "red3", imageLocation: "Red_3.png" },
        { name: "red4", imageLocation: "Red_4.png" },
        { name: "red5", imageLocation: "Red_5.png" },
        { name: "red6", imageLocation: "Red_6.png" },
        { name: "red7", imageLocation: "Red_7.png" },
        { name: "red8", imageLocation: "Red_8.png" },
        { name: "red9", imageLocation: "Red_9.png" },
        { name: "redDraw", imageLocation: "Red_Draw.png" },
        { name: "redReverse", imageLocation: "Red_Reverse.png" },
        { name: "redSkip", imageLocation: "Red_Skip.png" },
        //blue
        { name: "blue0", imageLocation: "Blue_0.png" },
        { name: "blue1", imageLocation: "Blue_1.png" },
        { name: "blue2", imageLocation: "Blue_2.png" },
        { name: "blue3", imageLocation: "Blue_3.png" },
        { name: "blue4", imageLocation: "Blue_4.png" },
        { name: "blue5", imageLocation: "Blue_5.png" },
        { name: "blue6", imageLocation: "Blue_6.png" },
        { name: "blue7", imageLocation: "Blue_7.png" },
        { name: "blue8", imageLocation: "Blue_8.png" },
        { name: "blue9", imageLocation: "Blue_9.png" },
        { name: "blueDraw", imageLocation: "Blue_Draw.png" },
        { name: "blueReverse", imageLocation: "Blue_Reverse.png" },
        { name: "blueSkip", imageLocation: "Blue_Skip.png" },
        { name: "blue1", imageLocation: "Blue_1.png" },
        { name: "blue2", imageLocation: "Blue_2.png" },
        { name: "blue3", imageLocation: "Blue_3.png" },
        { name: "blue4", imageLocation: "Blue_4.png" },
        { name: "blue5", imageLocation: "Blue_5.png" },
        { name: "blue6", imageLocation: "Blue_6.png" },
        { name: "blue7", imageLocation: "Blue_7.png" },
        { name: "blue8", imageLocation: "Blue_8.png" },
        { name: "blue9", imageLocation: "Blue_9.png" },
        { name: "blueDraw", imageLocation: "Blue_Draw.png" },
        { name: "blueReverse", imageLocation: "Blue_Reverse.png" },
        { name: "blueSkip", imageLocation: "Blue_Skip.png" },
        //green
        { name: "green0", imageLocation: "Green_0.png" },
        { name: "green1", imageLocation: "Green_1.png" },
        { name: "green2", imageLocation: "Green_2.png" },
        { name: "green3", imageLocation: "Green_3.png" },
        { name: "green4", imageLocation: "Green_4.png" },
        { name: "green5", imageLocation: "Green_5.png" },
        { name: "green6", imageLocation: "Green_6.png" },
        { name: "green7", imageLocation: "Green_7.png" },
        { name: "green8", imageLocation: "Green_8.png" },
        { name: "green9", imageLocation: "Green_9.png" },
        { name: "greenDraw", imageLocation: "Green_Draw.png" },
        { name: "greenReverse", imageLocation: "Green_Reverse.png" },
        { name: "greenSkip", imageLocation: "Green_Skip.png" },
        { name: "green1", imageLocation: "Green_1.png" },
        { name: "green2", imageLocation: "Green_2.png" },
        { name: "green3", imageLocation: "Green_3.png" },
        { name: "green4", imageLocation: "Green_4.png" },
        { name: "green5", imageLocation: "Green_5.png" },
        { name: "green6", imageLocation: "Green_6.png" },
        { name: "green7", imageLocation: "Green_7.png" },
        { name: "green8", imageLocation: "Green_8.png" },
        { name: "green9", imageLocation: "Green_9.png" },
        { name: "greenDraw", imageLocation: "Green_Draw.png" },
        { name: "greenReverse", imageLocation: "Green_Reverse.png" },
        { name: "greenSkip", imageLocation: "Green_Skip.png" },
        //yellow
        { name: "yellow0", imageLocation: "Yellow_0.png" },
        { name: "yellow1", imageLocation: "Yellow_1.png" },
        { name: "yellow2", imageLocation: "Yellow_2.png" },
        { name: "yellow3", imageLocation: "Yellow_3.png" },
        { name: "yellow4", imageLocation: "Yellow_4.png" },
        { name: "yellow5", imageLocation: "Yellow_5.png" },
        { name: "yellow6", imageLocation: "Yellow_6.png" },
        { name: "yellow7", imageLocation: "Yellow_7.png" },
        { name: "yellow8", imageLocation: "Yellow_8.png" },
        { name: "yellow9", imageLocation: "Yellow_9.png" },
        { name: "yellow1", imageLocation: "Yellow_1.png" },
        { name: "yellow2", imageLocation: "Yellow_2.png" },
        { name: "yellow3", imageLocation: "Yellow_3.png" },
        { name: "yellow4", imageLocation: "Yellow_4.png" },
        { name: "yellow5", imageLocation: "Yellow_5.png" },
        { name: "yellow6", imageLocation: "Yellow_6.png" },
        { name: "yellow7", imageLocation: "Yellow_7.png" },
        { name: "yellow8", imageLocation: "Yellow_8.png" },
        { name: "yellow9", imageLocation: "Yellow_9.png" },
        { name: "yellowDraw", imageLocation: "Yellow_Draw.png" },
        { name: "yellowReverse", imageLocation: "Yellow_Reverse.png" },
        { name: "yellowSkip", imageLocation: "Yellow_Skip.png" },
        { name: "yellowDraw", imageLocation: "Yellow_Draw.png" },
        { name: "yellowReverse", imageLocation: "Yellow_Reverse.png" },
        { name: "yellowSkip", imageLocation: "Yellow_Skip.png" },
        //wildcards
        { name: "wild", imageLocation: "Wild.png" },
        { name: "wild", imageLocation: "Wild.png" },
        { name: "wild", imageLocation: "Wild.png" },
        { name: "wild", imageLocation: "Wild.png" },
        { name: "wildDraw", imageLocation: "Wild_Draw.png" },
        { name: "wildDraw", imageLocation: "Wild_Draw.png" },
        { name: "wildDraw", imageLocation: "Wild_Draw.png" },
        { name: "wildDraw", imageLocation: "Wild_Draw.png" }
    ];

    StockPile = [];
    Deck = [];
    Players = [];
    Rules = new UnoRuleChecker();
    DirectionClockwise = true;
    //newPlayerObjects contains an array of newly created playerObjects with their respective clientId's in it.
    //these id's relate to an array server side that then relates to client id's to send requests to
    constructor(newPlayerObjects) {
        //set the stockPile to the predefined Deck and check how big the deck should be
        console.log(this.FullDeck.length);
        if (newPlayerObjects !== null) {
            if (newPlayerObjects.length > 1 && newPlayerObjects.length <= 8) {
                //8 players use 1 set of cards
                this.StockPile = this.FullDeck;
            } else if (newPlayerObjects.length > 8 && newPlayerObjects.length <= 16) {
                //up to 16 players, use 2 sets of cards
                this.StockPile = this.FullDeck.concat(this.FullDeck);
            } else if (newPlayerObjects.length > 16 && newPlayerObjects.length <= 24) {
                //up to 24 players, use 3 sets of cards
                this.StockPile = this.FullDeck.concat(this.FullDeck).concat(this.FullDeck);
            } else if (newPlayerObjects.length > 24 && newPlayerObjects.length <= 32) {
                //up to 32 players, use 4 sets of cards
                this.StockPile = this.FullDeck.concat(this.FullDeck).concat(this.FullDeck).concat(this.FullDeck);
            }
        }
        else {
            this.StockPile = [];
        }
        this.Deck = [];
        this.Players = newPlayerObjects;
    }

    //gets the amount of cards currently in the stockpile
    getCardsInStockPile() {
        return this.StockPile.length;
    }

    takeCardFromStock() {
        let random = Math.round(Math.random() * (this.getCardsInStockPile() - 1));
        let card = this.StockPile.splice(random, 1);
        this.checkForStockPileRecycle();
        return card[0];
    }

    checkForStockPileRecycle() {
        let cardsAvail = this.getCardsInStockPile();
        if (cardsAvail <= 1) {
            console.log("doing a recycle");
            //take all the current cards out of the deck, and place them back into the stockpile, but leave the last played card.
            this.StockPile = this.Deck.splice(0, this.Deck.length - 1); //take 1 off for array index
        }
    }

    dealCardsToPlayers() {
        for (let player of this.Players) {
            //deal 7 cards to each player
            for (let i = 0; i < 7; i++) {
                let card = this.takeCardFromStock();
                player.cards.push(card);
            }
        }
    }

    //handles all the logic for playing a card, it checks if its your turn, then it checks if you own that card, if the card is playable, and then displays it.
    playCard(card, playerId) {
        //check if this is the first card on the pile
        let player = this.Players.find(function (element) {
            return element.id === playerId;
        });

        let fndCard = player.cards.find(function (element) {
            return element.name === card.name;
        });

        let playerIndex = this.Players.findIndex(obj => obj.id === playerId);
        let cardsIndex = this.Players[playerIndex].cards.findIndex(obj => obj.name === card.name);

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
                    this.Players[playerIndex].cards.splice(cardsIndex, 1);


                    //check if the current top card has any special functions (skip, turn around, take 2, take 4)
                    let specialEffect = this.Rules.checkSpecials(card.name);

                    return { played: true, effects: specialEffect };
                }
            }
        }

        return { played: false, effects: null };
    }

    //deals cards to all player objects, then puts one card on the deck
    dealFirstRoundToPlayers() {
        for (let player of this.Players) {
            //deal 7 cards to each player
            for (let i = 0; i < 7; i++) {
                let card = this.takeCardFromStock();
                player.cards.push(card);
            }
        }

        let card1 = this.takeCardFromStock();
        this.Deck.push(card1);
    }

    //deals cards to the provided player, with the specified amount
    dealCardAmountToPlayer(playerId, amount) {
        //get the player index from the player Array
        let playerIndex = this.Players.findIndex(obj => obj.id === playerId);

        let player = this.Players[playerIndex];
        //deal 7 cards to each player
        for (let i = 0; i < amount; i++) {
            let card = this.takeCardFromStock();
            player.cards.push(card);
        }
    }

    //gets the current card that should be displayed
    getTopCardFromPlayingStack() {
        console.log("deck size: ", this.Deck.length);
        return this.Deck[this.Deck.length - 1];
    }
}