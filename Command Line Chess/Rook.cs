using System;
using System.Collections.Generic;
using System.Text;

namespace Command_Line_Chess
{
    class Rook : AbstractChessPiece
    {
        public Rook(string startColour, string startPosition)
        {
            this.name = "Rook";
            this.colour = startColour.ToLower();
            if (colour == "b") { this.symbol = " ♖"; }
            else if (colour == "w") { this.symbol = " ♜"; }
            this.position = startPosition;
            this.possibleMoves = new List<string>();
        }

        public override void FindPossibleMoves(Dictionary<string, AbstractChessPiece> piecePositions)
        {
            List<string> moves = new List<string>();

            Tuple<int, int> gridReference = BoardRender.ChessPositionToGridReference(this.position);

            // Check up direction
            moves.AddRange(FindMovesUpDirection(gridReference));
            // Check down direction
            moves.AddRange(FindMovesDownDirection(gridReference));
            // Check right direction
            moves.AddRange(FindMovesRightDirection(gridReference));
            // Check left direction
            moves.AddRange(FindMovesLeftDirection(gridReference));

            this.possibleMoves = moves;
        }
    }
}
