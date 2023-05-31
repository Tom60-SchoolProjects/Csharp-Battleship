using System.Diagnostics;
using Battleship.API;

namespace Battleship;

public class Game {
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

        ;
    }

    private void InitGrid() {
        foreach (Ship ship in Config.Ships) {
            int startX = Random.Next((int) Config.Size.X),
                startY = Random.Next((int) Config.Size.Y);

            List<(int, int)> directions = AvailableDirections(startX, startY, ship.Size);
            (int x, int y)   direction  = directions[Random.Next(directions.Count)];

            foreach (int distance in Enumerable.Range(0, (int) ship.Size)) {
                Grid[startX + direction.x * distance]
                    [startY + direction.x * distance] = new ShipCell(ship);
            }
        }
    }

    private List<(int, int)> AvailableDirections(int startX, int startY, uint size) {
        return Directions.Where(vector => Enumerable.Range(0, (int) size)
                                                    .Select(distance => (x: startX + vector.x * distance,
                                                                         y: startY + vector.y * distance))
                                                    .Where(pos => 0 < pos.x && pos.x < Grid.GetLength(0))
                                                    .Where(pos => 0 < pos.y && pos.y < Grid.GetLength(1))
                                                    .All(pos => Grid[pos.x][pos.y].IsEmpty()))
                         .ToList();
    }
    #endregion

    #region Constructor
    public Game() {
        Config           = new GridConfig();
        (uint x, uint y) = Config.Size;

        // ReSharper disable once CoVariantArrayConversion
        Grid = Enumerable.Repeat(Enumerable.Repeat(new EmptyCell(), (int) y).ToArray(), (int) x).ToArray();

        InitGrid();
    }
    #endregion
}