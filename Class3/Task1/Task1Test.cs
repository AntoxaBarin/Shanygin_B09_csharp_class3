using NUnit.Framework;
using static NUnit.Framework.Assert;
using static Task1.Task1;

namespace Task1;

public class Tests
{
    [Test]
    public void RoundWinnerTest()
    {
        var table1 = new List<Card>() { new Card(Rank.six, Suit.clubs), new Card(Rank.six, Suit.hearts) };
        Card card1 = new Card(Rank.jack, Suit.diamonds);
        Card card2 = new Card(Rank.ace, Suit.spades);
        Player? Winner1 = RoundWinner(card1, card2);
        That(Winner1, Is.EqualTo(Player.SecondPlayer));
        
        var table2 = new List<Card>() { new Card(Rank.eight, Suit.clubs), new Card(Rank.king, Suit.hearts) };
        Card card3 = new Card(Rank.seven, Suit.hearts);
        Card card4 = new Card(Rank.six, Suit.hearts);
        Player? Winner2 = RoundWinner(card3, card4);
        That(Winner2, Is.EqualTo(Player.FirstPlayer));
    }

    [Test]
    public void FullDeckTest()
    {
        var deck = FullDeck();
        That(deck, Has.Count.EqualTo(DeckSize));
    }

    [Test]
    public void RoundTest()
    {
        //List<Card> table = new List<Card>() {new Card(Rank.queen, Suit.clubs), new Card(Rank.king, Suit.hearts)};
        Card card10 = new Card(Rank.queen, Suit.hearts);
        Card card20 = new Card(Rank.king, Suit.hearts);
        Player? winner = RoundWinner(card10, card20);
        That(winner, Is.EqualTo(Player.SecondPlayer));
    }

    [Test]
    public void Game2CardsTest()
    {
        var six = new Card(Rank.six, Suit.clubs);
        var ace = new Card(Rank.ace, Suit.hearts);
        Dictionary<Player, List<Card>> hands = new Dictionary<Player, List<Card>>
        {
            { Player.FirstPlayer, new List<Card> {six} },
            { Player.SecondPlayer, new List<Card> {ace} }
        };
        var gameWinner = Game(hands);
        That(gameWinner, Is.EqualTo(Player.SecondPlayer));
    }
    
}

