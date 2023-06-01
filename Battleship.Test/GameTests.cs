namespace Battleship.Test;

public class GameTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void GameInitTest()
    {
        var game = new Game();

        Assert.IsTrue(game.PlayersField.Length != 0);
        
        Assert.Pass();
    }
}