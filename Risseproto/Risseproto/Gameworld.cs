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

        private List<Gameobject> backgrounds = new List<Gameobject>();
        private List<Gameobject> backgrounds2 = new List<Gameobject>();

        private List<Gameobject> platforms = new List<Gameobject>();
        private List<Gameobject> collidables = new List<Gameobject>();
        private List<Gameobject> ground = new List<Gameobject>();
        //BECAUSE FUCK YOU THAT'S WHY

        public Gameworld(ContentHolder contentHolder)
        {
            risseObject = new Gameobject(contentHolder.texture_risse, new Vector2(100, 0), Vector2.Zero, 100, 256, 256);

            backgrounds.Add(new Gameobject(contentHolder.texture_background4, Vector2.Zero, new Vector2(-1, 0)));
            
            backgrounds2.Add(new Gameobject(contentHolder.texture_background3, new Vector2(contentHolder.texture_background3.Width, 0), new Vector2(-3, 0)));
            backgrounds.Add(new Gameobject(contentHolder.texture_background3, Vector2.Zero, new Vector2(-3, 0)));

            backgrounds2.Add(new Gameobject(contentHolder.texture_background1, new Vector2(contentHolder.texture_background1.Width, 0), new Vector2(-6, 0)));
            backgrounds.Add(new Gameobject(contentHolder.texture_background1, Vector2.Zero, new Vector2(-6, 0)));

            backgrounds2.Add(new Gameobject(contentHolder.texture_background2, new Vector2(contentHolder.texture_background2.Width, 0), new Vector2(-8, 0)));
            backgrounds.Add(new Gameobject(contentHolder.texture_background2, Vector2.Zero, new Vector2(-8, 0)));

            platforms.Add(new Gameobject(contentHolder.texture_platform, new Vector2(900, 500), new Vector2(-6,0)));

            int platformWidth = contentHolder.texture_platform.Width;
            for (int i = 0; i < NUMBEROFPLATFORMS; i++)
            {
                ground.Add(new Gameobject(contentHolder.texture_platform, new Vector2(- 200 + platformWidth, 550), Vector2.Zero));
                platformWidth += 200;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < backgrounds.Count; i++)
            {
                backgrounds[i].DrawDuplicateMUTHAAAAAAA(spriteBatch);
            }


            risseObject.DrawAnimation(spriteBatch);

            for (int i = 0; i < ground.Count; i++)
            {
                ground[i].Draw(spriteBatch);
            }
            foreach (Gameobject go in platforms)
            {
                go.Draw(spriteBatch);
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

        public List<Gameobject> Backgrounds
        {
            get { return backgrounds; }
            set { this.backgrounds = value; }
        }

        public List<Gameobject> Backgrounds2
        {
            get { return backgrounds2; }
            set { this.backgrounds2 = value; }
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
