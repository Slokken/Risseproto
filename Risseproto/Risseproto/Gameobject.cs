﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Risseproto
{
    class Gameobject
    {
        private Vector2 position;
        private Texture2D texture;
        private Vector2 velocity;
        private Vector2 accelleration;

        public Gameobject(Texture2D texture, Vector2 position)
        {
            this.texture = texture;
            this.position = position;
        }

        public Rectangle BoundingBox
        {
            get
            {
                return new Rectangle(
                    (int)position.X - texture.Width / 2,
                    (int)position.Y - texture.Height / 2,
                    texture.Width,
                    texture.Height);
            }
        }
        
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
            //spriteBatch.Draw(texture, position, null, Color.White, 0f,
            //        position, 0.2f, SpriteEffects.None, 0f);
        }

        public void update (GameTime gameTime)
        {
            position += velocity; 
        }
    }
}
