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
            collisionResolution(gameWorld, prePos);
            risse.update(gameTime);

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
                risse.Position = new Vector2(risse.Position.X, risse.Position.Y -6);
                risse.Velocity = new Vector2(0, -15);
                soundManager.Play();
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
                        //break;
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
                            risse.OnTheGround = true;
                            //break;
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
                collisionVertical(gameworld, new Vector2(prePos.X, platform.Position.Y - (risse.BoundingBox.Height - 1)));
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
            gameworld.Risse.collisionFall();
        }

        //handles landing on or jumping up and hitting a platform
        protected void collisionVertical(Gameworld gameworld, Vector2 prePos)
        {
            gameworld.Risse.Position = new Vector2(gameworld.Risse.Position.X, prePos.Y);
            gameworld.Risse.Velocity = new Vector2(gameworld.Risse.Velocity.X, 0);
        }
    }
}
