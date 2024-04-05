using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosmic_Labirynth.Sprites
{
    public class Enemy : Sprite
    {
        public override Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, 32 * (int)Scale, 32 * (int)Scale);
            }
        }
        public Enemy(Texture2D texture, Vector2 position) : base(texture)
        {
            Position = position;
            PositionOnMap = position;
        }

        public override void Update(GameTime gameTime, List<Sprite> sprites)
        {
            Position += Velocity;
            Velocity = Vector2.Zero;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, null, Color.White, 0, new Vector2(0, 0), Scale, SpriteEffects.None, 1.0f);
        }

        public override void SetMapMove(Vector2 moveVector) // ustawienie kierunku w którym ma poruszać się objekt
        {
            Velocity = -moveVector;
        }
        public override void Move() // zmiana pozycji na mapie objektu
        {
            PositionOnMap += Velocity;
            Velocity = Vector2.Zero;
        }

        public override void SetEntityMove(List<Sprite> sprites) // ustawienie kierunku w którym ma poruszać się objekt
        {
            // wyznaczenie kierunku ruchu gracza na podstawie klawisza // tu by było miejsce na wyznaczenie ruchu dla NPC jeśli by były


            // sprawdzanie kolizji
            foreach (var sprite in sprites)
            {
                if (sprite == this)
                    continue;
                if (sprite.Collision)
                {
                    if ((this.Velocity.X > 0 && this.IsTouchingLeft(sprite)) || (this.Velocity.X < 0 && this.IsTouchingRight(sprite)))
                        this.Velocity.X = 0;

                    if ((this.Velocity.Y > 0 && this.IsTouchingTop(sprite)) || (this.Velocity.Y < 0 && this.IsTouchingBottom(sprite)))
                        this.Velocity.Y = 0;
                }
            }
        }

    }
}

    