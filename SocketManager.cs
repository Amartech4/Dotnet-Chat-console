namespace Chat_examination;

using SocketIOClient;

public class SocketManager
{
    protected static SocketIO _client;
    private static readonly string Path = "/sys25d";
    protected static string EventName = "general";

    protected static List<string> messages = new List<string>();


    public static bool IsConnected;


    /*static SocketManager()
    {
        messages = [];
    }*/

    // Ansluter till socketio server
    public virtual async Task Connect()
    {

        _client = new SocketIO("wss://api.leetcode.se", new SocketIOOptions
        {
            Path = Path
        });

        _client.On(EventName, response =>
        {
            string receivedMessage = response.GetValue<string>();
            Console.WriteLine($"\n Received: {receivedMessage}");

            // GetMessage hämtar meddelanden. DisplayMessageHistory
            // visar historik av skickade meddelanden
            GetMessage(receivedMessage);
        });

        _client.On("typing", response =>
        {
            var obj = response.GetValue<string>();

        });

        // Kod vi kan köra när vi etablerar en anslutning
        _client.OnConnected += (sender, args) =>
        {
            Console.WriteLine("Connected!");
        };

        // Kod vi kan köra när vi tappar anslutningen
        _client.OnDisconnected += (sender, args) =>
        {
            Console.WriteLine("Disconnected!");
        };



        await _client.ConnectAsync();
        await Task.Delay(2000);

        IsConnected = _client.Connected;
        Console.WriteLine($"Connected: {_client.Connected}");
    }

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


    /*public static async Task GetMessage(string message)
    {
        await _client.EmitAsync("receive_message", message);
        Console.WriteLine($"they said: {message}");
        messages.Add($"from them {message}");
    }*/

    protected static void GetMessage(string msg)
    {
        UpdateChat(msg);
        //messages.Add(msg);
    }

    protected virtual async Task SendMessage(string message)
    {
        if (_client.Connected)
        {
            await _client.EmitAsync(EventName, message);
            Console.WriteLine($"You sent: {message}");
            UpdateChat($"You: {message}");
        }
        else
        {
            Console.WriteLine("Not connected to server!");
        }
    }

    public static void DisplayMessageHistory()
    {
        // Console.Clear();
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
