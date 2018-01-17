using Microsoft.Xna.Framework;
using System;

namespace Game1
{
    public class ReddishParticlePrimitive : ParticlePrimitive
    {
        public ReddishParticlePrimitive(Vector2 position, float size, int lifeSpan) :
            base(position, size, lifeSpan)
        {
            mVelocityDir.Y = 5f * Math.Abs(mVelocityDir.Y);
            mVelocityDir.Normalize();
            mSpeed *= 5.25f;
            mSizeChangeRate *= 1.5f;
            mSize.X *= 0.7f;
            mSize.Y = mSize.X;

            mTintColor = Color.DarkOrange;
        }
        public override void Update()
        {
            base.Update();

            Color s = mTintColor;
            if (s.R < 255)
                s.R += 1;
            if (s.G != 0)
                s.G -= 1;
            if (s.B != 0)
                s.B -= 1;
            mTintColor = s;
        }
    }
}