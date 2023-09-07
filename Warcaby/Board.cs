using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Warcaby
{
    public class Board
    {
        private CheckerPiece[,] pieces;
        private Color currentPlayerColor = Color.White;
        private bool isJumpInProgress = false;
        private int jumpStartRow;
        private int jumpStartCol;
        private Color defaultCellColor = Color.Empty; // Domyślny kolor tła komórki
        private GameForm gameForm;
        private int forwardDirection;

        public Color CurrentPlayerColor => currentPlayerColor;

        public Board(GameForm gameForm)
        {
            this.gameForm = gameForm; // Przypisz referencję do obiektu GameForm
            pieces = new CheckerPiece[8, 8];
            InitializeBoard();
        }

        private void InitializeBoard()
        {
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    if ((row + col) % 2 == 1)
                    {
                        if (row < 3)
                        {
                            pieces[row, col] = new CheckerPiece(Color.Red, row, col); // Zamiana kolorów
                        }
                        else if (row > 4)
                        {
                            pieces[row, col] = new CheckerPiece(Color.White, row, col); // Zamiana kolorów
                        }
                    }
                }
            }
        }


        public List<Tuple<int, int>> GetAvailableMoves(int row, int col)
        {
            Console.WriteLine("GetAvailableMoves called");
            List<Tuple<int, int>> availableMoves = new List<Tuple<int, int>>();

            CheckerPiece piece = PieceAt(row, col);

            if (piece != null && piece.PieceColor == CurrentPlayerColor)
            {
                int forwardDirection = (piece.PieceColor == Color.White) ? -1 : 1;
                int forwardRow = row + forwardDirection;
                int leftCol = col - 1;
                int rightCol = col + 1;

                if (IsValidMove(piece.Row, piece.Col, forwardRow, leftCol))
                {
                    availableMoves.Add(new Tuple<int, int>(forwardRow, leftCol));
                }

                if (IsValidMove(piece.Row, piece.Col, forwardRow, rightCol))
                {
                    availableMoves.Add(new Tuple<int, int>(forwardRow, rightCol));
                }
            }

            return availableMoves;
        }

        public void MovePiece(int fromRow, int fromCol, int toRow, int toCol)
        {
            Console.WriteLine("MovePiece called");
            CheckerPiece piece = pieces[fromRow, fromCol];

            // Usuń istniejący pionek ze starego miejsca
            pieces[fromRow, fromCol] = null;
            Panel fromCell = gameForm.GetCellByPosition(fromCol, fromRow);
            fromCell.Controls.Remove(piece);

            // Stwórz nowy pionek w nowym miejscu
            pieces[toRow, toCol] = new CheckerPiece(piece.PieceColor, toRow, toCol);
            CheckerPiece newPiece = pieces[toRow, toCol];
            Panel toCell = gameForm.GetCellByPosition(toCol, toRow);
            toCell.Controls.Add(newPiece);
            newPiece.Location = new Point(toCol * 60, toRow * 60);

            // Usuń pionek przeskakiwany
            if (Math.Abs(toRow - fromRow) == 2)
            {
                int jumpedRow = (fromRow + toRow) / 2;
                int jumpedCol = (fromCol + toCol) / 2;
                Console.WriteLine("usuwamy: " + jumpedRow + " " + jumpedCol);
                CheckerPiece jumpedPiece = pieces[jumpedRow, jumpedCol];
                if (jumpedPiece != null)
                {
                    Console.WriteLine("usuwamy pionka");
                    pieces[jumpedRow, jumpedCol] = null; // Usuń pionka z planszy
                    jumpedPiece.Dispose(); // Usuń pionka z formularza
                    Panel jumpedCell = gameForm.GetCellByPosition(jumpedCol, jumpedRow);
                    jumpedCell.Controls.Remove(jumpedPiece);
                    Console.WriteLine("pionek usunięty");
                }
            }


            // Awansuj pionka na damkę, jeśli dotrze do końca planszy
            if ((toRow == 0 && newPiece.PieceColor == Color.White) || (toRow == 7 && newPiece.PieceColor == Color.Red))
            {
                newPiece.IsKing = true;
                newPiece.BackColor = Color.Gold;
            }

            // Odznacz wybrany pionek po wykonaniu ruchu
            newPiece.BackColor = Color.White; // Tutaj użyj odpowiedniego koloru tła

            // Zakończ bieżący skok (jeśli taki istnieje)
            isJumpInProgress = false;

            // Przełącz gracza
            gameForm.SwitchPlayer();
        }



        public bool IsValidMove(int fromRow, int fromCol, int toRow, int toCol)
        {
            Console.WriteLine("IsValidMove called");
            CheckerPiece piece = pieces[fromRow, fromCol];

            if (piece == null)
            {
                Console.WriteLine("Pole jest puste, nie można przesunąć pionka.");
                return false;
            }

            // Sprawdź, czy docelowa komórka jest pusta
            if (pieces[toRow, toCol] != null)
            {
                Console.WriteLine("Docelowa komórka jest zajęta, nie można przesunąć pionka.");
                return false;
            }

            // Sprawdź, czy ruch jest o jedno pole w prawo lub lewo i w górę (dla gracza 1) lub w dół (dla gracza 2)
            int rowDiff = Math.Abs(toRow - fromRow);
            int colDiff = Math.Abs(toCol - fromCol);

            if (rowDiff == 1 && colDiff == 1)
            {
                if ((piece.PieceColor == Color.White && toRow < fromRow) ||
                    (piece.PieceColor == Color.Red && toRow > fromRow))
                {
                    Console.WriteLine("Ruch o jedno pole w prawo lub lewo i w górę (lub w dół) jest dozwolony.");
                    return true;
                }
            }

            // Sprawdź, czy pionek jest damką
            if (piece.IsKing)
            {
                // Sprawdź, czy ruch jest o jedno pole w prawo lub lewo i w dół (lub w górę)
                if (rowDiff == 1 && colDiff == 1)
                {
                    Console.WriteLine("Ruch o jedno pole w prawo lub lewo i w dół (lub w górę) jest dozwolony dla damki.");
                    return true;
                }
            }

            Console.WriteLine("Ten ruch nie jest dozwolony.");
            return false;
        }

        public bool IsValidJump(int fromRow, int fromCol, int toRow, int toCol)
        {
            Console.WriteLine("IsValidJump called");
            CheckerPiece piece = pieces[fromRow, fromCol];

            if (piece == null)
            {
                Console.WriteLine("Pole jest puste, nie można wykonać skoku.");
                return false;
            }

            // Sprawdź, czy docelowa komórka jest pusta
            if (pieces[toRow, toCol] != null)
            {
                Console.WriteLine("Docelowa komórka jest zajęta, nie można wykonać skoku.");
                return false;
            }

            // Sprawdź, czy pole, na którym jest skakane, zawiera pionka przeciwnika
            int jumpedRow = (fromRow + toRow) / 2;
            int jumpedCol = (fromCol + toCol) / 2;
            CheckerPiece jumpedPiece = pieces[jumpedRow, jumpedCol];

            if (jumpedPiece == null || jumpedPiece.PieceColor == piece.PieceColor)
            {
                Console.WriteLine("Nie ma pionka przeciwnika na środkowym polu lub jest to twój własny pionek, nie można wykonać skoku.");
                return false;
            }

            // Sprawdź, czy skok jest o dwa pola w prawo lub lewo i w górę (lub w dół) (dla gracza 1) lub o dwa pola w prawo lub lewo i w dół (lub w górę) (dla gracza 2)
            int rowDiff = Math.Abs(toRow - fromRow);
            int colDiff = Math.Abs(toCol - fromCol);

            if (rowDiff == 2 && colDiff == 2)
            {
                if ((piece.PieceColor == Color.White && toRow < fromRow) ||
                    (piece.PieceColor == Color.Red && toRow > fromRow))
                {
                    Console.WriteLine("Skok o dwa pola w prawo lub lewo i w górę (lub w dół) jest dozwolony.");
                    return true;
                }
            }

            // Sprawdź, czy pionek jest damką
            if (piece.IsKing)
            {
                // Sprawdź, czy skok jest o dwa pola w prawo lub lewo i w dół (lub w górę)
                if (rowDiff == 2 && colDiff == 2)
                {
                    Console.WriteLine("Skok o dwa pola w prawo lub lewo i w dół (lub w górę) jest dozwolony dla damki.");
                    return true;
                }
            }

            Console.WriteLine("Ten skok nie jest dozwolony.");
            return false;
        }





        public Color GetCellColor(int row, int col)
        {
            return (row + col) % 2 == 0 ? Color.White : Color.Black;
        }

        public CheckerPiece PieceAt(int row, int col)
        {
            return pieces[row, col];
        }
        public void RemovePiece(int row, int col)
        {
            pieces[row, col] = null;
        }




    }
}