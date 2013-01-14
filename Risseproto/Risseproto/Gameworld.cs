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
        private const int NUMBEROFPLATFORMS = 10;
        private Gameobject risseObject;
        private Gameobject background;
        private List<Gameobject> platforms = new List<Gameobject>();
        private List<Gameobject> collidables = new List<Gameobject>();
        private List<Gameobject> ground = new List<Gameobject>();
        //BECAUSE FUCK YOU THAT'S WHY

        public Gameworld(ContentHolder contentHolder)
        {
            risseObject = new Gameobject(contentHolder.texture_risse, Vector2.Zero, Vector2.Zero);
            background = new Gameobject(contentHolder.texture_background, Vector2.Zero, new Vector2(-10, 0));


            int platformWidth = contentHolder.texture_platform.Width;
            for (int i = 0; i < NUMBEROFPLATFORMS; i++)
            {
                ground.Add(new Gameobject(contentHolder.texture_platform, new Vector2(- 200 + platformWidth, 688), Vector2.Zero));
                platformWidth += 200;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            background.Draw(spriteBatch);
            risseObject.Draw(spriteBatch);

            for (int i = 0; i < ground.Count; i++)
            {
                ground[i].Draw(spriteBatch);
            }

        }

        public List<Gameobject> Collidables
        {
            get { return collidables; }
        }

        public List<Gameobject> Ground
        {
            get { return ground; }
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

        public void makeGround()
        {

        }
    }
}
