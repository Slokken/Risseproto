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

        public Gameobject(Texture2D texture, Vector2 position, Vector2 velocity)
        {
            Texture = texture;
            Position = position;
            this.velocity = velocity;
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

        protected Vector2 Position
        {
            get { return this.position; }
            set { this.position = value; }
        }

        protected Texture2D Texture
        {
            get { return this.texture; }
            set { this.texture = value; }
        }

        protected Vector2 Velocity
        {
            get { return this.velocity; }
            set { this.velocity = value; }
        }

        protected Vector2 Acceleration
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


    }
}
