using MongoDB.Bson.Serialization;
using System.Net.Sockets;
using System.Text;
using TetrisServer2.DataBase;
using TetrisServer2.Game;

namespace TetrisServer2.Server
{
    public class Client
    {
        public string Name { get; set; }

        private GameManager gameManager;



        private Socket clientSocket;
        private Server server;
        private DBOperator DbOperator;


        public Client(Socket clientSocket, Server server)
        {

            this.clientSocket = clientSocket;
            this.server = server;
            DbOperator = new DBOperator();
        }

        public async Task HandleResponseAsync()
        {
            try
            {
                while (true)
                {

                    var response = await ReceiveMessageAsync();

                    switch (response.Split('-')[0])
                    {
                        case "StartGame":
                            var fieldSize = response.Split('-')[1];
                            switch (fieldSize)
                            {
                                case "small":
                                    gameManager = new GameManager(FieldSizes.Small);
                                    break;
                                case "medium":
                                    gameManager = new GameManager(FieldSizes.Medium);
                                    break;
                                case "large":
                                    gameManager = new GameManager(FieldSizes.Large);
                                    break;
                            }
                            await SendResponseAsync($"GameStarted-{gameManager.Field.Rows}-{gameManager.Field.Columns}");
                            Console.WriteLine("Игра началась");
                            break;
                        case "NextFigure":
                            await SendResponseAsync(gameManager.CurrentBlock.BlockId.ToString());
                            break;
                        case "GetRecords":

                            fieldSize = response.Split('-')[1];
                            var records = await GetRecords(fieldSize);
                            await SendResponseAsync(records);

                            break;
                        case "GetGrid":
                            StringBuilder stringBuilder = new StringBuilder();
                            if (gameManager.GameOver)
                            {
                                 if (await IsRecord())
                                {
                                    await SendResponseAsync("GameOver-Record");
                                    break;
                                }

                                await SendResponseAsync($"GameOver-NoRecord");
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
                        case "WriteRecord":
                            var name = response.Split('-')[1];

                            if (!await WriteRecord(name))
                            {
                                await SendResponseAsync("failed");
                                break;
                            }

                            await SendResponseAsync("success");
                            break;
                        case "GetBlock":
                            string positions = "";

                            foreach (var position in gameManager.CurrentBlock.BlockTilesPositions())
                            {
                                positions += position.Row + "-" + position.Column + "-" + position.BlockPosId + "n";
                            }

                            await SendResponseAsync(positions);
                            break;
                        case "GetNextBlock":
                            await SendResponseAsync(gameManager.BlockPicker.NextBlock.BlockId.ToString());
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

        private async Task<bool> WriteRecord(string name)
        {
            var result = await DbOperator.SaveHighScore(name, gameManager.FieldSize, gameManager.Score, gameManager.Time);

            return result;
        }


        private async Task<string> GetRecords(string fieldSize)
        {
            var highScores = await DbOperator.GetHighScoresAsync(fieldSize);

            string records = null;

            foreach (var highScore in highScores.Take(10).ToList())
            {
                var score = BsonSerializer.Deserialize<Score>(highScore);
                records += score.ToString() + "\r";
            }

            return records;
        }

        private async Task<bool> IsRecord()
        {
             var highScores = await DbOperator.GetHighScoresAsync(gameManager.FieldSize.Name);

            if (highScores.Count < 10)
                return true;

            foreach (var highScore in highScores.Take(10).ToList())
            {
                var score = BsonSerializer.Deserialize<Score>(highScore);
                if (gameManager.Score > score.ScoreNum)
                {
                    return true;
                }
            }

            return false;
        }

        public async Task SendResponseAsync(string message)
        {
            await clientSocket.SendAsync(Encoding.UTF8.GetBytes(message + '\n'), SocketFlags.None);
        }

        public async Task<string> ReceiveMessageAsync()
        {
            var buffer = new List<byte>();
            var bytesRead = new byte[1];

            while (true)
            {
                var nextByte = await clientSocket.ReceiveAsync(bytesRead, SocketFlags.None);

                if (nextByte == 0 || bytesRead[0] == '\n') break;

                buffer.Add(bytesRead[0]);
            }


            return Encoding.UTF8.GetString(buffer.ToArray());
        }

    }
}
