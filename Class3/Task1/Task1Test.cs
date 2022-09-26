using NUnit.Framework;
using static NUnit.Framework.Assert;
using static Task1.Task1;

namespace Task1;

public class Tests
{
    [Test]
    public void RoundWinnerTest()
    {
        throw new NotImplementedException();
    }

    [Test]
    public void FullDeckTest()
    {
        var deck = FullDeck();
        That(deck, Has.Count.EqualTo(DeckSize));
        throw new NotImplementedException();
    }

    [Test]
    public void RoundTest()
    {
        throw new NotImplementedException();
    }

    [Test]
    public void Game2CardsTest()
    {
        var six = TODO<Card>();
        var ace = TODO<Card>();
        Dictionary<Player, List<Card>> hands = new Dictionary<Player, List<Card>>
        {
            { TODO<Player>(), new List<Card> {six} },
            { TODO<Player>(), new List<Card> {ace} }
        };
        var gameWinner = Game(hands);
        That(gameWinner, Is.EqualTo(TODO<Player>()));
    }
    
    private static T TODO<T>()
    {
        throw new NotImplementedException();
    }
}