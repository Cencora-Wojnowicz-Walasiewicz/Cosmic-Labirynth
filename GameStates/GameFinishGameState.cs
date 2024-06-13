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
        private SpriteFont _font;
        private int _score;

        public GameFinishGameState(GraphicsDevice graphicsDevice, int score) : base(graphicsDevice)
        {
            _score = score;
        }

        public override void Initialize()
        {
            // Brak dodatkowej inicjalizacji potrzebnej
        }

        public override void LoadContent(ContentManager content)
        {
            _font = content.Load<SpriteFont>("Fonts/Font");

            Debug.WriteLine("Game Finish Game State loaded.");
        }

        public override void UnloadContent()
        {
            // Brak potrzeby rozładowania zasobów specyficznych dla tego stanu
        }

        public override void Update(GameTime gameTime)
        {
            // Aktualizacja logiki gry, jeśli jest wymagana
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            _graphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            // Renderowanie tekstu bez użycia Label
            string congratulationsText = "Congratulations!";
            Vector2 congratulationsPosition = new Vector2(300, 100);
            spriteBatch.DrawString(_font, congratulationsText, congratulationsPosition, Color.White);

            string scoreText = $"Your Score: {_score}";
            Vector2 scorePosition = new Vector2(300, 150);
            spriteBatch.DrawString(_font, scoreText, scorePosition, Color.White);

            // Renderowanie przycisków
            DrawButtons(spriteBatch);

            spriteBatch.End();
        }

        private void DrawButtons(SpriteBatch spriteBatch)
        {
            // Przykładowe pozycje i tekstury przycisków
            Texture2D buttonTexture = null; // Wczytaj teksturę przycisku
            SpriteFont buttonFont = _font; // Ustaw czcionkę przycisku

            // Przycisk "Try Again"
            string tryAgainText = "Try Again";
            Vector2 tryAgainPosition = new Vector2(300, 250);
            spriteBatch.DrawString(buttonFont, tryAgainText, tryAgainPosition, Color.White);

            // Przycisk "Title Menu"
            string titleMenuText = "Title Menu";
            Vector2 titleMenuPosition = new Vector2(300, 300);
            spriteBatch.DrawString(buttonFont, titleMenuText, titleMenuPosition, Color.White);
        }

        // Obsługa zdarzeń przycisków

        public void TryAgainButton_Click(object sender, EventArgs e)
        {
            GameStateManager.Instance.ChangeScreen(new InGameGameState(_graphicsDevice));
        }

        public void TitleMenuButton_Click(object sender, EventArgs e)
        {
            GameStateManager.Instance.ChangeScreen(new TitleGameState(_graphicsDevice));
        }
    }
}
