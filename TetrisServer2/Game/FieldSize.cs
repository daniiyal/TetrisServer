using System.Globalization;

namespace TetrisServer2.Game
{
    public class FieldSize
    {
        public int Rows { get; set; }
        public int Cols { get; set; }
        public string Name { get; set; }

        public FieldSize(int rows, int cols, string name)
        {
            Rows = rows;
            Cols = cols;
            Name = name;
        }
    }
}
