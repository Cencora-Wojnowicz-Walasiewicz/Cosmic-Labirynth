using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosmic_Labirynth.GameStates
{
    public class TitleGameState : GameState
    {
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
            //
        }

        public override void UnloadContent()
        {
            //
        }

        public override void Update(GameTime gameTime)
        {
            //
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            _graphicsDevice.Clear(Color.White);
            spriteBatch.Begin();
            //
            spriteBatch.End();
        }

    }
}
