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
        int MovementCounter = 60;
        Random random = new Random();

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
            Position += Velocity;
            Velocity = Vector2.Zero;
        }

        public override void SetEntityMove(List<Sprite> sprites) // ustawienie kierunku w którym ma poruszać się objekt
        {
            // wyznaczenie kierunku ruchu gracza na podstawie klawisza // tu by było miejsce na wyznaczenie ruchu dla NPC jeśli by były
            MovementCounter++;
            if(MovementCounter > 60)
            {
                int rand1 = random.Next(3);
                int rand2 = random.Next(3);
                MovementCounter = 0;
                if(rand1 == 0) { preVelocity.X = -Speed;  } else if(rand1 == 1) {  preVelocity.X = 0; } else if(rand1 == 2) {  preVelocity.X = Speed; }
                if (rand2 == 0) { preVelocity.Y = -Speed; } else if (rand2 == 1) { preVelocity.Y = 0; } else if (rand2 == 2) { preVelocity.Y = Speed; }
            }

            //random.Next(3);

          



            // sprawdzanie kolizji
            foreach (var sprite in sprites)
            {
                if (sprite == this)
                    continue;
                if (sprite.Collision)
                {
                    if ((this.preVelocity.X > 0 && this.IsTouchingLeft(sprite)) || (this.preVelocity.X < 0 && this.IsTouchingRight(sprite)))
                        this.preVelocity.X = 0;

                    if ((this.preVelocity.Y > 0 && this.IsTouchingTop(sprite)) || (this.preVelocity.Y < 0 && this.IsTouchingBottom(sprite)))
                        this.preVelocity.Y = 0;
                }
            }
            Velocity += preVelocity;
        }

    }
}

    