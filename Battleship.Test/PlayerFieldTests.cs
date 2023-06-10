namespace Battleship.Test;

public class PlayerFieldTests
{
    [Test]
    public void ShipAtTest()
    {
        var field = new PlayerField();

        foreach (var ship in field.Ships)
        {
            Assert.That(field.ShipAt(ship.Start), Is.Not.Null);

            for (uint i = 1; i < ship.Length; i++)
            {
                Assert.That(ship.Direction switch
                {
                    API.Direction.North => field.ShipAt((ship.Start.x, ship.Start.y - i)),
                    API.Direction.South => field.ShipAt((ship.Start.x, ship.Start.y + i)),
                    API.Direction.East => field.ShipAt((ship.Start.x + i, ship.Start.y)),
                    API.Direction.West => field.ShipAt((ship.Start.x - i, ship.Start.y)),
                    _ => throw new NotImplementedException()
                }, Is.Not.Null);
            }
        }

        Assert.Pass();
    }

    [Test]
    public void ShootAtTest()
    {
        var field = new PlayerField();

        foreach (var ship in field.Ships)
        {
            Assert.That(field.ShootAt(ship.Start), Is.True);

            for (uint i = 1; i < ship.Length; i++)
            {
                Assert.That(ship.Direction switch
                {
                    API.Direction.North => field.ShootAt((ship.Start.x, ship.Start.y - i)),
                    API.Direction.South => field.ShootAt((ship.Start.x, ship.Start.y + i)),
                    API.Direction.East => field.ShootAt((ship.Start.x + i, ship.Start.y)),
                    API.Direction.West => field.ShootAt((ship.Start.x - i, ship.Start.y)),
                    _ => throw new NotImplementedException()
                }, Is.True);
            }
        }

        Assert.Pass();
    }
}