using System.Net;
using System.Net.Sockets;
using System.Text;
using TetrisServer2.DataBase;
using TetrisServer2.Game;

namespace TetrisServer2.Server
{
    public class Server
    {
        private IPEndPoint IpEndPoint { get; }

        private Socket Listener { get; }

        private UdpClient UdpClient { get; }



        private int Port { get; }
        private bool _active = false;

        private Socket clientSocket;

        public Server(string ip, int port)
        {
            Port = port;
            IpEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            Listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            UdpClient = new UdpClient();

        }

        public void StartServer()
        {
            if (!_active)
            {
                Listener.Bind(IpEndPoint);
                Listener.Listen(Port);

                UdpClient.Client.Bind(IpEndPoint);

                _active = true;
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
                clientSocket = await Listener.AcceptAsync();
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
        }


        private void ReceiveRequest()
        {

            var clientEp = new IPEndPoint(IPAddress.Any, 0);
            var responseData = Encoding.ASCII.GetBytes("Come and play");

            while (true)
            {
                var receiveBUffer = UdpClient.Receive(ref clientEp);

                if (Encoding.UTF8.GetString(receiveBUffer) == "Want to play")
                {
                    Console.WriteLine($"received request from {clientEp.Address}");
                    UdpClient.Send(responseData, responseData.Length, clientEp.Address.ToString(), clientEp.Port);
                };
            }
        }


        public void Stop()
        {
            Listener.Shutdown(SocketShutdown.Both);
        }
    }
}