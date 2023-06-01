namespace Battleship; 

public class Game {
    public           bool          TurnP1       { get; } 
    public  readonly PlayerField[] Fields;

    public Game() {
        TurnP1 = true;
        
        Fields = new PlayerField[2];
        Fields[0] = new PlayerField();
        Fields[1] = new PlayerField();
    }
}