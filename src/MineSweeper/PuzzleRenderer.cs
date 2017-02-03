using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace Delete1
{
    // Todo: Refactor!
    internal static class PuzzleRenderer
    {
        private const string MineText = "\uf1c0";
        private const string EmptyHintText = "_";

        // Todo: Dispose all of this!
        private static readonly Font _mineFont = new Font(FontAwesome.FontFamily, 20f);
        private static readonly Font _hintFont = new Font("Consolas", 12f, FontStyle.Bold);
        private static readonly Font _coordinateFont = new Font("Consolas", 8f);

        private static readonly Brush _hintBrush = Brushes.Blue;
        private static readonly Brush _mineCellBrush = Brushes.Tomato;
        private static readonly Brush _mineTextBrush = Brushes.Black;
        private static readonly Brush _visibleCellBrush = Brushes.LightGray;
        private static readonly Brush _coordinateBrush = Brushes.Gray;

        private static readonly StringFormat _defaultFontFormat = StringFormat.GenericTypographic;

        // Todo: Ignore the horrible variable names!
        internal static Image Draw(Puzzle puzzle, int width, int height)
        {
            var result = new Bitmap(width, height);
            var graphics = Graphics.FromImage(result);

            SetHighQuality(graphics);

            var sizeX = width  / puzzle.Width;
            var sizeY = height / puzzle.Height;

            for (var y = 0; y < puzzle.Height; y++)
            {
                for (var x = 0; x < puzzle.Width; x++)
                {
                    var fx = x * sizeX;
                    var fy = y * sizeY;
                    var piece = puzzle[x, y];

                    if (piece.IsVisible)
                    {
                        var cx = fx + sizeX / 2f;
                        var cy = fy + sizeY / 2f;

                        if (piece.IsMine || piece.ProposeAsMine)
                        {
                            DrawMine(graphics, fx, fy, sizeX, sizeY, cx, cy);
                        }
                        else
                        {
                            var mineHintCount = puzzle.GetMineHintCount(x, y);
                            var mineCountText = mineHintCount == 0 ? EmptyHintText : mineHintCount.ToString();

                            DrawHint(graphics, mineCountText, cx, cy, sizeX, sizeY, fx, fy);
                        }
                    }

                    DrawCell(graphics, fx, fy, sizeX, sizeY, x, y);
                }
            }

            return result;
        }
        
        private static void DrawMine(Graphics g, int x, int y, int width, int height, float cx, float cy)
        {
            var size = MeasureString(g, MineText, _mineFont);

            var ax = cx - size.Width  / 2f;
            var ay = cy - size.Height / 2f;

            g.FillRectangle(_mineCellBrush, x, y, width, height);
            g.DrawString(MineText, _mineFont, _mineTextBrush, ax, ay, _defaultFontFormat);
        }

        private static void DrawHint(Graphics g, string text, float x, float y, int width, int height, float cx, float cy)
        {
            var size = MeasureString(g, text, _hintFont);
            g.FillRectangle(_visibleCellBrush, cx, cy, width, height);
            g.DrawString(text, _hintFont, _hintBrush, x - size.Width / 2f, y - size.Height / 2f, _defaultFontFormat);
        }

        private static void DrawCell(Graphics g, int x, int y, int width, int height, int xCell, int yCell)
        {
            g.DrawRectangle(Pens.DarkSlateGray, x, y, width, height);
            g.DrawString($"({xCell + 1},{yCell + 1})", _coordinateFont, _coordinateBrush, x + 4, y + 4, _defaultFontFormat);
        }

        private static SizeF MeasureString(Graphics g, string text, Font font)
        {
            return g.MeasureString(text, font, PointF.Empty, _defaultFontFormat);
        }

        private static void SetHighQuality(Graphics g)
        {
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.InterpolationMode  = InterpolationMode.HighQualityBicubic;
            g.SmoothingMode      = SmoothingMode.AntiAlias;
            g.TextRenderingHint  = TextRenderingHint.AntiAlias;
        }
    }
}