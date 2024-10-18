namespace TTT.TicTacToeGame
{
    public struct TicTacToePiecePosition
    {
        public int row;
        public int column;

        public TicTacToePiecePosition(int row, int column)
        {
            this.row = 0;
            this.column = 0;
            SetData(row, column);
        }

        public void SetData(int row, int column)
        {
            this.row = row;
            this.column = column;
        }
    }
}