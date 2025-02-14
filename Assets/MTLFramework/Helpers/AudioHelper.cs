using MTLFramework.Helper;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MTLFramework.Helpers {
    public class AudioHelper : SingletonBehaviour<AudioHelper> {
        List<AudioSource> audioSources = new List<AudioSource>();

        public AudioSource Play(AudioClip clip, int loopCount = 1) {
            if (clip == null)
                return null;

            StopSound(clip);
            var audioSource = GetAudioSource();
            audioSource.clip = clip;
            if (loopCount == 1)
                audioSource.loop = false;
            else {
                audioSource.loop = true;
                if (loopCount > 1)
                    DelayDo(clip.length * loopCount, () => {
                        audioSource.Stop();
                    });
            }
            audioSource.Play();

            return audioSource;
        }

        public void StopSound(AudioClip clip) {
            foreach (var audioSound in audioSources.Where(s => s.isPlaying && s.clip == clip)) {
                audioSound.Stop();
            }
        }

        AudioSource GetAudioSource() {
            var audioSource = audioSources.Find(s => !s.loop && !s.isPlaying);
            if (audioSource == null) {
                var gameObject = new GameObject("Audio Source");
                gameObject.transform.SetParent(transform);
                audioSource = gameObject.AddComponent<AudioSource>();
                audioSources.Add(audioSource);
            }
            return audioSource;
        }
    }
}
