using System;
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
        private Vector2 acceleration;
        private Rectangle boundingBox;

        public Gameobject(Texture2D texture, Vector2 position, Vector2 velocity)
        {
            Texture = texture;
            Position = position;
            Velocity = velocity;
            BoundingBox = new Rectangle(
                    (int)Position.X - Texture.Width / 2,
                    (int)Position.Y - Texture.Height / 2,
                    Texture.Width,
                    Texture.Height);
        }

        public Rectangle BoundingBox
        {
            set
            {
                this.boundingBox = value;
            }
            get
            {
                refreshRectangle();
                return this.boundingBox;
            }
        }

        public Vector2 Position
        {
            get { return this.position; }
            set { this.position = value; }
        }

        public Texture2D Texture
        {
            get { return this.texture; }
            set { this.texture = value; }
        }

        public Vector2 Velocity
        {
            get { return this.velocity; }
            set { this.velocity = value; }
        }

        public Vector2 Acceleration
        {
            get { return this.acceleration; }
            set { this.acceleration = value; }
        }
        
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
            //spriteBatch.Draw(texture, position, null, Color.White, 0f,
            //        position, 0.2f, SpriteEffects.None, 0f);
        }

        public int TextureWidth
        {
            get { return texture.Width; }
        }

        public void update ()
        {
            position += velocity; 
        }

        protected void refreshRectangle()
        {
            this.boundingBox.X = (int)Position.X - Texture.Width / 2;
            this.boundingBox.Y = (int)Position.Y - Texture.Height / 2;
        }
    }
}
