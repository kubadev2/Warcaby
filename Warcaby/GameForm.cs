using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Warcaby
{
    public partial class GameForm : Form
    {
        private Board board;
        private CheckerPiece selectedPiece = null;
        private Color defaultCellColor;  // Domyślny kolor tła komórki

        public GameForm(string difficulty)
        {
            InitializeComponent();

            if (difficulty != "Multiplayer")
            {
                lblCurrentPlayer.Text = "Gra z komputerem: " + difficulty;
            }
            else
            {
                lblCurrentPlayer.Text = "Gra dla dwóch graczy";
            }

            board = new Board();
            SetupBoard();
        }

        private void SetupBoard()
        {
            tableLayoutPanel1.Controls.Clear();

            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    Panel cellPanel = new Panel
                    {
                        Dock = DockStyle.Fill,
                        BackColor = board.GetCellColor(row, col)
                    };

                    Label label = new Label
                    {
                        Text = $"{row},{col}",
                        ForeColor = Color.Green,
                        Font = new Font("Arial", 10, FontStyle.Bold),
                        TextAlign = ContentAlignment.MiddleCenter
                    };

                    cellPanel.Controls.Add(label);

                    tableLayoutPanel1.Controls.Add(cellPanel, col, row);
                    cellPanel.Click += CellPanel_Click;

                    CheckerPiece piece = board.PieceAt(row, col);
                    if (piece != null)
                    {
                        cellPanel.Controls.Add(piece);
                        piece.Dock = DockStyle.Fill;
                        piece.Click += Piece_Click;
                    }
                }
            }
        }


        private int GetRowIndex(Control control)
        {
            return tableLayoutPanel1.GetRow(control);
        }

        private int GetColumnIndex(Control control)
        {
            return tableLayoutPanel1.GetColumn(control);
        }

        private void Piece_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Piece_Click wywołane");
            CheckerPiece clickedPiece = sender as CheckerPiece;

            if (selectedPiece != null)
            {
                selectedPiece.BackColor = defaultCellColor;
                ClearHighlightedCells();
            }

            if (clickedPiece.PieceColor == board.CurrentPlayerColor)
            {
                selectedPiece = clickedPiece;
                defaultCellColor = selectedPiece.BackColor;
                selectedPiece.BackColor = Color.Yellow;

                List<Tuple<int, int>> availableMoves = board.GetAvailableMoves(GetRowIndex(selectedPiece), GetColumnIndex(selectedPiece));

                Console.WriteLine($"Available moves for selected piece:");
                foreach (var move in availableMoves)
                {
                    Console.WriteLine($"({move.Item1}, {move.Item2})");
                }

                foreach (var move in availableMoves)
                {
                    int row = move.Item1;
                    int col = move.Item2;
                    Panel cellPanel = tableLayoutPanel1.GetControlFromPosition(col, row) as Panel;
                    cellPanel.BackColor = Color.Green;
                }
            }
            else
            {
                selectedPiece = null;
            }
        }



        private void CellPanel_Click(object sender, EventArgs e)
        {
            Console.WriteLine("CellPanel_Click");
            Control clickedControl = sender as Control;
            Panel clickedCell = clickedControl as Panel;

            if (selectedPiece != null && clickedCell != null && clickedCell.BackColor != Color.Black)
            {
                int fromRow = GetRowIndex(selectedPiece);
                int fromCol = GetColumnIndex(selectedPiece);
                int toRow = tableLayoutPanel1.GetRow(clickedCell);
                int toCol = tableLayoutPanel1.GetColumn(clickedCell);

                if (board.IsValidMove(fromRow, fromCol, toRow, toCol))
                {
                    Panel fromCell = tableLayoutPanel1.GetControlFromPosition(fromCol, fromRow) as Panel;
                    fromCell.Controls.Remove(selectedPiece);
                    clickedCell.Controls.Add(selectedPiece);
                    selectedPiece.Dock = DockStyle.Fill;

                    board.MovePiece(fromRow, fromCol, toRow, toCol);

                    // Usunięcie pionka przeskakiwanego
                    int jumpedRow = (fromRow + toRow) / 2;
                    int jumpedCol = (fromCol + toCol) / 2;
                    CheckerPiece jumpedPiece = board.PieceAt(jumpedRow, jumpedCol);
                    if (jumpedPiece != null)
                    {
                        Panel jumpedCell = tableLayoutPanel1.GetControlFromPosition(jumpedCol, jumpedRow) as Panel;
                        jumpedCell.Controls.Remove(jumpedPiece);
                    }

                    // Awansowanie pionka na damkę
                    if ((toRow == 0 && selectedPiece.PieceColor == Color.White) ||
                        (toRow == 7 && selectedPiece.PieceColor == Color.Red))
                    {
                        selectedPiece.IsKing = true;
                        selectedPiece.BackColor = Color.Gold;
                    }

                    selectedPiece = null;
                    board.SwitchPlayer();
                    ClearHighlightedCells();
                    UpdateUI();
                }
            }
        }


        private void UpdateUI()
        {
            Console.WriteLine("UpdateUI wywołane");
            lblCurrentPlayer.Text = "Aktualny gracz: " + (board.CurrentPlayerColor == Color.White ? "Biały" : "Czerwony");
        }

        private List<Panel> highlightedCells = new List<Panel>(); // Dodaj pole

        private void ClearHighlightedCells()
        {
            Console.WriteLine("ClearHighlightedCells wywołane");
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                Panel cellPanel = control as Panel;
                if (cellPanel != null && cellPanel.BackColor == Color.Green)
                {
                    cellPanel.BackColor = board.GetCellColor(tableLayoutPanel1.GetRow(cellPanel), tableLayoutPanel1.GetColumn(cellPanel));

                    if (cellPanel.Controls.Count == 0)
                    {
                        cellPanel.BackColor = Color.Black;
                    }
                }
            }

            // Wyczyść listę podświetlonych pól
            foreach (Panel highlightedCell in highlightedCells)
            {
                highlightedCell.BackColor = board.GetCellColor(tableLayoutPanel1.GetRow(highlightedCell), tableLayoutPanel1.GetColumn(highlightedCell));
            }
            highlightedCells.Clear();
        }

    }
}
