namespace Chat_examination;


class Program
{
    static async Task Main(string[] args)
    {
        bool LeftProgram = false;

        DateTime lastKeyPress = DateTime.MinValue;
        bool isTyping = false;

        var chat = new Chat();

        // Vi ansluter till Socket servern.
        //var socketManager = new SocketManager();
        await chat.StartChat();

        string currentInput = "";

        while (!LeftProgram)
        {


            if (Console.KeyAvailable)
            {

                var key = Console.ReadKey(intercept: true);

                if (key.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();

                    //string? input = Console.ReadLine();


                    //await SocketManager.SendMessage(input ?? "");

                    // await chat.SendChatMessage(currentInput ?? "");

                    if (!string.IsNullOrWhiteSpace(currentInput))
                        await chat.SendChatMessage(currentInput);




                    if (currentInput?.ToLower() == "start")
                    {
                        await chat.Connect();
                    }

                    if (currentInput?.ToLower() == "leave")
                    {
                        await chat.Disconnect();
                        Console.WriteLine($"if you want to reconnect type start");
                    }

                    if (currentInput?.ToLower() == "exit")
                    {
                        LeftProgram = true;
                    }
                }
                else if (key.Key == ConsoleKey.Backspace)
                {
                    if (currentInput.Length > 0)
                    {
                        currentInput = currentInput[..^1];
                        Console.Write("\b \b");
                    }
                }
                else
                {

                    currentInput += key.KeyChar;
                    Console.Write(key.KeyChar);
                    await chat.SendTyping();

                }

            }



            await Task.Delay(50);
        }


    }
}