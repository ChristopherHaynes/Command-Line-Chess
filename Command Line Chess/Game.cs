using System;
using System.Collections.Generic;
using System.Text;

namespace Command_Line_Chess
{
    class Game
    {
        public static Dictionary<string, AbstractChessPiece> piecePositions;
        King whiteKing;
        King blackKing;
        public bool isWhiteTurn;
        public bool isCheck;
        public bool isCheckMate;

        public Game()
        {
            isWhiteTurn = true;
            isCheck = false;
            isCheckMate = false;
            piecePositions = new Dictionary<string, AbstractChessPiece>();

            InitialiseBoard();
            whiteKing = (King)piecePositions["e1"];
            blackKing = (King)piecePositions["e8"];

            GameLoop();
        }

        private void GameLoop()
        {
            // A "Turn"
            while (true)
            {
                // Update all the possible moves for each piece
                UpdateAllPiecePossibleMoves();

                //Check and Checkmate conditions for winning the game
                if (isWhiteTurn) { isCheck = whiteKing.IsInCheck(); }
                else { isCheck = blackKing.IsInCheck(); }

                if (isWhiteTurn && isCheck) { isCheckMate = whiteKing.IsInCheckmate(); }
                else if (!isWhiteTurn && isCheck) { isCheckMate = blackKing.IsInCheckmate(); }

                //DEBUG: Testing checkmate condition
                if (isCheckMate) break;

                // Redraw the board and await user input
                BoardRender.PrintBoardFromDict(piecePositions);
                if (isWhiteTurn) { Console.WriteLine("***** WHITE TURN *****"); }
                else { Console.WriteLine("***** BLACK TURN *****"); }
                if (isCheck) { Console.WriteLine("You are in Check!"); }
                Console.Write("Please enter your move: ");

                // Check the validity of the entered move
                string userMove;               
                while (!IsValidUserInput(userMove = Console.ReadLine().Trim().ToLower()))
                {
                    if (isWhiteTurn) { Console.WriteLine("***** WHITE TURN *****"); }
                    else { Console.WriteLine("***** BLACK TURN *****"); }
                    Console.Write("Please enter your move: ");
                }

                // Complete the move
                string startPosition = "" + userMove[0] + userMove[1];
                string endPosition = "" + userMove[2] + userMove[3];
                piecePositions[startPosition].Move(endPosition);

                // Flip the turn to the other player
                isWhiteTurn = !isWhiteTurn;

                //OPTION? - Wait for input before starting next turn (displays description text)
                PressToContinue();
            }
            //DEBUG: Testing game flow options
            string colour = isWhiteTurn ? "White" : "Black";
            Console.WriteLine("CHECKMATE! - " + colour + "'s King is Checkmated!");
            Console.WriteLine("Press any key for a new game!");
            Console.ReadKey();
            Game newGame = new Game();
        }

        private bool IsValidUserInput(string move)
        {
            // Ensure move is exactly 4 chars long
            if (move.Length == 4)
            {
                char[] splitMove = move.ToCharArray();
                // Check the first and third chars are alphabetic and in the range a-h (inclusive)
                if (Convert.ToInt32(splitMove[0]) < 105 && Convert.ToInt32(splitMove[0]) > 96 &&
                    Convert.ToInt32(splitMove[2]) < 105 && Convert.ToInt32(splitMove[2]) > 96)
                {
                    // Check the second and third characters are numeric and in the range 1-8 (inclusive)
                    if (Convert.ToInt32(splitMove[1]) - 48 < 9 && Convert.ToInt32(splitMove[1]) - 48 > 0 &&
                        Convert.ToInt32(splitMove[3]) - 48 < 9 && Convert.ToInt32(splitMove[3]) - 48 > 0)
                    {
                        return ValidatePositions(move);
                    }
                }
            }
            else if (move.Length == 3 && move[0].Equals('m'))
            {
                PrintPiecePossibleMoves("" + move[1] + move[2]);
                PressToContinue();
                return false;
            }
            else if (move.Length == 3 && move[0].Equals('s'))
            {
                PrintCheckState("" + move[1] + move[2]);
                PressToContinue();
                return false;
            }
            Console.WriteLine("INVALID FORMAT - please use \"b7b6\"");
            PressToContinue();
            return false;
        }

