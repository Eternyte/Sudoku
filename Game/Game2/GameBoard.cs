using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game {
    class GameBoard {
        private int size;
        private Random random;
        public int[,] Solution { get; set; }
        public int[,] Original { get; set; }
        public int[,] Board { get; set; }
        public List<Tuple<int, int>> Offending { get; set; }

        public GameBoard(int newSize) {
            size = newSize;
            random = new Random();
            Solution = new int[size, size];
            Original = new int[size, size];
            Board = new int[size, size];
            Offending = new List<Tuple<int, int>>();

            generate();
        }

        public int this[int x, int y] {
            get { return Board[x, y]; }
            set { 
                Board[x, y] = value;
                getOffending(x, y);
            }
        }

        private void generate() {
            // Generate a valid solution.
            for (int i = 0; i < Solution.GetLength(0); ++i) {
                for (int j = 0; j < Solution.GetLength(1); ++j) {
                    Solution[i, j] = 0;
                }
            }

            List<int> validCol = new List<int>(new int[] {0,1,2,3,4,5,6,7,8});
            randomizeList<int>(ref validCol);
            generateSolution(Solution, 1, 0, validCol, 0);

            // Generate a valid board.
            for (int i = 0; i < Board.GetLength(0); ++i) {
                for (int j = 0; j < Board.GetLength(1); ++j) {
                    Board[i, j] = Solution[i, j];
                }
            }

            List<Tuple<int,int>> validPos = new List<Tuple<int, int>>();
            for (int i = 0; i < size; ++i) {
                for (int j = 0; j < size; ++j) {
                    validPos.Add(new Tuple<int,int>(i, j));
                }
            }

            randomizeList(ref validPos);
            generateBoard(Board, validPos);

            // Copy onto original.
            for (int i = 0; i < Original.GetLength(0); ++i) {
                for (int j = 0; j < Original.GetLength(1); ++j) {
                    Original[i, j] = Board[i, j];
                }
            }
        }

        // Generates a solution based on given variables. Recursive.
        private bool generateSolution(int[,] array, int value, int curRow, List<int> validCol, int colIndex)
        {
            int curCol = validCol[colIndex];

            // Base case: Last 9 to be placed somewhere in row 8.
            if (value == 9 && curRow == 8) {
                if (isValid(array, value, curRow, curCol)) {
                    array[curRow, curCol] = value;
                    return true;
                } else {
                    return false;
                }

            // Finished placing all the values less than 9.
            } else if (value < 9 && curRow == 8) {
                if (isValid(array, value, curRow, curCol)) {
                    // Update parameters for next level of recursion.
                    array[curRow, curCol] = value;
                    List<int> newValidCol = new List<int>(new int[] {0,1,2,3,4,5,6,7,8});
                    randomizeList(ref newValidCol);

                    // Find next possible move.
                    int tempColIndex = 0;
                    bool hasChanged;
                    while (tempColIndex < newValidCol.Count) {
                        hasChanged = generateSolution(array, value + 1, 0, newValidCol, tempColIndex);
                        if (hasChanged) {
                            return true;
                        } else {
                            tempColIndex++;
                        }
                    }
   
                    // None of the choices on this branch is valid.
                    curRow = 8;
                    array[curRow, curCol] = 0;
                    return false;

                // Not valid.
                } else {
                    return false;
                }

            // For all other values not at the last row.
            } else {
                if (isValid(array, value, curRow, curCol)) {
                    // Update parameters for next level of recursion.
                    array[curRow, curCol] = value;
                    validCol.RemoveAt(colIndex);

                    // Find next possible move.
                    int tempColIndex = 0;
                    bool hasChanged;
                    while (tempColIndex < validCol.Count) {
                        hasChanged = generateSolution(array, value, curRow + 1, validCol, tempColIndex);
                        if (hasChanged) {
                            return true;
                        } else {
                            tempColIndex++;
                        }
                    }

                    // None of the choices on this branch is valid.
                    array[curRow, curCol] = 0;
                    validCol.Insert(colIndex, curCol);
                    return false;
                
                // Not valid.
                } else {
                    return false;
                }
            }
        }

        private void generateBoard(int[,] board, List<Tuple<int,int>> validPos) {
            List<Tuple<int, int>> removedPos = new List<Tuple<int, int>>();
            int row, col, value;

            while (validPos.Count > 35) {
                // Get the data and update variables.
                row = validPos[0].Item1;
                col = validPos[0].Item2;
                value = board[row, col];
                board[row, col] = 0;
                removedPos.Insert(0, validPos[0]);
                validPos.RemoveAt(0);

                // Check if removing the value is alright.
                bool hasSolution;
                for (int tempValue = 1; tempValue <= board.GetLength(0); ++tempValue) {
                    if (tempValue != value) {
                        hasSolution = hasPossibleSolution(board, removedPos, 0, tempValue);
                        if (hasSolution) {
                            removedPos.RemoveAt(0);
                            board[row, col] = value;
                            break;
                        }
                    }
                }
            }
        }

        // if solution, then return true
        // if not valid, the return false
        private bool hasPossibleSolution(int[,] array, List<Tuple<int,int>> removedPos, int posIndex, int value) {
            // If board is filled and valid, then board is a solution.
            if (posIndex == removedPos.Count) {
                return true;
            // If not yet filled,
            } else {
                int row = removedPos[posIndex].Item1;
                int col = removedPos[posIndex].Item2;

                // And current configuration is not valid, then not a solution.
                if (isValid(array, value, row, col) == false) {
                    return false;
                // Else, could be a solution and try next empty square.
                } else {
                    array[row, col] = value;
                    bool hasSolution = false;
                    for (int tempValue = 0; tempValue < array.GetLength(0); ++tempValue) {
                        hasSolution = hasPossibleSolution(array, removedPos, posIndex + 1, tempValue);
                        if (hasSolution) {
                            break;
                        }
                    }
                    array[row, col] = 0;
                    return hasSolution;
                }
            }
            
        }

        // HELPER FUNCTIONS
        // isValid checks if the value at a position in the array is valid.
        private bool isValid(int[,] array, int value, int row, int col) {
            // Check that the space is empty.
            if (array[row, col] != 0) {
                return false;
            }

            // Check row.
            for (int j = 0; j < array.GetLength(1); ++j) {
                if (array[row, j] == value) {
                    return false;
                }
            }

            // Check column.
            for (int i = 0; i < array.GetLength(0); ++i) {
                if (array[i, col] == value) {
                    return false;
                }
            }

            // Check grid.
            int rowQuotient = row / 3;
            int colQuotient = col / 3;
            for (int i = rowQuotient * 3; i < (rowQuotient + 1) * 3; ++i) {
                for (int j = colQuotient * 3; j < (colQuotient + 1) * 3; ++j) {
                    if (array[i, j] == value) {
                        return false;
                    }
                }
            }

            // Else, valid.
            return true;
        }

        // randomizeList takes a list and randomizes the results.
        private void randomizeList<T>(ref List<T> list) {
            List<T> randList = new List<T>();
            int index;
            while (list.Count > 0) {
                index = (int)(random.NextDouble() * (double)list.Count);
                randList.Add(list[index]);
                list.RemoveAt(index);
            }
            list = randList;
        }
        // END HELPER FUNCTIONS

        private void getOffending(int row, int col) {
            Offending.Clear();
            int value = Board[row, col];

            // Check row.
            for (int j = 0; j < Board.GetLength(1); ++j) {
                if (Board[row, j] == value && j != col) {
                    Offending.Add(new Tuple<int, int>(row, j));
                }
            }

            // Check column.
            for (int i = 0; i < Board.GetLength(0); ++i) {
                if (Board[i, col] == value && i != row) {
                    Offending.Add(new Tuple<int, int>(i, col));
                }
            }

            // Check grid.
            int rowQuotient = row / 3;
            int colQuotient = col / 3;
            for (int i = rowQuotient * 3; i < (rowQuotient + 1) * 3; ++i) {
                for (int j = colQuotient * 3; j < (colQuotient + 1) * 3; ++j) {
                    if (Board[i, j] == value && i != row && j != col) {
                        Offending.Add(new Tuple<int, int>(i, j));
                    }
                }
            }
        }

        public bool isSolved() {
            for (int i = 0; i < Board.GetLength(0); ++i) {
                for (int j = 0; j < Board.GetLength(1); ++j) {
                    if (Board[i, j] != Solution[i, j]){
                        return false;
                    }
                }
            }
            return true;
        }

        // Debugging method.
        private void printBoard(int[,] array) {
            for (int i = 0; i < array.GetLength(0); ++i) {
                Console.Write("[");
                for (int j = 0; j < array.GetLength(0); ++j) {
                    Console.Write(array[i, j] + " "); 
                }
                Console.Write("]");
                Console.WriteLine();
            }
        }
    }
}
