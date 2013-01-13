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
        public Texture2D background;
        public Texture2D menuStart;
        public Texture2D menuStartHover;
        public Texture2D menuStartClicked;

        public ContentHolder(ContentManager content)
        {
            loadTextures(content);
            //loadSounds(content);
            //loadParticles(content);
            //loadFonts(content);
        }

        public void loadTextures(ContentManager content)
        {
            risse = content.Load<Texture2D>("Risse\\risse");
            background = content.Load<Texture2D>("Background\\background");
            menuStart = content.Load<Texture2D>("MenuObjects\\start");
            menuStartHover = content.Load<Texture2D>("MenuObjects\\start_hover");
            menuStartClicked = content.Load<Texture2D>("MenuObjects\\start_clicked");
        }
    }
}
