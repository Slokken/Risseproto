﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Risseproto
{
    class Controller
    {

        public Gameobject risse;
        private PhysicsEngine physicsEngine;
        private SoundManager soundManager;
        private Input key;
        Vector2 prePos;

        enum state { running, jumping, ducking, idonteven, facedown, crash }
        private state theState = state.running;

        public Controller(Input input, SoundManager soundManager)
        {
            physicsEngine = new PhysicsEngine();
            this.key = input;
            this.soundManager = soundManager;
            input.jump += new Input.EventHandler(jump);
            input.duck += new Input.EventHandler(duck);
        }

        public void update(Gameworld gameWorld, GameTime gameTime)
        {

            risse = gameWorld.Risse;
            parallaxBackground(gameWorld);

            physicsEngine.gravitation(risse, gameTime);
            collisionResolution(gameWorld, prePos);
            prePos = new Vector2(risse.BoundingBox.X, risse.BoundingBox.Y);
            risse.update(gameTime);
            foreach (Gameobject go in gameWorld.Platforms)
            {
                go.update();
            }

            foreach (Gameobject go in gameWorld.BackgroundFluff)
            {
                go.updateFluff((int)risse.BoundingBox.X);
            }

            //foreach (Gameobject ground in gameWorld.Ground)
            //{
            //    ground.update();
            //}

            foreach (List<Gameobject> g in gameWorld.Ground)
            {
                foreach (Gameobject obj in g)
                {
                    obj.update();
                }
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
            if (theState == state.ducking)
            {
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
                risse.Animation = (int)state.ducking;
                //risse.Position = new Vector2(risse.Position.X, risse.Position.Y - 128);
            }
        }
        public void ResetBoundingBox()
        {
            risse.Position = new Vector2(risse.Position.X, risse.Position.Y - risse.BoundingBox.Height);
            risse.BoundingBox = new Rectangle(0, 0, risse.BoundingBox.Width / 2, risse.NormalBoundingHeight);
        }

        protected void collisionResolution(Gameworld gameworld, Vector2 prePos)
        {
            risse.OnTheGround = false;
            foreach (List<Gameobject> list in gameworld.Ground)
            {
                foreach (Gameobject ground in list)
                {
                    if (physicsEngine.collisionDetection(risse, ground))
                    {
                        if (risse.BoundingBox.Bottom <= ground.Position.Y + 20 && theState == state.ducking)
                        {
                            risse.Position = new Vector2(risse.Position.X, ground.Position.Y - (risse.BoundingBox.Height * 2) + 1);
                            risse.Velocity = Vector2.Zero;
                            risse.OnTheGround = true;
                        }
                        else if (risse.BoundingBox.Bottom <= ground.Position.Y + 20 && risse.Velocity.Y > 0)
                        {
                            risse.Position = new Vector2(risse.Position.X, ground.Position.Y - risse.BoundingBox.Height + 1);
                            risse.Velocity = Vector2.Zero;
                            risse.OnTheGround = true;
                        }
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

            if (risse.OnTheGround && risse.BoundingBox.Y + risse.BoundingBox.Height < platform.BoundingBox.Y + platform.BoundingBox.Height)
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
