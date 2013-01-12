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
        public Texture2D risse;

        public ContentHolder(ContentManager content)
        {
            loadTextures(content);
            //loadSounds(content);
            //loadParticles(content);
            //loadFonts(content);
        }

        public void loadTextures(ContentManager content)
        {
            risse = content.Load<Texture2D>("risse");
        }
    }
}
