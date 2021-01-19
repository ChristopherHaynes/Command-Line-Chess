using System;
using System.Collections.Generic;
using System.Text;

namespace Command_Line_Chess
{
    class Knight : AbstractChessPiece
    {
        public Knight(string startColour, string startPosition)
        {
            this.name = "Knight";
            this.colour = startColour.ToLower();
            if (colour == "b") { this.symbol = " ♘"; }
            else if (colour == "w") { this.symbol = " ♞"; }
            this.position = startPosition;
            this.possibleMoves = new List<string>();
        }       

        public override void FindPossibleMoves(Dictionary<string, AbstractChessPiece> piecePositions)
        {
            List<string> moves = new List<string>();
            Tuple<int, int> gridReference = BoardRender.ChessPositionToGridReference(this.position);

            // Loop through all positions +2/-2 from the current position
            for (int deltaX = -2; deltaX < 3; deltaX++)
            {
                for (int deltaY = -2; deltaY < 3; deltaY++)
                {
                    int x = gridReference.Item1 + deltaX;
                    int y = gridReference.Item2 + deltaY;

                    // Ensure the postions checked are in bounds of the board and only allow opposition piece positions to be valid moves
                    if (x < 0 || x > 7 || y < 0 || y > 7 || IsPieceInPositionSameColour(piecePositions, x, y)) { continue; }

                    // A valid move is always 3 h/v steps away, so any absolute sum of deltaX and deltaY which equals 3 is valid
                    if (Math.Abs(deltaX) + Math.Abs(deltaY) == 3) { moves.Add(BoardRender.GridReferenceToChessPosition(x, y)); }                    
                }
            }
            this.possibleMoves = moves;
        }

        private bool IsPieceInPositionSameColour(Dictionary<string, AbstractChessPiece> piecePositions, int x, int y)
        {
            if (piecePositions.ContainsKey(BoardRender.GridReferenceToChessPosition(x, y)))
            {
                if (piecePositions[BoardRender.GridReferenceToChessPosition(x, y)].colour.Equals(this.colour))
                {
                    return true;
                }
            }
            return false;
        }

        public void AlternateFindPossibleMoves(Dictionary<string, AbstractChessPiece> piecePositions)
        {
            List<string> moves = new List<string>();

            Tuple<int, int> gridReference = BoardRender.ChessPositionToGridReference(this.position);
            int x = gridReference.Item1;
            int y = gridReference.Item2;

            // Two positions for x - 2
            if (x - 2 > 0)
            {
                if (y + 1 < 8 && !IsPieceInPositionSameColour(piecePositions, x - 2, y + 1))
                { moves.Add(BoardRender.GridReferenceToChessPosition(x - 2, y + 1)); }
                if (y - 1 > -1 && !IsPieceInPositionSameColour(piecePositions, x - 2, y - 1))
                { moves.Add(BoardRender.GridReferenceToChessPosition(x - 2, y - 1)); }
            }
            // Two positions for x - 1
            if (x - 1 > 0)
            {
                if (y + 2 < 8 && !IsPieceInPositionSameColour(piecePositions, x - 1, y + 2))
                { moves.Add(BoardRender.GridReferenceToChessPosition(x - 1, y + 2)); }
                if (y - 2 > -1 && !IsPieceInPositionSameColour(piecePositions, x - 1, y - 2))
                { moves.Add(BoardRender.GridReferenceToChessPosition(x - 1, y - 2)); }
            }
            // Two positions for x + 1
            if (x + 1 < 8)
            {
                if (y + 2 < 8 && !IsPieceInPositionSameColour(piecePositions, x + 1, y + 2))
                { moves.Add(BoardRender.GridReferenceToChessPosition(x + 1, y + 2)); }
                if (y - 2 > -1 && !IsPieceInPositionSameColour(piecePositions, x + 1, y - 2))
                { moves.Add(BoardRender.GridReferenceToChessPosition(x + 1, y - 2)); }
            }
            // Two positions for x + 2
            if (x + 2 < 8)
            {
                if (y + 1 < 8 && !IsPieceInPositionSameColour(piecePositions, x + 2, y + 1))
                { moves.Add(BoardRender.GridReferenceToChessPosition(x + 2, y + 1)); }
                if (y - 1 > -1 && !IsPieceInPositionSameColour(piecePositions, x + 2, y - 1))
                { moves.Add(BoardRender.GridReferenceToChessPosition(x + 2, y - 1)); }
            }

            this.possibleMoves = moves;
        }
    }
}
