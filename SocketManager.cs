namespace Chat_examination;

using SocketIOClient;

public class SocketManager
{
    protected static SocketIO _client;
    private static readonly string Path = "/sys25d";
    protected static string EventName = "general";

    protected static List<string> messages = new List<string>();

    public static bool IsConnected;


    // Connects to the socket server
    public virtual async Task Connect()
    {

        _client = new SocketIO("wss://api.leetcode.se", new SocketIOOptions
        {
            Path = Path
        });

        _client.On(EventName, response =>
        {
            string receivedMessage = response.GetValue<string>();

            // GetMessage() recieves messages. 
            GetMessage(receivedMessage);
        });

        _client.OnConnected += (sender, args) =>
        {
            Console.WriteLine("Connected!");
        };

        _client.OnDisconnected += (sender, args) =>
        {
            Console.WriteLine("Disconnected!");
        };



        await _client.ConnectAsync();
        await Task.Delay(2000);

        IsConnected = _client.Connected;
    }

    //disconnects from the server socket.
    public virtual async Task Disconnect()
    {
        if (_client != null && _client.Connected)
        {
            await _client.DisconnectAsync();
            IsConnected = _client.Connected;
            Console.WriteLine("Disconnected from server!");
        }
        else
        {
            Console.WriteLine("Already disconnected!");
        }
    }

    protected static void GetMessage(string msg)
    {
        UpdateChat(msg);
    }

    protected virtual async Task SendMessage(string message)
    {
        if (_client.Connected)
        {
            await _client.EmitAsync(EventName, message);
            UpdateChat($"You: {message}");
        }
        else
        {
            Console.WriteLine("Not connected to server!");
        }
    }

    public static void DisplayMessageHistory()
    {
        Console.Clear();
        var messagesToShow = messages.Count > 20
            ? messages.Skip(messages.Count - 20)
            : messages;

        Console.WriteLine("\n--- Messages ---");
        foreach (var msg in messagesToShow)
        {
            Console.WriteLine(msg);
        }
        Console.WriteLine("-----------------------\n");

        Console.WriteLine("type to send message");
    }


    public static void UpdateChat(string message)
    {
        messages.Add(message);
        DisplayMessageHistory();
    }
}
