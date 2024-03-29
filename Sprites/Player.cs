﻿using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace Cosmic_Labirynth.Sprites
{
    public class Player : Sprite
    {
        public override Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, 32 * (int)Scale, 32 * (int)Scale);
            }
        }
        public Player(Texture2D texture, Vector2 position) : base(texture)
        {
            Position = position;
            PositionOnMap = position;
        }

        public override void Update(GameTime gameTime, List<Sprite> sprites)
        {
            Position += Velocity;
            Velocity = Vector2.Zero;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, null, Color.White, 0, new Vector2(0, 0), Scale, SpriteEffects.None, 1.0f);
        }

        public override void SetEntityMove(List<Sprite> sprites) // ustawienie kierunku w którym ma poruszać się objekt
        {
            // wyznaczenie kierunku ruchu gracza na podstawie klawisza // tu by było miejsce na wyznaczenie ruchu dla NPC jeśli by były
            if (Keyboard.GetState().IsKeyDown(Input.Left))
                Velocity.X = -Speed;
            else if (Keyboard.GetState().IsKeyDown(Input.Right))
                Velocity.X = Speed;
            if (Keyboard.GetState().IsKeyDown(Input.Up))
                Velocity.Y = -Speed;
            else if (Keyboard.GetState().IsKeyDown(Input.Down))
                Velocity.Y = Speed;

            // sprawdzanie kolizji
            foreach (var sprite in sprites)
            {
                if (sprite == this)
                    continue;
                if (sprite.Collision)
                {
                    if ((this.Velocity.X > 0 && this.IsTouchingLeft(sprite)) || (this.Velocity.X < 0 && this.IsTouchingRight(sprite)))
                        this.Velocity.X = 0;

                    if ((this.Velocity.Y > 0 && this.IsTouchingTop(sprite)) || (this.Velocity.Y < 0 && this.IsTouchingBottom(sprite)))
                        this.Velocity.Y = 0;
                }
            }
        }
        public override void MoveEntity(List<Sprite> sprites, int screenWidth, int screenHeight, Rectangle map) // wykonanie ruchu na mapie i korekta mapy
        {
            if (Velocity != Vector2.Zero) // sprawdzenie czy się rusza
            {
                // RUCH USTALANY DLA KAŻDEGO KIERUNKU ODDZIELNIE
                Vector2 VelocityTMP = new Vector2(0, 0);
                //LEWO
                if (Velocity.X < 0)
                {
                    if (Position.X > BorderDistance)
                    {
                        //ruch swobodny po środku
                        PositionOnMap.X += Velocity.X;
                    }
                    else if (Position.X <= BorderDistance && PositionOnMap.X > Position.X)
                    {
                        // ruch mapy
                        VelocityTMP.X += Velocity.X;
                        PositionOnMap.X += Velocity.X;
                        Velocity.X = 0;
                    }
                    else if (Position.X <= BorderDistance && PositionOnMap.X <= Position.X)
                    {
                        //ruch swobodny po krawędzi mapy
                        PositionOnMap.X += Velocity.X;
                    }
                }
                //PRAWO
                else if (Velocity.X > 0)
                {
                    if (screenWidth - (Position.X) > BorderDistance + (32 * Scale))
                    {
                        //ruch swobodny po środku
                        PositionOnMap.X += Velocity.X;
                    }
                    else if (screenWidth - (Position.X) <= BorderDistance + (32 * Scale) && map.Width - (PositionOnMap.X) > screenWidth - (Position.X))
                    {
                        // ruch mapy
                        VelocityTMP.X += Velocity.X;
                        PositionOnMap.X += Velocity.X;
                        Velocity.X = 0;
                    }
                    else if (screenWidth - (Position.X) <= BorderDistance + (32 * Scale) && map.Width - (PositionOnMap.X) <= screenWidth - (Position.X))
                    {
                        //ruch swobodny po krawędzi mapy
                        PositionOnMap.X += Velocity.X;
                    }
                }
                //GÓRA
                if (Velocity.Y < 0)
                {
                    if (Position.Y > BorderDistance)
                    {
                        //ruch swobodny po środku
                        PositionOnMap.Y += Velocity.Y;
                    }
                    else if (Position.Y <= BorderDistance && PositionOnMap.Y > Position.Y)
                    {
                        // ruch mapy
                        VelocityTMP.Y += Velocity.Y;
                        PositionOnMap.Y += Velocity.Y;
                        Velocity.Y = 0;
                    }
                    else if (Position.Y <= BorderDistance && PositionOnMap.Y <= Position.Y)
                    {
                        //ruch swobodny po krawędzi mapy
                        PositionOnMap.Y += Velocity.Y;
                    }
                }
                //DÓŁ
                else if (Velocity.Y > 0)
                {
                    if (screenHeight - (Position.Y) > BorderDistance + (32 * Scale))
                    {
                        //ruch swobodny po środku
                        PositionOnMap.Y += Velocity.Y;
                    }
                    else if (screenHeight - (Position.Y) <= BorderDistance + (32 * Scale) && map.Height - (PositionOnMap.Y) > screenHeight - (Position.Y))
                    {
                        // ruch mapy
                        VelocityTMP.Y += Velocity.Y;
                        PositionOnMap.Y += Velocity.Y;
                        Velocity.Y = 0;
                    }
                    else if (screenHeight - (Position.Y) <= BorderDistance + (32 * Scale) && map.Height - (PositionOnMap.Y) <= screenHeight - (Position.Y))
                    {
                        //ruch swobodny po krawędzi mapy
                        PositionOnMap.Y += Velocity.Y;
                    }
                }

                foreach (Sprite sprite in sprites)
                {
                    if (sprite != this)
                        sprite.SetMapMove(VelocityTMP);
                }

            }

        }
    }
}
