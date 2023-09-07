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

        private Label kingLabel;

        public CheckerPiece(Color color, int row, int col)
        {
            PieceColor = color;
            IsKing = false; // Początkowo pionek nie jest damą
            Row = row;
            Col = col;

            // Inicjalizacja etykiety "K" i ukrycie jej na początku
            kingLabel = new Label();
            kingLabel.Text = "K";
            kingLabel.TextAlign = ContentAlignment.MiddleCenter;
            kingLabel.Visible = false;
            Controls.Add(kingLabel);
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

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            // Ustaw rozmiar etykiety i jej pozycję na środku pionka
            kingLabel.Size = new Size(this.Width, this.Height);
            kingLabel.Location = new Point(0, 0);
        }

        // Metoda do pokazania/ukrycia litery "K" na pionku
        public void ShowKingLabel(bool show)
        {
            kingLabel.Visible = show;
        }
    }

}
