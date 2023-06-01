namespace Battleship.TUI;

public class MessageSystem{
    public readonly string[] messageBuffer = { "", "", "" };
    public string consoleInput { get; private set; } = "";
    private bool writingMessage = false;

    public void UpdateKey(ConsoleKeyInfo keyInfo)
    {
        switch (keyInfo.Key)
        {
            case ConsoleKey.Enter:
                writingMessage = false;
                break;

            case ConsoleKey.Backspace
            when consoleInput.Length > 0 && writingMessage:
                consoleInput = consoleInput.Remove(consoleInput.Length - 1);
                break;

            default:
                if (writingMessage)
                    consoleInput += keyInfo.KeyChar;
                break;
        }
    }

    public void WriteMessage(string message)
    {
        for (int i = 0; i < messageBuffer.Length - 1; i++)
            messageBuffer[i] = messageBuffer[i + 1];

        messageBuffer[messageBuffer.Length - 1] = message;
    }

    public async Task<string> ReadMessage() {
        
        writingMessage = true;

        while (writingMessage) await Task.Delay(100);

        var message = consoleInput;
        consoleInput = "";
        return message;
    }
}