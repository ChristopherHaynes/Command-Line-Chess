using System;
using System.Collections.Generic;
using System.Text;

namespace Command_Line_Chess
{
    class Queen : AbstractChessPiece
    {
        public Queen(string startColour, string startPosition)
        {
            this.name = "Queen";
            this.colour = startColour.ToLower();
            if (colour == "b") { this.symbol = " ♕"; }
            else if (colour == "w") { this.symbol = " ♛"; }
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
            // Check up left direction
            moves.AddRange(FindMovesUpLeftDirection(gridReference));
            // Check up right direction
            moves.AddRange(FindMovesUpRightDirection(gridReference));
            // Check down left direction
            moves.AddRange(FindMovesDownLeftDirection(gridReference));
            // Check down right direction
            moves.AddRange(FindMovesDownRightDirection(gridReference));

            this.possibleMoves = moves;
        }
    }
}
