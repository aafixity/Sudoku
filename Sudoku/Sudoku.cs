namespace Sudoku
{
    internal static class Sudoku
    {
        public const int ClearValue = 0;
        public const int MinValue = 1;
        public const int MaxValue = 9;

        public const int BoardSize = 9;
        public const int BoardSubSize = 3;

        public class BoardSizeException : Exception
        {
            private const string _message = "The board size is too big for this handler.";
            public BoardSizeException() : base(_message) { }
        }
        public class IndexSizeException : Exception
        {
            private const string _message = "The value of the index outside the board.";
            public IndexSizeException() : base(_message) { }
        }
        public class IndexSubSizeException : Exception
        {
            private const string _message = "An index outside the subarray.";
            public IndexSubSizeException() : base(_message) { }
        }

        public static void CheckSize(int[,] board)
        {
            if (board.GetLength(0) != board.GetLength(1) && board.GetLength(1) != BoardSize)
            {
                throw new BoardSizeException();
            }
        }
        public static void CheckIndices(params int[] indices)
        {
            foreach (int i in indices)
            {
                if (i < 0 || i > BoardSize)
                {
                    throw new IndexSizeException();
                }
            }
        }
        public static void CheckSubIndex(params int[] indices)
        {
            foreach (int i in indices)
            {
                if (i < 0 || i > BoardSubSize)
                {
                    throw new IndexSubSizeException();
                }
            }
        }
    }
}
