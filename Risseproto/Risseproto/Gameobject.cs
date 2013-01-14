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

        private float timer;
        private float interval;
        private int currentFrame;
        private int spriteWidth;
        private int spriteHeight;
        Rectangle sourceRect;
        Vector2 origin;

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

        public Gameobject(Texture2D texture, Vector2 position, Vector2 velocity, float interval, int spriteWidth, int spriteHeight)
        {
            Texture = texture;
            Position = position;
            Velocity = velocity;
            BoundingBox = new Rectangle(
                (int)Position.X - spriteWidth / 2,
                (int)Position.Y - spriteHeight /2,
                spriteWidth / 2,
                spriteHeight);
            Interval = interval;
            SpriteWidth = spriteWidth;
            SpriteHeight = spriteHeight;
        }

        public Rectangle BoundingBox
        {
            set { this.boundingBox = value; }
            get { return refreshRectangle(); }
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

        public float Interval
        {
            get { return this.interval; }
            set { this.interval = value; }
        }

        public int SpriteWidth
        {
            get { return this.spriteWidth; }
            set { this.spriteWidth = value; }
        }

        public int SpriteHeight
        {
            get { return this.spriteHeight; }
            set { this.spriteHeight = value; }
        }
        
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
            //spriteBatch.Draw(texture, position, null, Color.White, 0f,
            //        position, 0.2f, SpriteEffects.None, 0f);
        }

        public void DrawAnimation(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, sourceRect, Color.White, 0f, origin, 1.0f, SpriteEffects.None, 0);
        }

        public int TextureWidth
        {
            get { return texture.Width; }
        }

        public void update ()
        {
            position += velocity;
 
        }

        public void update(GameTime gameTime)
        {
            position += velocity;

            timer += (float)gameTime.ElapsedGameTime.Milliseconds;

            if (timer > interval)
            {
                currentFrame++;
                timer = 0f;
            }

            if (currentFrame == 3)
            {
                currentFrame = 0;
            }

            sourceRect = new Rectangle(currentFrame * spriteWidth, 0, spriteWidth, spriteHeight);
            origin = new Vector2(sourceRect.Width / 2, sourceRect.Height / 2);
        }

        protected Rectangle refreshRectangle()
        {
            this.boundingBox.X = (int)Position.X - Texture.Width / 2;
            this.boundingBox.Y = (int)Position.Y - Texture.Height / 2;
            return this.boundingBox;
        }
    }
}
