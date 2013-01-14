using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Risseproto
{
    class SoundManager
    {
        private ContentHolder soundContent;

        public SoundManager(ContentHolder soundContent)
        {
            this.soundContent = soundContent;
        }

       public void Play()
        {
            soundContent.jump.Play();
        }

       public void playSoundtrack()
       {
           Microsoft.Xna.Framework.Media.Song instance = soundContent.soundtrack;
       }
    }
}
