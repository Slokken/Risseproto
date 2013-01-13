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
        private MouseState preMouseState, MouseState;
        public event EventHandler duck, jump;
        public delegate void EventHandler();
        private int preY = 0;

        private bool WasMouseLeftPressed()
        {
            return (preMouseState.LeftButton == ButtonState.Pressed);
        }

        private bool IsMouseLeftPressed()
        {
            return (MouseState.LeftButton == ButtonState.Pressed);
        }

        private void Swipe()
        {
            if (IsMouseLeftPressed() && !WasMouseLeftPressed())
                preY = MouseState.Y;
            else if(WasMouseLeftPressed() && !IsMouseLeftPressed()) {
                if (preY < MouseState.Y)
                    duck();
                else
                    jump();
            }
        }

        private void Click()
        {
            if (preKeyState.IsKeyUp(Keys.Up) && keyState.IsKeyDown(Keys.Up))
                jump();
            else if (preKeyState.IsKeyUp(Keys.Down) && keyState.IsKeyDown(Keys.Down))
                duck();
        }

        public void Update()
        {
            Swipe();
            Click();
            preKeyState = keyState;
            keyState = Keyboard.GetState();

            preMouseState = MouseState;
            MouseState = Mouse.GetState();
        }
    }
}
