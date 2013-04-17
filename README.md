# Sudoku
Sudoku is a game played on a 9 x 9 grid, such that the player aims to fill each column, row, 
and 3 x 3 sub-grids with digits 1 through 9, in a manner which will allow every digit to be 
represented once. The game normally starts with a partially completed grid and the game is 
completed when every cell in the 9 by 9 grid is filled.

![Sudoku Game](https://raw.github.com/Eternyte/Sudoku/Snapshots/10.JPG)
-----------------------
### How to Run with Visual Studios
Click **ZIP** between **Clone in Windows** and **HTTP**.  
Then unzip the file and run Game.sln on Visual Studios.  
Run an instance of the game with Start Debugging (F5).  

### Controls
###### Mouse
Click a cell on the 9 x 9 grid to select a tile.  
Click a digit at the 3 x 3 grid to the right to set the tile to a value.  
Right-click a digit at the 3 x 3 to the right to remove digit as a possible answer at the selected tile.  
To deselect, click the Deselect button.  
To quit the game, click the Quit button.

###### Keyboard
TO DO

### Tools
**Programmed in C# and XNA**  
**Artwork created in Inkscape and GIMP**

### To be Implemented
- [ ] Proper documentation for some parts of the code.
- [ ] An additional color to signify original digits.
- [ ] Show possible digits for each cell.
- [ ] Actual difficulty logic when removing tiles.
