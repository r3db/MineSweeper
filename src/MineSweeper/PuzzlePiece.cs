using System;

namespace Delete1
{
    internal sealed class PuzzlePiece
    {
        private PuzzlePiece(bool isMine, bool proposeAsMine)
        {
            IsMine        = isMine;
            IsVisible     = false;
            ProposeAsMine = proposeAsMine;
        }

        internal static PuzzlePiece Mine => new PuzzlePiece(true, false);

        internal static PuzzlePiece Empty => new PuzzlePiece(false, false);

        internal bool IsMine { get; }

        internal bool IsVisible { get; set; }

        internal bool ProposeAsMine { get; set; }
    }
}