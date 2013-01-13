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
        private Gameobject background;
        private List<Gameobject> platforms = new List<Gameobject>();

        public Gameworld(ContentHolder contentHolder)
        {
            risseObject = new Gameobject(contentHolder.risse, Vector2.Zero, Vector2.Zero);
            background = new Gameobject(contentHolder.background, Vector2.Zero, new Vector2(-10, 0));
            platforms = new 
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            background.Draw(spriteBatch);
            risseObject.Draw(spriteBatch);

        }

        public List<Gameobject> Platforms
        {
            get { return platforms; }
        }

        public Gameobject Background
        {
            get { return background; }
            set { this.background = value; }
        }

        public Gameobject Risse
        {
            get { return risseObject; }
        }
    }
}
