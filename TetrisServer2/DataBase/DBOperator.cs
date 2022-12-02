using MongoDB.Bson;
using MongoDB.Driver;
using TetrisServer2.Game;

namespace TetrisServer2.DataBase
{
    public class DBOperator
    {
        private MongoClient MongoClient { get; set; }
        private static string DBpath = "mongodb://localhost:27017";
        private static string DBname = "highscores";

        public DBOperator()
        {
            MongoClient = new MongoClient(DBpath);
        }

        public IMongoCollection<BsonDocument>? GetDbCollection()
        {
            IMongoCollection<BsonDocument> scoresCollection = null;
            try
            {
                var db = MongoClient.GetDatabase(DBname);
                scoresCollection = db.GetCollection<BsonDocument>("scores");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return scoresCollection;
        }


        public async Task<bool> SaveHighScore(string name, FieldSize fieldSize, int scoreNum, TimeSpan time)
        {
            var scorecollection = await Task.Run(GetDbCollection);
            Score score = new Score(name, fieldSize, scoreNum, time);

            if (scorecollection == null) 
                return false;

            await scorecollection.InsertOneAsync(score.ToBsonDocument());
            return true;
        }

        public async Task<List<BsonDocument>> GetHighScoresAsync(string fieldSize)
        {
            var scorecollection = await Task.Run(GetDbCollection);

            var filter = new BsonDocument { { "FieldSize", fieldSize } };

            var scores = await scorecollection.Find(filter).Sort("{ScoreNum:-1, Name:1}").ToListAsync();

            return scores;
        }
    }
}
