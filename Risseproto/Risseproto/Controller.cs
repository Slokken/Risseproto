using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Risseproto
{
    class Controller
    {

        public Gameobject risse;
        private PhysicsEngine physicsEngine;
        private SoundManager soundManager;

        enum state { running, jumping, ducking, idonteven, facedown, crash }

        public Controller(Input input, SoundManager soundManager)
        {
            physicsEngine = new PhysicsEngine();

            this.soundManager = soundManager;
            input.jump += new Input.EventHandler(jump);
            //input.duck += new Input.EventHandler(duck);
        }

        public void update(Gameworld gameWorld, GameTime gameTime)
        {
            risse = gameWorld.Risse;
            Vector2 prePos = risse.Position;
            parallaxBackground(gameWorld);

            physicsEngine.gravitation(risse, gameTime);
            risse.update(gameTime);
            collisionResolution(gameWorld, prePos);
            foreach (Gameobject go in gameWorld.Platforms)
            {
                go.update();
            }

            Console.Out.WriteLine(risse.Position);
        }


        //TODO: actual parallaxing
        public void parallaxBackground(Gameworld gameWorld)
        {
            for (int i = 0; i < gameWorld.Backgrounds.Count; i++ )
            {
                if (gameWorld.Backgrounds[i].Position.X < (-gameWorld.Backgrounds[i].TextureWidth))
                {
                    gameWorld.Backgrounds[i].Position = new Vector2(gameWorld.Backgrounds[i].Position.X + gameWorld.Backgrounds[i].TextureWidth, 0);
                    //gameWorld.Backgrounds[i+1].Position = new Vector2(gameWorld.Backgrounds[i+1].Position.X + gameWorld.Backgrounds[i+1].TextureWidth, 0);
                }
                gameWorld.Backgrounds[i].update();
            }
        }

        public void jump()
        {
            if(risse.OnTheGround)
            {
                risse.Animation = (int)state.jumping;
                risse.Position = new Vector2(risse.Position.X, risse.Position.Y -6);
                risse.Velocity = new Vector2(0, -15);
                soundManager.Play();
            }
        }
        public void duck()
        {
            if (risse.OnTheGround)
            {

            }
        }

        protected void collisionResolution(Gameworld gameworld, Vector2 prePos)
        {
            bool collidedWithPlatformSide = false;
            risse.OnTheGround = false;
            foreach (Gameobject platform in gameworld.Platforms)
            {
                if (physicsEngine.collisionDetection(risse, platform))
                {
                    if (collisionDetermineType(gameworld, risse, platform, prePos))
                    {
                        collidedWithPlatformSide = true;
                        risse.OnTheGround = true;
                    }
                }
            }

            if (!collidedWithPlatformSide)
            {
                foreach (Gameobject ground in gameworld.Ground)
                {
                    if (physicsEngine.collisionDetection(risse, ground))
                    {
                        if (collisionDetermineType(gameworld, risse, ground, prePos))
                        {
                            collidedWithPlatformSide = true;
                        }
                    }
                }
            }

            if (!collidedWithPlatformSide)
            {
                foreach (Gameobject collidable in gameworld.Collidables)
                {
                    if (physicsEngine.collisionDetection(risse, collidable))
                    {
                        collisionHorizontal(gameworld, prePos);
                    }
                }
            }
        }

        // returns true if colliding from side
        protected bool collisionDetermineType(Gameworld gameworld, Gameobject risse, Gameobject platform, Vector2 prePos)
        {
            if (risse.BoundingBox.Right - (risse.BoundingBox.Width/2) > platform.BoundingBox.Left && risse.BoundingBox.Right - (risse.BoundingBox.Width/2) < platform.BoundingBox.Right)
            {
                if (risse.Position.Y + risse.BoundingBox.Height < platform.BoundingBox.Y + platform.BoundingBox.Height){
                    collisionVertical(gameworld, new Vector2(risse.Position.X, platform.Position.Y - (risse.BoundingBox.Height - 1)));
                    risse.OnTheGround = true;
                    if (gameworld.Risse.Animation == (int)state.jumping)
                    {
                        gameworld.Risse.Animation = (int)state.running;
                    }
                }
                else
                {
                    //collisionVertical(gameworld, new Vector2(risse.Position.X, platform.Position.Y + (platform.BoundingBox.Height + 1)));
                    Console.Out.WriteLine("facepalmed");
                }
                return false;
            }

            collisionHorizontal(gameworld, prePos);
            return true;
        }

        //handles colliding with something from the side
        protected void collisionHorizontal(Gameworld gameworld, Vector2 prePos)
        {
            gameworld.Risse.Velocity = Vector2.Zero;
            gameworld.Risse.Position = new Vector2(prePos.X, gameworld.Risse.Position.Y);
            //gameworld.Risse.Animation = (int)state.crash;
        }

        //handles landing on or jumping up and hitting a platform
        protected void collisionVertical(Gameworld gameworld, Vector2 newPos)
        {
            gameworld.Risse.Position = newPos;
            gameworld.Risse.Velocity = new Vector2(gameworld.Risse.Velocity.X, 0);
        }
    }
}
