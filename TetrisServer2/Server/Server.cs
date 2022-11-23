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
        private int port { get; }
        private bool active = false;

        private Socket clientSocket;
        private GameManager gameManager;
        public Server(string ip, int port)
        {
            this.port = port;
            this.ipEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            this.listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void startServer()
        {
            if (!active)
            {
                listener.Bind(ipEndPoint);
                listener.Listen(port);
                active = true;
                Console.WriteLine("Сервер запущен. Ожидание подключений...");
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
                    Console.WriteLine("Игра началась");
                    var rows = Convert.ToInt32(response.Split(' ')[1]);
                    var columns = Convert.ToInt32(response.Split(' ')[2]);
                    gameManager = new GameManager(rows, columns);
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
    }
}