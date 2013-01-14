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

        public Controller()
        {
            physicsEngine = new PhysicsEngine();
        }

        public void update(Gameworld gameWorld, GameTime gameTime)
        {
            risse = gameWorld.Risse;
            parallaxBackground(gameWorld);
            physicsEngine.gravitation(risse, gameTime);
            collisionResolution(gameWorld);

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

            risse.Velocity = new Vector2(0, -10);
        }

        protected void collisionResolution(Gameworld gameworld)
        {
            bool collidedWithPlatformSide = false;
            foreach (Gameobject platform in gameworld.Platforms)
            {
                if (physicsEngine.collisionDetection(risse, platform))
                {
                    if (collisionDetermineType(gameworld, risse, platform))
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
                        if (collisionDetermineType(gameworld, risse, platform))
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
                        collisionHorizontal(gameworld);
                    }
                }
            }
        }

        // returns true if colliding from side
        protected bool collisionDetermineType(Gameworld gameworld, Gameobject risse, Gameobject platform)
        {
            if (risse.BoundingBox.Right - (risse.BoundingBox.Width/2) > platform.BoundingBox.Left && risse.BoundingBox.Right - (risse.BoundingBox.Width/2) < platform.BoundingBox.Right)
            {
                collisionHorizontal(gameworld);
                return true;
            }

            collisionVertical(gameworld);
            return false;
        }

        //handles crashing
        protected void collisionHorizontal(Gameworld gameworld)
        {
            //gameworld.Risse.Velocity = Vector2.Zero;

            //FUCK MY LIFE HE HAS TO FALL AND STOP
        }

        //handles landing on or jumping up and hitting a platform
        protected void collisionVertical(Gameworld gameworld)
        {
            gameworld.Risse.Position = new Vector2(gameworld.Risse.Position.X, gameworld.Risse.Position.Y - gameworld.Risse.Velocity.Y);
            gameworld.Risse.Velocity = new Vector2(gameworld.Risse.Velocity.X, 0);
        }
    }
}
