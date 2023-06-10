using System.Diagnostics;
using Battleship.API;

namespace Battleship;

public class PlayerField {
    #region Properties
    public  GridConfig Config { get; }
    public  List<Ship> Ships  { get; }
    private Random     Random { get; } = new();
    #endregion

    #region Methods
    public Ship? ShipAt((uint x, uint y) pos) {
        return Ships.FirstOrDefault(ship => ship.Direction switch {
                                                Direction.East => Range(ship.Start.x, ship.Length)
                                                                     .Contains(pos.x)
                                                               && pos.y == ship.Start.y,
                                                Direction.South => Range(ship.Start.y, ship.Length)
                                                                      .Contains(pos.y)
                                                                && pos.x == ship.Start.x,
                                                Direction.West => Range(ship.Start.x - ship.Length, ship.Length)
                                                                     .Contains(pos.x)
                                                               && pos.y == ship.Start.y,
                                                Direction.North => Range(ship.Start.x - ship.Length, ship.Length)
                                                                      .Contains(pos.x)
                                                                && pos.y == ship.Start.y,
                                                _ => throw new UnreachableException(),
                                            });
    }

    public bool ShootAt((uint x, uint y) pos) {
        Ship? shipAt = ShipAt(pos);

        // Check if there is a ship at the given position
        if (shipAt is null)
            return false;

        long xDiff = (long) shipAt.Start.x - pos.x;
        long yDiff = (long) shipAt.Start.y - pos.y;
        long idx   = long.Max(xDiff, yDiff);

        if (shipAt.Broken[idx]) {
            return false;
        }

        shipAt.Broken[idx] = true;
        return true;
    }

    private void InitShips() {
        var toPlace  = new List<ConfigShip>(Config.Ships);
        var attempts = 0;

        while (toPlace.Count != 0) {
            ConfigShip ship = toPlace[0];

            uint startX = Convert.ToUInt32(Random.Next((int) Config.Size.X)),
                 startY = Convert.ToUInt32(Random.Next((int) Config.Size.Y));

            List<Direction> directions = AvailableDirections(startX, startY, ship.Size);
            if (directions.Count is 0) {
                attempts += 1;
                if (attempts > 10_000) {
                    throw new TimeoutException("Max number of placement attempts reached");
                }

                continue;
            }

            Direction direction = directions[Random.Next(directions.Count)];

            Ships.Add(new Ship((startX, startY), ship.Size, direction));
            toPlace.RemoveAt(0);
        }
    }

    private List<Direction> AvailableDirections(uint startX, uint startY, uint size) {
        return DirectionExt.All.Where(direction => Enumerable.Range(0, (int) size)
                                                             .Select(distance
                                                                         => (x: startX + direction.Tuple().x * distance,
                                                                             y: startY
                                                                              + direction.Tuple().y * distance))
                                                             .All(pos => pos.x >= 0
                                                                      && pos.x < Config.Size.X
                                                                      && pos.y >= 0
                                                                      && pos.y < Config.Size.Y
                                                                      && ShipAt((Convert.ToUInt32(pos.x),
                                                                                 Convert.ToUInt32(pos.y))) is null))
                           .ToList();
    }

    private static IEnumerable<uint> Range(uint startValue, uint count) {
        uint index = startValue;

        while (count > 0) {
            yield return index++;

            count--;
        }
    }
    #endregion

    #region Constructor
    public PlayerField() {
        Config = new GridConfig();

        Ships = new List<Ship>();
        for (var i = 0; i < 10; i++) {
            try {
                InitShips();
            } catch (TimeoutException) {
                continue;
            }

            return;
        }

        throw new TimeoutException("Couldn't generate ship field");
    }
    #endregion
}