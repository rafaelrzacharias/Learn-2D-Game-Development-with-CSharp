using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game1
{
    public class ParticleSystem
    {
        private List<ParticlePrimitive> mAllParticles;
        private List<ParticleEmitter> mAllEmitters;

        public ParticleSystem()
        {
            mAllParticles = new List<ParticlePrimitive>();
            mAllEmitters = new List<ParticleEmitter>();
        }

        public void AddEmitterAt(Vector2 pos)
        {
            ParticleEmitter e = new ParticleEmitter(pos, (int)Game1.RandomNumber(50, 100));
            mAllEmitters.Add(e);
        }

        public void UpdateParticles()
        {
            int emittersCount = mAllEmitters.Count;
            for (int i = emittersCount - 1; i >= 0; i--)
            {
                mAllEmitters[i].EmitParticles(mAllParticles);
                if (mAllEmitters[i].Expired)
                    mAllEmitters.RemoveAt(i);
            }

            int particleCounts = mAllParticles.Count;
            for (int i = particleCounts - 1; i >= 0; i--)
            {
                mAllParticles[i].Update();
                if (mAllParticles[i].Expired)
                    mAllParticles.RemoveAt(i);  // remove expired ones
            }
        }

        public void DrawParticleSystem()
        {
            // 1. Switch blend mode to "Additive"
            Game1.sSpriteBatch.End();
            Game1.sSpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);

            // 2. Draw all particles
            foreach (var particle in mAllParticles)
                particle.Draw();

            // 3. Switch blend mode back to AlphaBlend
            Game1.sSpriteBatch.End();
            Game1.sSpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
        }
    }
}