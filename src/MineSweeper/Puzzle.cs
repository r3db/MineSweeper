using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Delete1
{
    internal sealed class Puzzle
    {
        internal event EventHandler Invalid  = delegate { };
        internal event EventHandler Complete = delegate { };

        private readonly PuzzlePiece[,] _data;
        private bool _isValid = true;

        internal Puzzle(int width, int height, string puzzle)
        {
            _data = new PuzzlePiece[height, width];

            foreach (var index in Iterator())
            {
                var x = index.X;
                var y = index.Y;
                var i = y * width + x;

                this[x, y] = puzzle[i] == '*'
                    ? PuzzlePiece.Mine
                    : PuzzlePiece.Empty;
            }
        }

        internal int Width => _data.GetLength(1);

        internal int Height => _data.GetLength(0);

        internal PuzzlePiece this[Point index]
        {
            get { return this[index.X, index.Y]; }
            set { this[index.X, index.Y] = value; }
        }

        internal PuzzlePiece this[int x, int y]
        {
            get { return _data[y, x]; }
            set { _data[y, x] = value; }
        }

        internal void Clear(int x, int y)
        {
            if (_isValid == false)
            {
                return;
            }

            var piece = this[x, y];

            if (piece.IsVisible)
            {
                return;
            }

            if (piece.IsMine)
            {
                piece.IsVisible = true;
                _isValid = false;

                ShowAllMines();
                Invalid.Invoke(this, EventArgs.Empty);
                return;
            }

            piece.IsVisible = true;

            if (IsComplete())
            {
                Complete.Invoke(this, EventArgs.Empty);
                return;
            }

            if (GetMineHintCount(x, y) != 0)
            {
                return;
            }

            foreach (var index in GetNeighbours(x, y))
            {
                var cPiece = this[index.X, index.Y];

                if (cPiece.IsMine)
                {
                    continue;
                }

                if (GetMineHintCount(index.X, index.Y) == 0)
                {
                    Clear(index.X, index.Y);
                }

                cPiece.IsVisible = true;
            }
        }

        internal void MarkAsMine(int x, int y)
        {
            if (_isValid == false)
            {
                return;
            }

            var piece = this[x, y];

            if (piece.IsVisible == false || piece.ProposeAsMine)
            {
                piece.ProposeAsMine = !piece.ProposeAsMine;
                piece.IsVisible     = !piece.IsVisible;
            }

            if (IsComplete())
            {
                Complete.Invoke(this, EventArgs.Empty);
            }
        }

        internal int GetMineHintCount(int x, int y)
        {
            return GetNeighbours(x, y).Sum(v => this[v].IsMine ? 1 : 0);
        }

        private IEnumerable<Point> GetNeighbours(int x, int y)
        {
            var fx = x - 1;
            var tx = x + 1;

            var fy = y - 1;
            var ty = y + 1;

            for (var cy = fy; cy <= ty; cy++)
            {

                if (cy < 0 || cy > Height - 1)
                {
                    continue;
                }

                for (var cx = fx; cx <= tx; cx++)
                {

                    if (cx < 0 || cx > Width - 1)
                    {
                        continue;
                    }

                    yield return new Point(cx, cy);
                }
            }
        }

        private IEnumerable<Point> Iterator()
        {
            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    yield return new Point(x, y);
                }
            }
        }

        private void ShowAllMines()
        {
            foreach (var index in Iterator())
            {
                var piece = this[index];

                if (piece.IsMine)
                {
                    piece.IsVisible = true;
                }

                piece.ProposeAsMine = false;
            }
        }

        private bool IsComplete()
        {
            foreach (var index in Iterator())
            {
                var piece = this[index];

                if (piece.IsMine != piece.ProposeAsMine || piece.IsVisible == false)
                {
                    return false;
                }
            }

            return true;
        }
    }
}