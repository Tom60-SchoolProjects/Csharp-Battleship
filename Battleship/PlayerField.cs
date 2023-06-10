using System.Diagnostics;
using Battleship.API;

namespace Battleship;

/// <summary>
/// Define the ships that a player has on its board
/// </summary>
public class PlayerField {
    #region Properties
    public  GridConfig             Config { get; }
    public  List<Ship>             Ships  { get; }
    public  List<(uint x, uint y)> Missed { get; } = new();
    private Random                 Random { get; } = new();
    #endregion

    #region Methods
    /// <summary>
    /// Check if there is a ship at the position <c>pos</c>
    /// </summary>
    public Ship? ShipAt((uint x, uint y) pos) {
        return Ships.FirstOrDefault(ship => ship.Direction switch {
                                                Direction.East => Range(ship.Start.x, ship.Length)
                                                                     .Contains(pos.x)
                                                               && pos.y == ship.Start.y,
                                                Direction.South => Range(ship.Start.y, ship.Length)
                                                                      .Contains(pos.y)
                                                                && pos.x == ship.Start.x,
                                                Direction.West => Range(ship.Start.x - ship.Length + 1, ship.Length)
                                                                     .Contains(pos.x)
                                                               && pos.y == ship.Start.y,
                                                Direction.North => Range(ship.Start.y - ship.Length + 1, ship.Length)
                                                                      .Contains(pos.y)
                                                                && pos.x == ship.Start.x,
                                                _ => throw new UnreachableException(),
                                            });
    }

    /// <summary>
    /// Shoot at the position <c>pos</c>
    /// </summary>
    /// <returns>Whether a ship was hit</returns>
    public bool ShootAt((uint x, uint y) pos) {
        Ship? shipAt = ShipAt(pos);

        // Check if there is a ship at the given position
        if (shipAt is null)
            return false;

        long xDiff = Math.Abs((long) shipAt.Start.x - pos.x);
        long yDiff = Math.Abs((long) shipAt.Start.y - pos.y);
        long idx   = long.Max(xDiff, yDiff);

        if (shipAt.Broken[idx]) {
            return false;
        }

        shipAt.Broken[idx] = true;
        return true;
    }

    /// <summary>
    /// Generate ships on the field, depending on the config supplied
    /// </summary>
    /// <exception cref="TimeoutException">Thrown if the generation reached a seemingly "dead" state</exception>
    private void InitShips() {
        var toPlace  = new List<ConfigShip>(Config.Ships);
        var attempts = 0;

        while (toPlace.Count != 0) {
            ConfigShip ship = toPlace[0];

            uint startX = Convert.ToUInt32(Random.Next((int) Config.Size.X)),
                 startY = Convert.ToUInt32(Random.Next((int) Config.Size.Y));

            List<Direction> directions = AvailableDirections((startX, startY), ship.Size);
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

    /// <summary>
    /// For a given position, find all available direction for a ship of size <c>size</c>
    /// </summary>
    private List<Direction> AvailableDirections((uint x, uint y) start, uint size) {
        return DirectionExt.All.Where(direction => Enumerable.Range(0, (int) size)
                                                             .Select(distance
                                                                         => (x: start.x + direction.Tuple().x * distance,
                                                                             y: start.y
                                                                              + direction.Tuple().y * distance))
                                                             .All(pos => pos.x >= 0
                                                                      && pos.x < Config.Size.X
                                                                      && pos.y >= 0
                                                                      && pos.y < Config.Size.Y
                                                                      && ShipAt((Convert.ToUInt32(pos.x),
                                                                                 Convert.ToUInt32(pos.y))) is null))
                           .ToList();
    }

    /// <summary>
    /// Mimic the <c>Range</c> method normally only defined for <c>int</c>
    /// </summary>
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
        Config = GridConfig.Singleton;

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

    public PlayerField(List<Ship> ships) {
        Config = GridConfig.Singleton;
        Ships  = ships;
    }
    #endregion
}