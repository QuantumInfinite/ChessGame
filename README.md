# ChessGame

This project was made for a COSC 3P71 class at Brock University
The goal was to use minmax trees (with alpha beta pruning) to create a chess AI.
The Ai uses very little actually strategical chess theory, only tallying up how good a potential move is based on the fitness of the board.
The fitness is calculated by tallying up the friendly peices on the board less the enemies peices.
Tally values are as follows
pawn = 10, knight = 30, bishop = 30, rook = 50, queen = 90, king = 900
