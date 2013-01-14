﻿using System;
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

        public Texture2D texture_background1;
        public Texture2D texture_background2;
        public Texture2D texture_background3;
        public Texture2D texture_background4;

        public Texture2D texture_platform_start;
        public Texture2D texture_platform_middle;
        public Texture2D texture_platform_end;
        public Texture2D texture_platform;

        public Texture2D texture_checkpoint;

        public Texture2D menuStart;
        public Texture2D menuStartHover;
        public Texture2D menuStartClicked;
        public Texture2D menuBackground;

        public static Texture2D textureRectangle;

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

            texture_background1 = content.Load<Texture2D>("Background\\background1");
            texture_background2 = content.Load<Texture2D>("Background\\background2");
            texture_background3 = content.Load<Texture2D>("Background\\background3");
            texture_background4 = content.Load<Texture2D>("Background\\background4");

            texture_platform        = content.Load<Texture2D>("Object\\platform");
            texture_platform_start  = content.Load<Texture2D>("Object\\platform_start");
            texture_platform_middle = content.Load<Texture2D>("Object\\platform_middle");
            texture_platform_end    = content.Load<Texture2D>("Object\\platform_end");

            texture_checkpoint  = content.Load<Texture2D>("Background\\SnowmanSpriteSheet"); 

            menuStart           = content.Load<Texture2D>("MenuObjects\\start");
            menuStartHover      = content.Load<Texture2D>("MenuObjects\\start_hover");
            menuStartClicked    = content.Load<Texture2D>("MenuObjects\\start_clicked");
            menuBackground      = content.Load<Texture2D>("MenuObjects\\meny_bg");

            textureRectangle = content.Load<Texture2D>("rasstangle");
        }
    }
}
