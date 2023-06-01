namespace Battleship.API;

public class Ship {
    #region Properties
    public (int x, int y) Start     { get; }
    public int            Length    { get; }
    public Direction      Direction { get; }
    public bool[ ]        Broken    { get; set; }
    #endregion

    #region Constructors
    public Ship((int, int) start, int length, Direction direction) {
        Start     = start;
        Length    = length;
        Direction = direction;

        Broken = new bool[Length];
        Array.Fill(Broken, false);
    }
    #endregion
}