using System;
using UnityEngine;

namespace Sounds
{
    [Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;

        [Range(0f, 1f)]
        public float volume;
        [Range(.1f, 3f)]
        public float pitch;

        public bool generated = true;

        [HideInInspector]
        public AudioSource source;
    }
}