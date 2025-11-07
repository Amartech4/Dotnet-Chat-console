namespace Chat_exmination;


class Program
{
    static async Task Main(string[] args)
    {

        // convert int to string input and validate

        string username = GetUsername();
        Console.WriteLine($"Welcome, {username}! you will join chat room shortly");

        static string GetUsername()
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



    }
}