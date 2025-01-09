using UnityEngine;
using UnityEngine.Audio;

namespace Naninovel.U.LibraryAudio
{
    public class AudioLibraryAudioSourcePlayer
    {
        public string Group;
        public string Key;
        public GameObject GameObject { get; private set; }
        public AudioSource AudioSource { get; private set; }

        public AudioLibraryAudioSourcePlayer(Transform perent, AudioMixerGroup audioMixerGroup, string objName = "")
        {
            GameObject = Engine.CreateObject();
            GameObject.name = $"ALASP {objName}";
            GameObject.transform.SetParent(perent);
            AudioSource = GameObject.AddComponent<AudioSource>();
            AudioSource.playOnAwake = false;
            AudioSource.outputAudioMixerGroup = audioMixerGroup;
        }
        public void Dispose()
        {
            //Debug.Log($"{GameObject.name} Dispose!");
            ObjectUtils.DestroyOrImmediate(GameObject);
        }
    }
}
