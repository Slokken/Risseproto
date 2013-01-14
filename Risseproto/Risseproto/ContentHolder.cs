using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace Risseproto
{
    class ContentHolder
    {
        public Texture2D texture_risse;
        public Texture2D texture_background;
        public Texture2D texture_platform;

        public Texture2D menuStart;
        public Texture2D menuStartHover;
        public Texture2D menuStartClicked;

        public SoundEffect jump;

        public ContentHolder(ContentManager content)
        {
            loadTextures(content);
            loadSounds(content);
            //loadParticles(content);
            //loadFonts(content);
        }

        private void loadSounds(ContentManager content)
        {
            jump = content.Load<SoundEffect>("SoundEffects\\jump");
        }

        private void loadTextures(ContentManager content)
        {
            texture_risse       = content.Load<Texture2D>("Risse\\spriteSheet");
            texture_background  = content.Load<Texture2D>("Background\\background");
            texture_platform    = content.Load<Texture2D>("Object\\platform"); 
            menuStart = content.Load<Texture2D>("MenuObjects\\start");
            menuStartHover = content.Load<Texture2D>("MenuObjects\\start_hover");
            menuStartClicked = content.Load<Texture2D>("MenuObjects\\start_clicked");
        }
    }
}
