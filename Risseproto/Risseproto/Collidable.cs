using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Risseproto
{
    class Collidable : Gameobject
    {
        public Collidable(Texture2D texture, Vector2 position)
            : base(texture, position)
        {
            
        }
    }
}
