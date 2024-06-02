using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Cosmic_Labirynth.Sprites
{
    public class Bullet : Sprite
    {
        private float _timer = 0;
        private Vector2 VelocityTMP;
        public event EventHandler OnEnemyDeath;
        public override Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, 5 * (int)Scale, 5 * (int)Scale);
            }
        }

        public Bullet(Texture2D texture)
          : base(texture)
        {

        }

        public override void SetMapMove(Vector2 moveVector) // ustawienie kierunku w którym ma poruszać się objekt
        {
            VelocityTMP += -moveVector;
        }

        public override void SetEntityMove(List<Sprite> sprites)
        {
            _timer++;

            // usuwanie jeśli skończy się życie
            if (_timer >= LifeSpan)
                IsRemoved = true;

            // sprawdzanie kolizji ze ścianami i przeciwnikami i usuwanie przeciwników jeśli ich HP zejdzie do 0
            foreach (var sprite in sprites)
            {
                if (sprite == this)
                    continue;
                if (sprite.IsEnemy)
                {
                    if (this.IsTouchingBottom(sprite) || this.IsTouchingLeft(sprite) || this.IsTouchingRight(sprite) || this.IsTouchingTop(sprite) || this.Rectangle.Intersects(sprite.Rectangle))
                    {
                        (sprite as Enemy).HP--;
                        if ((sprite as Enemy).HP <= 0)
                        {
                            (sprite as Enemy).IsRemoved = true;
                            OnEnemyDeath?.Invoke(this, EventArgs.Empty);
                        }
                            IsRemoved = true;
                    }
                }
                else if(sprite.IsMap && sprite.Collision)
                {
                    if (this.IsTouchingBottom(sprite) || this.IsTouchingLeft(sprite) || this.IsTouchingRight(sprite) || this.IsTouchingTop(sprite) || this.Rectangle.Intersects(sprite.Rectangle))
                    {
                        IsRemoved = true;
                    }
                }
            }
        }
        public override void Update(GameTime gameTime, List<Sprite> sprites)
        {
            Position += Velocity + VelocityTMP; // zmiana pozycji z uwzględnieniem ruchu mapy
            PositionOnMap += Velocity;
            VelocityTMP = Vector2.Zero; // zerowanie ruchu mapy jesli mapa przestała się ruszać
        }
    }
}