        private bool ValidatePositions(string move)
        {
            string startPosition = "" + move[0] + move[1];
            string endPosition = "" + move[2] + move[3];
            bool checkResult;
            if (!DoesPiecePositionMatchPlayerColour(startPosition))
            {
                Console.WriteLine("INVALID START POSITION - please select a start position containing one of your pieces");
                PressToContinue();
                return false;
            }
            if (!IsPossibleMove(startPosition, endPosition))
            {
                Console.WriteLine("INVALID MOVE POSITION - please select a valid move position");
                PressToContinue();
                return false;
            }
            // If start and end positions are valid, ensure the move does not put the current player into check          
            SimulatePieceMove(startPosition, endPosition, piecePositions[startPosition]);
            UpdateAllPiecePossibleMoves();

            if (isWhiteTurn) { checkResult = whiteKing.IsInCheck(); }
            else { checkResult = blackKing.IsInCheck(); }

            SimulatePieceMove(endPosition, startPosition, piecePositions[endPosition]);
            if (piecePositions.ContainsKey("temp"))
            {
                AbstractChessPiece removedPiece = piecePositions["temp"];
                removedPiece.position = endPosition;
                piecePositions.Remove("temp");
                piecePositions.Add(endPosition, removedPiece);
            }
            UpdateAllPiecePossibleMoves();
            if (checkResult)
            {
                Console.WriteLine("INVALID MOVE - This would place your King into check");
                PressToContinue();
                return false;
            }
            return true;
        }

        private bool IsPossibleMove(string piecePosition, string movePosition)
        {
            AbstractChessPiece piece = piecePositions[piecePosition];
            if (piece.possibleMoves.Contains(movePosition))
            {
                return true;
            }
            return false;
        }

        private bool DoesPiecePositionMatchPlayerColour(string position)
        {
            if (piecePositions.ContainsKey(position))
            {
                if ((isWhiteTurn && piecePositions[position].colour.Equals("w")) ||
                    (!isWhiteTurn && piecePositions[position].colour.Equals("b")))
                {
                    return true;
                }
            }
            return false;
        }

        public static void UpdateAllPiecePossibleMoves()
        {
            foreach(KeyValuePair<string, AbstractChessPiece> piece in piecePositions)
            {
                piece.Value.FindPossibleMoves(piecePositions);
            }
        }

        private void SimulatePieceMove(string startPosition, string endPosition, AbstractChessPiece piece)
        {
            piecePositions.Remove(startPosition);
            piece.position = endPosition;
            if (piecePositions.ContainsKey(endPosition)) //DIRTY FIX for when there is already a piece in the position being simulated
            {
                AbstractChessPiece removedPiece = piecePositions[endPosition];
                removedPiece.position = "??";
                piecePositions.Remove(endPosition);
                piecePositions.Add("temp", removedPiece);
            }
            piecePositions.Add(endPosition, piece);
        }

