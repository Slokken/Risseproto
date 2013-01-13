using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Risseproto
{
    class Input
    {
        private KeyboardState preKeyState, keyState;
        private MouseState preMouseState, mouseState;
        public event EventHandler duck, jump;
        public delegate void EventHandler();
        private int preY = 0;

        public MouseState PreMouseState
        {
            get { return preMouseState; }
        }
        public MouseState MouseState
        {
            get { return mouseState; }
        }

        private bool WasMouseLeftPressed()
        {
            return (preMouseState.LeftButton == ButtonState.Pressed);
        }

        private bool IsMouseLeftPressed()
        {
            return (mouseState.LeftButton == ButtonState.Pressed);
        }
        public bool WasMouseClicked()
        {
            return (WasMouseLeftPressed() && !IsMouseLeftPressed());
        }

        public void Swipe()
        {
            if (IsMouseLeftPressed() && !WasMouseLeftPressed())
                preY = MouseState.Y;
            else if(WasMouseClicked()) {
                if (preY < MouseState.Y)
                    duck();
                else
                    jump();
            }
        }

        public void Click()
        {
            if (preKeyState.IsKeyUp(Keys.Up) && keyState.IsKeyDown(Keys.Up))
                jump();
            else if (preKeyState.IsKeyUp(Keys.Down) && keyState.IsKeyDown(Keys.Down))
                duck();
        }

        public void Update()
        {
            preKeyState = keyState;
            keyState = Keyboard.GetState();

            preMouseState = MouseState;
            mouseState = Mouse.GetState();
        }
    }
}
