namespace Battleship.API;

public class Ship {
    #region Properties
    public (uint x, uint y) Start     { get; }
    public uint             Length    { get; }
    public Direction        Direction { get; }
    public bool[ ]          Broken    { get; set; }
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