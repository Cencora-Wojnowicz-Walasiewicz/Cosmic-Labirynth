using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cosmic_Labirynth.Sprites;
using Cosmic_Labirynth.Misc;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using System.Diagnostics;
using System.IO.IsolatedStorage;
using System.Reflection.Metadata;
using System.Net.Mail;
using Microsoft.Xna.Framework.Audio;


namespace Cosmic_Labirynth.GameStates
{
    public class InGameGameState : GameState
    {
        ///////////////////////////////////////////////////   DECLARATIONS   /////////////////////////////////////////////

        private List<Sprite> _sprites;
        private MapHandler _mapHandler = new MapHandler();
        private float _Scale = 2f;
        private ContentManager _content;
        private int mapNumber = 1;

        private SoundEffect _blasterSound;

        private SoundEffect _deathSound;

       

        private Input _Input;

        private Rectangle Map;

        private int _screenHeight;
        private int _screenWidth;
        Player player;
        PlayerLifeInterface _playerLife;
        private SpriteFont _scoreFont;
        private SoundEffect _deathSoundPlayer;

        public InGameGameState(GraphicsDevice graphicsDevice) : base(graphicsDevice)
        {
            _screenHeight = GameStateManager.Instance._graphics.PreferredBackBufferHeight;
            _screenWidth = GameStateManager.Instance._graphics.PreferredBackBufferWidth;
        }

        ///////////////////////////////////////////////////   INITIALIZATION AND LOADING   /////////////////////////////////////////////
        #region Initialisation and Loading
        public override void Initialize()
        {
            _Input = new Input
            {
                Up = Keys.Up,
                Right = Keys.Right,
                Down = Keys.Down,
                Left = Keys.Left,
                Fire = Keys.Space,
                HoldDirection = Keys.LeftShift
            };
        }

