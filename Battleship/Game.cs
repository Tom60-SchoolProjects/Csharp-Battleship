namespace Battleship;

public class Game {
    #region Properties
    public int[,] Grid { get; } = CreateGrid();
    #endregion
    
    #region Methods
    private static int[,] CreateGrid() {
        var config = new GridConfig();
        
        (int x, int y) = config.Size;
        // TODO: Ships

        return new int[x,y];
    }
    #endregion
}