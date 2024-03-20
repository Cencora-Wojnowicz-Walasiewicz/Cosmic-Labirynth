using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosmic_Labirynth.Misc
{
    public class MapHandler
    {
        public Rectangle GetSourceRectangle(Texture2D texture, int TileRows, int TileCols, int MapRows, int MapCols, int TileType)
        {
            int TileWidth = texture.Width / TileCols;
            int TileHeight = texture.Height / TileRows;
            int column = GetColumn(TileType, TileCols);
            int row = GetRow(TileType, TileCols);
            Rectangle rectangle = new Rectangle(TileWidth * column, TileHeight * row, TileWidth, TileHeight);
            return rectangle;
        }

        public Vector2 GetPosition(Texture2D texture, int TileRows, int TileCols, int MapRows, int MapCols, int TileNumber, float Scale)
        {
            int TileWidth = texture.Width / TileCols;
            int TileHeight = texture.Height / TileRows;
            int currentRow = GetRow(TileNumber, MapCols);
            Vector2 position = new Vector2((TileNumber - (currentRow * MapCols)) * TileWidth * Scale,
                        currentRow * TileHeight * Scale);
            return position;
        }

        #region Functions
        private int GetColumn(int tileNumber, int columns)
        {
            int tmp = tileNumber;
            while (tmp - columns >= 0)
                tmp -= columns;
            return tmp;
        }

        private int GetRow(int tileNumber, int columns)
        {
            int tmp = tileNumber;
            int counter = 0;
            while (tmp - columns >= 0)
            {
                counter++;
                tmp -= columns;
            }
            return counter;
        }
        #endregion
    }
}
