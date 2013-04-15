using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Game {
    public class TileArray {
        private Texture2D tileMap;
        private int numRow;
        private int numCol;
        public Rectangle[,] TilePos { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }

        public TileArray(Texture2D newTileMap, int newRow, int newCol) {
            tileMap = newTileMap;
            numRow = newRow;
            numCol = newCol;
            TilePos = new Rectangle[numRow, numCol];

            Height = tileMap.Height / numRow;
            Width = tileMap.Width / numCol;

            for (int curRow = 0; curRow < numRow; ++curRow) {
                for (int curCol = 0; curCol < numCol; ++curCol) {
                    TilePos[curRow, curCol] =
                        new Rectangle(Width * curCol, Height * curRow, Width, Height);
                }
            }
        }

        public Rectangle this[int x, int y] {
            get { return TilePos[x, y]; }
            set { TilePos[x, y] = value; }
        }
    }
}
