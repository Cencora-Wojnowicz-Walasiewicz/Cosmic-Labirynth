using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Cosmic_Labirynth.GameStates
{
    public class TitleGameState : GameState
    {
        private List<Component> _components;


        private Viewport _viewport;

        private Texture2D _backgroundTexture;

        public TitleGameState(GraphicsDevice graphicsDevice) : base(graphicsDevice) 
        {
             _viewport = graphicsDevice.Viewport;
        }

        public override void Initialize()
        {
            //
        }

        public override void LoadContent(ContentManager content)
        {
            var buttonTexture = content.Load<Texture2D>("Control/Button");
            var buttonFont = content.Load<SpriteFont>("Fonts/Font");


            _backgroundTexture = content.Load<Texture2D>("kk");

            var newGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(300, 200),
                Text = "New Game",
            };

            newGameButton.Click += NewGameButton_Click;

       

            var quitGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(300, 250),
                Text = "Quit Game",
            };

            quitGameButton.Click += QuitGameButton_Click;

            _components = new List<Component>()
             {
                 newGameButton,
                 quitGameButton,
             };

            Debug.WriteLine(_components);

        }

        public override void UnloadContent()
        {
            //
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var component in _components)
                component.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            _graphicsDevice.Clear(Color.White);
            spriteBatch.Begin();

            float scaleX = (float)_viewport.Width / _backgroundTexture.Width;
            float scaleY = (float)_viewport.Height / _backgroundTexture.Height;

            spriteBatch.Draw(_backgroundTexture, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, new Vector2(scaleX, scaleY), SpriteEffects.None, 0f);
            foreach (var component in _components)
                component.Draw(spriteBatch);
            spriteBatch.End();
        }

        private void NewGameButton_Click(object sender, EventArgs e)
        {
            GameStateManager.Instance.ChangeScreen(new InGameGameState(_graphicsDevice));
            //GameStateManager.Instance.ChangeScreen(new GameFinishGameState(_graphicsDevice, 5));
        }

        private void QuitGameButton_Click(object sender, EventArgs e)
        {
            GameStateManager.Instance.ClearScreens();
            GameStateManager.Instance.CloseGame();
        }

    }
}
