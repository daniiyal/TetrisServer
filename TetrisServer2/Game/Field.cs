namespace TetrisServer2.Game
{
    public class Field
    {
        private readonly int[,] field;

        public int Rows { get; }
        public int Columns { get; }

        public int this[int row, int col]
        {
            get => field[row, col];
            set => field[row, col] = value;
        }

        public Field(int rows, int cols)
        {
            Rows = rows;
            Columns = cols;

            field = new int[rows, cols];
        }

        public bool IsInsideField(int row, int column)
        {
            return row >= 0 && row < Rows && column >= 0 && column < Columns;
        }

        public bool IsEmpty(int row, int col)
        {
            return IsInsideField(row, col) && field[row, col] == 0;
        }

        public bool IsRowFull(int row)
        {
            for (int col = 0; col < Columns; col++)
                if (field[row, col] == 0)
                    return false;

            return true;
        }

        public bool IsRowEmpty(int row)
        {
            for (int col = 0; col < Columns; col++)
                if (field[row, col] != 0)
                    return false;

            return true;
        }

        public void ClearRow(int row)
        {
            for (int col = 0; col < Columns; col++)
                field[row, col] = 0;
        }

        public void DropRow(int row, int numOfRows)
        {
            for (int col = 0; col < Columns; col++)
            {
                field[row + numOfRows, col] = field[row, col];
                field[row, col] = 0;
            }
        }

        public int ClearFullRows()
        {
            int cleared = 0;

            for (int row = Rows - 1; row >= 0; row--)
            {
                if (IsRowFull(row))
                {
                    ClearRow(row);
                    cleared++;
                }

                if (cleared > 0)
                    DropRow(row, cleared);
            }

            return cleared;
        }

    }
}
