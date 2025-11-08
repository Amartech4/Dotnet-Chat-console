namespace Chat_examination;


class Program
{
    static async Task Main(string[] args)
    {


        var chat = new Chat();

        // Vi ansluter till Socket servern.
        //var socketManager = new SocketManager();
        await chat.Connect();

        while (true)
        {
            string? input = Console.ReadLine();

            if (input?.ToLower() == "exit")
            {
                await chat.Disconnect();
                break;
            }

            //await SocketManager.SendMessage(input ?? "");
            await chat.SendChatMessage(input ?? "");


            //SocketManager.DisplayMessageHistory();


        }
        Console.WriteLine("Disconnected from server.");


    }
}