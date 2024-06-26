﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosmic_Labirynth.GameStates
{
    public class GameOverGameState : GameState
    {
        private List<Component> _components;
        public GameOverGameState(GraphicsDevice graphicsDevice) : base(graphicsDevice)
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
            _graphicsDevice.Clear(Color.Red);
            spriteBatch.Begin();
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
