using Cosmic_Labirynth.GameStates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Cosmic_Labirynth
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);

            // Wymiary okna
            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 500;
            _graphics.ApplyChanges();

            GameStateManager.Instance.SetGraphics(_graphics);

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            GameStateManager.Instance.SetContent(Content);
            GameStateManager.Instance.AddScreen(new TitleGameState(GraphicsDevice));
        }

        protected override void UnloadContent()
        {
            GameStateManager.Instance.UnloadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            GameStateManager.Instance.Update(gameTime);
            base.Update(gameTime);
            if (!GameStateManager.Instance.GetGamePower())
                Exit();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            GameStateManager.Instance.Draw(_spriteBatch);
            base.Draw(gameTime);
        }
    }
}
