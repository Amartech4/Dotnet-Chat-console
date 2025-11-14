namespace Chat_examination;
public class Chat : SocketManager
{
    private string _username;

    // for enhanced history for future use
    public List<ChatModel> _chatHistory = new List<ChatModel>();


    private bool isTyping = false;


    //asks user for name before connecting
    public Chat()
    {
        _username = GetUsername();
        Console.Clear();
        Console.WriteLine($"Welcome, {_username}! You will join chat room shortly");
    }

    //user can include numbers in their name
    private string GetUsername()
    {
        while (true)
        {
            Console.WriteLine("Enter Username:");
            var input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Username cannot be empty.");
            }
            else if (int.TryParse(input, out int num))
            {
                return num.ToString();
            }
            else
            {
                return input;
            }
        }
    }

    public async Task StartChat()
    {
        await base.Connect();
        ListenForTyping();
        await SendMessage($"{_username} joined the chat");
    }

    //updated connect
    public override async Task Connect()
    {
        // Send a message that user joined
        if (!_client.Connected)
        {
            await base.Connect();
            await SendMessage($"{_username} joined the chat");
        }
        else
        {
            Console.WriteLine("Already connected!");
        }

    }

    //updated disconnect
    public new async Task Disconnect()
    {
        if (_client != null && _client.Connected)
        {
            await SendMessage($"{_username} left the chat");
            await Task.Delay(500);
            await _client.DisconnectAsync();
            Console.WriteLine($"{_username} disconnected from server!");
        }
        else
        {
            Console.WriteLine("Already disconnected!");
        }
    }


    // updated sendMessage() 
    protected override async Task SendMessage(string message)
    {
        if (_client.Connected)
        {
            await _client.EmitAsync(EventName, message);
            UpdateChat(message);
            await Task.Delay(500);
            isTyping = false;
        }
        else
        {
            Console.WriteLine("Not connected to server!");
        }
    }

    // formats message based on Chat model
    public async Task SendChatMessage(string message, string receiver = "", MessageType type = MessageType.Global)
    {
        var chatModel = new ChatModel
        {
            Username = _username,
            Message = message,
            Reciever = receiver,
            Type = type,
            Timestamp = DateTime.Now
        };

        string formattedMessage = FormatChatMessage(chatModel);
        await SendMessage(formattedMessage);
    }


    private string FormatChatMessage(ChatModel chatModel)
    {
        return $"[{chatModel.Timestamp:HH:mm}] {chatModel.Username}: {chatModel.Message}";
    }


    private void PrintAbove(string text)
    {
        // Save input cursor position
        int inputLeft = Console.CursorLeft;
        int inputTop = Console.CursorTop;

        // Move cursor up one line (or more if needed)
        Console.SetCursorPosition(0, inputTop - 1);

        // Clear that entire line
        Console.Write(new string(' ', Console.WindowWidth));
        Console.SetCursorPosition(0, inputTop - 1);

        // Print the typing notification
        Console.WriteLine(text);

        // Restore the input cursor
        Console.SetCursorPosition(inputLeft, inputTop);
    }

    // displays to others if you are typing or displays others is typing to you
    private void ListenForTyping()
    {
        _client.On("typing", response =>
        {
            string msg = response.GetValue<string>();

            //moves one line down in console
            PrintAbove(msg);

        });

    }

    // indicates if other users is typing. displays it only once
    public async Task SendTyping()
    {
        if (_client.Connected && !isTyping)
        {
            await _client.EmitAsync("typing", $"{_username} is typing...");
            isTyping = true;

            // resets after one second so each user can send indicator once
            _ = Task.Run(async () =>
            {
                await Task.Delay(1000);
                isTyping = false;
            });
        }

    }

}