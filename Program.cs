namespace Chat_examination;


class Program
{
    static async Task Main(string[] args)
    {
        // this keeps you from leaving program when you disconnect from chats
        bool LeftProgram = false;

        var chat = new Chat();
        var commands = new Commands();

        // Starting chat and asks first for username before proceeding
        await chat.StartChat();

        string currentInput = "";

        while (!LeftProgram)
        {

            // checks if user has typed something
            if (Console.KeyAvailable)
            {

                var key = Console.ReadKey(intercept: true);

                if (key.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();

                    if (!string.IsNullOrWhiteSpace(currentInput))
                    {

                        commands.Help(currentInput);

                        if (currentInput?.ToLower() == "/start")
                        {
                            await chat.Connect();
                        }
                        else if (currentInput?.ToLower() == "/leave")
                        {
                            // disconnects but doesnt leave the program
                            await chat.Disconnect();
                            Console.WriteLine($"if you want to reconnect type /start");
                        }
                        else if (currentInput?.ToLower() == "/exit")
                        {
                            // leaves program
                            LeftProgram = true;
                        }
                        else
                        {
                            // sends message
                            await chat.SendChatMessage(currentInput);
                        }

                        currentInput = "";

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
                    //typing indicator
                    currentInput += key.KeyChar;
                    Console.Write(key.KeyChar);
                    await chat.SendTyping();

                }

            }



            await Task.Delay(50);
        }


    }
}