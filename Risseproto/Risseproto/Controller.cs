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


    }
}
