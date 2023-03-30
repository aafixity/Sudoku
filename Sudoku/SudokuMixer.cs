namespace Sudoku
{
    internal class SudokuMixer
    {
        private readonly int[,] _board;
        private readonly Random _random = new();

        public SudokuMixer(int[,] board)
        {
            Sudoku.CheckSize(board);
            _board = board;
        }

        public void Flip()
        {
            int[,] temp = new int[Sudoku.BoardSize, Sudoku.BoardSize];
            for (int y = 0; y < Sudoku.BoardSize; y++)
                for (int x = 0; x < Sudoku.BoardSize; x++)
                    temp[y, x] = _board[x, y];

            Array.Copy(temp, _board, _board.Length);
        }

        public void SwapRowsInSub(int firstRowY, int secondRowY)
        {
            Sudoku.CheckIndices(firstRowY, secondRowY);

            if (firstRowY / Sudoku.BoardSubSize != secondRowY / Sudoku.BoardSubSize)
            {
                throw new Sudoku.IndexSubSizeException();
            }

            var temp = new int[Sudoku.BoardSize, Sudoku.BoardSize];
            Array.Copy(_board, temp, _board.Length);
            for (int x = 0; x < Sudoku.BoardSize; x++)
            {
                temp[firstRowY, x] = _board[secondRowY, x];
                temp[secondRowY, x] = _board[firstRowY, x];
            }
            Array.Copy(temp, _board, _board.Length);
        }
        public void SwapRowsInSub()
        {
            int subY = _random.Next(Sudoku.BoardSubSize);

            int start = subY * Sudoku.BoardSubSize;
            int end = start + Sudoku.BoardSubSize - 1;

            int firstRowY = _random.Next(start, end + 1);
            int secndRowY;
            do
            {
                secndRowY = _random.Next(start, end + 1);
            } while (firstRowY == secndRowY);

            SwapRowsInSub(firstRowY, secndRowY);
        }

        public void SwapColumnsInSub(int firstColX, int secondColX)
        {
            Flip();
            SwapRowsInSub(firstColX, secondColX);
            Flip();
        }
        public void SwapColumnsInSub()
        {
            Flip();
            SwapRowsInSub();
            Flip();
        }

        public void SwapRowOfSubsInColumn(int firstSubY, int secondSubY)
        {
            Sudoku.CheckSubIndex(firstSubY, secondSubY);

            int firstStartY = firstSubY * Sudoku.BoardSubSize;
            int secondStartY = secondSubY * Sudoku.BoardSubSize;

            int[] firstSubYIndices = Enumerable.Range(firstStartY, Sudoku.BoardSubSize).ToArray();
            int[] secondSubYIndices = Enumerable.Range(secondStartY, Sudoku.BoardSubSize).ToArray();

            for (int i = 0; i < Sudoku.BoardSubSize; i++)
            {
                SwapRows(firstSubYIndices[i], secondSubYIndices[i]);
            }

            void SwapRows(int firstY, int secondY)
            {
                for (int x = 0; x < Sudoku.BoardSize; x++)
                {
                    (_board[firstY, x], _board[secondY, x]) = (_board[secondY, x], _board[firstY, x]);
                }
            }
        }
        public void SwapRowOfSubsInColumn()
        {
            int firstSubY = _random.Next(Sudoku.BoardSubSize);
            int secondSubY;
            do
            {
                secondSubY = _random.Next(Sudoku.BoardSubSize);
            } while (firstSubY == secondSubY);
            SwapRowOfSubsInColumn(firstSubY, secondSubY);
        }

        public void SwapColumnOfSubsInRow(int firstSubX, int secondSubX)
        {
            Flip();
            SwapRowOfSubsInColumn(firstSubX, secondSubX);
            Flip();
        }
        public void SwapColumnOfSubsInRow()
        {
            Flip();
            SwapRowOfSubsInColumn();
            Flip();
        }

        private delegate void Swap();
        public void RandomSwap()
        {
            Swap[] swapMethods = new Swap[]
            {
                new Swap(Flip),
                new Swap(SwapRowsInSub),
                new Swap(SwapColumnsInSub),
                new Swap(SwapRowOfSubsInColumn),
                new Swap(SwapColumnOfSubsInRow),
            };

            int randomIndex = _random.Next(swapMethods.Length);
            swapMethods[randomIndex]();
            Console.WriteLine($"{swapMethods[randomIndex].Method}");
        }
    }
}
