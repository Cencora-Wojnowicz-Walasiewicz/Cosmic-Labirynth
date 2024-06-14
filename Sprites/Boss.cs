using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Cosmic_Labirynth.GameStates;

namespace Cosmic_Labirynth.Sprites
{
    public class Boss : Sprite
    {
        public int HP;

        public int Rows = 4;
        public int Columns = 3;
        private int currentFrame;
        private int currentFrameCounter = 0;
        private int currentRow = 2;
        private int baseFrame = 1;
        Vector2 PositionOnMapTMP = Vector2.Zero;

        public Texture2D _textureNormal;
        public Texture2D _textureAngry;
        public Texture2D _textureTransition1;
        public Texture2D _textureTransition2;
        public Texture2D _bulletTexture;

        private Vector2 ClearVelocity = Vector2.Zero;

        private float frameCounter = 0.0f;
        private float shootCooldown = 0.0f;
        private float shootInterval = 60.0f;

        int MovementCounter = 60;
        Random random = new Random();
        public Vector2 preVelocity;
        int ChaseRadius = 200;
        int Chasing = 0;

        public new float Scale = 10.0f;
        public int damage = 10;

        public event EventHandler OnBossDeath;

        public List<Bullet> BossBullets { get; private set; } = new List<Bullet>();

        public override Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, 32 * (int)Scale, 32 * (int)Scale);
            }
        }

        public Boss(Texture2D texture, Texture2D textureTransition1, Texture2D textureTransition2, Texture2D bulletTexture, Vector2 position) : base(texture)
        {
            Position = position;
            PositionOnMap = position;
            IsEnemy = true;

            _textureNormal = texture;
            _textureTransition1 = textureTransition1;
            _textureTransition2 = textureTransition2;
            _bulletTexture = bulletTexture;
            HP = 30;
        }

        public override void Update(GameTime gameTime, List<Sprite> sprites)
        {
            if (PositionOnMap != PositionOnMapTMP)
            {
                if (frameCounter >= 20)
                {
                    frameCounter = 0;
                    NextFrame();
                }
                frameCounter += Speed;
                PositionOnMapTMP = PositionOnMap;
            }
            else
            {
                currentFrame = baseFrame;
                currentFrameCounter = baseFrame;
            }

            PositionOnMap += ClearVelocity;
            Position += Velocity;
            Velocity = Vector2.Zero;
            ClearVelocity = Vector2.Zero;

            if (Chasing == 1)
            {
                shootCooldown += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (shootCooldown >= shootInterval / 60.0f)
                {
                    ShootAtPlayer(sprites.OfType<Player>().FirstOrDefault());
                    shootCooldown = 0.0f;
                }
            }

            var player = sprites.OfType<Player>().FirstOrDefault();
            for (int i = 0; i < BossBullets.Count; i++)
            {
                var bullet = BossBullets[i];
                bullet.Update(gameTime, sprites);

                if (bullet.IsRemoved)
                {
                    BossBullets.RemoveAt(i);
                    i--;
                }
                else if (player != null && bullet.Rectangle.Intersects(player.Rectangle))
                {
                    player.TakeDamage(damage);
                    bullet.IsRemoved = true;
                    Debug.WriteLine("Player hit by boss bullet.");
                }
            }
            // Update texture based on HP
            if (HP > 20)
            {
                _texture = _textureNormal;
            }
            else if (HP > 10)
            {
                _texture = _textureTransition2;
            }
            else
            {
                _texture = _textureTransition1;
            }

            // Check for HP and handle death
            if (HP <= 0)
            {
                IsRemoved = true;

                OnBossDeath?.Invoke(this, EventArgs.Empty);

                Debug.WriteLine("Boss removed after HP dropped to 0.");
            }
        }


        private void ShootAtPlayer(Player player)
        {
            Vector2 direction = Vector2.Normalize(player.Position - this.Position);

            // Main bullet
            Bullet mainBullet = new Bullet(_bulletTexture)
            {
                Position = this.Position + new Vector2(16*Scale,16*Scale),
                Velocity = direction * 5f,
                LifeSpan = 100
            };
            BossBullets.Add(mainBullet);

            if (HP <= 5)
            {
                // Left bullet
                float angle = MathHelper.ToRadians(15); // 15 degrees to the left
                Vector2 leftDirection = Vector2.Transform(direction, Matrix.CreateRotationZ(angle));
                Bullet leftBullet = new Bullet(_bulletTexture)
                {
                    Position = this.Position + new Vector2(16 * Scale, 16 * Scale),
                    Velocity = leftDirection * 5f,
                    LifeSpan = 100
                };
                BossBullets.Add(leftBullet);

                // Right bullet
                angle = MathHelper.ToRadians(-15); // 15 degrees to the right
                Vector2 rightDirection = Vector2.Transform(direction, Matrix.CreateRotationZ(angle));
                Bullet rightBullet = new Bullet(_bulletTexture)
                {
                    Position = this.Position + new Vector2(16 * Scale, 16 * Scale),
                    Velocity = rightDirection * 5f,
                    LifeSpan = 100
                };
                BossBullets.Add(rightBullet);
            }
        }

        private void NextFrame()
        {
            switch (currentFrameCounter)
            {
                case 0:
                    currentFrameCounter++;
                    currentFrame = 1;
                    break;
                case 1:
                    currentFrameCounter++;
                    currentFrame = 2;
                    break;
                case 2:
                    currentFrameCounter++;
                    currentFrame = 1;
                    break;
                case 3:
                    currentFrameCounter = 0;
                    currentFrame = 0;
                    break;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            int width = _texture.Width / Columns;
            int height = _texture.Height / Rows;
            int row = currentRow;
            int column = currentFrame;

            Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);
            spriteBatch.Draw(_texture, Position, sourceRectangle, Color.White, 0, new Vector2(0, 0), Scale, SpriteEffects.None, 1.0f);

            foreach (var bullet in BossBullets)
            {
                bullet.Draw(spriteBatch);
            }
        }

        public override void SetMapMove(Vector2 moveVector)
        {
            Velocity += -moveVector;
        }

        public override void SetEntityMove(List<Sprite> sprites)
        {
            if (Chasing == 1)
            {
                if (_texture != _textureAngry) { _texture = _textureAngry; }
                Player player = sprites.OfType<Player>().FirstOrDefault();
                Vector2 directionToPlayer = player.Position - this.Position;
                float distanceToPlayer = directionToPlayer.Length();
                preVelocity = Vector2.Normalize(directionToPlayer) * Speed;
            }
            else
            {
                if (_texture != _textureNormal) { _texture = _textureNormal; }
                MovementCounter++;
                if (MovementCounter > 60)
                {
                    int rand1 = random.Next(3);
                    int rand2 = random.Next(3);
                    MovementCounter = 0;
                    if (rand1 == 0) { preVelocity.X = -Speed; } else if (rand1 == 1) { preVelocity.X = 0; } else if (rand1 == 2) { preVelocity.X = Speed; }
                    if (rand2 == 0) { preVelocity.Y = -Speed; } else if (rand2 == 1) { preVelocity.Y = 0; } else if (rand2 == 2) { preVelocity.Y = Speed; }
                }
            }

            if (preVelocity.X < 0)
                currentRow = 3;
            else if (preVelocity.X > 0)
                currentRow = 1;
            if (preVelocity.Y < 0 && preVelocity.Y <= preVelocity.X)
                currentRow = 0;
            else if (preVelocity.Y > 0 && preVelocity.Y >= preVelocity.X)
                currentRow = 2;

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
            ClearVelocity += preVelocity;
        }

        public override void EventChecker(List<Sprite> sprites)
        {
            Player player = sprites.OfType<Player>().FirstOrDefault();
            Vector2 directionToPlayer = player.Position - this.Position;
            float distanceToPlayer = directionToPlayer.Length();

            if (distanceToPlayer < ChaseRadius)
            {
                Chasing = 1;
                if (HP < 20 && HP >= 10)
                {
                    _texture = _textureTransition1;
                    damage = 1;
                }
                else if (HP < 10)
                {
                    _texture = _textureTransition1;
                    damage = 1;
                }
                else
                {
                    _texture = _textureTransition2;
                    damage = 1;
                }
            }
            else
            {
                Chasing = 0;
            }
        }
    }
}