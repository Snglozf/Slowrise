using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace PROJECT
{
    public static class SoundManager
    {
        private static Dictionary<string, SoundEffect> soundEffects = new Dictionary<string, SoundEffect>();
        private static SoundEffectInstance currentMusicInstance = null;

        public static void LoadContent(ContentManager content)
        {
          
            soundEffects["Audio/level_music"] = content.Load<SoundEffect>("Audio/level_music");
            soundEffects["Audio/win_music"] = content.Load<SoundEffect>("Audio/win_music");
            soundEffects["Audio/lose_music"] = content.Load<SoundEffect>("Audio/lose_music");
            
        }

        public static void PlayMusic(string soundPath, bool isRepeating)
        {
            if (currentMusicInstance != null)
            {
                currentMusicInstance.Stop();
                currentMusicInstance.Dispose();
                currentMusicInstance = null;
            }

            if (soundEffects.ContainsKey(soundPath))
            {
                currentMusicInstance = soundEffects[soundPath].CreateInstance();
                currentMusicInstance.IsLooped = isRepeating;
                currentMusicInstance.Play();
            }
        }

        public static void StopMusic()
        {
            if (currentMusicInstance != null)
            {
                currentMusicInstance.Stop();
            }
        }
    }
}