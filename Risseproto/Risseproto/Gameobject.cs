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
        private Vector2 accelllllllllleration;

        public Gameobject(Texture2D texture, Vector2 position, Vector2 velocity)
        {
            this.texture = texture;
            this.position = position;
            this.velocity = velocity;
        }
        
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
            //spriteBatch.Draw(texture, position, null, Color.White, 0f,
            //        position, 0.2f, SpriteEffects.None, 0f);
        }

        public Vector2 Position
        {
            get { return position; }
            set { this.position = value; }
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
