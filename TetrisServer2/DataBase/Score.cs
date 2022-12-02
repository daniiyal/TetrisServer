using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using TetrisServer2.Game;

namespace TetrisServer2.DataBase
{
    public class Score
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public string FieldSize { get; set; }
        public int ScoreNum { get; set; }
        public TimeSpan Time { get; set; }

        public Score(string name, FieldSize fieldSize, int scoreNum, TimeSpan time)
        {
            Name = name;
            FieldSize = fieldSize.Name;
            ScoreNum = scoreNum;
            Time = time;
        }

        public override string ToString()
        {
            return $"{Name}-{FieldSize}-{ScoreNum}-{Time.Minutes}:{Time.Seconds}";
        }
    }
}
