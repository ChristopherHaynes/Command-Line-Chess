using System;
using System.Collections.Generic;
using System.Text;

namespace Command_Line_Chess
{
    class Pawn : AbstractChessPiece
    {
        private int turnNumber;

        public Pawn(string startColour, string startPosition)
        {
            this.name = "Pawn";
            this.colour = startColour.ToLower();
            if (colour == "b") { this.symbol = " ♙"; }
            else if (colour == "w") { this.symbol = " ♟"; }
            this.position = startPosition;
            this.possibleMoves = new List<string>();
            this.turnNumber = 1;
        }

        public override void Move(string newPosition)
        {
            base.Move(newPosition);
            this.turnNumber++;
        }

        public override void FindPossibleMoves(Dictionary<string, AbstractChessPiece> piecePositions)
        {
            List<string> moves = new List<string>();

            (int x, int y) = BoardRender.ChessPositionToGridReference(this.position);

            if (this.colour.Equals("b")) // Black pawns move down
            {
                // If space "ahead" of the pawn is empty add it to possible moves list
                if (!Game.piecePositions.ContainsKey(BoardRender.GridReferenceToChessPosition(x, y + 1)))
                { moves.Add(BoardRender.GridReferenceToChessPosition(x, y + 1)); }
                // Pawns can move two spaces on first move 
                if (turnNumber == 1 && !Game.piecePositions.ContainsKey(BoardRender.GridReferenceToChessPosition(x, y + 2)))
                { moves.Add(BoardRender.GridReferenceToChessPosition(x, y + 2)); } 

                // Pawns only take diagonally
                if (piecePositions.ContainsKey(BoardRender.GridReferenceToChessPosition(x - 1, y + 1)) &&
                    !piecePositions[BoardRender.GridReferenceToChessPosition(x - 1, y + 1)].colour.Equals(this.colour))
                {
                    moves.Add(BoardRender.GridReferenceToChessPosition(x - 1, y + 1));
                }
                if (piecePositions.ContainsKey(BoardRender.GridReferenceToChessPosition(x + 1, y + 1)) &&
                    !piecePositions[BoardRender.GridReferenceToChessPosition(x + 1, y + 1)].colour.Equals(this.colour))
                {
                    moves.Add(BoardRender.GridReferenceToChessPosition(x + 1, y + 1));
                }

            }
            else // White pawns move up
            {
                // If space "ahead" of the pawn is empty add it to possible moves list
                if (!Game.piecePositions.ContainsKey(BoardRender.GridReferenceToChessPosition(x, y - 1)))
                { moves.Add(BoardRender.GridReferenceToChessPosition(x, y - 1)); }
                // Pawns can move two spaces on first move 
                if (turnNumber == 1 && !Game.piecePositions.ContainsKey(BoardRender.GridReferenceToChessPosition(x, y - 2)))
                { moves.Add(BoardRender.GridReferenceToChessPosition(x, y - 2)); }

                // Pawns only take diagonally
                if (piecePositions.ContainsKey(BoardRender.GridReferenceToChessPosition(x - 1, y - 1)) &&
                    !piecePositions[BoardRender.GridReferenceToChessPosition(x - 1, y - 1)].colour.Equals(this.colour))
                {
                    moves.Add(BoardRender.GridReferenceToChessPosition(x - 1, y - 1));
                }
                if (piecePositions.ContainsKey(BoardRender.GridReferenceToChessPosition(x + 1, y - 1)) &&
                    !piecePositions[BoardRender.GridReferenceToChessPosition(x + 1, y - 1)].colour.Equals(this.colour))
                {
                    moves.Add(BoardRender.GridReferenceToChessPosition(x + 1, y - 1));
                }
            }
            this.possibleMoves = moves;
        }
    }
}
