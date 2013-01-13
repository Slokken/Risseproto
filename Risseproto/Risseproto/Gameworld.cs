using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Risseproto
{
    class Gameworld
    {
        private Gameobject risseObject;
        private Background background;

        public Gameworld(ContentHolder contentHolder)
        {
            risseObject = new Gameobject(contentHolder.risse, Vector2.Zero);
            background = new Background();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            risseObject.Draw(spriteBatch);

        }
    }
}
