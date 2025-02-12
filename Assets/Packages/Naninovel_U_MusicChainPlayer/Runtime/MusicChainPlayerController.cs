using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

namespace Naninovel.U.MusicChainPlayer
{
    public class MusicChainPlayerController : MonoBehaviour
    {
        private AudioSource musicSource;
        private Coroutine musicCoroutine;
        private bool isPlaying;
        private float fadeDuration = 2.5f; // Длительность затухания и нарастания

        public void Init(AudioMixer audioMixer)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("BGM")[0];
            musicSource.loop = false;
            musicSource.playOnAwake = false;
        }

        public void PlayMusicChain(AudioClip[] clips)
        {
            if (clips == null || clips.Length == 0)
            {
                //Debug.LogWarning("MusicChainPlayerController: No clips provided to play.");
                return;
            }

            // Если музыка уже играет, плавно затушим текущую музыку
            if (musicCoroutine != null)
            {
                StopCoroutine(musicCoroutine);
                StartCoroutine(StopWithFadeOut(() =>
                {
                    isPlaying = true;
                    musicCoroutine = StartCoroutine(PlayMusicChainCoroutine(clips));
                }));
            }
            else
            {
                isPlaying = true;
                musicCoroutine = StartCoroutine(PlayMusicChainCoroutine(clips));
            }
        }

        public void StopMusicChain()
        {
            if (musicCoroutine != null)
            {
                StopCoroutine(musicCoroutine);
                musicCoroutine = null;
            }

            if (musicSource.isPlaying)
            {
                StartCoroutine(StopWithFadeOut());
            }
            else
            {
                isPlaying = false;
                musicSource.Stop();
            }
        }

        private IEnumerator StopWithFadeOut(System.Action onComplete = null)
        {
            isPlaying = false; // Прекращаем основное воспроизведение
            yield return StartCoroutine(FadeVolume(musicSource.volume, 0f)); // Затухание громкости
            musicSource.Stop(); // Полная остановка

            onComplete?.Invoke(); // Вызовем коллбек для начала новой музыки
        }

        public void DestoryObj()
        {
            StopMusicChain();
            Destroy(gameObject);
        }

        public void ResetMusic()
        {
            StopMusicChain();
            musicSource.clip = null;
            musicSource.time = 0;
        }

        private IEnumerator PlayMusicChainCoroutine(AudioClip[] clips)
        {
            int index = 0;

            while (isPlaying)
            {
                musicSource.clip = clips[index];
                musicSource.volume = 0f;
                musicSource.Play();

                // Плавное нарастание громкости
                yield return StartCoroutine(FadeVolume(0f, 1f));

                // Ожидаем до момента, когда пора начать затухание
                float clipLength = musicSource.clip.length;
                float fadeStartTime = clipLength - fadeDuration;

                while (musicSource.time < fadeStartTime && isPlaying)
                {
                    yield return null;
                }

                // Начинаем затухание громкости
                if (isPlaying)
                {
                    yield return StartCoroutine(FadeVolume(1f, 0f));
                }

                // Проверяем, играем ли мы еще, и останавливаем текущий трек
                if (isPlaying)
                {
                    musicSource.Stop();
                }

                // Переход к следующему треку
                index = (index + 1) % clips.Length;
            }
        }

        private IEnumerator FadeVolume(float from, float to)
        {
            float elapsed = 0f;

            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                musicSource.volume = Mathf.Lerp(from, to, elapsed / fadeDuration);
                yield return null;
            }

            musicSource.volume = to;
        }
    }
}
