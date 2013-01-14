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

        public Controller(Input input)
        {
            physicsEngine = new PhysicsEngine();

            input.jump += new Input.EventHandler(jump);
            //input.duck += new Input.EventHandler(duck);
        }

        public void update(Gameworld gameWorld, GameTime gameTime)
        {
            risse = gameWorld.Risse;
            Vector2 prePos = risse.Position;
            parallaxBackground(gameWorld);

            physicsEngine.gravitation(risse, gameTime);
            collisionResolution(gameWorld, prePos);
            risse.update(gameTime);
            //Console.Out.WriteLine("Position: " + risse.Position);
        }


        //TODO: actual parallaxing
        public void parallaxBackground(Gameworld gameWorld)
        {
            Gameobject background = gameWorld.Background;

            if (background.Position.X < (-background.TextureWidth / 2))
            {
                background.Position = Vector2.Zero;
            }
            gameWorld.Background.update();

            
        }

        public void jump()
        {
            //risse.Position = new Vector2(risse.Position.X, risse.Position.Y -5);
            risse.Velocity = new Vector2(0, -10);
            //if (risse.Velocity.Y == 0) //Trokke det her funker
            //{
            //}

            System.Console.WriteLine("JUMPMUTHAAA");
        }

        protected void collisionResolution(Gameworld gameworld, Vector2 prePos)
        {
            bool collidedWithPlatformSide = false;
            foreach (Gameobject platform in gameworld.Platforms)
            {
                if (physicsEngine.collisionDetection(risse, platform))
                {
                    if (collisionDetermineType(gameworld, risse, platform, prePos))
                    {
                        collidedWithPlatformSide = true;
                        break;
                    }
                }
            }

            if (!collidedWithPlatformSide)
            {
                foreach (Gameobject platform in gameworld.Ground)
                {
                    if (physicsEngine.collisionDetection(risse, platform))
                    {
                        if (collisionDetermineType(gameworld, risse, platform, prePos))
                        {
                            collidedWithPlatformSide = true;
                            break;
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
                collisionVertical(gameworld, prePos);
                return true;
            }

            collisionHorizontal(gameworld, prePos);
            return false;
        }

        //handles crashing
        protected void collisionHorizontal(Gameworld gameworld, Vector2 prePos)
        {
            //gameworld.Risse.Velocity = Vector2.Zero;

            //FUCK MY LIFE HE HAS TO FALL AND STOP
        }

        //handles landing on or jumping up and hitting a platform
        protected void collisionVertical(Gameworld gameworld, Vector2 prePos)
        {
            gameworld.Risse.Position = new Vector2(gameworld.Risse.Position.X, prePos.Y);
            gameworld.Risse.Velocity = new Vector2(gameworld.Risse.Velocity.X, 0);
        }
    }
}
