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

namespace Cosmic_Labirynth.GameStates
{
    public class InGameGameState : GameState
    {

        private List<Sprite> _sprites;
        private MapHandler _mapHandler = new MapHandler();
        private float _Scale = 1f;

        private Input _Input;

        private Rectangle Map;

        private int _screenHeight;
        private int _screenWidth;

        public InGameGameState(GraphicsDevice graphicsDevice) : base(graphicsDevice)
        {
            _screenHeight = GameStateManager.Instance._graphics.PreferredBackBufferHeight;
            _screenWidth = GameStateManager.Instance._graphics.PreferredBackBufferWidth;
        }

        public override void Initialize()
        {
            _Input = new Input
            {
                Up = Keys.Up,
                Right = Keys.Right,
                Down = Keys.Down,
                Left = Keys.Left
            };
        }

        public override void LoadContent(ContentManager content)
        {
            // Wczytanie danych mapy
            JObject mdata = JObject.Parse(File.ReadAllText(@"..\..\..\Maps\map1.json"));
            MapData mapData = JsonConvert.DeserializeObject<MapData>(mdata.ToString());
            var playerTexture = content.Load<Texture2D>("PlayerTest");
            var mapTileset = content.Load<Texture2D>(mapData.tilesetName);

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
            _sprites.Add(new Player(playerTexture, new Vector2(0, 0))
            {
                Input = _Input,
                Speed = 3.0f,
                Scale = _Scale
            });
        }

        public override void UnloadContent()
        {
            //
        }

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
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            _graphicsDevice.Clear(Color.Red);
            spriteBatch.Begin();
            foreach (var sprite in _sprites)
                sprite.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}
