using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;

namespace Game1
{
    static public class AudioSupport
    {
        private static Dictionary<String, SoundEffect> sAudioEffects =
            new Dictionary<String, SoundEffect>();
        private static SoundEffectInstance sBackgroundAudio = null;

        static private SoundEffect FindAudioClip(String name)
        {
            SoundEffect sound = null;
            if (sAudioEffects.ContainsKey(name))
                sound = sAudioEffects[name];
            else
            {
                sound = Game1.sContent.Load<SoundEffect>(name);
                if (sound != null)
                    sAudioEffects.Add(name, sound);
            }
            return sound;
        }

        static public void PlayCue(String cueName)
        {
            SoundEffect sound = FindAudioClip(cueName);
            if (sound != null)
                sound.Play();
        }

        static private void StartBg(String name, float level)
        {
            SoundEffect bgm = FindAudioClip(name);
            sBackgroundAudio = bgm.CreateInstance();
            sBackgroundAudio.IsLooped = true;
            sBackgroundAudio.Volume = level;
            sBackgroundAudio.Play();
        }

        static private void StopBg()
        {
            if (sBackgroundAudio != null)
            {
                sBackgroundAudio.Pause();
                sBackgroundAudio.Stop();
                sBackgroundAudio.Volume = 0f;
                sBackgroundAudio.Dispose();
            }
            sBackgroundAudio = null;
        }

        static public void PlayBackgroundAudio(String bgAudio, float level)
        {
            StopBg();
            if ((bgAudio != "") || (bgAudio != null))
            {
                level = MathHelper.Clamp(level, 0f, 1f);
                StartBg(bgAudio, level);
            }
        }
    }
}
