using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Game1
{
    public class ParticlePrimitive : GameObject
    {
        protected float kLifeSpanRandomness = 0.4f;
        protected float kSizeChangeRandomness = 0.5f;
        protected float kSizeRandomness = 0.3f;
        protected float kSpeedRandomness = 0.1f;

        // Number of updates before a particle disappear
        protected int mLifeSpan;
        // How fast does the particle changes size
        protected float mSizeChangeRate;

        public ParticlePrimitive(Vector2 position, float size, int lifeSpan) :
            base("ParticleImage", position, new Vector2(size, size))
        {
            mLifeSpan = (int)(lifeSpan *
                        Game1.RandomNumber(-kLifeSpanRandomness, kLifeSpanRandomness));

            mVelocityDir.X = Game1.RandomNumber(-0.5f, 0.5f);
            mVelocityDir.Y = Game1.RandomNumber(-0.5f, 0.5f);
            mVelocityDir.Normalize();
            mSpeed = Game1.RandomNumber(kSpeedRandomness);

            mSizeChangeRate = Game1.RandomNumber(kSizeChangeRandomness);

            mSize.X *= Game1.RandomNumber(1f - kSizeRandomness, 1 + kSizeRandomness);
            mSize.Y = mSize.X;
        }

        public bool Expired { get { return (mLifeSpan < 0); } }

        public override void Update()
        {
            base.Update();

            mLifeSpan--;   // Continue to approach expiration

            // Change its size
            mSize.X += mSizeChangeRate;
            mSize.Y += mSizeChangeRate;

            // Change the tint color randomly
            Byte[] b = new Byte[3];
            Game1.sRan.NextBytes(b);
            mTintColor.R += b[0];
            mTintColor.G += b[1];
            mTintColor.B += b[2];
        }
    }
}