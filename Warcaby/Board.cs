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

            // Usuń pionek przeskakiwanego
            if (Math.Abs(toRow - fromRow) == 2)
            {
                int jumpedRow = (fromRow + toRow) / 2;
                int jumpedCol = (fromCol + toCol) / 2;
                CheckerPiece jumpedPiece = pieces[jumpedRow, jumpedCol];
                if (jumpedPiece != null)
                {
                    pieces[jumpedRow, jumpedCol] = null;
                    jumpedPiece.Dispose(); // Usuń pionka z planszy
                    Panel jumpedCell = gameForm.GetCellByPosition(jumpedCol, jumpedRow);
                    jumpedCell.Controls.Remove(jumpedPiece);
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






        // Zaktualizuj funkcję IsValidMove w klasie Board
        public bool IsValidMove(int fromRow, int fromCol, int toRow, int toCol)
        {
            Console.WriteLine("IsValidMove called");

            // Sprawdź, czy pole docelowe jest poza planszą
            if (toRow < 0 || toRow >= 8 || toCol < 0 || toCol >= 8)
            {
                Console.WriteLine("1");
                return false;
            }

            CheckerPiece piece = pieces[fromRow, fromCol];
            if (piece == null)
            {
                Console.WriteLine("Pole jest puste, nie można przesunąć pionka.");
                return false;
            }

            CheckerPiece targetPiece = pieces[toRow, toCol];

            // Sprawdź, czy pole docelowe jest zajęte przez inny pionek
            if (targetPiece != null)
            {
                Console.WriteLine("2");
                return false;
            }

            // Oblicz różnice w wierszach i kolumnach
            int rowDiff = Math.Abs(toRow - fromRow);
            int colDiff = Math.Abs(toCol - fromCol);

            // Sprawdź, czy ruch jest o jedno pole do przodu
            if (rowDiff == 1 && colDiff == 1)
            {
                // Sprawdź, czy pionek jest królem (może ruszać się w przód i w tył)
                if (!piece.IsKing)
                {
                    // Jeśli nie jest królem, to tylko w przód
                    forwardDirection = (piece.PieceColor == Color.White) ? -1 : 1;
                    if (toRow != fromRow + forwardDirection)
                    {
                        Console.WriteLine("4");
                        return false;
                    }
                }
            }
            // Sprawdź, czy ruch jest skokiem (biciem)
            else if (rowDiff == 2 && colDiff == 2)
            {
                // Sprawdź, czy pole między polem początkowym a docelowym jest zajęte przez przeciwnika
                int jumpedRow = (fromRow + toRow) / 2;
                int jumpedCol = (fromCol + toCol) / 2;
                CheckerPiece jumpedPiece = pieces[jumpedRow, jumpedCol];

                if (jumpedPiece == null || jumpedPiece.PieceColor == piece.PieceColor)
                {
                    Console.WriteLine("5");
                    return false;
                }
                else
                {
                    // Jeśli to jest pierwszy skok w sekwencji, zapisz pozycję startową skoku
                    if (!isJumpInProgress)
                    {
                        isJumpInProgress = true;
                        jumpStartRow = fromRow;
                        jumpStartCol = fromCol;
                    }
                }
            }
            else
            {
                //Console.WriteLine("6");
                return false; // Nieprawidłowy ruch
            }

            // Jeśli jesteśmy w trakcie skoku, sprawdź, czy ruch jest kontynuacją tego skoku
            if (isJumpInProgress)
            {
                if (fromRow != jumpStartRow || fromCol != jumpStartCol)
                {
                    Console.WriteLine("7");
                    return false; // Musisz kontynuować ten sam skok
                }
            }

            // Sprawdź, czy pole początkowe zawiera pionka
            if (pieces[fromRow, fromCol] == null)
            {
                Console.WriteLine("8");
                return false;
            }

            // Dodaj warunek, który ograniczy ruch tylko do przodu lub do tyłu
            int forwardRow = fromRow + forwardDirection;

            if (toRow != forwardRow)
            {
                Console.WriteLine("9");
                return false; // Ruch w prawo lub w lewo nie jest dozwolony
            }

            // Dodaj warunek, który ograniczy ruch tylko do przodu
            if (forwardDirection == -1 && toRow != fromRow - 1)
            {
                Console.WriteLine("10");
                return false; // Ruch w prawo lub w lewo nie jest dozwolony
            }

            return true;
        }



        public bool IsValidJump(int fromRow, int fromCol, int toRow, int toCol)
        {
            Console.WriteLine("is valid jump");
            int jumpedRow = (fromRow + toRow) / 2;
            int jumpedCol = (fromCol + toCol) / 2;

            // Sprawdź, czy pole docelowe jest puste i znajduje się dokładnie o 2 pola po przekątnej od pozycji startowej
            if (PieceAt(toRow, toCol) == null && Math.Abs(toRow - fromRow) == 2 && Math.Abs(toCol - fromCol) == 2)
            {
                Console.WriteLine("jump");
                // Sprawdź, czy na polu przeskakiwanym znajduje się pionek przeciwnika
                CheckerPiece jumpedPiece = PieceAt(jumpedRow, jumpedCol);
                if (jumpedPiece != null && jumpedPiece.PieceColor != PieceAt(fromRow, fromCol).PieceColor)
                {
                    Console.WriteLine("jump jump");
                    return true; // Skok jest dozwolony
                }
            }

            return false; // Skok jest niedozwolony
        }



        public Color GetCellColor(int row, int col)
        {
            return (row + col) % 2 == 0 ? Color.White : Color.Black;
        }

        public CheckerPiece PieceAt(int row, int col)
        {
            return pieces[row, col];
        }



    }
}