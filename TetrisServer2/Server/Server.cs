using System.Net;
using System.Net.Sockets;
using System.Text;
using TetrisServer2.Game;

namespace TetrisServer2.Server
{
    public class Server
    {
        private IPEndPoint ipEndPoint { get; }

        private Socket listener { get; }

        private UdpClient udpClient { get; }



        private int port { get; }
        private bool active = false;

        private Socket clientSocket;

        public Dictionary<FieldSize, Field> FieldSizes { get; set; }

        public Server(string ip, int port)
        {
            this.port = port;
            ipEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            udpClient = new UdpClient();

            FieldSizes = new Dictionary<FieldSize, Field>()
            {
                {FieldSize.SMALL, new Field(20, 10)},
                {FieldSize.MEDIUM, new Field(20, 15)},
                {FieldSize.LARGE, new Field(20, 20)}
            };

        }

        public void startServer()
        {
            if (!active)
            {
                listener.Bind(ipEndPoint);
                listener.Listen(port);

                udpClient.Client.Bind(ipEndPoint);

                active = true;
                Console.WriteLine("Сервер запущен. Ожидание подключений...");

                Task.Run(ReceiveRequest);
            }
            else
            {
                Console.WriteLine("Сервер уже запущен. Ожидание подключений...");
            }

        }

        public async Task ConnectNewClient()
        {
            while (true)
            {
                clientSocket = await listener.AcceptAsync();
                Client client = new Client(clientSocket, this);
                Console.WriteLine($"Подключился игрок - {clientSocket.RemoteEndPoint}");

                try
                {
                    Task.Run(client.HandleResponseAsync);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    clientSocket.Shutdown(SocketShutdown.Both);
                    break;
                }
            }
            //clientSocket = listener.Accept();
            //Console.WriteLine($"Подключился игрок - {clientSocket.RemoteEndPoint}");
        }


        

        private async Task ReceiveRequest()
        {

            var clientEp = new IPEndPoint(IPAddress.Any, 0);
            var responseData = Encoding.ASCII.GetBytes("Come and play");

            while (true)
            {
                var receiveBUffer = udpClient.Receive(ref clientEp);

                if (Encoding.UTF8.GetString(receiveBUffer) == "Want to play")
                {
                    Console.WriteLine($"received request from {clientEp.Address}");
                    udpClient.Send(responseData, responseData.Length, clientEp.Address.ToString(), clientEp.Port);
                };
            }
        }


        public void Stop()
        {
            listener.Shutdown(SocketShutdown.Both);
        }
    }
}