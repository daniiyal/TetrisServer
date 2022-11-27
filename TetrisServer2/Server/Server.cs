using System.Net;
using System.Net.Sockets;
using System.Text;
using TetrisServer2.Game;

namespace TetrisServer2.Server
{
    public class Server
    {
        private IPEndPoint ipEndPoint { get; }

        private IPEndPoint broadCastEndPoint { get; }

        private Socket listener { get; }

        private UdpClient udpClient { get; }

        private UdpClient UdpClient {get; set; }

        Dictionary<FieldSize, Field> FieldSizes { get; set; }

        private int port { get; }
        private bool active = false;

        private Socket clientSocket;
        private GameManager gameManager;

        public Server(string ip, int port)
        {
            this.port = port;
            ipEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            udpClient = new UdpClient();
            broadCastEndPoint = new IPEndPoint(IPAddress.Parse(ip), 333);

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

        public void connectNewClient()
        {
            clientSocket = listener.Accept();
            Console.WriteLine($"Подключился игрок - {clientSocket.RemoteEndPoint}");
        }


        public void HandleResponse()
        {
            var buffer = new List<byte>();
            var bytesRead = new byte[1];

            while (true)
            {
                var nextByte = clientSocket.Receive(bytesRead);

                if (nextByte == 0 || bytesRead[0] == '\n') break;

                buffer.Add(bytesRead[0]);
            }

            var response = Encoding.UTF8.GetString(buffer.ToArray());

            switch (response.Split(' ')[0])
            {
                case "StartGame":
                    var fieldSize = response.Split(' ')[1];
                    switch (fieldSize)
                    {
                        case "s":
                            gameManager = new GameManager(FieldSizes[FieldSize.SMALL].Rows, FieldSizes[FieldSize.SMALL].Columns);
                            break;
                        case "m":
                            gameManager = new GameManager(FieldSizes[FieldSize.MEDIUM].Rows, FieldSizes[FieldSize.MEDIUM].Columns);
                            break;
                        case "l":
                            gameManager = new GameManager(FieldSizes[FieldSize.LARGE].Rows, FieldSizes[FieldSize.LARGE].Columns);
                            break;
                    }
                    SendResponse($"GameStarted-{gameManager.Field.Rows}-{gameManager.Field.Columns}");
                    Console.WriteLine("Игра началась");
                    break;
                case "NextFigure":
                    SendResponse(gameManager.CurrentBlock.BlockId.ToString());
                    break;
                case "GetGrid":
                    StringBuilder stringBuilder = new StringBuilder();
                    if (gameManager.GameOver)
                    {
                        SendResponse("GameOver");
                        break;
                    }

                    for (int i = 0; i < gameManager.Field.Rows; i++)
                    {
                        for (int j = 0; j < gameManager.Field.Columns; j++)
                        {
                            stringBuilder.Append(gameManager.Field[i, j]);
                            stringBuilder.Append('-');
                        }
                        stringBuilder.Append('n');
                    }

                    stringBuilder.Append(gameManager.Score);
                    SendResponse(stringBuilder.ToString());
                    break;
                case "GetBlock":
                    string positions = "";
                    foreach (var position in gameManager.CurrentBlock.BlockTilesPositions())
                    {
                        positions += position.Row + "-" + position.Column + "-" + position.BlockPosId + "n";
                    }
                    //positions += gameManager.CurrentBlock.BlockId;
                    SendResponse(positions);
                    break;
                case "Left":
                    gameManager.MoveBlockLeft();
                    break;
                case "Right":
                    gameManager.MoveBlockRight();
                    break;
                case "Rotate":
                    gameManager.RotateBlock();
                    break;
                case "Drop":
                    gameManager.DropBlock();
                    break;
            }


        }

        public async void SendResponse(string message)
        {
            await clientSocket.SendAsync(Encoding.UTF8.GetBytes(message + '\n'), SocketFlags.None);
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



    }
}