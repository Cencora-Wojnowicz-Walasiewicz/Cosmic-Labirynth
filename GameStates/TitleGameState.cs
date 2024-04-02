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
        public TitleGameState(GraphicsDevice graphicsDevice) : base(graphicsDevice) 
        {
            //
        }

        public override void Initialize()
        {
            //
        }

        public override void LoadContent(ContentManager content)
        {
            var buttonTexture = content.Load<Texture2D>("Control/Button");
            var buttonFont = content.Load<SpriteFont>("Fonts/Font");
            

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

            // if (Keyboard.GetState().IsKeyDown(Keys.Space))
            //   GameStateManager.Instance.ChangeScreen(new InGameGameState(_graphicsDevice));

            //   if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            // {
            //   GameStateManager.Instance.ClearScreens();
            // GameStateManager.Instance.CloseGame();
            //}
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            _graphicsDevice.Clear(Color.White);
            spriteBatch.Begin();
            foreach (var component in _components)
                component.Draw(spriteBatch);
            spriteBatch.End();
        }

        private void NewGameButton_Click(object sender, EventArgs e)
        {
            GameStateManager.Instance.ChangeScreen(new InGameGameState(_graphicsDevice));
        }

        private void QuitGameButton_Click(object sender, EventArgs e)
        {
            GameStateManager.Instance.ClearScreens();
            GameStateManager.Instance.CloseGame();
        }

    }
}
