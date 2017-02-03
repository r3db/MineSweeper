using System;
using System.Windows.Forms;

namespace Delete1
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            var puzzle = new Puzzle(14, 9, @"
                ...*......*.**
                .*...*........
                .........*....
                ...*......*...
                .....*....*.**
                ..*.....*.....
                .*..*.........
                ..............
                ...*....*....."
            .Replace(" ", string.Empty)
            .Replace(Environment.NewLine, string.Empty));

            puzzle.Complete += (sender, args) =>
            {
                Draw(puzzle);
                MessageBox.Show(@"Complete");
            };

            puzzle.Invalid += (sender, args) =>
            {
                Draw(puzzle);
                MessageBox.Show(@"Kabum!!!");
            };

            Resize += (sender, args) =>
            {
                Draw(puzzle);
                Refresh();
            };

            _pictureBox1.Click += (sender, args) =>
            {
                var eventArgs = (MouseEventArgs)args;

                var x = (int)(eventArgs.Location.X / (float)_pictureBox1.Width  * puzzle.Width);
                var y = (int)(eventArgs.Location.Y / (float)_pictureBox1.Height * puzzle.Height);


                switch (eventArgs.Button)
                {
                    case MouseButtons.Left:
                        puzzle.Clear(x, y);
                        break;

                    case MouseButtons.Right:
                        puzzle.MarkAsMine(x, y);
                        break;
                }

                Draw(puzzle);
            };

            Draw(puzzle);
        }

        private void Draw(Puzzle puzzle)
        {
            _pictureBox1.Image = PuzzleRenderer.Draw(puzzle, _pictureBox1.Width, _pictureBox1.Height);
        }
    }
}