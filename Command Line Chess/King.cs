using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Command_Line_Chess
{
    class King : AbstractChessPiece
    {
        public King(string startColour, string startPosition)
        {
            this.name = "King";
            this.colour = startColour.ToLower();
            if (colour == "b") { this.symbol = " ♔"; }
            else if (colour == "w") { this.symbol = " ♚"; }
            this.position = startPosition;
            this.possibleMoves = new List<string>();
        }

        public override void FindPossibleMoves(Dictionary<string, AbstractChessPiece> piecePositions)
        {
            List<string> moves = new List<string>();

            Tuple<int, int> gridReference = BoardRender.ChessPositionToGridReference(this.position);

            for (int x = gridReference.Item1 - 1; x < gridReference.Item1 + 2; x++)
            {
                for (int y = gridReference.Item2 - 1; y < gridReference.Item2 + 2; y++)
                {
                    // Ensure the positions checked are always in bounds of the board
                    if (x < 0 || x > 7 || y < 0 || y > 7 || (x == gridReference.Item1 && y == gridReference.Item2))
                    {
                        continue;
                    }
                    // Check if there is already a piece on a valid position
                    if (piecePositions.ContainsKey(BoardRender.GridReferenceToChessPosition(x, y)))
                    {
                        // Ignore that position if the piece is the same colour as the current piece
                        if (piecePositions[BoardRender.GridReferenceToChessPosition(x, y)].colour.Equals(this.colour))
                        {
                            continue;
                        }
                    }
                    moves.Add(BoardRender.GridReferenceToChessPosition(x, y));
                }
            }
            this.possibleMoves = moves;
        }

        public bool IsInCheck()
        {
            // Create a list of all pieces for better iteration 
            List<AbstractChessPiece> piecesList = new List<AbstractChessPiece>();
            piecesList.AddRange(Game.piecePositions.Values);

            // Collect all possible moves from all pieces of the opposite colour
            List<string> oppositionAllPossibleMoves = new List<string>();
            foreach (AbstractChessPiece piece in piecesList)
            {
                if (!piece.colour.Equals(this.colour))
                {
                    oppositionAllPossibleMoves.AddRange(piece.possibleMoves);
                }
            }

            // If any of the possible moves of the opposition match this position, check is true
            foreach (string move in oppositionAllPossibleMoves)
            {
                if (move.Equals(this.position))
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsInCheckmate()
        {
            // Keep copy of the original piece positions to return the original state after simulation           
            Dictionary<string, AbstractChessPiece> originalPiecePositions = Game.piecePositions.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);         

            // Collect all the pieces which are the same colour as the king
            List<AbstractChessPiece> sameColourPieces = new List<AbstractChessPiece>();
            foreach (KeyValuePair<string, AbstractChessPiece> piece in Game.piecePositions)
            {
                if (piece.Value.colour.Equals(this.colour))
                {
                    sameColourPieces.Add(piece.Value);
                }
            }

            // Simulate the board for each of the same colour pieces possible next moves
            bool isCheckmate = true;
            foreach (AbstractChessPiece piece in sameColourPieces)
            {               
                string originalPosition = piece.position;
                foreach (string position in piece.possibleMoves)
                {
                    Game.piecePositions.Remove(piece.position);
                    piece.position = position;
                    if (Game.piecePositions.ContainsKey(position))
                    {
                        Game.piecePositions.Remove(position);
                    }
                    Game.piecePositions.Add(position, piece);
                    Game.UpdateAllPiecePossibleMoves();
                    isCheckmate = isCheckmate && IsInCheck();
                    // Return piece to original position after simulation
                    piece.position = originalPosition;
                    Game.piecePositions = originalPiecePositions.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
                    Game.UpdateAllPiecePossibleMoves();
                }               
            }
            return isCheckmate;
        }
    }
}
