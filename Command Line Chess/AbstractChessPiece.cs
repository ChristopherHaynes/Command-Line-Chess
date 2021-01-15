using System;
using System.Collections.Generic;
using System.Text;

namespace Command_Line_Chess
{
    abstract class AbstractChessPiece 
    {
        public string name;
        public string colour;
        public string symbol;
        public string position;
        public List<string> possibleMoves;

        public virtual void Move(string newPosition)
        {
            string oldPosition = this.position;
            bool isPieceTaken = false;
            string pieceRemovedName = "";

            // Check if there is already a piece on the new space
            if (Game.piecePositions.ContainsKey(newPosition))
            {
                // Remove the taken piece from the piece positions dictionary
                pieceRemovedName = Game.piecePositions[newPosition].name;
                Game.piecePositions.Remove(newPosition);
                isPieceTaken = true;
            }
            // Update this piece's position and update the dictionary
            Game.piecePositions.Remove(this.position);
            position = newPosition;
            Game.piecePositions.Add(this.position, this);

            // Print a message describing the move
            string pieceColour = this.colour.Equals("b") ? "Black " : "White ";
            string moveMessage = pieceColour + this.name + " moved from " + oldPosition + " to " + this.position;
            if (isPieceTaken)
            {
                moveMessage += " and captured opponents " + pieceRemovedName;
            }
            Console.WriteLine(moveMessage);            
        }
        public abstract void FindPossibleMoves(Dictionary<string, AbstractChessPiece> piecePositions);
        protected List<string> FindMovesUpDirection(Tuple<int, int> gridReference)
        {
            List<string> moves = new List<string>();

            // Check up from the current position until another piece is discovered, adding each position along the way
            int x = gridReference.Item1;
            for (int y = gridReference.Item2 - 1; y > -1; y--)
            {
                string position = BoardRender.GridReferenceToChessPosition(x, y);
                if (!Game.piecePositions.ContainsKey(position))
                {
                    moves.Add(position);
                }
                else
                {
                    if (Game.piecePositions[position].colour.Equals(this.colour)) { break; }
                    else { moves.Add(position); break; }
                }
            }
            return moves;
        }
        protected List<string> FindMovesDownDirection(Tuple<int, int> gridReference)
        {
            List<string> moves = new List<string>();

            // Check down from the current position until another piece is discovered, adding each position along the way
            int x = gridReference.Item1;
            for (int y = gridReference.Item2 + 1; y < 8; y++)
            {
                string position = BoardRender.GridReferenceToChessPosition(x, y);
                if (!Game.piecePositions.ContainsKey(position))
                {
                    moves.Add(position);
                }
                else
                {
                    if (Game.piecePositions[position].colour.Equals(this.colour)) { break; }
                    else { moves.Add(position); break; }
                }
            }
            return moves;
        }
        protected List<string> FindMovesRightDirection(Tuple<int, int> gridReference)
        {
            List<string> moves = new List<string>();

            // Check down from the current position until another piece is discovered, adding each position along the way
            int y = gridReference.Item2;
            for (int x = gridReference.Item1 + 1; x < 8; x++)
            {
                string position = BoardRender.GridReferenceToChessPosition(x, y);
                if (!Game.piecePositions.ContainsKey(position))
                {
                    moves.Add(position);
                }
                else
                {
                    if (Game.piecePositions[position].colour.Equals(this.colour)) { break; }
                    else { moves.Add(position); break; }
                }
            }
            return moves;
        }
        protected List<string> FindMovesLeftDirection(Tuple<int, int> gridReference)
        {
            List<string> moves = new List<string>();

            // Check down from the current position until another piece is discovered, adding each position along the way
            int y = gridReference.Item2;
            for (int x = gridReference.Item1 - 1; x > -1; x--)
            {
                string position = BoardRender.GridReferenceToChessPosition(x, y);
                if (!Game.piecePositions.ContainsKey(position))
                {
                    moves.Add(position);
                }
                else
                {
                    if (Game.piecePositions[position].colour.Equals(this.colour)) { break; }
                    else { moves.Add(position); break; }
                }
            }
            return moves;
        }
        protected List<string> FindMovesUpLeftDirection(Tuple<int, int> gridReference)
        {
            List<string> moves = new List<string>();

            // Check down from the current position until another piece is discovered, adding each position along the way
            int x = gridReference.Item1 - 1;
            for (int y = gridReference.Item2 - 1; y > -1; y--)
            {
                string position = BoardRender.GridReferenceToChessPosition(x, y);
                if (!Game.piecePositions.ContainsKey(position))
                {
                    moves.Add(position);
                }
                else
                {
                    if (Game.piecePositions[position].colour.Equals(this.colour)) { break; }
                    else { moves.Add(position); break; }
                }
                x--;
                // Check the new x position is still in bounds
                if (x < 0) break;
            }
            return moves;
        }
        protected List<string> FindMovesUpRightDirection(Tuple<int, int> gridReference)
        {
            List<string> moves = new List<string>();

            // Check down from the current position until another piece is discovered, adding each position along the way
            int x = gridReference.Item1 + 1;
            for (int y = gridReference.Item2 - 1; y > -1; y--)
            {
                string position = BoardRender.GridReferenceToChessPosition(x, y);
                if (!Game.piecePositions.ContainsKey(position))
                {
                    moves.Add(position);
                }
                else
                {
                    if (Game.piecePositions[position].colour.Equals(this.colour)) { break; }
                    else { moves.Add(position); break; }
                }
                x++;
                // Check the new x position is still in bounds
                if (x > 7) break;
            }
            return moves;
        }
        protected List<string> FindMovesDownLeftDirection(Tuple<int, int> gridReference)
        {
            List<string> moves = new List<string>();

            // Check down from the current position until another piece is discovered, adding each position along the way
            int x = gridReference.Item1 - 1;
            for (int y = gridReference.Item2 + 1; y < 8; y++)
            {
                string position = BoardRender.GridReferenceToChessPosition(x, y);
                if (!Game.piecePositions.ContainsKey(position))
                {
                    moves.Add(position);
                }
                else
                {
                    if (Game.piecePositions[position].colour.Equals(this.colour)) { break; }
                    else { moves.Add(position); break; }
                }
                x--;
                // Check the new x position is still in bounds
                if (x < 0) break;
            }
            return moves;
        }
        protected List<string> FindMovesDownRightDirection(Tuple<int, int> gridReference)
        {
            List<string> moves = new List<string>();

            // Check down from the current position until another piece is discovered, adding each position along the way
            int x = gridReference.Item1 + 1;
            for (int y = gridReference.Item2 + 1; y < 8; y++)
            {
                string position = BoardRender.GridReferenceToChessPosition(x, y);
                if (!Game.piecePositions.ContainsKey(position))
                {
                    moves.Add(position);
                }
                else
                {
                    if (Game.piecePositions[position].colour.Equals(this.colour)) { break; }
                    else { moves.Add(position); break; }
                }
                x++;
                // Check the new x position is still in bounds
                if (x > 7) break;
            }
            return moves;
        }
    }
}
