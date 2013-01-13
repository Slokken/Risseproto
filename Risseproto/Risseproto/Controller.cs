using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Risseproto
{
    class Controller
    {
        public Controller()
        {

        }

        public void update(Gameworld gameWorld, GameTime gameTime)
        {
            parallaxBackground(gameWorld);
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
    }
}
