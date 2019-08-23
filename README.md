# ChessGame

This project was made for a COSC 3P71 class at Brock University
The goal was to use minmax trees (with alpha beta pruning) to create a chess AI. So we programmed ours in Unity
The Ai uses very little actually strategical chess theory, only tallying up how good a potential move is based on the fitness of the board.
The fitness is calculated by tallying up the friendly peices on the board less the enemies peices.
Tally values are as follows
pawn = 10, knight = 30, bishop = 30, rook = 50, queen = 90, king = 900



The task:
Working alone or in a group of two implement a chess-playing program whose system
requirements are as follows:
§ The program should respect the rules of chess, for example,
§ the movement of pieces (including castling and en passant),
§ piece promotion, check
§ checkmate
§ stalemate
Please obtain a book on chess to verify your understanding of the game!
§ You can implement your system on any platform and language you want as long as
it is available in our labs. You may have to show me/TA it works in some cases.
§ The program must use a game tree search scheme with alpha-beta pruning.
Furthermore, the program should permit user-supplied control parameters, for
example, the depth of search.
§ Put effort towards designing an effective board evaluation function. You should
research the literature on computer chess to find strategies used by other systems.
You can borrow ideas from the literature (properly acknowledged in your report).
I also encourage you to try your own ideas!
§ The program should interact with a human player. Moves should be given via board
coordinates. At the minimum, the program should dump out the current board as an
ASCII table (e.g., upper case = black, lower case = white, space = “-“,). Although
a graphical user interface is not required, an effective GUI will be positively
considered during evaluation.
§ Your program should permit any board setup to be used initially. (This is good for
testing purposes)
§ An option is that your program should dump out the game in terms of a standard
chess output text file.
