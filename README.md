# Sudoku
Sudoku is a game played on a 9 x 9 grid, such that the player aims to fill each column, row, 
and 3 x 3 sub-grids with digits 1 through 9, in a manner which will allow every digit to be 
represented once. The game normally starts with a partially completed grid and the game is 
completed when every cell in the 9 by 9 grid is filled.  

This implementation of Sudoku generates a random board through recursive backtracking.
Therefore, each game is assured to have a valid solution. The player is able to play the game 
through mouse or keyboard commands. Some addition features include eliminating possible values
from each cell and displaying conflicting digits.

![Sudoku Game](https://raw.github.com/Eternyte/Sudoku/master/Snapshots/10.JPG)

### Background
I am currently studying at Rochester Institute of Technology (RIT). And while the courses I have
taken have allowed me to learn Python, Java, and C++, I was not taught C#. However, this is
one of the first languages which the Game Design and Development (GDD) majors at RIT learn.
Therefore, over the course of my one week Winter Break, I decided to learn C# on my own time with 
the help of Whitaker's XNA tutorial and of my GDD friends. This was the result.

-----------------------
### How to Run with Visual Studios
Click **ZIP** between **Clone in Windows** and **HTTP**.  
Then unzip the file and run Game.sln on Visual Studios.  
Run an instance of the game with **Start Debugging (F5)**.  

### Controls
###### Mouse
To select a cell, click a cell on the 9 x 9 grid.  
To select a value for the cell, click a digit on the 3 x 3 to the right.  
To eliminate a value for the cell, right-click a digit on the 3 x 3 to the right.  
To deselect, click outside of the buttons.  
To quit the game, click the Quit button.

###### Keyboard
To move to adjacent cells, use arrow keys.  
To select a value for the cell, enter a digit.  
To eliminate a value for the cell, hold spacebar and enter a digit.

-----------------------
### Tools
**Programmed in C# and XNA**  
**Artwork created in Inkscape and GIMP**

### To be Implemented
- [ ] Proper documentation for some parts of the code.
- [ ] Actual difficulty logic when removing tiles.
- [ ] Implement some form of initial selection for keyboard.
