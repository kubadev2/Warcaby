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
        private bool isPlayer1Turn = true; // Zmienna śledząca aktualnego gracza (true - gracz 1, false - gracz 2)

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

            // Utwórz instancję klasy Board i przekaż do niej referencję do tego obiektu GameForm
            board = new Board(this);
            SetupBoard();
            Console.WriteLine("GameForm constructor called");
        }

        private void SetupBoard()
        {
            bool isWhite = true;
            int cellSize = 60; // Rozmiar komórki planszy

            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    Panel cellPanel = new Panel
                    {
                        Location = new Point(col * cellSize, row * cellSize),
                        Size = new Size(cellSize, cellSize),
                        BackColor = board.GetCellColor(row, col)
                    };

                    cellPanel.Click += CellPanel_Click;

                    CheckerPiece piece = board.PieceAt(row, col);
                    if (piece != null)
                    {
                        cellPanel.Controls.Add(piece);
                        piece.Dock = DockStyle.Fill;

                        // Dodaj obsługę kliknięcia na pionka
                        piece.Click += CheckerPiece_Click;
                    }

                    cellPanel.Tag = new Tuple<int, int>(row, col); // Przypisz pozycję planszy jako Tag dla panelu
                    cellPanel.BackColor = isWhite ? Color.White : Color.Black; // Ustaw kolor tła

                    this.Controls.Add(cellPanel); // Dodaj panel do formularza
                    isWhite = !isWhite; // Zmień kolor tła na przemian
                }

                isWhite = !isWhite; // Na początku kolejnego wiersza zmień kolor na przemian
            }
            Console.WriteLine("SetupBoard called");
        }

        private void CheckerPiece_Click(object sender, EventArgs e)
        {
            CheckerPiece clickedPiece = sender as CheckerPiece;

            if (clickedPiece != null)
            {
                Console.WriteLine($"CheckerPiece_Click called for piece at Row={clickedPiece.Row}, Col={clickedPiece.Col}");

                // Sprawdź, czy to jest tura aktualnego gracza
                if ((isPlayer1Turn && clickedPiece.PieceColor == Color.White) ||
                    (!isPlayer1Turn && clickedPiece.PieceColor == Color.Red))
                {
                    // Jeśli nie ma jeszcze wybranego pionka, zaznacz go
                    if (selectedPiece == null)
                    {
                        selectedPiece = clickedPiece;
                        selectedPiece.BackColor = Color.Yellow; // Zaznacz wybrany pionek na żółto

                        // Ustaw domyślny kolor tła komórki na kolor tła wybranej komórki
                        Panel selectedCell = GetCellByPosition(selectedPiece.Col, selectedPiece.Row);
                        defaultCellColor = selectedCell.BackColor;

                        // Od razu wyświetl dostępne ruchy
                        RefreshAvailableMoves();
                        HighlightAvailableMoves();
                    }
                    else if (clickedPiece == selectedPiece)
                    {
                        // Jeśli kliknięto ponownie ten sam pionek, odznacz go
                        selectedPiece.BackColor = defaultCellColor;
                        selectedPiece = null;
                    }
                    else
                    {
                        // Jeśli kliknięto inny pionek, odznacz aktualnie wybrany i zaznacz nowy
                        selectedPiece.BackColor = defaultCellColor;
                        selectedPiece = clickedPiece;
                        selectedPiece.BackColor = Color.Yellow;

                        // Od razu wyświetl dostępne ruchy
                        RefreshAvailableMoves();
                        HighlightAvailableMoves();
                    }
                }
            }
        }

        private void CellPanel_Click(object sender, EventArgs e)
        {
            Panel clickedCell = sender as Panel;

            object tagObject = clickedCell.Tag;

            if (tagObject != null && tagObject is Tuple<int, int> position)
            {
                int clickedRow = position.Item1;
                int clickedCol = position.Item2;

                Console.WriteLine($"CellPanel_Click called with Row={clickedRow}, Col={clickedCol}");

                if (selectedPiece != null)
                {
                    int fromRow = selectedPiece.Row;
                    int fromCol = selectedPiece.Col;
                    int toRow = clickedRow;
                    int toCol = clickedCol;

                    CheckerPiece clickedPiece = board.PieceAt(clickedRow, clickedCol);

                    if (clickedPiece != null && clickedPiece.PieceColor == selectedPiece.PieceColor)
                    {
                        selectedPiece.BackColor = defaultCellColor;
                        selectedPiece = clickedPiece;
                        selectedPiece.BackColor = Color.Yellow;
                    }
                    else if (board.IsValidMove(fromRow, fromCol, toRow, toCol) &&
                             Math.Abs(toRow - fromRow) == 1) // Ruch dozwolony po skosie do góry
                    {
                        Panel fromCell = GetCellByPosition(fromCol, fromRow);
                        clickedCell.Controls.Add(selectedPiece);
                        Console.WriteLine("Pionek dodany");
                        fromCell.Controls.Remove(selectedPiece);
                        Console.WriteLine("Pionek usunięty");

                        selectedPiece.Dock = DockStyle.Fill;
                        //SwitchPlayer();

                        if (fromCell != clickedCell) // Sprawdzamy, czy pionek zmienił pole
                        {
                            board.MovePiece(fromRow, fromCol, toRow, toCol);
                        }

                        // Usunięcie pionka przeskakiwanego
                        int jumpedRow = (fromRow + toRow) / 2;
                        int jumpedCol = (fromCol + toCol) / 2;
                        CheckerPiece jumpedPiece = board.PieceAt(jumpedRow, jumpedCol);
                        if (jumpedPiece != null)
                        {
                            Panel jumpedCell = GetCellByPosition(jumpedCol, jumpedRow);
                            jumpedCell.Controls.Remove(jumpedPiece);
                        }

                        // Awansowanie pionka na damkę
                        if ((toRow == 0 && selectedPiece.PieceColor == Color.White) ||
                            (toRow == 7 && selectedPiece.PieceColor == Color.Red))
                        {
                            selectedPiece.IsKing = true;
                            selectedPiece.BackColor = Color.Gold;
                        }

                        selectedPiece.BackColor = defaultCellColor;
                        selectedPiece = null;

                        RefreshAvailableMoves();
                    }
                    else if (board.IsValidJump(fromRow, fromCol, toRow, toCol) &&
                             Math.Abs(toRow - fromRow) == 2) // Bicie dozwolone po skosie do góry
                    {
                        Console.WriteLine("jump in cell panel click");
                        Panel fromCell = GetCellByPosition(fromCol, fromRow);
                        fromCell.Controls.Remove(selectedPiece);
                        clickedCell.Controls.Add(selectedPiece);
                        selectedPiece.Dock = DockStyle.Fill;

                        board.MovePiece(fromRow, fromCol, toRow, toCol);

                        int jumpedRow = (fromRow + toRow) / 2;
                        int jumpedCol = (fromCol + toCol) / 2;
                        CheckerPiece jumpedPiece = board.PieceAt(jumpedRow, jumpedCol);
                        if (jumpedPiece != null)
                        {
                            Panel jumpedCell = GetCellByPosition(jumpedCol, jumpedRow);
                            jumpedCell.Controls.Remove(jumpedPiece);
                        }

                        if ((toRow == 0 && selectedPiece.PieceColor == Color.White) ||
                            (toRow == 7 && selectedPiece.PieceColor == Color.Red))
                        {
                            selectedPiece.IsKing = true;
                            selectedPiece.BackColor = Color.Gold;
                        }

                        selectedPiece.BackColor = defaultCellColor;
                        selectedPiece = null;

                        RefreshAvailableMoves();
                    }
                }
                else if (clickedCell.Controls.Count > 0)
                {
                    selectedPiece = clickedCell.Controls[0] as CheckerPiece;
                }
            }
        }

        private void RefreshAvailableMoves()
        {
            // Usuń podświetlenie dostępnych ruchów (przywróć domyślny kolor tła)
            foreach (Control control in this.Controls)
            {
                if (control is Panel cellPanel)
                {
                    Tuple<int, int> position = cellPanel.Tag as Tuple<int, int>;
                    if (position != null)
                    {
                        int row = position.Item1;
                        int col = position.Item2;
                        cellPanel.BackColor = board.GetCellColor(row, col);
                    }
                }
            }

            // Jeśli wybrano pionka, podświetl dostępne ruchy
            if (selectedPiece != null)
            {
                int fromRow = selectedPiece.Row;
                int fromCol = selectedPiece.Col;

                for (int row = 0; row < 8; row++)
                {
                    for (int col = 0; col < 8; col++)
                    {
                        if (board.IsValidMove(fromRow, fromCol, row, col) &&
                            GetCellByPosition(col, row).BackColor == Color.Black) // Sprawdź, czy pole jest czarne
                        {
                            Panel cellPanel = GetCellByPosition(col, row);
                            cellPanel.BackColor = Color.Green; // Podświetl dostępne ruchy na zielono
                        }
                    }
                }
            }
        }

        private void HighlightAvailableMoves()
        {
            // Przechodź przez wszystkie komórki na planszy
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    // Sprawdź, czy ruch jest dozwolony z aktualnie wybranej pozycji
                    if (board.IsValidMove(selectedPiece.Row, selectedPiece.Col, row, col))
                    {
                        Panel cellPanel = GetCellByPosition(col, row);
                        cellPanel.BackColor = Color.Green; // Podświetl dostępne ruchy na zielono
                    }
                }
            }
        }

        public Panel GetCellByPosition(int col, int row)
        {
            foreach (Control control in this.Controls)
            {
                if (control is Panel cellPanel)
                {
                    Tuple<int, int> position = cellPanel.Tag as Tuple<int, int>;
                    if (position != null && position.Item1 == row && position.Item2 == col)
                    {
                        return cellPanel;
                    }
                }
            }
            return null;
        }
        public void SwitchPlayer()
        {
            isPlayer1Turn = !isPlayer1Turn;

            if (isPlayer1Turn)
            {
                lblCurrentPlayer.Text = "Gracz 1";
            }
            else
            {
                lblCurrentPlayer.Text = "Gracz 2";
            }
        }

    }
}