        public override void LoadContent(ContentManager content)
        {
            // Wczytanie danych mapy
            //JObject mdata1 = JObject.Parse(File.ReadAllText(@"..\..\..\Maps\map1.json"));
            //JObject mdata2 = JObject.Parse(File.ReadAllText(@"..\..\..\Maps\map2.json"));

            JObject mdata = JObject.Parse(File.ReadAllText(@"..\..\..\Maps\map1.json"));
            if (mapNumber == 2) mdata = JObject.Parse(File.ReadAllText(@"..\..\..\Maps\map2.json"));
            MapData mapData = JsonConvert.DeserializeObject<MapData>(mdata.ToString());


            _blasterSound = content.Load<SoundEffect>(@"sound/blaster");

            _deathSound = content.Load<SoundEffect>(@"sound/death1");

            _deathSoundPlayer = content.Load<SoundEffect>(@"sound/death");

            var playerTexture = content.Load<Texture2D>("Tilesets/Player1");
            var mapTileset = content.Load<Texture2D>(mapData.tilesetName);
            var enemyTexture = content.Load<Texture2D>("Tilesets/Enemy1");
            var enemyTextureAlt = content.Load<Texture2D>("Tilesets/Enemy1alt");
            var heartTexture = content.Load<Texture2D>("Tilesets/heart");
            var doorTexture = content.Load<Texture2D>("Tilesets/Door");
            var bossTexture = content.Load<Texture2D>("Boss5"); // Use an existing texture or a placeholder
            var bossTextureTransition1 = content.Load<Texture2D>("Tilesets/Boss3"); // Use an existing texture or a placeholder
            var bossTextureTransition2 = content.Load<Texture2D>("Boss4"); // Use an existing texture or a placeholder
            var bulletTexture = content.Load<Texture2D>("Bullet"); // Use an existing texture or a placeholder
            var bossTextureAngry = content.Load<Texture2D>("Tilesets/Enemy1alt"); // Use an existing texture or a placeholder

            

            _scoreFont = content.Load<SpriteFont>("Fonts/Font");
            _content = content;


            _playerLife = new PlayerLifeInterface(heartTexture);

            _sprites = new List<Sprite>();

            // Wyznaczanie wielkości mapy
            Map = new Rectangle(0, 0, mapData.mapCols * 32 * (int)_Scale, mapData.mapRows * 32 * (int)_Scale);

            // Dodawanie pierwszej warstwy mapy
            for (int i = 0; i < mapData.mapCols * mapData.mapRows; i++)
            {
                if (mapData.tiles1[i] != 0)
                    _sprites.Add(new MapSprite(
                        mapTileset,
                        _mapHandler.GetSourceRectangle(mapTileset, mapData.tileRows, mapData.tileCols, mapData.mapRows, mapData.mapCols, mapData.tiles1[i] - 1),
                        _mapHandler.GetPosition(mapTileset, mapData.tileRows, mapData.tileCols, mapData.mapRows, mapData.mapCols, i, _Scale),
                        false, _Scale)
                    {
                        Input = _Input,
                        Speed = 3f
                    });
            }

            //Druga warstwa mapy
            for (int i = 0; i < mapData.mapCols * mapData.mapRows; i++)
            {
                if (mapData.tiles2[i] != 0)
                    _sprites.Add(new MapSprite(
                        mapTileset,
                        _mapHandler.GetSourceRectangle(mapTileset, mapData.tileRows, mapData.tileCols, mapData.mapRows, mapData.mapCols, mapData.tiles2[i] - 1),
                        _mapHandler.GetPosition(mapTileset, mapData.tileRows, mapData.tileCols, mapData.mapRows, mapData.mapCols, i, _Scale),
                        true, _Scale)
                    {
                        Input = _Input,
                        Speed = 3f
                    });
            }

            

            // Dodanie gracza
            if (mapNumber == 1)
            {    
                Door door = new Door(doorTexture, new Vector2(4 * 32 * _Scale, 2 * 32 * _Scale))
                {
                    Scale = _Scale
                };
                door.OnDoorTouch += Door_OnDoorTouch;

                _sprites.Add(door);

                player = new Player(playerTexture, new Vector2(32 * _Scale, 32 * _Scale), _blasterSound, _deathSoundPlayer)
                {
                    Input = _Input,
                    Speed = 2.0f * _Scale,
                    Scale = _Scale,
                    HP = 3,
                    Position = new Vector2(100, 100),
                    Bullet = new Bullet(content.Load<Texture2D>("Bullet")),

                };
            

                player.OnEnemyCollision += Player_OnEnemyCollision;
                player.OnEnoughScore += Player_OnEnoughScore;
                player.OnBossBulletHit += Player_OnBossBulletHit;


            }

            player.PositionOnMap = player.Position;
            _playerLife.UpdatePlayerLife(player.HP);
            _sprites.Add(player);

            // dodanie przeciwników
            if (mapNumber == 1)
            {
                _sprites.Add(new Enemy(enemyTexture, new Vector2(7 * 32 * _Scale, 10 * 32 * _Scale), _deathSound)
                {
                    Speed = 1.0f * _Scale,
                    Scale = _Scale,
                    HP = 2,
                    _textureAngry = enemyTextureAlt
                });

                _sprites.Add(new Enemy(enemyTexture, new Vector2(5 * 32 * _Scale, 5 * 32 * _Scale), _deathSound)
                {
                    Speed = 1.0f * _Scale,
                    Scale = _Scale,
                    HP = 2,
                    _textureAngry = enemyTextureAlt
                });
                _sprites.Add(new Enemy(enemyTexture, new Vector2(6 * 32 * _Scale, 10 * 32 * _Scale), _deathSound)
                {
                    Speed = 1.0f * _Scale,
                    Scale = _Scale,
                    HP = 2,
                    _textureAngry = enemyTextureAlt
                });

                _sprites.Add(new Enemy(enemyTexture, new Vector2(5 * 32 * _Scale, 18 * 32 * _Scale), _deathSound)
                {
                    Speed = 1.0f * _Scale,
                    Scale = _Scale,
                    HP = 2,
                    _textureAngry = enemyTextureAlt
                });

                _sprites.Add(new Enemy(enemyTexture, new Vector2(15 * 32 * _Scale, 6 * 32 * _Scale), _deathSound)
                {
                    Speed = 1.0f * _Scale,
                    Scale = _Scale,
                    HP = 2,
                    _textureAngry = enemyTextureAlt
                });

                _sprites.Add(new Enemy(enemyTexture, new Vector2(16 * 32 * _Scale, 3 * 32 * _Scale), _deathSound)
                {
                    Speed = 1.0f * _Scale,
                    Scale = _Scale,
                    HP = 2,
                    _textureAngry = enemyTextureAlt
                });

                _sprites.Add(new Enemy(enemyTexture, new Vector2(14 * 32 * _Scale, 16 * 32 * _Scale), _deathSound)
                {
                    Speed = 1.0f * _Scale,
                    Scale = _Scale,
                    HP = 2,
                    _textureAngry = enemyTextureAlt
                });
            }else if(mapNumber == 2)
            {
                Boss boss = new Boss(bossTexture, bossTextureTransition1, bossTextureTransition2, bulletTexture, new Vector2(10 * 32 * _Scale, 10 * 32 * _Scale))
                {
                    Speed = 1.0f * _Scale,
                    Scale = 2.0f * _Scale,
                    HP = 30,
                    _textureAngry = bossTexture // Ensure you have this texture loaded
                };
                boss.OnBossDeath += Boss_OnBossDeath;
                _sprites.Add(boss);
            }


        }

