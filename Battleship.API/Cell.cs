using Battleship.API;

namespace Battleship;

public interface Cell {
    public bool IsEmpty();
}

public class EmptyCell : Cell {
    public EmptyCell() { }

    public bool IsEmpty()
        => true;
}

public class ShipCell : Cell {
    public Ship Parent { get; }
    public bool Hit    { get; set; }

    public ShipCell(Ship parent) {
        Parent = parent;
        Hit    = false;
    }

    public bool IsEmpty()
        => false;
}