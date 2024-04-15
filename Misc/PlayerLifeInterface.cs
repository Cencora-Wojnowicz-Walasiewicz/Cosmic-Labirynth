using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosmic_Labirynth.Misc
{
    public class PlayerLifeInterface
    {
        private Texture2D _texture;
        List<Heart> hearts = new List<Heart>();
        public PlayerLifeInterface(Texture2D texture)
        {
            _texture = texture;
        }

        public void UpdatePlayerLife(int hpQuatity)
        {
            hearts.Clear();
            for (int i = 0; i < hpQuatity; i++)
            {
                Heart heart = new Heart(_texture, new Vector2(i*32,0));
                hearts.Add(heart);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Heart heart in hearts)
            {
                heart.Draw(spriteBatch);
            }
        }
    }
}
