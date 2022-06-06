using System;
using System.Collections;
using UnityEngine;

namespace Sounds
{
    public enum SoundCategory
    {
        Ui,
        Actor
    }

    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance;
        [SerializeField] private Sound[] ActorSounds;
        [SerializeField] private Sound[] UiSounds;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                foreach (Sound s in UiSounds)
                {
                    if (!s.generated)
                    {
                        continue;
                    }
                    s.source = gameObject.AddComponent<AudioSource>();
                    s.source.clip = s.clip;

                    s.source.volume = s.volume;
                    s.source.pitch = s.pitch;
                }
                foreach (Sound s in ActorSounds)
                {
                    if (!s.generated)
                    {
                        continue;
                    }
                    s.source = gameObject.AddComponent<AudioSource>();
                    s.source.clip = s.clip;

                    s.source.volume = s.volume;
                    s.source.pitch = s.pitch;
                }
            }
        }

        public void PlaySound(string nameSound, SoundCategory category, bool loop = false)
        {
            Sound s;
            switch (category)
            {
                case SoundCategory.Actor:
                    s = Array.Find(ActorSounds, sound => sound.name == nameSound);
                    break;
                case SoundCategory.Ui:
                    s = Array.Find(UiSounds, sound => sound.name == nameSound);
                    break;
                default:
                    return;
            }

            if (s.source.loop)
            {
                return;
            }
            if (loop)
            {
                s.source.loop = true;
            }

            s.source.Play();
        }

        public void LoopSound(float time, SoundCategory category,string nameSound, bool infinity)
        {
            StartCoroutine(InvokeEveryXSecond(time, category, nameSound, infinity));
        }

        private IEnumerator InvokeEveryXSecond(float time, SoundCategory category, string nameSound, bool infinity)
        {
            yield return new WaitForSeconds(time);
            PlaySound(nameSound, category, infinity);
            LoopSound(time, category, nameSound, infinity);
        }
    }
}