        private void InitialiseBoard()
        {
            // Create all the pieces for both players and add them to the Dict
            // Variable naming style = <type><colour><position>
            // Back row black
            AbstractChessPiece rookBL = new Rook("b", "a8"); piecePositions.Add(rookBL.position, rookBL);
            AbstractChessPiece knightBL = new Knight("b", "b8"); piecePositions.Add(knightBL.position, knightBL);
            AbstractChessPiece bishopBL = new Bishop("b", "c8"); piecePositions.Add(bishopBL.position, bishopBL);
            AbstractChessPiece queenB = new Queen("b", "d8"); piecePositions.Add(queenB.position, queenB);
            AbstractChessPiece kingB = new King("b", "e8"); piecePositions.Add(kingB.position, kingB);
            AbstractChessPiece bishopBR = new Bishop("b", "f8"); piecePositions.Add(bishopBR.position, bishopBR);
            AbstractChessPiece knightBR = new Knight("b", "g8"); piecePositions.Add(knightBR.position, knightBR);
            AbstractChessPiece rookBR = new Rook("b", "h8"); piecePositions.Add(rookBR.position, rookBR);
            // Front row black
            AbstractChessPiece pawnB1 = new Pawn("b", "a7"); piecePositions.Add(pawnB1.position, pawnB1);
            AbstractChessPiece pawnB2 = new Pawn("b", "b7"); piecePositions.Add(pawnB2.position, pawnB2);
            AbstractChessPiece pawnB3 = new Pawn("b", "c7"); piecePositions.Add(pawnB3.position, pawnB3);
            AbstractChessPiece pawnB4 = new Pawn("b", "d7"); piecePositions.Add(pawnB4.position, pawnB4);
            AbstractChessPiece pawnB5 = new Pawn("b", "e7"); piecePositions.Add(pawnB5.position, pawnB5);
            AbstractChessPiece pawnB6 = new Pawn("b", "f7"); piecePositions.Add(pawnB6.position, pawnB6);
            AbstractChessPiece pawnB7 = new Pawn("b", "g7"); piecePositions.Add(pawnB7.position, pawnB7);
            AbstractChessPiece pawnB8 = new Pawn("b", "h7"); piecePositions.Add(pawnB8.position, pawnB8);
            // Front row white
            AbstractChessPiece pawnW1 = new Pawn("w", "a2"); piecePositions.Add(pawnW1.position, pawnW1);
            AbstractChessPiece pawnW2 = new Pawn("w", "b2"); piecePositions.Add(pawnW2.position, pawnW2);
            AbstractChessPiece pawnW3 = new Pawn("w", "c2"); piecePositions.Add(pawnW3.position, pawnW3);
            AbstractChessPiece pawnW4 = new Pawn("w", "d2"); piecePositions.Add(pawnW4.position, pawnW4);
            AbstractChessPiece pawnW5 = new Pawn("w", "e2"); piecePositions.Add(pawnW5.position, pawnW5);
            AbstractChessPiece pawnW6 = new Pawn("w", "f2"); piecePositions.Add(pawnW6.position, pawnW6);
            AbstractChessPiece pawnW7 = new Pawn("w", "g2"); piecePositions.Add(pawnW7.position, pawnW7);
            AbstractChessPiece pawnW8 = new Pawn("w", "h2"); piecePositions.Add(pawnW8.position, pawnW8);
            // Back row white
            AbstractChessPiece rookWL = new Rook("w", "a1"); piecePositions.Add(rookWL.position, rookWL);
            AbstractChessPiece knightWL = new Knight("w", "b1"); piecePositions.Add(knightWL.position, knightWL);
            AbstractChessPiece bishopWL = new Bishop("w", "c1"); piecePositions.Add(bishopWL.position, bishopWL);
            AbstractChessPiece queenW = new Queen("w", "d1"); piecePositions.Add(queenW.position, queenW);
            AbstractChessPiece kingW = new King("w", "e1"); piecePositions.Add(kingW.position, kingW);
            AbstractChessPiece bishopWR = new Bishop("w", "f1"); piecePositions.Add(bishopWR.position, bishopWR);
            AbstractChessPiece knightWR = new Knight("w", "g1"); piecePositions.Add(knightWR.position, knightWR);
            AbstractChessPiece rookWR = new Rook("w", "h1"); piecePositions.Add(rookWR.position, rookWR);
        }

        private void PressToContinue()
        {
            Console.Write("\nPress any key to continue...");
            Console.ReadKey();
            BoardRender.PrintBoardFromDict(piecePositions);
        }

        //DEBUG - Used for testing rules and debugging purposes
        private void PrintPiecePossibleMoves(string piecePosition)
        {
            // Check if valid position
            if (!piecePositions.ContainsKey(piecePosition))
            {
                Console.WriteLine("ERROR: No piece found at " + piecePosition);
                return;
            }

            AbstractChessPiece piece = piecePositions[piecePosition];
            Console.WriteLine("Possible Moves for " + piece.colour + " " + piece.name + ":");

            foreach (string move in piece.possibleMoves)
            {
                Console.WriteLine(move);
            }
        }

        private void PrintCheckState(string piecePosition)
        {
            // Check if valid position
            if (!piecePositions.ContainsKey(piecePosition))
            {
                Console.WriteLine("ERROR: No piece found at " + piecePosition);
                return;
            }
            // Check if the piece is a "King"
            if (!piecePositions[piecePosition].name.Equals("King"))
            {
                Console.WriteLine("ERROR: Piece is not a King");
                return;
            }

            King king = (King)piecePositions[piecePosition];
            bool check = king.IsInCheck();
            bool checkmate = king.IsInCheckmate();
            Console.WriteLine("The " + king.colour + " King is in Check = " + check);
            Console.WriteLine("The " + king.colour + " King is in Checkmate = " + checkmate);
        }
    }
}
