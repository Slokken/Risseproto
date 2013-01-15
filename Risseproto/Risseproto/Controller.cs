using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;

namespace Risseproto
{
    class Controller
    {

        public Gameobject risse;
        private PhysicsEngine physicsEngine;
        private SoundManager soundManager;
        private ContentHolder contentHolder;
        Vector2 prePos;

        enum state { running, jumping, ducking, idonteven, facedown, crash }
        private state theState = state.running;

        public Controller(Input input, SoundManager soundManager, ContentHolder contentHolder)
        {
            physicsEngine = new PhysicsEngine();
            
            this.contentHolder = contentHolder;

            this.soundManager = soundManager;
            input.jump += new Input.EventHandler(jump);
            input.duck += new Input.EventHandler(duck);

            soundManager.playSoundtrack();
        }

        public void update(Gameworld gameWorld, GameTime gameTime)
        {

            risse = gameWorld.Risse;
            parallaxBackground(gameWorld);

            physicsEngine.gravitation(risse, gameTime);
            collisionResolution(gameWorld, prePos);
            prePos = new Vector2(risse.BoundingBox.X, risse.BoundingBox.Y);
            risse.update(gameTime);
            //foreach (Gameobject go in gameWorld.Platforms)
            //{
            //    go.update();
            //}

            foreach (List<Gameobject> p in gameWorld.Platforms)
            {
                foreach (Gameobject obj in p)
                {
                    obj.update();
                }
            }
            //if (!(MediaPlayer.State == MediaState.Playing))
            //{
            //    MediaPlayer.Play(contentHolder.soundtrack);
            //}

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

            foreach (Gameobject obj in gameWorld.Collidables)
            {
                obj.update();
            }

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
            foreach (List<Gameobject> list in gameworld.Platforms)
            {
                foreach (Gameobject platform in list)
                {
                    if (physicsEngine.collisionDetection(risse, platform))
                    {
                        if (risse.BoundingBox.Bottom <= platform.Position.Y + 20 && theState == state.ducking)
                        {
                            risse.Position = new Vector2(risse.Position.X, platform.Position.Y - (risse.BoundingBox.Height * 2) + 1);
                            risse.Velocity = Vector2.Zero;
                            risse.OnTheGround = true;
                        }
                        else if (risse.BoundingBox.Bottom <= platform.Position.Y + 20 && risse.Velocity.Y > 0)
                        {
                            risse.Position = new Vector2(risse.Position.X, platform.Position.Y - risse.BoundingBox.Height + 1);
                            risse.Velocity = Vector2.Zero;
                            risse.OnTheGround = true;
                        }
                    }
                }
            }
            if (risse.OnTheGround && theState != state.ducking)
            {
                risse.Animation = (int)state.running;
            }
        }
    }
}
