using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Cosmic_Labirynth.GameStates
{
  public abstract class Component
  {
    public abstract void Draw(SpriteBatch spriteBatch);

    public abstract void Update(GameTime gameTime);
  }
}
