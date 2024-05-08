using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace Cosmic_Labirynth.Sprites
{
    public class MapSprite : Sprite
    {
        Rectangle _sourceRectangle;
        public override Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, 32 * (int)Scale, 32 * (int)Scale);
            }
        }

        public MapSprite(Texture2D texture, Rectangle sourceRectangle, Vector2 position, bool collision, float scale) : base(texture)
        {
            _sourceRectangle = sourceRectangle; // wyznacznik który element tilesetu
            Position = position; // miejsce elementu
            Collision = collision;
            Scale = scale;
            PositionOnMap = position;
            IsMap = true;

        }

        public override void Update(GameTime gameTime, List<Sprite> sprites)
        {
            Position += Velocity;
            Velocity = Vector2.Zero;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, _sourceRectangle, Color.White, 0, new Vector2(0, 0), Scale, SpriteEffects.None, 1.0f);
        }


        public override void SetMapMove(Vector2 moveVector) // ustawienie kierunku w którym ma poruszać się objekt
        {
            Velocity = -moveVector;
        }

        /*public override void Move() // zmiana pozycji na mapie objektu
        {
            PositionOnMap += Velocity;
            Velocity = Vector2.Zero;
        }*/

        public override void Movement() // ustawienie pozycji z mapy do pozycji wyświetlanej
        {
            //
        }

    }
}
