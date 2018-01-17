using System;
using Microsoft.Xna.Framework;

namespace Game1
{
    public class RotateObject : GameObject
    {
        public RotateObject(String image, Vector2 center, float radius)
                : base(image, center, new Vector2(radius * 2f, radius * 2f))
        {
        }

        public float Radius { get { return mSize.X / 2f; } set { mSize.X = 2f * value; mSize.Y = mSize.X; } }

        override public void Update()
        {
            base.Update(); // Moves object by velocity

            #region Step 2a.
            Vector2 v = Velocity;
            v.Y -= GameState.sGravity;
            Velocity = v;
            #endregion

            #region Step 2b.
            // Now rotate the object according to the speed in x direction
            float angularDisplace = (v.X / Radius);
            #endregion

            #region Step 2b.
            // This assumes the object is rolling "on-top of" surfaces
            if (v.X > 0)
                mRotateAngle += angularDisplace;
            else
                mRotateAngle -= angularDisplace;
            #endregion
        }
    }
}
