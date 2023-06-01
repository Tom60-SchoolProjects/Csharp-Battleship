using System.Diagnostics;
using Battleship.API;

namespace Battleship;

public class PlayerField {
    #region Constants
    private static readonly (int x, int y)[ ] Directions = { (0, 1), (1, 0), (0, -1), (-1, 0) };
    #endregion

    #region Properties
    public  GridConfig Config { get; }
    public  Cell[ ][ ] Grid   { get; }
    private Random     Random { get; } = new();
    #endregion

    #region Methods
    public bool ShootAt((int x, int y) pos) {
        Cell cell = Grid[pos.x][pos.y];
        switch (cell) {
            case EmptyCell: {
                return false;
            }

            case ShipCell shipCell: {
                shipCell.Hit = true;
                return true;
            }

            default: {
                throw new UnreachableException();
            }
        }
    }

    private void InitGrid() {
        var toPlace  = new List<Ship>(Config.Ships);
        var attempts = 0;

        while (toPlace.Count != 0) {
            Ship ship = toPlace[0];

            int startX = Random.Next((int) Config.Size.X),
                startY = Random.Next((int) Config.Size.Y);

            List<(int, int)> directions = AvailableDirections(startX, startY, ship.Size);
            if (directions.Count is 0) {
                attempts += 1;
                if (attempts > 1_000_000) {
                    throw new TimeoutException("Max number of placement attempts reached");
                }

                continue;
            }

            (int x, int y) direction = directions[Random.Next(directions.Count)];

            foreach (int distance in Enumerable.Range(0, (int) ship.Size)) {
                Grid[startX + direction.x * distance]
                    [startY + direction.y * distance] = new ShipCell(ship);
            }

            toPlace.RemoveAt(0);
        }
    }

    private List<(int, int)> AvailableDirections(int startX, int startY, uint size) {
        return Directions.Where(vector => Enumerable.Range(0, (int) size)
                                                    .Select(distance => (x: startX + vector.x * distance,
                                                                         y: startY + vector.y * distance))
                                                    .All(pos => pos.x >= 0
                                                             && pos.x < Grid.Length
                                                             && pos.y >= 0
                                                             && pos.y < Grid[0].Length
                                                             && Grid[pos.x][pos.y].IsEmpty()))
                         .ToList();
    }
    #endregion

    #region Constructor
    public PlayerField() {
        Config           = new GridConfig();
        (uint x, uint y) = Config.Size;

        // ReSharper disable once CoVariantArrayConversion
        Grid = Enumerable.Repeat(Enumerable.Repeat((Cell) new EmptyCell(), (int) y).ToArray(), (int) x).ToArray();

        for (var i = 0; i < 10; i++) {
            try {
                InitGrid();
            } catch (TimeoutException) {
                continue;
            }

            break;
        }
    }
    #endregion
}