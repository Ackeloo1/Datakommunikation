using Newtonsoft.Json;
using System.Net;
using System.Net.Sockets;
using System.Text;

class Client1
{
    static async Task Main(string[] args)
    {
        int localPort = 1212;  // Lokal port
        int remotePort = 1313; // Port till Client2
        string remoteIPAddress = "127.0.0.1"; //Localhost ipadress

        UdpClient udpClient = new UdpClient(localPort);
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(remoteIPAddress), remotePort);

        // Tar emot och visar meddelanden från Client2
        Task receiveTask = Task.Run(async () =>
        {
            while (true)
            {
                UdpReceiveResult receiveResult = await udpClient.ReceiveAsync();
                byte[] data = receiveResult.Buffer;
                string message = Encoding.ASCII.GetString(data);
                Console.WriteLine("Them: " + message);
            }
        });

        try
        {
            while (true)
            {
                string message = Console.ReadLine()!;

                Car car = new Car
                {
                    Brand = message
                };

                string jsonString = JsonConvert.SerializeObject(car);

                if (!string.IsNullOrEmpty(jsonString))
                {
                    byte[] data = Encoding.ASCII.GetBytes(jsonString);
                    await udpClient.SendAsync(data, data.Length, endPoint);
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Error: " + e.Message);
        }
    }
    class Car
    {
        public string Brand { get; set; } = null!;
    }
}
