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
        bool onTheGround = false;

        int animation;
        int previousAnimation;

        private int checkpoints;
        bool checkpointActivated = false;
        public event EventHandler incrementCheckpoint;
        public delegate void EventHandler(int i);

        

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
                spriteWidth / 2,
                spriteHeight / 2);
            Interval = interval;
            SpriteWidth = spriteWidth;
            SpriteHeight = spriteHeight;

            //this.origin = new Vector2(texture.Width , texture.Height );

            origin = position;

            animation = 0;
            previousAnimation = 0;

            timer = 0f;
            currentFrame = 0;
            numberOfFrames = 7;
        }

        public Gameobject(Texture2D texture, Vector2 position, Vector2 velocity, int spriteWidth, int spriteHeight)
        {
            Texture = texture;
            Position = position;
            Velocity = velocity;

            SpriteWidth = spriteWidth;
            SpriteHeight = spriteHeight;

            BoundingBox = new Rectangle(
                    (int)Position.X - Texture.Width,
                    (int)Position.Y - Texture.Height,
                    Texture.Width,
                    Texture.Height);
            
            origin = position;
            //this.origin = new Vector2(SpriteWidth / 2, SpriteWidth / 2);

            currentFrame = 0;
            numberOfFrames = 2;
            Checkpoints = 0;
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

        public int Checkpoints
        {
            get { return this.checkpoints; }
            set { this.checkpoints = value; }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
            //spriteBatch.Draw(texture, position, null, Color.White, 0f,
            //        position, 0.2f, SpriteEffects.None, 0f);

            //spriteBatch.Draw(rectangle, BoundingBox, Color.Black);
        }

        public void DrawFluff(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, sourceRect, Color.White, 0f, origin, 1.0f, SpriteEffects.None, 0);
        }

        public void DrawDuplicateMUTHAAAAAAA(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
            spriteBatch.Draw(texture, new Vector2(position.X + texture.Width, position.Y), Color.White);
        }

        public void DrawAnimation(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, sourceRect, Color.White, 0f, origin, 1.0f, SpriteEffects.None, 0);
            //spriteBatch.Draw(rectangle, BoundingBox, Color.Black);
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

            

            if (previousAnimation != animation)
            {
                currentFrame = 0;
            }

            switch (animation)
            {
                case 0:
                    interval = 67;
                    numberOfFrames = 8;
                    break;
                case 1:
                    interval = 400;
                    numberOfFrames = 3;
                    runAnimationOnce(0);
                    break;
                case 2:
                    numberOfFrames = 0;
                    break;
                case 3:
                    interval = 400;
                    numberOfFrames = 2;
                    runAnimationOnce(0);
                    break;
                case 4:
                    interval = 400;
                    numberOfFrames = 3;
                    runAnimationOnce(0);
                    break;
            }

           

            timer += (float)gameTime.ElapsedGameTime.Milliseconds;


            if (currentFrame >= numberOfFrames)
            {
                currentFrame = 0;
            }

           

            sourceRect = new Rectangle(currentFrame * spriteWidth, animation * spriteHeight, spriteWidth, spriteHeight);

            previousAnimation = animation;
            
            if (timer > interval)
            {
                currentFrame++;
                timer = 0f;
            }

            //Console.Out.WriteLine(currentFrame);
            //origin = new Vector2(sourceRect.Width , sourceRect.Height );

            //origin = position;
        }
        
        public void updateFluff(Gameobject risse, ContentHolder contentHolder)
        {
            position += velocity;
            if (BoundingBox.Intersects(risse.BoundingBox))
            {
                
                if (!checkpointActivated)
                {
                    currentFrame = 1;
                    incrementCheckpoint((int)position.X);
                    contentHolder.sound_checkpoint.Play();
                    checkpointActivated = true;
                }
            }
            if (Position.X < -SpriteWidth)
            {
                Checkpoints = Checkpoints + 1;
                position.X += 4000;
                currentFrame = 0;
                checkpointActivated = false;
            }
            sourceRect = new Rectangle(currentFrame * spriteWidth, 0, spriteWidth, spriteHeight);
            origin = new Vector2(sourceRect.Width / 2, sourceRect.Height / 2);
            
        }

        private void runAnimationOnce(int nextAnimation)
        {
            if (currentFrame >= numberOfFrames)
            {
                animation = nextAnimation;
            }
        }

        // updates boundingbox position
        protected Rectangle refreshRectangle()
        {
            this.boundingBox.X = (int)Position.X ;// -Texture.Width / 2;
            this.boundingBox.Y = (int)Position.Y;
            this.boundingBoxDuck.X = (int)Position.X;// -Texture.Width / 2;
            this.boundingBoxDuck.Y = (int)Position.Y + spriteHeight / 2;

            return (animation == 2) ? this.boundingBoxDuck : this.boundingBox;
        }
    }
}
