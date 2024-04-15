using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosmic_Labirynth.Misc
{
    public class Heart
    {
        public Texture2D _texture;
        public Vector2 _position;
        public Heart(Texture2D texture, Vector2 position) 
        {
            _texture = texture;
            _position = position;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, Color.White);
        }
    }
}
