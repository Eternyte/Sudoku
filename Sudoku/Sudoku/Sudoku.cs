// Sudoku.cs
// Description: Performs the main game logic loop with helper functions.
//
// Author: Stacy Chen
// ///////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Sudoku {
    public class Sudoku : Microsoft.Xna.Framework.Game {
        // The display of the game. //
        GraphicsDeviceManager graphics;
        // The collection of sprites to draw. //
        SpriteBatch spriteBatch;

        // CONSTANTS
        // The size of the board. //
        private const int SIZE = 9;
        // The screenwidth of the display. //
        private const int SCREENWIDTH = 1000;
        // The screenheight of the display. //
        private const int SCREENHEIGHT = 675;

        // Texture2D
        // The background of the game; wood texture. //
        private Texture2D background;
        // The cell to place a tile in; silver box. //
        private Texture2D cell;
        // A blank tile; creme-colored box. //
        private Texture2D blank;
        // A set of tiles; creme-colored with numbers. //
        private Texture2D tileMap;
        // Reset button when mouse is not there. //
        private Texture2D resetOff;
        // Reset button when mouse is there. //
        private Texture2D resetOn;
        // Quit button when mouse is not there. //
        private Texture2D quitOff;
        // Quit button when mouse is there. //
        private Texture2D quitOn;

        // Source and Destination Rectangle Arrays
        // Array for the current values inputted into the board. //
        private TileArray tileArray;
        // Array for tile positions on the board. //
        private Rectangle[,] tileBoardDest;
        // Array for tile positions for the mouse input section. //
        private Rectangle[,] moveBoardDest;
        // Array for the cell positions on the board. //
        private Rectangle[,] cellBoardDest;

        // Buttons
        // Clear button to clear the selected tile. //
        private Button clear;
        // Reset button to reset the whole map. //
        private Button reset;
        // Quit button to end the game. //
        private Button quit;

        // Keyboard and Mouse
        // The last key pressed. //
        private KeyboardState oldKeyState;
        // The last mouse button pressed. //
        private MouseState oldState;
        // If the mouse has moved. //
        private bool hasMoved;
        // The position of the last left click. //
        private Point lastClickedLeft;
        // The position of the last right click. //
        private Point lastClickedRight;
        // The tile currently selected. //
        private Point selectedTile;

        // Game
        // The Sudoku board. //
        private GameBoard curBoard;
        // The values of the tiles in the mouse input section. //
        private int[,] moveBoard;
        // The values crossed out in relation to the tile at the point. //
        private Dictionary<Point, List<int>> crossout;
        // If the player has won the game. //
        private bool hasWon;

        // Constructor for the game object.
        public Sudoku() {
            // Sets the traits of the screen.
            this.Window.Title = "Sudoku";
            this.IsMouseVisible = true;

            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = SCREENWIDTH;
            graphics.PreferredBackBufferHeight = SCREENHEIGHT;
            graphics.ApplyChanges();
            Content.RootDirectory = "Content";
        }

        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        protected override void Initialize() {
            // Destination Rectangle Arrays
            tileBoardDest = new Rectangle[SIZE, SIZE];
            moveBoardDest = new Rectangle[3, 3];
            cellBoardDest = new Rectangle[SIZE, SIZE];

            // Mouse
            hasMoved = false;
            lastClickedLeft = new Point();
            lastClickedRight = new Point();
            selectedTile = new Point(-1, -1);

            // Game
            curBoard = new GameBoard(SIZE);
            moveBoard = new int[3, 3] { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 9 } };
            crossout = new Dictionary<Point, List<int>>();
            hasWon = false;

            // Set a dictionary for a 9x9 grid board.
            for (int i = 0; i < SIZE; ++i) {
                for (int j = 0; j < SIZE; ++j) {
                    crossout.Add(new Point(i, j), new List<int>());
                }
            }

            // Initialize
            base.Initialize();
        }

        // LoadContent will be called once per game and is the place to load all of your content.
        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load board content.
            background = Content.Load<Texture2D>("Wood");
            cell = Content.Load<Texture2D>("Cell");
            setDestinations(cellBoardDest, cell, 15, 15, 0);
            blank = Content.Load<Texture2D>("Blank");
            tileMap = Content.Load<Texture2D>("Tiles");
            tileArray = new TileArray(tileMap, 4, 9);
            setDestinations(tileBoardDest, blank, 15, 15, 3);
            setDestinations(moveBoardDest, blank, 750, 350, 2);

            // Add buttons.
            clear = new Button("clear", blank, blank,
                new Rectangle(750, 575, blank.Width, blank.Height));
            resetOff = Content.Load<Texture2D>("Buttons/ResetOff");
            resetOn = Content.Load<Texture2D>("Buttons/ResetOn");
            reset = new Button("reset", resetOff, resetOn,
             new Rectangle(820, 575, resetOff.Width, resetOff.Height));
            quitOff = Content.Load<Texture2D>("Buttons/QuitOff");
            quitOn = Content.Load<Texture2D>("Buttons/QuitOn");
            quit = new Button("quit", quitOff, quitOn, 
                new Rectangle(750, 25, quitOff.Width, quitOff.Height));
        }

        // setDestinations fills an array with rectangle positions
        // Allows for easy fill of destination based on values.
        private void setDestinations(Rectangle[,] array, Texture2D image, int startX, int startY, int buffer) {
            int width = image.Width;
            int height = image.Height;

            int y = startY + buffer;
            for (int i = 0; i < array.GetLength(0); ++i) {
                int x = startX + buffer;
                for (int j = 0; j < array.GetLength(0); ++j) {
                    array[i, j] = new Rectangle(x, y, width, height);
                    if (j == 2 || j == 5) { 
                        x += 10; 
                    }
                    x += width + (buffer * 2);
                }
                if (i == 2 || i == 5) { 
                    y += 10; 
                }
                y += height + (buffer * 2);
            }
        }

        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        protected override void UnloadContent() { }

        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// @param  gameTime    provides a snapshot of timing values.
        protected override void Update(GameTime gameTime) {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // Game Logic
            if (hasWon == false) {
                KeyboardState keyState = Keyboard.GetState();
                updateMouse();
                getMove(keyState);
                shiftCell(keyState);
                callClear();
                callReset();
                callQuit();
                hasWon = curBoard.isSolved();
                oldKeyState = keyState;
            }
            base.Update(gameTime);
        }

        private void updateMouse() {
            MouseState newState = Mouse.GetState();

            // Update hasMoved.
            if (newState.X == oldState.X && newState.Y == oldState.Y) {
                hasMoved = false;
            } else {
                hasMoved = true;
            }

            // Update lastClickedLeft.
            if (newState.LeftButton == ButtonState.Pressed && oldState.LeftButton == ButtonState.Released) {
                lastClickedLeft = new Point(newState.X, newState.Y);
            } else {
                lastClickedLeft = new Point(-1, -1);
            }
            
            // Update lastClickedRight.
            if (newState.RightButton == ButtonState.Pressed && oldState.RightButton == ButtonState.Released) {
                lastClickedRight = new Point(newState.X, newState.Y);
            } else {
                lastClickedRight = new Point(-1, -1);
            }

            // Update selectedTile.
            Point tempSelectedTile = new Point(-1, -1);
            for (int i = 0; i < tileBoardDest.GetLength(0); ++i) {
                for (int j = 0; j < tileBoardDest.GetLength(1); ++j) {
                    if (tileBoardDest[i, j].Contains(lastClickedLeft) && curBoard.Original[i, j] == 0) {
                        tempSelectedTile = new Point(i, j);
                    }
                }
            }

            // Implement clicking outside to deselect.
            if (lastClickedLeft != new Point(-1,-1) && tempSelectedTile == new Point(-1, -1)
                && clear.Destination.Contains(lastClickedLeft) == false
                && reset.Destination.Contains(lastClickedLeft) == false
                && quit.Destination.Contains(lastClickedLeft) == false) {
                Boolean clickedMove = false;
                for (int i = 0; i < moveBoardDest.GetLength(0); ++i) {
                    for (int j = 0; j < moveBoardDest.GetLength(1); ++j) {
                        if (moveBoardDest[i, j].Contains(lastClickedLeft)) {
                            clickedMove = true;
                            break;
                        }
                    }
                }
                if (clickedMove == false) {
                    selectedTile = new Point(-1, -1);
                }
            } else if (lastClickedLeft != new Point(-1, -1) && tempSelectedTile != new Point(-1, -1)) {
                selectedTile = tempSelectedTile;
            }
            
            // Update oldState.
            oldState = newState;
        }

        private void getMove(KeyboardState keyState) {
            // If there is a selected tile.
            if (0 <= selectedTile.X && 0 <= selectedTile.Y) {
                // Keyboard
                Keys[] pressedKeys = keyState.GetPressedKeys();
                Keys[] numPadKeys = new Keys[] {Keys.NumPad1, Keys.NumPad2, Keys.NumPad3, Keys.NumPad4,
                                                Keys.NumPad5, Keys.NumPad6, Keys.NumPad7, Keys.NumPad8, 
                                                Keys.NumPad9, Keys.NumPad0, Keys.D1, Keys.D2, Keys.D3, Keys.D4,
                                                Keys.D5, Keys.D6, Keys.D7, Keys.D8, Keys.D9, Keys.D0};

                for (int keyIndex = 0; keyIndex < pressedKeys.Length; ++keyIndex) {
                    if (keyState.IsKeyDown(pressedKeys[keyIndex]) && oldKeyState.IsKeyUp(pressedKeys[keyIndex])
                        && numPadKeys.Contains(pressedKeys[keyIndex])) {
                        int value = (Array.IndexOf(numPadKeys, pressedKeys[keyIndex]) % 10) + 1;

                        // Remove value from crossout.
                        if (pressedKeys.Contains(Keys.Space) && value != 10) {
                            // Remove value from crossout.
                            if (crossout[selectedTile].Contains(value)) {
                                crossout[selectedTile].Remove(value);
                            // Add value to crossout.
                            } else {
                                crossout[selectedTile].Add(value);
                            }
                            hasMoved = true;
                        // Change selected tile to value pressed.
                        } else if ((value == 10 || crossout[selectedTile].Contains(value) == false)
                            && curBoard.Original[selectedTile.X, selectedTile.Y] == 0) {
                            curBoard[selectedTile.X, selectedTile.Y] = value % 10;
                            hasMoved = true;
                        }
                    }
                }

                // Mouse
                for (int i = 0; i < moveBoardDest.GetLength(0); ++i) {
                    for (int j = 0; j < moveBoardDest.GetLength(1); ++j) {
                        // If right mouse clicked or left mouse + spacebar, then crossout value from possible values.
                        int value = moveBoard[i, j];
                        
                        // Crossout value from possible values.
                        if ((moveBoardDest[i, j].Contains(lastClickedLeft) && pressedKeys.Contains(Keys.Space))
                            || moveBoardDest[i, j].Contains(lastClickedRight)) {
                            // If have value, remove value.
                            if (crossout[selectedTile].Contains(value)) {
                                crossout[selectedTile].Remove(value);
                            // Else, add value.
                            } else {
                                crossout[selectedTile].Add(value);
                            }
                        // Change selected tile to value pressed.
                        } else if (moveBoardDest[i, j].Contains(lastClickedLeft) 
                            && crossout[selectedTile].Contains(value) == false
                            && curBoard.Original[selectedTile.X, selectedTile.Y] == 0) {
                            curBoard[selectedTile.X, selectedTile.Y] = value;
                        }
                    }
                }
            }
        }

        private void shiftCell(KeyboardState keyState) {
            Keys[] pressedKeys = keyState.GetPressedKeys();
            for (int keyIndex = 0; keyIndex < pressedKeys.Length; ++keyIndex) {
                Keys tempKey = pressedKeys[keyIndex];

                if (keyState.IsKeyDown(tempKey) && oldKeyState.IsKeyUp(tempKey)) {
                    switch (pressedKeys[keyIndex].ToString()) {
                        case "Up":
                            if (selectedTile.X - 1 >= 0) {
                                selectedTile = new Point(selectedTile.X - 1, selectedTile.Y);
                                hasMoved = true;
                            }
                            break;
                        case "Down":
                            if (selectedTile.X + 1 < 9) {
                                selectedTile = new Point(selectedTile.X + 1, selectedTile.Y);
                                hasMoved = true;
                            }
                            break;
                        case "Left":
                            if (selectedTile.Y - 1 >= 0) {
                                selectedTile = new Point(selectedTile.X, selectedTile.Y - 1);
                                hasMoved = true;
                            }
                            break;
                        case "Right":
                            if (selectedTile.Y + 1 < 9) {
                                selectedTile = new Point(selectedTile.X, selectedTile.Y + 1);
                                hasMoved = true;
                            }
                            break;
                    }
                }
            }
        }

        private void callClear() {
            if (clear.Destination.Contains(lastClickedLeft)) {
                clear.IsOn = true;
                if (selectedTile != new Point(-1, -1)) {
                    curBoard[selectedTile.X, selectedTile.Y] = 0;
                    if (crossout[new Point(selectedTile.X, selectedTile.Y)].Count > 0) {
                        crossout[new Point(selectedTile.X, selectedTile.Y)].Clear();
                    }
                }
            } else if (clear.Destination.Contains(new Point(oldState.X, oldState.Y)) == false) {
                clear.IsOn = false;
            }
        }

        private void callReset() {
            if (reset.Destination.Contains(lastClickedLeft)) {
                reset.IsOn = true;
                for (int i = 0; i < SIZE; ++i) {
                    for (int j = 0; j < SIZE; ++j) {
                        // Reset the current board to the original board.
                        if (curBoard.Original[i, j] == 0) {
                            curBoard[i, j] = 0;
                        }
                        // Reset the dictionary to empty strings.
                        crossout[new Point(i, j)] = new List<int>();
                    }
                }
                
            } else if (reset.Destination.Contains(new Point(oldState.X, oldState.Y)) == false) {
                reset.IsOn = false;
            }
        }

        private void callQuit() {
            if (quit.Destination.Contains(lastClickedLeft)) {
                quit.IsOn = true;
                curBoard.Board = curBoard.Solution;
                curBoard.Offending.Clear();
            } else if (quit.Destination.Contains(new Point(oldState.X, oldState.Y)) == false) {
                quit.IsOn = false;
            }
        }

        protected override bool BeginDraw() {
            if (hasMoved || lastClickedLeft != new Point(-1, -1) || lastClickedRight != new Point(-1, -1)) {
                return base.BeginDraw();
            } else {
                return false;
            }
        }

        /// This is called when the game should draw itself.
        /// @param  gameTime    provides a snapshot of timing values.
        protected override void Draw(GameTime gameTime) {
            spriteBatch.Begin();
            spriteBatch.Draw(background, new Rectangle(0, 0, SCREENWIDTH, SCREENHEIGHT), Color.White);
            drawFromArray(cell, cellBoardDest);
            drawTilesFromBoard(curBoard.Board, tileBoardDest);
            drawTilesFromBoard(moveBoard, moveBoardDest);
            clear.Draw(spriteBatch);
            reset.Draw(spriteBatch);
            quit.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void drawFromArray(Texture2D image, Rectangle[,] array) {
            for (int i = 0; i < array.GetLength(0); ++i) {
                for (int j = 0; j < array.GetLength(1); ++j) {
                    spriteBatch.Draw(image, array[i, j], Color.White);
                }
            }
        }

        // @precond board must be a square array
        private void drawTilesFromBoard(int[,] board, Rectangle[,] dest) {
            int size = board.GetLength(0);

            // Read the board and draw based on circumstances.
            for (int i = 0; i < size; ++i) {
                for (int j = 0; j < size; ++j) {
                    int value = board[i, j];

                    // If drawing the selected tile, scale the selected tile.
                    if (size == 9 && selectedTile.X == i && selectedTile.Y == j) {
                        if (value == 0) {
                            Rectangle rect = new Rectangle(0, 0, blank.Width, blank.Height);
                            expand(blank, dest[i, j], rect, 1);
                            if (crossout.ContainsKey(new Point(i, j)) && crossout[new Point(i, j)].Count > 0) {
                                for (int num = 1; num <= SIZE; ++num) {
                                    if (crossout[new Point(i, j)].Contains(num) == false) {
                                        spriteBatch.End();
                                        spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
                                        rect = tileArray[3, num - 1];
                                        expand(tileMap, dest[i, j], rect, 0);
                                        spriteBatch.End();
                                        spriteBatch.Begin();
                                    }
                                }
                            }
                        } else {
                            Rectangle rect = tileArray[0, value - 1];
                            expand(tileMap, dest[i, j], rect, 1);
                        }
                    // If drawing an offending tile, draw the tile with red.
                    } else if (size == 9 && value != 0 && curBoard.Offending.Contains(new Tuple<int, int>(i, j))) {
                        Rectangle rect = tileArray[1, value - 1];
                        spriteBatch.Draw(tileMap, dest[i, j], rect, Color.White);
                    // If drawing an already existing tile, draw the tile with blue.
                    } else if (size == 9 && value != 0 && curBoard.Original[i, j] != 0) {
                        Rectangle rect = tileArray[2, value - 1];
                        spriteBatch.Draw(tileMap, dest[i, j], rect, Color.White);
                    // If crossout has values on the tile, then draw a blank with the numbers.
                    } else if (size == 9 && value == 0 && crossout.ContainsKey(new Point(i, j)) && crossout[new Point(i, j)].Count > 0) {
                        spriteBatch.End();
                        spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.Additive);
                        for (int num = 1; num <= SIZE; ++num) {
                            if (crossout[new Point(i, j)].Contains(num) == false) {
                                Rectangle rect = tileArray[3, num - 1];
                                spriteBatch.Draw(tileMap, dest[i, j], rect, Color.White);
                            }
                        }
                        spriteBatch.End();
                        spriteBatch.Begin();
                    // If drawing a crossout tile, draw the tile in dark grey.
                    } else if (size == 3 && crossout.ContainsKey(selectedTile) && crossout[selectedTile].Contains(value)) {
                        Rectangle rect = tileArray[0, value - 1];
                        spriteBatch.Draw(tileMap, dest[i, j], rect, Color.DimGray);
                    // Else, draw the tile normally.
                    } else if (value != 0 && (size == 9 && curBoard.Original[i, j] == 0) || (size == 3)) {
                        Rectangle rect = tileArray[0, value - 1];
                        spriteBatch.Draw(tileMap, dest[i, j], rect, Color.White);
                    }
                }
            }
        }

        private void expand(Texture2D image, Rectangle dest, Rectangle source, int depth) {
            float scale = 1.15f;
            int xCoor = (int)(dest.X - (((scale - 1) / 2) * source.Width));
            int yCoor = (int)(dest.Y - (((scale - 1) / 2) * source.Height));
            spriteBatch.Draw(image, new Vector2(xCoor, yCoor), source, Color.White, 
                0f, Vector2.Zero, scale, SpriteEffects.None, depth);
        }
    }
}
