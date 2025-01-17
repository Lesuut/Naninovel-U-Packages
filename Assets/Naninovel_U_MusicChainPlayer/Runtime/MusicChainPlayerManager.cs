using System.Linq;
using UnityEngine.Audio;

namespace Naninovel.U.MusicChainPlayer
{
    [InitializeAtRuntime()]
    public class MusicChainPlayerManager : IMusicChainPlayerManager
    {
        public virtual MusicChainPlayerConfiguration Configuration { get; }
        public AudioMixer AudioMixer { get; }

        private readonly IStateManager stateManager;
        private MusicChainPlayerState state;

        private MusicChainPlayerController audioLibraryController;

        public MusicChainPlayerManager(MusicChainPlayerConfiguration config, IStateManager stateManager)
        {
            Configuration = config;
            this.stateManager = stateManager;

            AudioMixer = Configuration.AudioMixer ? Configuration.AudioMixer : Engine.LoadInternalResource<AudioMixer>("DefaultMixer");
        }
        public UniTask InitializeServiceAsync()
        {
            state = new MusicChainPlayerState();
            stateManager.AddOnGameSerializeTask(Serialize);
            stateManager.AddOnGameDeserializeTask(Deserialize);

            audioLibraryController = Engine.CreateObject<MusicChainPlayerController>();
            audioLibraryController.Init(AudioMixer);

            return UniTask.CompletedTask;
        }

        public void DestroyService()
        {
            audioLibraryController.DestoryObj();
            stateManager.RemoveOnGameSerializeTask(Serialize);
            stateManager.RemoveOnGameDeserializeTask(Deserialize);
        }

        public void ResetService() 
        {
            audioLibraryController.ResetMusic();
        }

        private void Serialize(GameStateMap map) => map.SetState(new MusicChainPlayerState(state));

        private UniTask Deserialize(GameStateMap map)
        {
            state = map.GetState<MusicChainPlayerState>();
            state = state == null ? new MusicChainPlayerState() : new MusicChainPlayerState(state);

            PlayCBgm(state.Keys);

            return UniTask.CompletedTask;
        }

        public void PlayCBgm(string[] keys)
        {
            state.Keys = keys;

            audioLibraryController.PlayMusicChain(
                Configuration.MusicItems
                    .Where(item => keys.Contains(item.Key)) // Проверяем, содержит ли массив `keys` ключ `item.Key`
                    .Select(item => item.AudioClip) // Извлекаем значения
                    .ToArray() // Преобразуем в список
            );
        }

        public void StopCBgm()
        {
            audioLibraryController.StopMusicChain();
            state.Keys = new string[0];
        }

        /// <summary>
        /// Write the body for the MusicChainPlayer service here
        /// </summary>
    }
}
