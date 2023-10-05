using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Net.Sockets;
using System.Text.Json.Serialization;

namespace Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Shoe shoe = new Shoe
                {
                    Brand = "Jordan's",
                    Color = "Red",
                    Size = "38"
                };

                Int32 port = 1338;

                using TcpClient client = new TcpClient("127.0.0.1", port);

                //Serialize Shoe
                string jsonShoe = JsonConvert.SerializeObject(shoe);

                //Convert JSON string to bytes
                Byte[] data = System.Text.Encoding.ASCII.GetBytes(jsonShoe);

                // Send the message to the connected TcpServer.
                NetworkStream stream = client.GetStream();
                stream.Write(data, 0, data.Length);

                Console.WriteLine("Sent: {0}", jsonShoe);

                //förvara datan i bytes
                data = new Byte[256];

                // String to store the response ASCII representation.
                String responseData = String.Empty;

                // Read the first batch of the TcpServer response bytes.
                Int32 bytes = stream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                Console.WriteLine("Received: {0}", responseData);

                // Explicit close is not necessary since TcpClient.Dispose() will be
                // called automatically.
                // stream.Close();
                // client.Close();
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }

            Console.WriteLine("\n Press Enter to continue...");
            Console.Read();
        }

    }
    public class Shoe
    {
        public string Brand { get; set; } = null!;
        public string Color { get; set; } = null!;
        public string Size { get; set; } = null!;
    }
}
