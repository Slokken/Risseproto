using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Graphics;

namespace Risseproto
{
    class Controller
    {
        public event EventHandler goToOutro;
        public delegate void EventHandler();
        public Gameobject risse;
        private PhysicsEngine physicsEngine;
        private SoundManager soundManager;
        private ContentHolder contentHolder;
        Vector2 prePos;
        private float ducking = 0;
        private float crashing = 0;

        enum state { running, jumping, ducking, facedown, crash }
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

            if (theState == state.ducking)
            {
                if (ducking > 1000)
                {
                    ducking = 0;
                    theState = state.running;
                    risse.Animation = (int)state.running;
                }
                ducking += (float)gameTime.ElapsedGameTime.Milliseconds;
            }
            if (theState == state.crash)
            {
                if (crashing > 1000)
                {
                    crashing = 0;
                    theState = state.running;
                    risse.Animation = (int)state.running;
                }
                crashing += (float)gameTime.ElapsedGameTime.Milliseconds;
            }

            foreach (List<Gameobject> p in gameWorld.Platforms)
            {
                foreach (Gameobject obj in p)
                {
                    obj.update();
                }
            }
            if (!(MediaPlayer.State == MediaState.Playing))
            {
                //MediaPlayer.Play(contentHolder.music_soundtrack);
            }

            foreach (Gameobject go in gameWorld.BackgroundFluff)
            {
                go.updateFluff(risse, contentHolder);
            }

            gameWorld.Platforms[0][0].Position = new Vector2(gameWorld.BackgroundFluff[0].Position.X - gameWorld.Platforms[0][0].Texture.Width, gameWorld.Platforms[0][0].Position.Y);
            gameWorld.Platforms[0][1].Position = new Vector2(gameWorld.BackgroundFluff[0].Position.X + gameWorld.Platforms[0][1].SpriteWidth, gameWorld.Platforms[0][1].Position.Y);

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

        public void playFootstep(bool jump)
        {
            if (risse.Animation == (int)state.jumping || jump)
            {
                Random rand = new Random();
                int footstep = rand.Next(4);
                switch (footstep)
                {
                    case 0:
                        contentHolder.sound_footstep0.Play();
                        break;
                    case 1:
                        contentHolder.sound_footstep1.Play();
                        break;
                    case 2:
                        contentHolder.sound_footstep2.Play();
                        break;
                    case 3:
                        contentHolder.sound_footstep3.Play();
                        break;
                }
            }
        }

        public void playSlide()
        {
            if (risse.Animation == (int)state.running)
                contentHolder.sound_slide.Play();
        }

        //TODO: actual parallaxing
        public void parallaxBackground(Gameworld gameWorld)
        {
            for (int i = 0; i < gameWorld.Backgrounds.Count; i++ )
            {
                if (gameWorld.Backgrounds[i].Position.X < (-gameWorld.Backgrounds[i].TextureWidth))
                {
                    if (i == 3)
                    {
                        gameWorld.Backgrounds[i].Position = new Vector2(gameWorld.Backgrounds[i].Position.X + gameWorld.Backgrounds[i].TextureWidth, 15);
                    }
                    else
                    {
                        gameWorld.Backgrounds[i].Position = new Vector2(gameWorld.Backgrounds[i].Position.X + gameWorld.Backgrounds[i].TextureWidth, 0);
                    }
                    //gameWorld.Backgrounds[i+1].Position = new Vector2(gameWorld.Backgrounds[i+1].Position.X + gameWorld.Backgrounds[i+1].TextureWidth, 0);
                }
                
                gameWorld.Backgrounds[i].update();
            }
        }

        public void jump()
        {
            if(risse.OnTheGround && theState != state.crash)
            {
           	    theState = state.jumping;
                risse.Animation = (int)state.jumping;
                risse.Position = new Vector2(risse.Position.X, risse.Position.Y -6);
                risse.Velocity = new Vector2(0, -22);
                //soundManager.Play();
                playFootstep(true);
                contentHolder.sound_jump.Play();
            }
        }
        public void duck()
        {
            if (risse.OnTheGround && theState != state.crash)
            {
                playSlide();
                theState = state.ducking;
                risse.Animation = (int)state.ducking;
                //risse.Position = new Vector2(risse.Position.X, risse.Position.Y - 128);
            }
        }

        public void crash()
        {
            theState = state.crash;
            risse.Animation = (int)state.facedown;
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
                        if (risse.BoundingBox.Bottom <= ground.Position.Y + 30 && theState == state.ducking)
                        {
                            risse.Position = new Vector2(risse.Position.X, ground.Position.Y - (risse.BoundingBox.Height * 2) + 1);
                            risse.Velocity = Vector2.Zero;
                            //playFootstep();
                            risse.OnTheGround = true;
                        }
                        else if (risse.BoundingBox.Bottom <= ground.Position.Y + 30 && risse.Velocity.Y > 0)
                        {
                            risse.Position = new Vector2(risse.Position.X, ground.Position.Y - risse.BoundingBox.Height + 1);
                            risse.Velocity = Vector2.Zero;
                            //playFootstep();
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
                        if (risse.BoundingBox.Bottom <= platform.Position.Y + 30 && risse.Velocity.Y > 0 && theState != state.ducking)
                        {
                            risse.Position = new Vector2(risse.Position.X, platform.Position.Y - risse.BoundingBox.Height + 1);
                            risse.Velocity = Vector2.Zero;
                            //playFootstep();
                            risse.OnTheGround = true;
                        }
                    }
                }
            }
            foreach (Gameobject collidable in gameworld.Collidables)
            {
                if (physicsEngine.collisionDetection(risse, collidable))
                {
                    if (risse.BoundingBox.Right <= collidable.Position.X + 30)// && risse.BoundingBox.Bottom >= collidable.Position.Y)
                    {
                        crash();
                        //Console.Out.WriteLine("pang");
                    }
                }
            }
            if (risse.OnTheGround && theState != state.ducking && theState != state.crash)
            {
                playFootstep(false);
                risse.Animation = (int)state.running;
            }

            if (risse.BoundingBox.Top > GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height)
            {
                goToOutro();
            }
        }
    }
}
