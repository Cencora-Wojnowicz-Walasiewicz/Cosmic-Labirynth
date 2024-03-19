using Cosmic_Labirynth.Misc;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosmic_Labirynth.Sprites
{
    public class Sprite
    {
        protected Texture2D _texture;
        public Vector2 Position; // miejsce gdzie się wyświetla objekt
        public Vector2 PositionOnMap;
        public Vector2 Velocity;
        public float Speed;
        public Input Input;
        public bool Collision = true;
        public float Scale;
        public float BorderDistance = 128f;
        public virtual Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, 32 * (int)Scale, 32 * (int)Scale);
            }
        }

        public Sprite(Texture2D texture)
        {
            _texture = texture;
        }

        public virtual void Update(GameTime gameTime, List<Sprite> sprites)
        {
            //
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, Color.White);
        }

        public virtual void SetMapMove(Vector2 moveVector) // ustawienie kierunku w którym ma poruszać się objekt
        { }

        public virtual void SetEntityMove(List<Sprite> sprites) // ustawienie kierunku w którym ma poruszać się objekt
        { }

        public virtual void MoveEntity(List<Sprite> sprites, int screenWidth, int screenHeight, Rectangle map)
        { }

        public virtual void Move() // zmiana pozycji na mapie objektu
        { }

        public virtual void Movement() // ustawienie pozycji z mapy do pozycji wyświetlanej
        { }

        #region Collision
        protected bool IsTouchingLeft(Sprite sprite)
        {
            return this.Rectangle.Right + this.Velocity.X > sprite.Rectangle.Left &&
                this.Rectangle.Left < sprite.Rectangle.Left &&
                this.Rectangle.Bottom > sprite.Rectangle.Top &&
                this.Rectangle.Top < sprite.Rectangle.Bottom;
        }
        protected bool IsTouchingRight(Sprite sprite)
        {
            return this.Rectangle.Left + this.Velocity.X < sprite.Rectangle.Right &&
                this.Rectangle.Right > sprite.Rectangle.Right &&
                this.Rectangle.Bottom > sprite.Rectangle.Top &&
                this.Rectangle.Top < sprite.Rectangle.Bottom;
        }
        protected bool IsTouchingTop(Sprite sprite)
        {
            return this.Rectangle.Bottom + this.Velocity.Y > sprite.Rectangle.Top &&
                this.Rectangle.Top < sprite.Rectangle.Top &&
                this.Rectangle.Right > sprite.Rectangle.Left &&
                this.Rectangle.Left < sprite.Rectangle.Right;
        }
        protected bool IsTouchingBottom(Sprite sprite)
        {
            return this.Rectangle.Top + this.Velocity.Y < sprite.Rectangle.Bottom &&
                this.Rectangle.Bottom > sprite.Rectangle.Bottom &&
                this.Rectangle.Right > sprite.Rectangle.Left &&
                this.Rectangle.Left < sprite.Rectangle.Right;
        }
        #endregion
    }
}
