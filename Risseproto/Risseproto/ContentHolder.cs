using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Risseproto
{
    class ContentHolder
    {
        public Texture2D texture_risse;
        public Texture2D texture_background;
        public Texture2D texture_platform;

        public ContentHolder(ContentManager content)
        {
            loadTextures(content);
            //loadSounds(content);
            //loadParticles(content);
            //loadFonts(content);
        }

        public void loadTextures(ContentManager content)
        {
            texture_risse       = content.Load<Texture2D>("Risse\\risse");
            texture_background  = content.Load<Texture2D>("Background\\background");
            texture_platform    = content.Load<Texture2D>("Object\\platform"); 
        }
    }
}
