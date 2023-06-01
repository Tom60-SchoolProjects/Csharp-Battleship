using System.Diagnostics;

namespace Battleship.API;

public enum Direction {
    East,
    South,
    West,
    North,
}

public static class DirectionExt {
    public static readonly Direction[ ] All
        = { Direction.East, Direction.South, Direction.West, Direction.North };

    public static (int x, int y) Tuple(this Direction direction)
        => direction switch {
               Direction.East  => (1, 0),
               Direction.South => (0, 1),
               Direction.West  => (-1, 0),
               Direction.North => (0, -1),
               _               => throw new UnreachableException(),
           };
}