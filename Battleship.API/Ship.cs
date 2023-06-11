namespace Battleship.API;

/// <summary>
/// Define a ship as it appears on a <c>PlayerField</c>
/// </summary>
public class Ship {
    #region Properties
    public (uint x, uint y) Start     { get; set; } // Same here, not the best way to do it
    public uint             Length    { get; }
    public Direction        Direction { get; set; } // ...
    public bool[ ]          Broken    { get; set; }
    public bool Visible = false;
    #endregion

    #region Constructors
    public Ship((uint, uint) start, uint length, Direction direction) {
        Start     = start;
        Length    = length;
        Direction = direction;

        Broken = new bool[Length];
        Array.Fill(Broken, false);
    }
    #endregion
}