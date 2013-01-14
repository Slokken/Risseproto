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

        private int risseSpeed = 10;

        private const int GROUNDHEIGHT = 670;

        private int numberOfSections = 30;           // HIRRHIRRHIRR
        private int checkpointInterval = 4000;

        private ContentHolder contentHolder;

        private List<Gameobject> backgrounds = new List<Gameobject>();
        private List<Gameobject> backgrounds2 = new List<Gameobject>();

        private List<List<Gameobject>> platforms = new List<List<Gameobject>>();
        private List<Gameobject> collidables = new List<Gameobject>();
        private List<List<Gameobject>> ground = new List<List<Gameobject>>();
        Random rand = new Random();
        //BECAUSE FUCK YOU THAT'S WHY
        private List<Gameobject> background_fluff = new List<Gameobject>();

        public Gameworld(ContentHolder contentHolder)
        {
            risseObject = new Gameobject(contentHolder.texture_risse, new Vector2(100, 0), Vector2.Zero, 100, 192, 192);

            backgrounds.Add(new Gameobject(contentHolder.texture_background4, Vector2.Zero, new Vector2(-1, 0)));
            backgrounds.Add(new Gameobject(contentHolder.texture_background3, Vector2.Zero, new Vector2(-3, 0)));
            backgrounds.Add(new Gameobject(contentHolder.texture_background1, Vector2.Zero, new Vector2(-6, 0)));
            backgrounds.Add(new Gameobject(contentHolder.texture_background2, Vector2.Zero, new Vector2(-8, 0)));

            //platforms.Add(new Gameobject(contentHolder.texture_platform, new Vector2(900, 500), new Vector2(-6,0)));

            background_fluff.Add(new Gameobject(contentHolder.texture_checkpoint, new Vector2(checkpointInterval, 550), new Vector2(-10, 0), 256, 256));
            platforms.Add(List < Gameobject > makePlatformSection(new Vector2(checkpointInterval, GROUNDHEIGHT), 0, 0));
            this.contentHolder = contentHolder;


            generateMap();
            generateObstacles();
            generatePlatforms();

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

            for (int i = 0; i < background_fluff.Count; i++)
            {
                background_fluff[i].DrawFluff(spriteBatch);
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

            foreach (Gameobject coll in collidables)
            {
                coll.Draw(spriteBatch);
            }

            foreach (List<Gameobject> p in platforms)
            {
                foreach (Gameobject obj in p)
                {
                    obj.Draw(spriteBatch);
                }
            }

        }


        public void generateMap()
        {
            int gap = rand.Next(50, 200);
            int startCoordinate;
            ground.Add(makePlatformSection(new Vector2(0, GROUNDHEIGHT), 20));

            // o_O
            startCoordinate = (int) (ground[ground.Count - 1][ground[ground.Count - 1].Count - 1].Position.X + gap);

            for(int i = 0; i < numberOfSections; i++)
            {

                ground.Add(makePlatformSection(new Vector2(startCoordinate, GROUNDHEIGHT), 10));
                gap = rand.Next(100, 600 );
                //startCoordinate = (int)(ground[ground.Count - 1].Position.X + gap);
                startCoordinate = (int)(ground[ground.Count - 1][ground[ground.Count - 1].Count - 1].Position.X + gap);
            }
        }

        private void generatePlatforms()
        {
            Vector2 position = new Vector2(900, 350);

            for (int i = 0; i < numberOfSections / 2; i++)
            {

                Platforms.Add(makePlatformSection(position, 2));

                position.X = position.X + rand.Next(600, 2000);
                position.Y = rand.Next(250, 350);
            }
        }

        private void generateObstacles()
        {
            int obstacleCoordinate = 0;

            foreach (List<Gameobject> g in ground)
            {
                if (getSectionLength(g) > 600)
                {
                    Texture2D texture = getRandomCollidableTexture();
                    obstacleCoordinate = rand.Next((int)g[0].Position.X + texture.Width * 2, (int)g[0].Position.X + getSectionLength(g) - texture.Width * 2);

                    collidables.Add(new Gameobject(texture, new Vector2(obstacleCoordinate, GROUNDHEIGHT - texture.Height + 20),new Vector2(- risseSpeed, 0)));
                }
            }
        }

        private Texture2D getRandomCollidableTexture()
        {
            int random = rand.Next(0, 3);

            switch (random)
            {
                case 0:
                    return contentHolder.collidables[0];
                    
                case 1:
                    return contentHolder.collidables[1];

                case 2:
                    return contentHolder.collidables[2];

                default:
                    return null;
            }
        }

        private int getSectionLength(List<Gameobject> section)
        {
            int sectionLength = 0;

            foreach(Gameobject sectionPiece in section)
            {
                sectionLength += sectionPiece.TextureWidth;
            }

            return sectionLength;
        }

        public List<Gameobject> Collidables
        {
            get { return collidables; }
        }

        public List<Gameobject> BackgroundFluff
        {
            get { return background_fluff; }
        }

        //public List<Gameobject> Ground
        //{
        //    get { return ground; }
        //}

        public List<List<Gameobject>> Ground
        {
            get { return ground; }
        }

        public List<List<Gameobject>> Platforms
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

        public List<Gameobject> makePlatformSection(Vector2 startCoordinates, int randomMax)
        {
            List<Gameobject> section = new List<Gameobject>();

            
            Vector2 platformVelocity = new Vector2(-risseSpeed, 0);
            int numberOfTiles = rand.Next(randomMax);
            

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
