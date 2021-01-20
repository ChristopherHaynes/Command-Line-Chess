# Command-Line-Chess
This is a simple C# application to simulate chess games using the standard console and unicode characters.

## Gameplay
When launched, a new game will begin on white's turn. After a move is made the turns will alternate between black and white until checkmate is achieved.
At this point a new game will start when any key is pressed. **The console may not display the chess characters correctly with the default font "consolas".
It is recommended to change your font to "NSimSun" at size "48" for the best results. Other True Type fonts will work, but not all have been tested.**

## User Input
All the moves in game are described using chessboard positions which are described in the x-axis by a letter (a - h), and
in the y-axis by a number (1 - 8). To make a move a 4-character string must be entered. The first two characters must refer to
a position on the board which holds one of your pieces. The second two characters must refer to a valid position which that
piece can move to. 

For example:
* _"e2e4"_ - This move would would find the piece in position "e2" and move it to position "e4"

There are also two formats of 3-character strings that can be input to retrieve more information about a specific piece. 
The first character determines what information you are trying to retrive, whilst the second and third characters represent the position of the piece.
A first character of "m" will show all the valid possible moves that a piece can make. 
A first character of "s" will show if a king is in check and/or checkmate (this only works if a valid position of a king is given)

For example:
* _"mb8"_ - This would print all the possible moves for the piece currently in position "b8"
* _"se1"_ - This would print the check and checkmate status of the king (given that the white king still resides on "e1")

## Application Design
Before starting this project, I decided that I wanted to create a chess game without any direct represenation of the board itself. This meant
that I did not use any arrays, lists, vectors etc to represent all the possible spaces on the board. Instead I decided to approach the game
from the pieces perspective. This lead to an object orientated approach, with the "board" being represented as a dictionary where the
keys are strings of board positions and the values are chess piece objects.

In terms of rendering the board it was mostly just the case of building strings for each line of the board and then printing them sequentially.
All of the responsibility of printing to the console is handled by the BoardRender class. I also wanted to be able to draw the board using
a more traditional co-ordinate system, so there are methods available to convert chess positions into x, y co-ordinates and vice verca.
See the image below for a representation of how the chess positions relate to the co-ordinate system.

![Chess Board Positions and Co-Ordinates](https://github.com/ChristopherHaynes/Command-Line-Chess/blob/master/Command%20Line%20Chess/res/ChessBoardLayout.png?raw=true)

## Currently Missing Features
There are currently 4 known missing features which are part of the standard game of chess. These are listed in order of priority.
* __Cross Check__ - This is when a piece is check, it can escape check by also placing the opponent into check. Currently this is not a valid move.
* __"En Passant"__ - If a pawn moves two spaces forward (as it each pawn can on its first move), then an opposition pawn can capture it in the
position it moved past, known as capturing "in passing". This can only occur on the move directly after the first pawn moved two spaces forward.
Currently this is not counted as a valid move
* __Castling__ - This is move which allows a Rook and King to move and switch places given specific circumstances. Currently not implemented.
* __Pawn Promotion - COMPLETED__ ~~If a pawn reaches the back rank then it can be promoted to any other piece. Currently not implemented.~~

## Possible Future Expansion
It would be nice to add basic online functionality so that two users can play remotely.
It would also be interesting to experiment with chess AI, both more traditional searching approaches (min-max, alpha-beta etc) as well as statistical and 
ML approaches (Reinforcement learning, monto carlo etc).
