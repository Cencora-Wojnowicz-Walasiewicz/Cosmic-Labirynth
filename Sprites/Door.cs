using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosmic_Labirynth.Sprites
{
    public class Door : Sprite
    {
        public int HP;

        public int Rows = 1;
        public int Columns = 3;
        private int currentFrame;
        private int currentFrameCounter = 0;
        private int currentRow = 0;

        public Texture2D _textureNormal;

        public bool active = false;
        public event EventHandler OnDoorTouch;


        private float frameCounter = 0.0f;

        public Vector2 preVelocity;

        public override Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, 32 * (int)Scale, 32 * (int)Scale);
            }
        }
        public Door(Texture2D texture, Vector2 position) : base(texture)
        {
            Position = position;
            PositionOnMap = position;
            IsEnemy = false;
            Collision = false;
            _textureNormal = texture;
        }
        public override void Update(GameTime gameTime, List<Sprite> sprites)
        {
            if (active)
            {
                if (frameCounter >= 20)
                {
                    frameCounter = 0;
                    NextFrame();
                }
                frameCounter += 1.0f;

                foreach (Sprite sprite in sprites)
                {
                    if (sprite is Player)
                    {
                        if((sprite as Player).Rectangle.Intersects(this.Rectangle)) 
                        {
                            OnDoorTouch?.Invoke(this, EventArgs.Empty);
                        }
                    }else continue;

                }
            }

            PositionOnMap += Velocity; // ruch po mapie bez uwzględnienia ruchu mapy
            Position += Velocity;
            Velocity = Vector2.Zero;
        }

        private void NextFrame()
        {
            switch (currentFrameCounter)
            {
                case 0:
                    {
                        currentFrameCounter++;
                        currentFrame = 1;
                        break;
                    }
                case 1:
                    {
                        currentFrameCounter++;
                        currentFrame = 2;
                        break;
                    }
                case 2:
                    {
                        currentFrameCounter++;
                        currentFrame = 1;
                        break;
                    }
                case 3:
                    {
                        currentFrameCounter = 0;
                        currentFrame = 0;
                        break;
                    }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (active)
            {
                int width = _texture.Width / Columns;
                int height = _texture.Height / Rows;
                int row = currentRow; // kierunek animacji
                int column = currentFrame; // klatka animacji

                Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);
                spriteBatch.Draw(_texture, Position, sourceRectangle, Color.White, 0, new Vector2(0, 0), Scale, SpriteEffects.None, 1.0f);
            }
        }

        public override void SetMapMove(Vector2 moveVector) // ustawienie kierunku w którym ma poruszać się objekt
        {
            Velocity += -moveVector;
        }

        
        public override void EventChecker(List<Sprite> sprites)
        {
            //
        }
    }
}
