namespace Chat_examination;
public class Chat : SocketManager
{
    private string _username;
    public List<ChatModel> _chatHistory = new List<ChatModel>();


    private bool isTyping = false;


    public Chat()
    {
        _username = GetUsername();
        Console.Clear();
        Console.WriteLine($"Welcome, {_username}! You will join chat room shortly");
    }
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
    public override async Task Connect()
    {


        // Send a message that user joined
        if (!_client.Connected)
        {
            await base.Connect();
            ListenForTyping();
            await SendMessage($"{_username} joined the chat");
        }
        else
        {
            Console.WriteLine("Already connected!");
        }

    }

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


    // Uppdaterade sendMessage() så den skriver bara joined och
    // inte "you sent: user joined" 
    protected override async Task SendMessage(string message)
    {
        if (_client.Connected)
        {
            await _client.EmitAsync(EventName, message);
            //Console.WriteLine($"{message}");
            //messages.Add($"{message}");
            UpdateChat(message);
        }
        else
        {
            Console.WriteLine("Not connected to server!");
        }
    }

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

        _chatHistory.Add(chatModel);

        string formattedMessage = FormatChatMessage(chatModel);
        await SendMessage(formattedMessage);
    }


    private string FormatChatMessage(ChatModel chatModel)
    {
        return $"[{chatModel.Timestamp:HH:mm}] {chatModel.Username}: {chatModel.Message}";
    }

    public void DisplayEnhancedHistory()
    {
        Console.WriteLine("\n--- Enhanced Message History ---");
        foreach (var chat in _chatHistory)
        {
            string formattedMessage = FormatChatMessage(chat);
            Console.WriteLine(formattedMessage);

            if (chat.Type == MessageType.Private)
            {
                Console.WriteLine($"  (Private to {chat.Reciever})");
            }
        }
        Console.WriteLine("---------------------------------\n");
    }



    private void ListenForTyping()
    {
        _client.On("typing", response =>
        {
            string msg = response.GetValue<string>();
            Console.WriteLine($"{msg}");
        });

    }
    public async Task SendTyping()
    {
        if (_client.Connected && !isTyping)
        {
            await _client.EmitAsync("typing", $"{_username} is typing...");
            isTyping = true;

        }

    }

}