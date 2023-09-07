using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace Warcaby
{
    public partial class GameForm : Form
    {
        private Board board;
        private CheckerPiece selectedPiece = null;
        private Color defaultCellColor;  // Domyślny kolor tła komórki
        private bool isPlayer1Turn = true; // Zmienna śledząca aktualnego gracza (true - gracz 1, false - gracz 2)
        private string level;
        private void PerformBotMove()
        {
            // Jeśli aktualny gracz to czerwony i poziom trudności to "Easy"
            if (!isPlayer1Turn && level == "Easy")
            {
                // Znajdź wszystkie dostępne ruchy dla czerwonych pionków
                List<Tuple<int, int, int, int>> availableMoves = FindAllAvailableMoves(Color.Red);

                // Jeśli są dostępne ruchy, wykonaj losowy ruch
                if (availableMoves.Count > 0)
                {
                    Random random = new Random();
                    int randomIndex = random.Next(availableMoves.Count);
                    var move = availableMoves[randomIndex];

                    int fromRow = move.Item1;
                    int fromCol = move.Item2;
                    int toRow = move.Item3;
                    int toCol = move.Item4;

                    CheckerPiece botPiece = board.PieceAt(fromRow, fromCol);

                    // Wykonaj ruch
                    Console.WriteLine(fromRow +" " +fromCol+ " " +toRow+ " " +toCol);
                    board.MovePiece(fromRow, fromCol, toRow, toCol);

                    GetCellByPosition(toCol, toRow).Controls.Add(botPiece);
                    botPiece.Dock = DockStyle.Fill;
                    botPiece.Row = toRow; // Aktualizacja pozycji pionka
                    botPiece.Col = toCol;

                    Console.WriteLine("zbijający powinien stać na" + toRow + " " + toCol);
                }
                else
                {
                    // Jeśli nie ma dostępnych ruchów, to czerwoni przegrywają
                    MessageBox.Show("Czerwoni przegrywają. Koniec gry.");
                    // Tutaj możesz dodać kod do resetowania gry lub wyjścia z aplikacji
                }
            }
        }
        



        private List<Tuple<int, int, int, int>> FindAllAvailableMoves(Color playerColor)
        {
            // Znajdź wszystkie dostępne ruchy dla danego koloru gracza
            List<Tuple<int, int, int, int>> availableMoves = new List<Tuple<int, int, int, int>>();

            for (int fromRow = 0; fromRow < 8; fromRow++)
            {
                for (int fromCol = 0; fromCol < 8; fromCol++)
                {
                    CheckerPiece piece = board.PieceAt(fromRow, fromCol);

                    if (piece != null && piece.PieceColor == playerColor)
                    {
                        for (int toRow = 0; toRow < 8; toRow++)
                        {
                            for (int toCol = 0; toCol < 8; toCol++)
                            {
                                if (board.IsValidMove(fromRow, fromCol, toRow, toCol) ||
                                    board.IsValidJump(fromRow, fromCol, toRow, toCol))
                                {
                                    availableMoves.Add(Tuple.Create(fromRow, fromCol, toRow, toCol));
                                }
                            }
                        }
                    }
                }
            }

            return availableMoves;
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
                PerformBotMove();
            }
        }
        public GameForm(string difficulty)
        {
            InitializeComponent();
            board = new Board(this);
            this.level = difficulty;
            SetupBoard();
            Console.WriteLine("GameForm constructor called");

            if (difficulty != "Multiplayer")
            {
                lblCurrentPlayer.Text = "Gra z komputerem: " + difficulty;
               
                    

            }
            else
            {
                lblCurrentPlayer.Text = "Gra dla dwóch graczy";
            }

            // Utwórz instancję klasy Board i przekaż do niej referencję do tego obiektu GameForm
            
        }

        private void SetupBoard()
        {
            bool isWhite = true;
            int cellSize = 60; // Rozmiar komórki planszy
            int leftPadding = 50; // Odległość od lewej krawędzi formularza

            char[] letters = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H' };
            int[] numbers = { 1, 2, 3, 4, 5, 6, 7, 8 };

            // Dodaj litery do górnego rzędu
            for (int col = 0; col < 8; col++)
            {
                Label letterLabel = new Label
                {
                    Location = new Point(leftPadding + col * cellSize, 0),
                    Size = new Size(cellSize, cellSize),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Text = letters[col].ToString(),
                    Font = new Font(Font.FontFamily, 14, FontStyle.Bold),
                    ForeColor = isWhite ? Color.Black : Color.White
                };

                this.Controls.Add(letterLabel);
            }

            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    Panel cellPanel = new Panel
                    {
                        Location = new Point(leftPadding + col * cellSize, (row + 1) * cellSize),
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

                    // Dodaj czarne cyfry do lewej kolumny
                    if (col == 0)
                    {
                        Label numberLabel = new Label
                        {
                            Location = new Point(col * cellSize, (row + 1) * cellSize),
                            Size = new Size(leftPadding, cellSize),
                            TextAlign = ContentAlignment.MiddleCenter,
                            Text = numbers[row].ToString(),
                            Font = new Font(Font.FontFamily, 14, FontStyle.Bold),
                            ForeColor = Color.Black // Ustaw kolor czcionki na czarny
                        };

                        this.Controls.Add(numberLabel);
                    }

                    isWhite = !isWhite; // Zmień kolor tła na przemian
                }

                isWhite = !isWhite; // Na początku kolejnego wiersza zmień kolor na przemian
            }
            Console.WriteLine("SetupBoard called");
        }



        //...


        private void HighlightAvailableMoves()
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

                        if (board.IsValidMove(selectedPiece.Row, selectedPiece.Col, row, col))
                        {
                            cellPanel.BackColor = Color.LightGreen;
                        }
                        else if (board.IsValidJump(selectedPiece.Row, selectedPiece.Col, row, col))
                        {
                            cellPanel.BackColor = Color.Red;
                        }
                    }
                }
            }
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
                else if (selectedPiece != null &&
                         selectedPiece.PieceColor == Color.White && clickedPiece.PieceColor == Color.Red)
                {
                    // Obsłuż sytuację, w której biały pionek próbuje zbijać czerwonego.
                    int fromRow = selectedPiece.Row;
                    int fromCol = selectedPiece.Col;
                    int toRow = clickedPiece.Row;
                    int toCol = clickedPiece.Col;

                    // Sprawdź, czy ruch jest dozwolony i czy jest to ruch "bicia"
                    if (board.IsValidJump(fromRow, fromCol, toRow, toCol) && Math.Abs(toRow - fromRow) == 2)
                    {
                        // Biały pionek zbija czerwonego
                        Panel fromCell = GetCellByPosition(fromCol, fromRow);
                        Panel jumpedCell = GetCellByPosition((fromCol + toCol) / 2, (fromRow + toRow) / 2);
                        CheckerPiece jumpedPiece = board.PieceAt((fromRow + toRow) / 2, (fromCol + toCol) / 2);

                        fromCell.Controls.Remove(selectedPiece);
                        jumpedCell.Controls.Remove(jumpedPiece); // Usuń zbitego pionka przeciwnika
                        GetCellByPosition(toCol, toRow).Controls.Add(selectedPiece);

                        selectedPiece.Row = toRow;
                        selectedPiece.Col = toCol;

                        board.MovePiece(fromRow, fromCol, toRow, toCol);
                        board.RemovePiece((fromRow + toRow) / 2, (fromCol + toCol) / 2);

                        // Sprawdź, czy biały pionek stał się królem
                        if (toRow == 0)
                        {
                            selectedPiece.IsKing = true;
                            selectedPiece.BackColor = Color.Gold;
                        }

                        selectedPiece.BackColor = defaultCellColor;
                        selectedPiece = null;

                        // Odśwież dostępne ruchy
                        RefreshAvailableMoves();

                        // Zmiana gracza
                        SwitchPlayer();
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
                    else if (board.IsValidMove(selectedPiece.Row, selectedPiece.Col, toRow, toCol) &&
                             Math.Abs(toRow - fromRow) == 1) // Ruch dozwolony po skosie do góry
                    {
                        Panel fromCell = GetCellByPosition(fromCol, fromRow);
                        clickedCell.Controls.Add(selectedPiece);
                        fromCell.Controls.Remove(selectedPiece);

                        selectedPiece.Row = toRow; // Aktualizacja pozycji pionka
                        selectedPiece.Col = toCol;

                        selectedPiece.Dock = DockStyle.Fill;

                        if (fromCell != clickedCell)
                        {
                            board.MovePiece(fromRow, fromCol, toRow, toCol);
                        }

                        selectedPiece.BackColor = defaultCellColor;
                        selectedPiece = null;

                        RefreshAvailableMoves();
                    }
                    else if (board.IsValidJump(selectedPiece.Row, selectedPiece.Col, toRow, toCol) &&
                             Math.Abs(toRow - fromRow) == 2) // Bicie dozwolone po skosie do góry
                    {
                        Panel fromCell = GetCellByPosition(fromCol, fromRow);
                        fromCell.Controls.Remove(selectedPiece);
                        clickedCell.Controls.Add(selectedPiece);
                        selectedPiece.Dock = DockStyle.Fill;
                        selectedPiece.Row = toRow; // Aktualizacja pozycji pionka
                        selectedPiece.Col = toCol;

                        board.MovePiece(fromRow, fromCol, toRow, toCol);

                        int jumpedRow = (fromRow + toRow) / 2;
                        int jumpedCol = (fromCol + toCol) / 2;
                        CheckerPiece jumpedPiece = board.PieceAt(jumpedRow, jumpedCol);
                        Console.WriteLine("usuwamy with cell click: " + jumpedRow + " " + jumpedCol);
                            Panel jumpedCell = GetCellByPosition(jumpedCol, jumpedRow);
                            jumpedCell.Controls.Remove(jumpedPiece);
                        jumpedCell.Controls.Clear();
                        //board.RemovePiece(jumpedRow, jumpedCol); // Usuń pionka z planszy
                      

                        if (toRow == 0 && selectedPiece.PieceColor == Color.White)
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
        

    }
}