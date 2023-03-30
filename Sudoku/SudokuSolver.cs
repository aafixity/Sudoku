namespace Sudoku
{
    internal class SudokuSolver
    {
        private readonly int[,] _board;
        public SudokuSolver(int[,] board)
        {
            _board = board;
        }

        public int GetSolutionCount()
        {
            return GetSolutionCountHeandler(0, 0);
        }
        private int GetSolutionCountHeandler(int y, int x)
        {
            if (y == 9)
                return 1;

            if (_board[y, x] != 0)
            {
                if (x == 8)
                {
                    return GetSolutionCountHeandler(y + 1, 0);
                }
                else
                {
                    return GetSolutionCountHeandler(y, x + 1);
                }
            }
            else
            {
                int count = 0;
                for (int num = Sudoku.MinValue; num <= Sudoku.MaxValue; num++)
                {
                    if (CanPlaced(y, x, num))
                    {
                        _board[y, x] = num;

                        count += x == 8 ? GetSolutionCountHeandler(y + 1, 0) : GetSolutionCountHeandler(y, x + 1);

                        _board[y, x] = 0;
                    }
                }
                return count;
            }
        }

        private bool CanPlacedInHorizontalLine(int y, int x, int value)
        {
            for (int iX = 0; iX < Sudoku.BoardSize; iX++)
            {
                if (iX == x)
                    continue;

                if (_board[y, iX] == value)
                    return false;
            }
            return true;
        }
        private bool CanPlacedInVerticalLine(int y, int x, int value)
        {
            for (int iY = 0; iY < Sudoku.BoardSize; iY++)
            {
                if (iY == y)
                    continue;

                if (_board[iY, x] == value)
                    return false;
            }
            return true;
        }
        private bool CanPlacedInSubMatrix(int y, int x, int value)
        {
            int startY = (y / Sudoku.BoardSubSize) * Sudoku.BoardSubSize;
            int startX = (x / Sudoku.BoardSubSize) * Sudoku.BoardSubSize;

            for (int iY = startY; iY < Sudoku.BoardSubSize + startY; iY++)
                for (int iX = startX; iX < Sudoku.BoardSubSize + startX; iX++)
                    if (_board[iY, iX] == value)
                        return false;

            return true;
        }
        private bool CanPlaced(int y, int x, int value)
        {
            return CanPlacedInHorizontalLine(y, x, value)
                && CanPlacedInVerticalLine(y, x, value)
                && CanPlacedInSubMatrix(y, x, value);
        }
        private List<int> FindPossibleValues(int y, int x)
        {
            var result = new List<int>();
            for (int value = Sudoku.MinValue; value <= Sudoku.MaxValue; value++)
            {
                if (CanPlaced(y, x, value))
                    result.Add(value);
            }
            return result;
        }
        private bool FindFirstClearCell(out int y, out int x)
        {
            for (y = 0; y < Sudoku.BoardSize; y++)
                for (x = 0; x < Sudoku.BoardSize; x++)
                    if (_board[y, x] == Sudoku.ClearValue)
                        return true;
            y = x = -1;
            return false;
        }

        public bool Solve()
        {
            Sudoku.CheckSize(_board);

            if (!FindFirstClearCell(out int y, out int x))
                return true;

            List<int> posibleValues = FindPossibleValues(y, x);
            foreach (var value in posibleValues)
            {
                _board[y, x] = value;
                if (Solve())
                    return true;
                _board[y, x] = Sudoku.ClearValue;
            }
            return false;
        }
    }
}
