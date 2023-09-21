using System.Net.Sockets;
using System.Net;
using Newtonsoft.Json;

namespace Server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TcpListener server = null;
            try
            {
                // Set the TcpListener on port 13000.
                Int32 port = 1338;
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");
                server = new TcpListener(localAddr, port);
                server.Start();

                while (true)
                {
                    Console.Write("Waiting for a connection... ");

                    // Perform a blocking call to accept requests.
                    using TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Connected!");

                    //stream objekt för att läsa och skriva data
                    NetworkStream stream = client.GetStream();

                    Byte[] bytes = new Byte[256];
                    String data = null!;
                    int i;

                    //loop för att få all data skickat från client
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                        Console.WriteLine("Received: {0}", data);

                        //konvertera från Json objekt till en sträng
                        Shoe receivedShoe = JsonConvert.DeserializeObject<Shoe>(data)!;

                        //skriver ut properties för mitt shoe-objekt
                        Console.WriteLine("Received Shoe Object: Make = {0}, Model = {1}, Year = {2}",
                                                    receivedShoe.Brand, receivedShoe.Color, receivedShoe.Size);

                        string responseMessage = "Received and processed the Shoe object.";
                        byte[] responseBytes = System.Text.Encoding.ASCII.GetBytes(responseMessage);

                        //skicka tillbaka en respons
                        stream.Write(responseBytes, 0, responseBytes.Length);
                        Console.WriteLine("Sent: {0}", responseMessage);
                    }
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                server!.Stop();
            }

            Console.WriteLine("\nHit enter to continue...");
            Console.Read();
        }
    }
    //Klass att ta emot från klienten
    public class Shoe
    {
        public string Brand { get; set; } = null!;
        public string Color { get; set; } = null!;
        public string Size { get; set; } = null!;
    }
}