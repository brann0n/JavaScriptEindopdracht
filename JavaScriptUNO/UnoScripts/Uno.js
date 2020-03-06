﻿class Uno {

    //108 cards in uno: 4 colors, each consisting of one 0 card, two 1 cards, two 2s, 3s, 4s, 5s, 6s, 7s, 8s and 9s; 
    //two Draw Two cards; two Skip cards; and two Reverse cards.In addition there are four Wild cards and four Wild Draw Four cards
    PathPrefix = "~/Content/UnoImages/";
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

    //playerIdList contains the id's of all the players participating in the game.
    //these id's relate to an array server side that then relates to client id's to send requests to
    constructor(playerIdList) {
        //set the stockPile to the predefined Deck.
        this.StockPile = this.FullDeck;
        this.Deck = [];
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
            if (count === random || count > random && random <= count + card.amount) {              
                return card;
            }
            count += card.amount;
        }
        return null;
    }

    dealCardsToPlayers() {
        for (var player of this.Players) {
            //
        }
    }
}