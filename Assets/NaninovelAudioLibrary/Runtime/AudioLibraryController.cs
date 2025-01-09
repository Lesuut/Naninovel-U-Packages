using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Naninovel.U.LibraryAudio
{
    public class AudioLibraryController : MonoBehaviour, IDisposable
    {
        private AudioMixer audioMixer;

        private AudioLibraryAudioSourcePlayer bgmPlayer;
        private List<AudioLibraryAudioSourcePlayer> sfxPlayers = new List<AudioLibraryAudioSourcePlayer>();

        public void Init(AudioMixer audioMixer)
        {
            this.audioMixer = audioMixer;

            bgmPlayer = new AudioLibraryAudioSourcePlayer(transform, audioMixer.FindMatchingGroups("BGM")[0], "BGM");
            sfxPlayers.Add(new AudioLibraryAudioSourcePlayer(transform, audioMixer.FindMatchingGroups("SFX")[0], "SFX Defoult"));
        }

        public void PlayBmg(AudioClip audioClip)
        {
            bgmPlayer.AudioSource.Stop();
            bgmPlayer.AudioSource.clip = audioClip;
            bgmPlayer.AudioSource.Play();
            StartCoroutine(AdjustVolumeCoroutine(bgmPlayer.AudioSource, 2));
        }
        public void StopBmg()
        {
            bgmPlayer.AudioSource.Stop();
        }

        public void PlaySfx(AudioClip audioClip)
        {
            sfxPlayers[0].AudioSource.PlayOneShot(audioClip);
        }
        public void PlaySfx(AudioClip audioClip, string key, string group, bool loop, float volume)
        {
            var player = new AudioLibraryAudioSourcePlayer(transform, audioMixer.FindMatchingGroups("SFX")[0], $"SFX {key}/{group}")
            {
                Key = key,
                Group = group,
            };

            player.AudioSource.loop = loop;
            player.AudioSource.volume = volume;
            player.AudioSource.clip = audioClip;
            player.AudioSource.Play();

            sfxPlayers.Add(player);
        }
        public void StopSfx(string key, string group)
        {
            AudioLibraryAudioSourcePlayer itemForDestroy = null;

            foreach (var item in sfxPlayers)
            {
                if (group == "")
                {
                    if (item.Key == key)
                    {
                        itemForDestroy = item;
                        break;
                    }
                }
                else
                {
                    if (item.Key == key && item.Group == group)
                    {
                        itemForDestroy = item;
                        break;
                    }
                }
            }

            if (itemForDestroy != null)
            {
                itemForDestroy.Dispose();
                sfxPlayers.Remove(itemForDestroy);
            }
        }
        public void Dispose()
        {
            bgmPlayer.Dispose();
            foreach (var item in sfxPlayers)
            {
                item.Dispose();
            }
            sfxPlayers.Clear();

            ObjectUtils.DestroyOrImmediate(gameObject);
        }
        public void ResetController()
        {
            for (int i = sfxPlayers.Count - 1; i > 0; i--)
            {
                var player = sfxPlayers[i];
                player.Dispose();
                sfxPlayers.RemoveAt(i);
            }
            bgmPlayer.AudioSource.Stop();
        }
        private IEnumerator AdjustVolumeCoroutine(AudioSource source, float duration = 1)
        {
            float originalVolume = source.volume;

            source.volume = 0f;

            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                source.volume = Mathf.Lerp(0f, originalVolume, elapsedTime / duration);

                elapsedTime += Time.deltaTime;

                yield return null;
            }

            source.volume = originalVolume;
        }
    }
}