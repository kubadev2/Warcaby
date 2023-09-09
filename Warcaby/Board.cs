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
        private Color defaultCellColor = Color.Empty;
        private GameForm gameForm;
        private int forwardDirection;

        public Color CurrentPlayerColor => currentPlayerColor;

        public Board(GameForm gameForm)
        {
            this.gameForm = gameForm;
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
                            pieces[row, col] = new CheckerPiece(Color.Red, row, col);
                        }
                        else if (row > 4)
                        {
                            pieces[row, col] = new CheckerPiece(Color.White, row, col);
                        }
                    }
                }
            }
        }

        public void MovePiece(int fromRow, int fromCol, int toRow, int toCol)
        {
            CheckerPiece piece = pieces[fromRow, fromCol];
            if (piece.IsKing)
            { 
                MoveKing(fromRow, fromCol, toRow, toCol);
            }
            else
            {
                // Usuń istniejący pionek ze starego miejsca
                pieces[fromRow, fromCol] = null;
                Panel fromCell = gameForm.GetCellByPosition(fromCol, fromRow);
                fromCell.Controls.Remove(piece);
                fromCell.Controls.Clear();

                // Stwórz nowy pionek w nowym miejscu
                pieces[toRow, toCol] = piece;
                CheckerPiece newPiece = pieces[toRow, toCol];
                Panel toCell = gameForm.GetCellByPosition(toCol, toRow);
                toCell.Controls.Add(newPiece);
                //newPiece.Location = new Point(toCol * 60, toRow * 60);
                newPiece.Dock = DockStyle.Fill;
                newPiece.Row = toRow; // Aktualizacja pozycji pionka
                newPiece.Col = toCol;


                // Usuń pionek przeskakiwany
                if (Math.Abs(toRow - fromRow) == 2)
                {
                    int jumpedRow = (fromRow + toRow) / 2;
                    int jumpedCol = (fromCol + toCol) / 2;
                    Console.WriteLine("przeskoczony pionek: " + jumpedRow + " " + jumpedCol);
                    CheckerPiece jumpedPiece = PieceAt(jumpedRow, jumpedCol);
                    pieces[jumpedRow, jumpedCol] = null;
                    Panel jumpedCell = gameForm.GetCellByPosition(jumpedCol, jumpedRow);
                    jumpedCell.Controls.Remove(jumpedPiece);
                    jumpedCell.Controls.Clear();
                    //jumpedPiece.Dispose();
                }

                // Awansuj pionek na damę, jeśli dotrze do końca planszy
                if ((toRow == 0 && newPiece.PieceColor == Color.White) || (toRow == 7 && newPiece.PieceColor == Color.Red))
                {
                    newPiece.IsKing = true;
                    newPiece.BackColor = Color.Gold;
                }

                // Odznacz wybrany pionek po wykonaniu ruchu
                newPiece.BackColor = Color.Black;

                // Zakończ bieżący skok (jeśli taki istnieje)
                isJumpInProgress = false;
                // Przełącz gracza
                gameForm.SwitchPlayer();
            }
        }
        public void MoveKing(int fromRow, int fromCol, int toRow, int toCol)
        {
            CheckerPiece piece = pieces[fromRow, fromCol];

            if (piece == null)
            {
                Console.WriteLine("Pole jest puste, nie można przesunąć pionka.");
                return;
            }

            if (pieces[toRow, toCol] != null)
            {
                Console.WriteLine("Docelowa komórka jest zajęta, nie można przesunąć pionka.");
                return;
            }

            if (!IsValidMoveKing(fromRow, fromCol, toRow, toCol))
            {
                Console.WriteLine("Ten ruch nie jest dozwolony.");
                return;
            }

            // Usuń istniejący pionek ze starego miejsca
            pieces[fromRow, fromCol] = null;
            Panel fromCell = gameForm.GetCellByPosition(fromCol, fromRow);
            fromCell.Controls.Remove(piece);
            fromCell.Controls.Clear();

            // Stwórz nowy pionek w nowym miejscu
            pieces[toRow, toCol] = piece;
            CheckerPiece newPiece = pieces[toRow, toCol];
            Panel toCell = gameForm.GetCellByPosition(toCol, toRow);
            toCell.Controls.Add(newPiece);
            newPiece.Dock = DockStyle.Fill;
            newPiece.Row = toRow;
            newPiece.Col = toCol;

            // Odznacz wybrany pionek po wykonaniu ruchu
            newPiece.BackColor = Color.Black;

            // Przełącz gracza
            gameForm.SwitchPlayer();
        }



        public bool IsValidMove(int fromRow, int fromCol, int toRow, int toCol)
        {
            CheckerPiece piece = pieces[fromRow, fromCol];

            if (piece == null)
            {
                Console.WriteLine("Pole jest puste, nie można przesunąć pionka.");
                return false;
            }

            if (pieces[toRow, toCol] != null)
            {
                Console.WriteLine("Docelowa komórka jest zajęta, nie można przesunąć pionka.");
                return false;
            }
            if (piece.IsKing)
            {
                return IsValidMoveKing(fromRow, fromCol, toRow, toCol);
            }

            int rowDiff = Math.Abs(toRow - fromRow);
            int colDiff = Math.Abs(toCol - fromCol);

          
                // Sprawdź, czy ruch jest dozwolony dla pionka (nie damy)
                if (rowDiff == 1 && colDiff == 1)
                {
                    if ((piece.PieceColor == Color.White && toRow < fromRow) ||
                        (piece.PieceColor == Color.Red && toRow > fromRow))
                    {
                        Console.WriteLine("Ruch o jedno pole w prawo lub lewo i w górę (lub w dół) jest dozwolony.");
                        return true;
                    }
                }
           

            Console.WriteLine("Ten ruch nie jest dozwolony.");
            return false;
        }

        public bool IsValidJump(int fromRow, int fromCol, int toRow, int toCol)
        {
            CheckerPiece piece = pieces[fromRow, fromCol];

            if (piece == null)
            {
                Console.WriteLine("Pole jest puste, nie można wykonać skoku.");
                return false;
            }

            if (pieces[toRow, toCol] != null)
            {
                Console.WriteLine("Docelowa komórka jest zajęta, nie można wykonać skoku.");
                return false;
            }
            if(piece.IsKing)
            {
                return IsValidJumpKing(fromRow, fromCol, toRow, toCol);
            }    

            int rowDiff = Math.Abs(toRow - fromRow);
            int colDiff = Math.Abs(toCol - fromCol);

            
                // Sprawdź, czy skok jest dozwolony dla pionka (nie damy)
                if (rowDiff == 2 && colDiff == 2)
                {
                    int jumpedRow = (fromRow + toRow) / 2;
                    int jumpedCol = (fromCol + toCol) / 2;
                    CheckerPiece jumpedPiece = pieces[jumpedRow, jumpedCol];

                    if (jumpedPiece != null && jumpedPiece.PieceColor != piece.PieceColor)
                    {
                        Console.WriteLine("Skok o dwa pola w prawo lub lewo i w górę (lub w dół) jest dozwolony.");
                        return true;
                    }
                }

            Console.WriteLine("Ten skok nie jest dozwolony.");
            return false;
        }
        public bool IsValidMoveKing(int fromRow, int fromCol, int toRow, int toCol)
        {
            CheckerPiece piece = pieces[fromRow, fromCol];

            if (piece == null)
            {
                Console.WriteLine("Pole jest puste, nie można przesunąć pionka.");
                return false;
            }

            if (pieces[toRow, toCol] != null)
            {
                Console.WriteLine("Docelowa komórka jest zajęta, nie można przesunąć pionka.");
                return false;
            }

            int rowDiff = Math.Abs(toRow - fromRow);
            int colDiff = Math.Abs(toCol - fromCol);

            // Sprawdź, czy ruch jest po przekątnej
            if (rowDiff == colDiff)
            {
                // Sprawdź, czy ruch jest dozwolony (bez przeskakiwania)
                int rowDirection = Math.Sign(toRow - fromRow);
                int colDirection = Math.Sign(toCol - fromCol);

                int currentRow = fromRow + rowDirection;
                int currentCol = fromCol + colDirection;

                while (currentRow != toRow && currentCol != toCol)
                {
                    CheckerPiece currentPiece = pieces[currentRow, currentCol];
                    if (currentPiece != null)
                    {
                        // Na drodze ruchu stoją inne pionki
                        Console.WriteLine("Nie można przeskakiwać swoich pionków.");
                        return false;
                    }

                    currentRow += rowDirection;
                    currentCol += colDirection;
                }

                Console.WriteLine("Ruch damy po przekątnej jest dozwolony.");
                return true;
            }

            Console.WriteLine("Ten ruch nie jest dozwolony.");
            return false;
        }

       public bool IsValidJumpKing(int fromRow, int fromCol, int toRow, int toCol)
        {
            return false;
        }





        public bool IsInBoard(int jumpRow, int jumpCol)
        {
            if(jumpRow >=0 && jumpRow <=7)
                if(jumpCol >=0 && jumpCol <=7)
                    return true;
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

        public void AddPiece(int row, int col, Color pieceColor)
        {
            pieces[row, col] = new CheckerPiece(pieceColor, row, col);
        }



    }
}