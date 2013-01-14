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

        private int numberOfSections = 30;           // HIRRHIRRHIRR

        private ContentHolder contentHolder;

        private List<Gameobject> backgrounds = new List<Gameobject>();
        private List<Gameobject> backgrounds2 = new List<Gameobject>();

        private List<Gameobject> platforms = new List<Gameobject>();
        private List<Gameobject> collidables = new List<Gameobject>();
        private List<List<Gameobject>> ground = new List<List<Gameobject>>();
        Random rand = new Random();
        //BECAUSE FUCK YOU THAT'S WHY

        public Gameworld(ContentHolder contentHolder)
        {
            risseObject = new Gameobject(contentHolder.texture_risse, new Vector2(100, 0), Vector2.Zero, 100, 256, 256);

            backgrounds.Add(new Gameobject(contentHolder.texture_background4, Vector2.Zero, new Vector2(-1, 0)));
            backgrounds.Add(new Gameobject(contentHolder.texture_background3, Vector2.Zero, new Vector2(-3, 0)));
            backgrounds.Add(new Gameobject(contentHolder.texture_background1, Vector2.Zero, new Vector2(-6, 0)));
            backgrounds.Add(new Gameobject(contentHolder.texture_background2, Vector2.Zero, new Vector2(-8, 0)));

            platforms.Add(new Gameobject(contentHolder.texture_platform, new Vector2(900, 500), new Vector2(-6,0)));
            this.contentHolder = contentHolder;


            generateMap();

            //ground = makePlatformSection(new Vector2(0, 670));

            //int platformWidth = contentHolder.texture_platform.Width;
            //for (int i = 0; i < NUMBEROFPLATFORMS; i++)
            //{
            //    ground.Add(new Gameobject(contentHolder.texture_platform, new Vector2(- contentHolder.texture_platform.Width + platformWidth, 688), new Vector2(-3, 0)));
            //    platformWidth += contentHolder.texture_platform.Width;
            //}

            //ground.Add(new Gameobject(contentHolder.texture_platform, new Vector2(1, 688), new Vector2(-3, 0)));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < backgrounds.Count; i++)
            {
                backgrounds[i].DrawDuplicateMUTHAAAAAAA(spriteBatch);
            }


            risseObject.DrawAnimation(spriteBatch);

            //for (int i = 0; i < ground.Count; i++)
            //{
            //    ground[i].Draw(spriteBatch);
            //}

            foreach (List<Gameobject> g in ground)
            {
                foreach (Gameobject obj in g)
                {
                    obj.Draw(spriteBatch);
                }
            }

            foreach (Gameobject go in platforms)
            {
                go.Draw(spriteBatch);
            }

        }


        public void generateMap()
        {
            int gap = 200;
            int startCoordinate;
            ground.Add(makePlatformSection(new Vector2(0, 670)));

            // o_O
            startCoordinate = (int) (ground[ground.Count - 1][ground[ground.Count - 1].Count - 1].Position.X + gap);

            for(int i = 0; i < numberOfSections; i++)
            {

                ground.Add(makePlatformSection(new Vector2(startCoordinate, 670)));
                //startCoordinate = (int)(ground[ground.Count - 1].Position.X + gap);
                startCoordinate = (int)(ground[ground.Count - 1][ground[ground.Count - 1].Count - 1].Position.X + gap);
            }
        }

        public List<Gameobject> Collidables
        {
            get { return collidables; }
        }

        //public List<Gameobject> Ground
        //{
        //    get { return ground; }
        //}

        public List<List<Gameobject>> Ground
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

        public List<Gameobject> makePlatformSection(Vector2 startCoordinates)
        {
            List<Gameobject> section = new List<Gameobject>();

            
            Vector2 platformVelocity = new Vector2(-10, 0);
            int numberOfTiles = rand.Next(10);
            

            int platformWidth = (int) (contentHolder.texture_platform_middle.Width + startCoordinates.X);

            section.Add(new Gameobject(contentHolder.texture_platform_start, startCoordinates, platformVelocity));

            for (int i = 0; i < numberOfTiles; i++)
            {
                section.Add(new Gameobject(contentHolder.texture_platform_middle, new Vector2(platformWidth, startCoordinates.Y), platformVelocity));
                platformWidth += contentHolder.texture_platform_middle.Width;
            }

            section.Add(new Gameobject(contentHolder.texture_platform_end, new Vector2(platformWidth, startCoordinates.Y), platformVelocity));

            return section;
        }
    }
}
