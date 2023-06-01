namespace Battleship.Test;

public class PlayerFieldTests {
    [Test]
    public void PlayerFieldTest()
    {
        var player = new PlayerField();

        player.ShootAt((0, 0));
        
        Assert.Pass();
    }
}