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
        private Vector2 acceleration;
        private const int normalBoundingHeight = 256;
        private Rectangle boundingBox;
        private Rectangle boundingBoxDuck;

        private Texture2D rectangle = ContentHolder.textureRectangle;

        private float timer;
        private float interval;
        private int currentFrame;
        private int numberOfFrames;
        private int spriteWidth;
        private int spriteHeight;
        Rectangle sourceRect;
        Vector2 origin;
        bool onTheGround = true;

        int animation;
        

        public Gameobject(Texture2D texture, Vector2 position, Vector2 velocity)
        {
            Texture = texture;
            Position = position;
            Velocity = velocity;
            BoundingBox = new Rectangle(
                    (int)Position.X - Texture.Width,
                    (int)Position.Y - Texture.Height,
                    Texture.Width,
                    Texture.Height);

            this.origin = new Vector2(texture.Width / 2, texture.Height / 2);
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
            boundingBoxDuck = new Rectangle(
                (int)Position.X,
                (int)Position.Y + spriteWidth / 2,
                spriteWidth,
                spriteHeight / 2);
            Interval = interval;
            SpriteWidth = spriteWidth;
            SpriteHeight = spriteHeight;

            //this.origin = new Vector2(texture.Width , texture.Height );

            origin = position;

            timer = 0f;
            currentFrame = 0;
            numberOfFrames = 7;
        }

        public Rectangle BoundingBox
        {
            set { this.boundingBox = value; }
            get { return refreshRectangle(); }
        }

        public int NormalBoundingHeight
        {
            get { return normalBoundingHeight; }
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

        public int Animation
        {
            get { return this.animation; }
            set { this.animation = value; }
        }
        
        public bool OnTheGround
        {
            get { return this.onTheGround; }
            set { this.onTheGround = value; }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
            //spriteBatch.Draw(texture, position, null, Color.White, 0f,
            //        position, 0.2f, SpriteEffects.None, 0f);

            //spriteBatch.Draw(rectangle, BoundingBox, Color.Black);
        }

        public void DrawDuplicateMUTHAAAAAAA(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
            spriteBatch.Draw(texture, new Vector2(position.X + texture.Width, position.Y), Color.White);
        }

        public void DrawAnimation(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, sourceRect, Color.White, 0f, origin, 1.0f, SpriteEffects.None, 0);
            spriteBatch.Draw(rectangle, BoundingBox, Color.Black);
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
            if (timer > interval)
            {
                currentFrame++;
                timer = 0f;
            }

            switch (animation)
            {
                case 0:
                    interval = 250;
                    numberOfFrames = 7;
                    break;
                case 1:
                    numberOfFrames = 2;
                    runAnimationOnce(1000, 0);
                    break;
                case 2:
                    numberOfFrames = 0;
                    break;
                case 3:
                    numberOfFrames = 1;
                    runAnimationOnce(500, 0);
                    break;
                case 4:
                    numberOfFrames = 2;
                    runAnimationOnce(500, 0);
                    break;
            }

            timer += (float)gameTime.ElapsedGameTime.Milliseconds;


            if (currentFrame >= numberOfFrames)
            {
                currentFrame = 0;
            }

            sourceRect = new Rectangle(currentFrame * spriteWidth, animation * spriteHeight, spriteWidth, spriteHeight);
            //origin = new Vector2(sourceRect.Width , sourceRect.Height );

            //origin = position;
        }

        private void runAnimationOnce(int animationSpeed, int nextAnimation)
        {
            if (currentFrame >= numberOfFrames)
            {
                animation = nextAnimation;
                interval = animationSpeed;
            }
        }

        // updates boundingbox position
        protected Rectangle refreshRectangle()
        {
            this.boundingBox.X = (int)Position.X;// -Texture.Width / 2;
            this.boundingBox.Y = (int)Position.Y; //+ Texture.Height / 2 ;

            this.boundingBoxDuck.X = (int)Position.X - spriteWidth / 2;// -Texture.Width / 2;
            this.boundingBoxDuck.Y = (int)Position.Y + spriteHeight / 2 ;

            return (animation == 2) ? this.boundingBoxDuck: this.boundingBox;
        }
    }
}
