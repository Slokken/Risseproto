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
        private state theState = state.running;

        public Controller(Input input, SoundManager soundManager)
        {
            physicsEngine = new PhysicsEngine();

            this.soundManager = soundManager;
            input.jump += new Input.EventHandler(jump);
            input.duck += new Input.EventHandler(duck);
        }

        public void update(Gameworld gameWorld, GameTime gameTime)
        {
            risse = gameWorld.Risse;
            Vector2 prePos = risse.Position;
            parallaxBackground(gameWorld);

            physicsEngine.gravitation(risse, gameTime);
            collisionResolution(gameWorld, prePos);
            risse.update(gameTime);
            foreach (Gameobject go in gameWorld.Platforms)
            {
                go.update();
            }

            foreach (Gameobject ground in gameWorld.Ground)
            {
                ground.update();
            }

            //Console.Out.WriteLine(risse.Position);
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
            if (theState == state.ducking)
            {
                ResetBoundingBox();
                risse.OnTheGround = true;
            }
            if(risse.OnTheGround)
            {
           	theState = state.jumping;
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
                theState = state.ducking;
                risse.BoundingBox = new Rectangle(risse.BoundingBox.X, risse.BoundingBox.Y, risse.BoundingBox.Width , risse.BoundingBox.Height / 2);
            }
        }
        public void ResetBoundingBox()
        {
            risse.BoundingBox = new Rectangle(risse.BoundingBox.X, risse.BoundingBox.Y - risse.BoundingBox.Height - 6, risse.BoundingBox.Width, risse.NormalBoundingHeight);
        }

        protected void collisionResolution(Gameworld gameworld, Vector2 prePos)
        {
            Console.Out.WriteLine(risse.OnTheGround);
            bool collidedWithPlatformSide = false;
            bool airborne = true;
            bool crouchingRisseHiddenBoundingBox;
            if (risse.Animation == (int)state.ducking){
                crouchingRisseHiddenBoundingBox = true;
            }
            else
            {
                crouchingRisseHiddenBoundingBox = false;
            }
            foreach (Gameobject platform in gameworld.Platforms)
            {
                if (physicsEngine.collisionDetection(risse, platform, crouchingRisseHiddenBoundingBox))
                {
                    if (collisionDetermineType(gameworld, risse, platform, prePos))
                    {
                        collidedWithPlatformSide = true;
                    }
                    else
                    {
                        airborne = false;
                    }
                }
            }

            if (!collidedWithPlatformSide)
            {
                foreach (Gameobject ground in gameworld.Ground)
                {
                    if (physicsEngine.collisionDetection(risse, ground, crouchingRisseHiddenBoundingBox))
                    {
                        if (collisionDetermineType(gameworld, risse, ground, prePos))
                        {
                            collidedWithPlatformSide = true;
                        }
                        else
                        {
                            airborne = false;
                        }
                    }
                }
            }

            if (!collidedWithPlatformSide)
            {
                foreach (Gameobject collidable in gameworld.Collidables)
                {
                    if (physicsEngine.collisionDetection(risse, collidable, crouchingRisseHiddenBoundingBox))
                    {
                        collisionHorizontal(gameworld, prePos);
                    }
                }
            }
            if (airborne)
            {
                //risse.OnTheGround = false;
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
                    if (risse.Animation != (int)state.crash && risse.Animation != (int)state.ducking)
                    {
                        gameworld.Risse.Animation = (int)state.running;
                        Console.Out.WriteLine("running");
                    }
                }
                else if (risse.Position.Y < platform.BoundingBox.Y + platform.BoundingBox.Height)
                {
                    if (risse.OnTheGround)
                    {
                        gameworld.Risse.Animation = (int)state.ducking;
                        Console.Out.WriteLine("ducking");
                    }
                    else
                    {
                        collisionVertical(gameworld, new Vector2(risse.Position.X, platform.Position.Y + (platform.BoundingBox.Height + 1)));
                    }
                }
                return false;
            }

            if (risse.OnTheGround && risse.Position.Y + risse.BoundingBox.Height < platform.BoundingBox.Y + platform.BoundingBox.Height)
            {
                collisionVertical(gameworld, new Vector2(risse.Position.X, platform.Position.Y - (risse.BoundingBox.Height - 1)));
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
            if (risse.Animation != (int)state.crash){
                gameworld.Risse.Animation = (int)state.crash;
                Console.Out.WriteLine("crashed");
            }
        }

        //handles landing on or jumping up and hitting a platform
        protected void collisionVertical(Gameworld gameworld, Vector2 newPos)
        {
            gameworld.Risse.Position = newPos;
            gameworld.Risse.Velocity = new Vector2(gameworld.Risse.Velocity.X, 0);
        }
    }
}
