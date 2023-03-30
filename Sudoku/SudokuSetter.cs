using System.Diagnostics;

namespace Sudoku
{
    internal class SudokuSetter
    {
        private readonly int[,] _board;
        private readonly Random _random = new();

        public SudokuSetter(int[,] board)
        {
            Sudoku.CheckSize(board);
            _board = board;
        }

        public void Clear()
        {
            for (int y = 0; y < Sudoku.BoardSize; y++)
                for (int x = 0; x < Sudoku.BoardSize; x++)
                    _board[y, x] = Sudoku.ClearValue;
        }
        public void SetDiagonalSubs()
        {
            Clear();

            List<int> possibleValues;
            int startY, startX;
            int randomIndex;

            for (int y = 0; y < Sudoku.BoardSubSize; y++)
            {
                for (int x = 0; x < Sudoku.BoardSubSize; x++)
                {
                    if (x == y)
                    {
                        possibleValues = Enumerable.Range(1, Sudoku.BoardSize).ToList();

                        startX = x * Sudoku.BoardSubSize;
                        startY = y * Sudoku.BoardSubSize;

                        for (int iY = startY; iY < Sudoku.BoardSubSize + startY; iY++)
                        {
                            for (int iX = startX; iX < Sudoku.BoardSubSize + startX; iX++)
                            {
                                randomIndex = _random.Next(0, possibleValues.Count);
                                _board[iY, iX] = possibleValues[randomIndex];
                                possibleValues.RemoveAt(randomIndex);
                            }
                        }
                    }
                }
            }
        }

        public void SetNums()
        {
            SetDiagonalSubs();
            var solver = new SudokuSolver(_board);
            solver.Solve();
        }
        public bool ClearCells(int removeCount, double timeoutInSec = 1, double timeoutForTryInSec = 0.1)
        {
            var startBoard = new int[Sudoku.BoardSize, Sudoku.BoardSize];
            Array.Copy(_board, startBoard, _board.Length);

            var inputStopwatch = Stopwatch.StartNew();
            while (inputStopwatch.Elapsed.TotalSeconds < timeoutInSec)
            {
                var unclearedCells = new List<(int y, int x)>();
                for (var y = 0; y < Sudoku.BoardSize; y++)
                    for (var x = 0; x < Sudoku.BoardSize; x++)
                        if (_board[y, x] != Sudoku.ClearValue)
                            unclearedCells.Add((y, x));

                var solver = new SudokuSolver(_board);

                var tryStopwatch = Stopwatch.StartNew();
                for (var i = 0; i < removeCount; i++)
                {
                    var randomUnclearedIndex = _random.Next(unclearedCells.Count);

                    int randomY = unclearedCells[randomUnclearedIndex].y;
                    int randomX = unclearedCells[randomUnclearedIndex].x;
                    int currentValue = _board[randomY, randomX];

                    _board[randomY, randomX] = Sudoku.ClearValue;

                    if (solver.GetSolutionCount() != 1)
                    {
                        _board[randomY, randomX] = currentValue;
                        i--;
                    }
                    else
                    {
                        unclearedCells.RemoveAt(randomUnclearedIndex);

                        if (i == removeCount - 1)
                            return true;
                    }

                    if (tryStopwatch.Elapsed.TotalSeconds > timeoutForTryInSec)
                    {
                        Array.Copy(startBoard, _board, _board.Length);
                        break;
                    }
                }
            }
            return false;
        }
    }
}
