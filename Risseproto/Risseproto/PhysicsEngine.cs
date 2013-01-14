using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Risseproto
{
    class PhysicsEngine
    {
        const float GRAVITYCONSTANT = 9.81f;

        public PhysicsEngine()
        {

        }

        public void gravitation(Gameobject gameObject, GameTime gameTime)
        {
            
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;//0.01666667 (or 16.67 ms) @ 60FPS
            gameObject.Velocity += new Vector2(0, GRAVITYCONSTANT * elapsed * 2);
            gameObject.Position += gameObject.Velocity * elapsed;
        }

        public bool collisionDetection(Gameobject object1, Gameobject object2)
        {
            if (object1.BoundingBox.Intersects(object2.BoundingBox))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool collisionDetectionCrouching(Rectangle secretBox, Gameobject object2)
        {

            if (secretBox.Intersects(object2.BoundingBox))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
