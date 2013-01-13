using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Risseproto
{
    class Button
    {
        private int state;
        private Rectangle box;
        private List<Texture2D> background;
        private string text;
        private Input input;
        public event EventHandler clicked;
        public delegate void EventHandler(string action);

        public Button(Rectangle box, string buttontext, ref Input input, List<Texture2D> background)
        {
            this.box = box;
            this.text = buttontext;
            this.input = input;
            this.background = background;
        }

        public void Update()
        {
            if (box.Contains(input.MouseState.X, input.MouseState.Y) && input.WasMouseClicked())
            {
                state = 2;
                clicked("start");
            }
            else if (box.Contains(input.MouseState.X, input.MouseState.Y) && state != 1)
                state = 1;
            else if (!box.Contains(input.MouseState.X, input.MouseState.Y) && state != 0)
                state = 0;
              //  System.Diagnostics.Debug.WriteLine("Normal");
        }
        public void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(background[state], box, Color.White);
        }
    }
}
