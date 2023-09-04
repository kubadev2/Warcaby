using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Warcaby
{
    public class CheckerPiece : Control
    {
        public Color PieceColor { get; private set; }
        public bool IsKing { get; set; }
        public int Row { get; set; }
        public int Col { get; set; }

        public CheckerPiece(Color color, int row, int col)
        {
            PieceColor = color;
            IsKing = false;
            Row = row;
            Col = col;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            using (SolidBrush brush = new SolidBrush(this.PieceColor))
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                int size = Math.Min(this.Width, this.Height) - 10;
                int x = (this.Width - size) / 2;
                int y = (this.Height - size) / 2;

                e.Graphics.FillEllipse(brush, x, y, size, size);
            }
        }
    }
}