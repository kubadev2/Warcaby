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

        public Color CurrentPlayerColor => currentPlayerColor;

        public Board()
        {
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
                            pieces[row, col] = new CheckerPiece(Color.White, row, col);
                        }
                        else if (row > 4)
                        {
                            pieces[row, col] = new CheckerPiece(Color.Red, row, col);
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
                int forwardRow = piece.Row + forwardDirection;
                int leftCol = piece.Col - 1;
                int rightCol = piece.Col + 1;

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
            pieces[fromRow, fromCol] = null;
            pieces[toRow, toCol] = piece;

            // Dodaj logikę przeskakiwania pionka, jeśli jest bicie
            // ...

            // Dodaj logikę awansowania pionka na damkę, jeśli dotrze do końca planszy
            // ...
        }

        public bool IsValidMove(int fromRow, int fromCol, int toRow, int toCol)
        {
            Console.WriteLine("IsValidMove called");
            // Implementuj logikę sprawdzania poprawności ruchu
            // ...

            return true;
        }

        public Color GetCellColor(int row, int col)
        {
            Console.WriteLine("GetCellColor called");
            return (row + col) % 2 == 0 ? Color.White : Color.Black;
        }

        public CheckerPiece PieceAt(int row, int col)
        {
            Console.WriteLine("PieceAt called");
            return pieces[row, col];
        }

        internal void SwitchPlayer()
        {
            Console.WriteLine("SwitchPlayer called");
            // Implementuj logikę zmiany gracza
            // ...
        }

        // Inne metody związane z ruchami i mechaniką gry
        // ...
    }
}