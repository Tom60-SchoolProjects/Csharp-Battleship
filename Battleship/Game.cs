namespace Battleship; 

public class Game {
    public           bool          TurnP1       { get; } 
    public  readonly PlayerField[] PlayersField;

    public Game() {
        TurnP1 = true;
        
        PlayersField = new PlayerField[2];
        PlayersField[0] = new PlayerField();
        PlayersField[1] = new PlayerField();
    }
}