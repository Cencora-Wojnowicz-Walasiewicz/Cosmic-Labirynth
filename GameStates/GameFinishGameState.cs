using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Cosmic_Labirynth.GameStates
{
    public class GameFinishGameState : GameState
    {
        private List<Component> _components;
        private int _score;
        SpriteFont buttonFont;
        public GameFinishGameState(GraphicsDevice graphicsDevice, int score) : base(graphicsDevice)
        {
            _score = score;
        }

        public override void Initialize()
        {
            //
        }

        public override void LoadContent(ContentManager content)
        {
            var buttonTexture = content.Load<Texture2D>("Control/Button");
            buttonFont = content.Load<SpriteFont>("Fonts/Font");


            var tryAgainButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(300, 200),
                Text = "Try Again",
            };

            tryAgainButton.Click += TryAgainButton_Click;



            var titleMenuButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(300, 250),
                Text = "Title Menu",
            };

            titleMenuButton.Click += TitleMenuButton_Click;

            _components = new List<Component>()
             {
                 tryAgainButton,
                 titleMenuButton,
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
            _graphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();

            string congratulationsText = "Congratulations!";
            Vector2 congratulationsPosition = new Vector2(300, 100);
            spriteBatch.DrawString(buttonFont, congratulationsText, congratulationsPosition, Color.White);

            string scoreText = $"Your Score: {_score}";
            Vector2 scorePosition = new Vector2(300, 150);
            spriteBatch.DrawString(buttonFont, scoreText, scorePosition, Color.White);

            foreach (var component in _components)
                component.Draw(spriteBatch);
            spriteBatch.End();
        }

        private void TryAgainButton_Click(object sender, EventArgs e)
        {
            GameStateManager.Instance.ChangeScreen(new InGameGameState(_graphicsDevice));
        }

        private void TitleMenuButton_Click(object sender, EventArgs e)
        {
            GameStateManager.Instance.ChangeScreen(new TitleGameState(_graphicsDevice));
        }
    }
}
