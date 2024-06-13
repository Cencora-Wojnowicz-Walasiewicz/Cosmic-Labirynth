using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework.Audio;

namespace Cosmic_Labirynth.Sprites
{
    public class Bullet : Sprite
    {
        private float _timer = 0;
        private Vector2 VelocityTMP;
        public event EventHandler OnEnemyDeath;

        //private SoundEffect _deathSound;

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
           // _deathSound = deathSound;
        }

        public override void SetMapMove(Vector2 moveVector)
        {
            VelocityTMP += -moveVector;
        }

        public override void SetEntityMove(List<Sprite> sprites)
        {
            _timer++;

            // Remove if life span is exceeded
            if (_timer >= LifeSpan)
            {
                IsRemoved = true;
                Debug.WriteLine("Bullet removed due to lifespan.");
            }

            // Collision detection with enemies and map objects
            foreach (var sprite in sprites)
            {
                if (sprite == this)
                    continue;

                if (sprite.IsEnemy && (IsTouching(sprite) || Rectangle.Intersects(sprite.Rectangle)))
                {
                    if (sprite is Enemy enemy)
                    {
                        enemy.HP--;
                        Debug.WriteLine($"Hit enemy. Remaining HP: {enemy.HP}");

                        if (enemy.HP <= 0)
                        {
                            enemy.IsRemoved = true;
                            OnEnemyDeath?.Invoke(this, EventArgs.Empty);
                            Debug.WriteLine("Enemy removed.");
                        }

                        IsRemoved = true;
                        Debug.WriteLine("Bullet removed after hitting enemy.");
                    }
                    else if (sprite is Boss boss)
                    {
                        boss.HP--;
                        Debug.WriteLine($"Hit boss. Remaining HP: {boss.HP}");

                        if (boss.HP <= 0)
                        {
                            boss.IsRemoved = true;
                            OnEnemyDeath?.Invoke(this, EventArgs.Empty);
                            //_deathSound?.Play();
                            Debug.WriteLine("Boss removed.");
                        }

                        IsRemoved = true;
                        Debug.WriteLine("Bullet removed after hitting boss.");
                    }
                    break;
                }
                else if (sprite.IsMap && sprite.Collision && (IsTouching(sprite) || Rectangle.Intersects(sprite.Rectangle)))
                {
                    IsRemoved = true;
                    Debug.WriteLine("Bullet removed after hitting map.");
                    break;
                }
            }
        }

        public override void Update(GameTime gameTime, List<Sprite> sprites)
        {
            Position += Velocity + VelocityTMP; // Update position with velocity
            PositionOnMap += Velocity;
            VelocityTMP = Vector2.Zero; // Reset temporary velocity
        }

        // Helper method to check for collisions with other sprites
        private bool IsTouching(Sprite sprite)
        {
            return this.IsTouchingLeft(sprite) || this.IsTouchingRight(sprite) ||
                   this.IsTouchingTop(sprite) || this.IsTouchingBottom(sprite);
        }
    }
}
