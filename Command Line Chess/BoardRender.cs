using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Command_Line_Chess
{
    static class BoardRender
    {
        static List<string> chessPiecesB = new List<string>(new string[] { " ♖", " ♘", " ♗", " ♕", " ♔", " ♙" });
        static List<string> chessPiecesW = new List<string>(new string[] { " ♜", " ♞", " ♝", " ♛", " ♚", " ♟" });

        public static void PrintEmptyBoard()
        {
            Console.Clear();
            String backRowB = "8 " + chessPiecesB[0] + chessPiecesB[1] + chessPiecesB[2] + chessPiecesB[3] + chessPiecesB[4] +
                              chessPiecesB[2] + chessPiecesB[1] + chessPiecesB[0];
            String backRowW = "1 " + chessPiecesW[0] + chessPiecesW[1] + chessPiecesW[2] + chessPiecesW[3] + chessPiecesW[4] +
                              chessPiecesW[2] + chessPiecesW[1] + chessPiecesW[0];

            //Print the column headers
            Console.Write("  ");
            for (int i = 0; i < 8; i++) {
                string columnHeader = " " + Convert.ToChar(97 + i);
                Console.Write(columnHeader);
            }
            Console.WriteLine(); Console.WriteLine();

            //Print the top row (black back row)
            Console.WriteLine(backRowB);

            //Print all the middle rows of the board
            bool isBlackSpace = false;
            for (int i = 7; i > 1; i--)
            {
                Console.Write(i + " ");
                for (int j = 0; j < 8; j++)
                {
                    if (i == 7) { Console.Write(chessPiecesB[5]); }
                    else if (i == 2) { Console.Write(chessPiecesW[5]); }
                    else
                    {
                        if (isBlackSpace) { Console.Write(" \x25AE"); }
                        else { Console.Write(" \x25AF"); }
                    }
                    //Switch square colour
                    if (j != 7) isBlackSpace = !isBlackSpace;
                }           
                Console.WriteLine();
            }

            //Print the bottom row (white back row
            Console.WriteLine(backRowW);
        }

        public static void PrintBoardFromDict(Dictionary<string, AbstractChessPiece> piecePositions)
        {
            //Start with clear
            Console.Clear();

            //Print the column headers
            Console.Write("  ");
            for (int i = 0; i < 8; i++)
            {
                string columnHeader = " " + Convert.ToChar(97 + i);
                Console.Write(columnHeader);
            }
            Console.WriteLine(); Console.WriteLine();

            //Iterative approach, row wise
            bool isBlackSpace = false;
            for (int row = 0; row < 8; row++)
            {
                string rowString = (8 - row) + " ";
                for (int col = 0; col < 8; col++)
                {
                    string position = GridReferenceToChessPosition(col, row);

                    if (piecePositions.ContainsKey(position)) // Draw chess piece
                    {
                        AbstractChessPiece chessPiece = piecePositions[position];
                        rowString += chessPiece.symbol;
                    }
                    else // Draw board grid space
                    {
                        rowString += isBlackSpace ? " \x25AF" : " \x25AE";
                    }

                    //Switch square colour
                    if (col != 7) isBlackSpace = !isBlackSpace;
                }
                Console.WriteLine(rowString);
            }
            Console.WriteLine();
        }

        public static Tuple<int, int> ChessPositionToGridReference(string position)
        {
            Tuple<int, int> gridReference;

            char[] splitPosition;
            if (position.Length == 2)
            {
                splitPosition = position.ToCharArray();
            }
            else return gridReference = new Tuple<int, int>(-1, -1);

            //Convert Column
            int x = Convert.ToInt32(splitPosition[0]) - 97;

            //Convert Row
            int y = 56 - Convert.ToInt32(splitPosition[1]);

            gridReference = new Tuple<int, int>(x, y);
            return gridReference;
        }

        public static string GridReferenceToChessPosition(int x, int y)
        {
            string chessPosition = "";

            // Convert Column
            string xConverted = Convert.ToString(Convert.ToChar(x + 97));

            //Convert Row
            string yConverted = Convert.ToString(8 - y);

            chessPosition = xConverted + yConverted;
            return chessPosition;
        }
    }
}
