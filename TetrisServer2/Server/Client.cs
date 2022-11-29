using System.Net.Sockets;
using System.Text;
using TetrisServer2.Game;

namespace TetrisServer2.Server
{
    public class Client
    {
        public string Name { get; set; }

        private GameManager gameManager;



        private Socket clientSocket;
        private Server server;

        public Client(Socket clientSocket, Server server)
        {

            this.clientSocket = clientSocket;
            this.server = server;
        }

        public async Task HandleResponseAsync()
        {
            try
            {
                while (true)
                {
                    var buffer = new List<byte>();
                    var bytesRead = new byte[1];

                    while (true)
                    {

                        var nextByte = await clientSocket.ReceiveAsync(bytesRead, SocketFlags.None);

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
                                    gameManager = new GameManager(server.FieldSizes[FieldSize.SMALL].Rows, server.FieldSizes[FieldSize.SMALL].Columns);
                                    break;
                                case "m":
                                    gameManager = new GameManager(server.FieldSizes[FieldSize.MEDIUM].Rows, server.FieldSizes[FieldSize.MEDIUM].Columns);
                                    break;
                                case "l":
                                    gameManager = new GameManager(server.FieldSizes[FieldSize.LARGE].Rows, server.FieldSizes[FieldSize.LARGE].Columns);
                                    break;
                            }
                            await SendResponseAsync($"GameStarted-{gameManager.Field.Rows}-{gameManager.Field.Columns}");
                            Console.WriteLine("Игра началась");
                            break;
                        case "NextFigure":
                            await SendResponseAsync(gameManager.CurrentBlock.BlockId.ToString());
                            break;
                        case "GetGrid":
                            StringBuilder stringBuilder = new StringBuilder();
                            if (gameManager.GameOver)
                            {
                                await SendResponseAsync("GameOver");
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
                            await SendResponseAsync(stringBuilder.ToString());
                            break;
                        case "GetBlock":
                            string positions = "";
                            foreach (var position in gameManager.CurrentBlock.BlockTilesPositions())
                            {
                                positions += position.Row + "-" + position.Column + "-" + position.BlockPosId + "n";
                            }
                            //positions += gameManager.CurrentBlock.BlockId;
                            await SendResponseAsync(positions);
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

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task SendResponseAsync(string message)
        {
            await clientSocket.SendAsync(Encoding.UTF8.GetBytes(message + '\n'), SocketFlags.None);
        }

    }
}
