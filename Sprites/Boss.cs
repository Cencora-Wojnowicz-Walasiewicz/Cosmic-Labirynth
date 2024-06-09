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

        private Vector2 ClearVelocity = Vector2.Zero;

        private float frameCounter = 0.0f;

        int MovementCounter = 60;
        Random random = new Random();
        public Vector2 preVelocity;
        int ChaseRadius = 200;
        int Chasing = 0;

        public float Scale = 3.0f;
        public int damage = 10;

        public override Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, 32 * (int)Scale, 32 * (int)Scale);
            }
        }

        public Boss(Texture2D texture, Texture2D textureTransition1, Texture2D textureTransition2, Vector2 position) : base(texture)
        {
            Position = position;
            PositionOnMap = position;
            IsEnemy = true;

            _textureNormal = texture;
            _textureTransition1 = textureTransition1;
            _textureTransition2 = textureTransition2;
            HP = 300;
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
                if (HP < 200 && HP >= 100)
                {
                    _texture = _textureTransition1;
                    damage = 20;
                }
                else if (HP < 100)
                {
                    _texture = _textureTransition2;
                    damage = 30;
                }
                else
                {
                    _texture = _textureNormal;
                    damage = 10;
                }
            }
            else
            {
                Chasing = 0;
            }
        }
    }
}
