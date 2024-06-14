using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;
using System.ComponentModel.Design;
using System.Security.Cryptography.X509Certificates;
using System.Diagnostics;
using Cosmic_Labirynth.GameStates;
using Microsoft.Xna.Framework.Audio;

namespace Cosmic_Labirynth.Sprites
{
    public class Player : Sprite
    {
        //////////////////////// PARAMETRS //////////////////////////
        #region Parametrs
        public int HP;
        #endregion

        //////////////////////////// PROPERTIES //////////////////////
        #region Properties 
        public int Rows = 4;
        public int Columns = 3;
        private int currentFrame;
        private int currentRow;
        private int baseFrame = 1;
        private float frameCounter = 0.0f;
        private bool Switcher = true; // parametr od animacji
        Vector2 PositionOnMapTMP = Vector2.Zero;
        public int AttackDelay = 0;
        public int Score = 0;
        public Bullet Bullet;

        private SoundEffect _deathSoundPlayer;

        private SoundEffect _blasterSound;

        public event EventHandler OnEnemyCollision;
        public event EventHandler OnEnoughScore;
        public event EventHandler OnBossBulletHit;

        public override Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, 32 * (int)Scale, 32 * (int)Scale);
            }
        }
        #endregion
        public Player(Texture2D texture, Vector2 position, SoundEffect blasterSound, SoundEffect deathSoundPlayer) : base(texture)
        {
            Position = position;
            PositionOnMap = position;
            PositionOnMapTMP = position;
            _blasterSound = blasterSound;
            _deathSoundPlayer = deathSoundPlayer;
        }
       //public Player(Texture2D texture) : base(texture) {}

        //////////////////////////////// METHODS //////////////////////////////////////
        #region Methods
        public override void Update(GameTime gameTime, List<Sprite> sprites)
        {
            _previousKey = _currentKey;
            _currentKey = Keyboard.GetState();

            if (_currentKey.IsKeyDown(Input.Fire) &&
                _previousKey.IsKeyUp(Input.Fire))
            {
                _blasterSound.Play();
                AddBullet(sprites);
            }

            if (PositionOnMap != PositionOnMapTMP)
            {
                if (frameCounter >= 20)
                {
                    frameCounter = 0;
                    NextFrame();
                }
                frameCounter += Speed;
                PositionOnMapTMP =  PositionOnMap;
            }
            else
            {
                currentFrame = baseFrame;
            }
            Position += Velocity;
            Velocity = Vector2.Zero;
        }
        private void AddBullet(List<Sprite> sprites)
        {
            var bullet = Bullet.Clone() as Bullet;
            bullet.Direction = this.Direction;
            bullet.Position = this.Position + new Vector2(16* Scale, 16* Scale);
            bullet.PositionOnMap = this.PositionOnMap + new Vector2(16 * Scale, 16 * Scale);
            bullet.Velocity = this.Direction * 10;
            bullet.LifeSpan = 300;
            bullet.Parent = this;
            bullet.OnEnemyDeath += Bullet_OnEnemyDeath;
            sprites.Add(bullet);

            

        }

        private void Bullet_OnEnemyDeath(object sender, EventArgs e)
        {
            Score++;
            if (Score >= 5)
            {
                OnEnoughScore?.Invoke(this, EventArgs.Empty);
            }
        }

        public void TakeDamage(int damage)
        {
            HP -= damage;
            OnBossBulletHit?.Invoke(this, EventArgs.Empty);
            Debug.WriteLine($"Player took {damage} damage. Remaining HP: {HP}");
            if (HP <= 0)
            {
                IsRemoved = true;
                _deathSoundPlayer.Play();
                Debug.WriteLine("Player removed after HP dropped to 0.");
                // Trigger game over or any other logic for player death
                
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            int width = _texture.Width / Columns;
            int height = _texture.Height / Rows;
            int row = currentRow; // kierunek animacji
            int column = currentFrame; // klatka animacji

            Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);
            spriteBatch.Draw(_texture, Position, sourceRectangle, Color.White, 0, new Vector2(0, 0), Scale, SpriteEffects.None, 1.0f);
        }
        
        private void NextFrame()
        {
            switch (currentFrame)
            {
                case 0:
                    {
                        currentFrame++;
                        break;
                    }
                case 1:
                    {
                        if (Switcher)
                        {
                            currentFrame++;
                            Switcher = false;
                        }
                        else
                        {
                            currentFrame--;
                            Switcher = true;
                        }
                        break;
                    }
                case 2:
                    {
                        currentFrame--;
                        break;
                    }
            }
        }

        public override void SetEntityMove(List<Sprite> sprites) // ustawienie kierunku w którym ma poruszać się objekt
        {
            // wyznaczenie kierunku ruchu gracza na podstawie klawisza // tu by było miejsce na wyznaczenie ruchu dla NPC jeśli by były
            if (Keyboard.GetState().IsKeyDown(Input.Left))
            { 
                Velocity.X = -Speed;
                currentRow = 3;
                _rotation = MathHelper.Pi;
            } 
            else if (Keyboard.GetState().IsKeyDown(Input.Right))
            { 
                Velocity.X = Speed;
                currentRow = 1;
                _rotation = 0; // 0 degrees (right)
            }
            
            if (Keyboard.GetState().IsKeyDown(Input.Up))
            {
                Velocity.Y = -Speed;
                currentRow = 0;
                _rotation = -MathHelper.PiOver2; // -90 degrees (up)
            } 
            else if (Keyboard.GetState().IsKeyDown(Input.Down))
            {
                Velocity.Y = Speed;
                currentRow = 2;
                _rotation = MathHelper.PiOver2; // 90 degrees (down)
            }

            // przy przytrzymaniu klawisza HoldDirection (LShift) blokuje zmianę kierunku strzału
            if (!Keyboard.GetState().IsKeyDown(Input.HoldDirection))
            {
                Direction = new Vector2((float)Math.Cos(_rotation), (float)Math.Sin(_rotation));
            }

            // sprawdzanie kolizji
            foreach (var sprite in sprites)
            {
                if (sprite == this) // ignorowanie kolizji jeśli  sprite to gracz
                    continue;
                if (sprite is Bullet) // ignorowanie kolizji jeśli sprite to Bullet
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
        public override void MoveEntity(List<Sprite> sprites, int screenWidth, int screenHeight, Rectangle map) // wykonanie ruchu na mapie i korekta mapy
        {
            if (Velocity != Vector2.Zero) // sprawdzenie czy się rusza
            {
                // RUCH USTALANY DLA KAŻDEGO KIERUNKU ODDZIELNIE
                Vector2 VelocityTMP = new Vector2(0, 0);
                //LEWO
                if (Velocity.X < 0)
                {
                    if (Position.X > BorderDistance)
                    {
                        //ruch swobodny po środku
                        PositionOnMap.X += Velocity.X;
                    }
                    else if (Position.X <= BorderDistance && PositionOnMap.X > Position.X)
                    {
                        // ruch mapy
                        VelocityTMP.X += Velocity.X;
                        PositionOnMap.X += Velocity.X;
                        Velocity.X = 0;
                    }
                    else if (Position.X <= BorderDistance && PositionOnMap.X <= Position.X)
                    {
                        //ruch swobodny po krawędzi mapy
                        PositionOnMap.X += Velocity.X;
                    }
                }
                //PRAWO
                else if (Velocity.X > 0)
                {
                    if (screenWidth - (Position.X) > BorderDistance + (32 * Scale))
                    {
                        //ruch swobodny po środku
                        PositionOnMap.X += Velocity.X;
                    }
                    else if (screenWidth - (Position.X) <= BorderDistance + (32 * Scale) && map.Width - (PositionOnMap.X) > screenWidth - (Position.X))
                    {
                        // ruch mapy
                        VelocityTMP.X += Velocity.X;
                        PositionOnMap.X += Velocity.X;
                        Velocity.X = 0;
                    }
                    else if (screenWidth - (Position.X) <= BorderDistance + (32 * Scale) && map.Width - (PositionOnMap.X) <= screenWidth - (Position.X))
                    {
                        //ruch swobodny po krawędzi mapy
                        PositionOnMap.X += Velocity.X;
                    }
                }
                //GÓRA
                if (Velocity.Y < 0)
                {
                    if (Position.Y > BorderDistance)
                    {
                        //ruch swobodny po środku
                        PositionOnMap.Y += Velocity.Y;
                    }
                    else if (Position.Y <= BorderDistance && PositionOnMap.Y > Position.Y)
                    {
                        // ruch mapy
                        VelocityTMP.Y += Velocity.Y;
                        PositionOnMap.Y += Velocity.Y;
                        Velocity.Y = 0;
                    }
                    else if (Position.Y <= BorderDistance && PositionOnMap.Y <= Position.Y)
                    {
                        //ruch swobodny po krawędzi mapy
                        PositionOnMap.Y += Velocity.Y;
                    }
                }
                //DÓŁ
                else if (Velocity.Y > 0)
                {
                    if (screenHeight - (Position.Y) > BorderDistance + (32 * Scale))
                    {
                        //ruch swobodny po środku
                        PositionOnMap.Y += Velocity.Y;
                    }
                    else if (screenHeight - (Position.Y) <= BorderDistance + (32 * Scale) && map.Height - (PositionOnMap.Y) > screenHeight - (Position.Y))
                    {
                        // ruch mapy
                        VelocityTMP.Y += Velocity.Y;
                        PositionOnMap.Y += Velocity.Y;
                        Velocity.Y = 0;
                    }
                    else if (screenHeight - (Position.Y) <= BorderDistance + (32 * Scale) && map.Height - (PositionOnMap.Y) <= screenHeight - (Position.Y))
                    {
                        //ruch swobodny po krawędzi mapy
                        PositionOnMap.Y += Velocity.Y;
                    }
                }

                foreach (Sprite sprite in sprites)
                {
                    if (sprite != this)
                        sprite.SetMapMove(VelocityTMP);
                }

            }

        }
        #endregion
        //////////////////////////////////////////////////////////// EVENTS ///////////////////////////////////////////////////////////////
        #region Event Methods
        public override void EventChecker(List<Sprite> sprites)
        {
            if(AttackDelay > 0) AttackDelay--;
                
            foreach (var sprite in sprites)
            {
                if (sprite == this)
                    continue;
                if(sprite.IsEnemy)
                {
                    if(this.IsTouchingBottom(sprite) || this.IsTouchingLeft(sprite) || this.IsTouchingRight(sprite) || this.IsTouchingTop(sprite))
                    {
                        OnEnemyCollision?.Invoke(this, EventArgs.Empty);
                    }
                }
            }
        }

        public override void EventExecuter()
        {
            //
        }
        public void EventExecuter(GraphicsDevice graphicsDevice)
        {
            Debug.Print("Wykryto śmierć gracza!");
            GameStateManager.Instance.ChangeScreen(new GameOverGameState(graphicsDevice));
        }
        #endregion
    }
}