        public override void UnloadContent()
        {
            //
        }
        #endregion

        ////////////////////////////////////////////   EVENTS EXECUTION   ///////////////////////////////////////////////////////
        #region Events Execution
        private void Player_OnEnemyCollision(object sender, EventArgs e) // event zarządzający zabieraniem HP gracza
        {
            foreach (var sprite in _sprites)
            {
                if (sprite is Player)
                {
                    if ((sprite as Player).AttackDelay <= 0)
                    {
                        (sprite as Player).AttackDelay = 120;
                        (sprite as Player).HP--;
                        _playerLife.UpdatePlayerLife((sprite as Player).HP);
                    }
                    Debug.Print(((sprite as Player).HP).ToString());
                    if ((sprite as Player).HP < 1) (sprite as Player).EventExecuter(_graphicsDevice);
                }
            }
        }

        private void Door_OnDoorTouch(object sender, EventArgs e)
        {
            foreach (var sprite in _sprites)
            {
                if (sprite is Player)
                {
                    player = (Player)sprite;
                }
            }
            _sprites.Clear();
            mapNumber = 2;
            LoadContent(_content);
        }

        private void Player_OnEnoughScore(object sender, EventArgs e)
        {
            foreach (var sprite in _sprites)
            {
                if (sprite is Door) (sprite as Door).active = true;
            }
        }

        private void Boss_OnBossDeath(object sender, EventArgs e)
        {
            foreach (var sprite in _sprites)
            {
                if (sprite is Player)
                {
                    GameStateManager.Instance.ChangeScreen(new GameFinishGameState(_graphicsDevice, (sprite as Player).Score));
                }
            }
            
        }

        private void Player_OnBossBulletHit(object sender, EventArgs e)
        {
            foreach(var sprite in _sprites)
            {
                if(sprite is Player)
                {
                    _playerLife.UpdatePlayerLife((sprite as Player).HP);
                    if ((sprite as Player).HP < 1) (sprite as Player).EventExecuter(_graphicsDevice);
                }
            }
        }
        #endregion

        //////////////////////////////////////////   UPDATE AND DRAW   ////////////////////////////////////////////////////////////
        #region Update and Draw
        public override void Update(GameTime gameTime)
        {
            // Wyznaczenie jak Entity mają iść i sprawdzenie kolizji // nadanie Velocity dla Player
            foreach (var sprite in _sprites)
                sprite.SetEntityMove(_sprites);

            // Wyznaczenie ruchu dla wszystkich elementów
            foreach (var sprite in _sprites)
                sprite.MoveEntity(_sprites, _screenWidth, _screenHeight, Map);

            // Zaktualizowanie wszystkich pozycji
            foreach (var sprite in _sprites)
                sprite.Update(gameTime, _sprites);

            // Sprawdzanie eventów
            foreach (var sprite in _sprites)
                sprite.EventChecker(_sprites);

            // Usuwanie oznaczonych spritów
            for (int i = 0; i < _sprites.Count; i++)
                if (_sprites[i].IsRemoved)
                {
                    _sprites.RemoveAt(i);  
                    i--;
                }
            // Handle boss bullets
            foreach (var sprite in _sprites.OfType<Boss>())
            {
                foreach (var bullet in sprite.BossBullets)
                {
                    bullet.Update(gameTime, _sprites);
                }
            }

            // Remove boss bullets that are marked as removed
            foreach (var sprite in _sprites.OfType<Boss>())
            {
                sprite.BossBullets.RemoveAll(b => b.IsRemoved);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            _graphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            foreach (var sprite in _sprites)
            {
                sprite.Draw(spriteBatch);
            }
            _playerLife.Draw(spriteBatch);
            spriteBatch.DrawString(_scoreFont, "Score: " + player.Score, new Vector2(720, 10), Color.White);
            spriteBatch.End();
        }
        #endregion
    }
}