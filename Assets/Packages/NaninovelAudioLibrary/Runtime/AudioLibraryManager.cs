using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Audio;

namespace Naninovel.U.LibraryAudio
{
    [InitializeAtRuntime]
    public class AudioLibraryManager : IEngineService<AudioLibraryConfiguration>, IStatefulService<GameStateMap>
    {
        [Serializable]
        public class GameState 
        {
            public AudioLibraryCurrentUseAudioItem currentBmgItem;
            public List<AudioLibraryCurrentUseAudioItem> currentSfxItems;
        }

        public AudioLibraryConfiguration Configuration { get; }
        public AudioMixer AudioMixer { get; }

        private IAudioManager audioManager;
        private AudioLibraryController controller;

        private GameState gameState;

        public AudioLibraryManager(AudioLibraryConfiguration configAudioLib, AudioConfiguration configAudio)
        {
            Configuration = configAudioLib;

            gameState = new GameState()
            {
                currentBmgItem = new AudioLibraryCurrentUseAudioItem(),
                currentSfxItems = new List<AudioLibraryCurrentUseAudioItem>(),
            };

            AudioMixer = configAudio.CustomAudioMixer ? configAudio.CustomAudioMixer : Engine.LoadInternalResource<AudioMixer>("DefaultMixer");           
        }
        public UniTask InitializeServiceAsync()
        {
            audioManager = Engine.GetService<IAudioManager>();

            controller = Engine.CreateObject<AudioLibraryController>();
            controller.Init(AudioMixer);

            return UniTask.CompletedTask;
        }
        public void DestroyService()
        {
            if (controller is IDisposable disposable)
                disposable.Dispose();
        }
        public void ResetService()
        {
            gameState.currentBmgItem.Key = "";
            gameState.currentSfxItems.Clear();

            controller.ResetController();
        }

        /*private AudioLibraryItem GetAudioItem(string key, string group)
        {
            *//*var targetGroup = string.IsNullOrEmpty(group)
                ? Configuration.AudioGroups.SelectMany(g => g.AudioItems)
                : Configuration.AudioGroups.Where(g => g.GroupName == group).SelectMany(g => g.AudioItems);*//*

            //return targetGroup.FirstOrDefault(i => i.Key == key);

            var metadatas = Configuration.Metadata.GetAllMetas();

            foreach (var item in metadatas)
            {
                item.audioLibraryItems
            }

            return ;
        }*/
        private AudioLibraryItem GetAudioItem(string key, string group)
        {
            // Получаем все метаданные
            var metadatas = Configuration.Metadata.GetAllMetas();

            // Фильтруем элементы по группе (если она указана) и ключу
            var targetItems = string.IsNullOrEmpty(group)
                ? metadatas.SelectMany(meta => meta.audioLibraryItems)
                : metadatas
                    .Where(meta => meta.GetResourceCategoryId() == group) // Убедимся, что у Metadata есть GroupName
                    .SelectMany(meta => meta.audioLibraryItems);

            // Ищем первый элемент с совпадающим ключом
            return targetItems.FirstOrDefault(item => item.Key == key);
        }


        private void LogAndForwardToAudioManager(string key, string group, string actionName = "")
        {
            //UnityEngine.Debug.LogWarning($"AudioLibraryManager {actionName}: Couldn't find the sound {key}/{(group == "" ? "Null" : group)}. Forwarding to AudioManager");
        }

        public void PlayBmg(string key, string group)
        {
            var item = GetAudioItem(key, group);

            if (item.Clip != null)
            {
                gameState.currentBmgItem.Key = key;
                gameState.currentBmgItem.Group = group;
                controller.PlayBmg(item.Clip);
            }
            else
            {
                audioManager.PlayBgmAsync(key);
                LogAndForwardToAudioManager(key, group, "PlayBmg");
            }
        }

        public void StopBmg(string key)
        {
            var item = GetAudioItem(gameState.currentBmgItem.Key, gameState.currentBmgItem.Group);

            if (item.Clip != null)
            {
                gameState.currentBmgItem.Key = "";
                gameState.currentBmgItem.Group = "";
                controller.StopBmg();
            }
            else
            {
                audioManager.StopBgmAsync(key);
                LogAndForwardToAudioManager(gameState.currentBmgItem.Key, gameState.currentBmgItem.Group, "StopBmg");
            }
        }

        public void PlaySfx(string key, string group, bool loop, float valume)
        {
            var item = GetAudioItem(key, group);

            if (item.Clip != null)
            {
                AudioLibraryCurrentUseAudioItem newSfxItem = new AudioLibraryCurrentUseAudioItem
                {
                    Key = key,
                    Group = group,
                    Loop = loop,
                    Valume = valume
                };

                gameState.currentSfxItems.Add(newSfxItem);

                if (loop || valume != AudioLibraryConfiguration.defSfxVolume)
                {
                    controller.PlaySfx(item.Clip, key, group, loop, valume);
                }
                else
                {
                    controller.PlaySfx(item.Clip);
                }
            }
            else
            {
                LogAndForwardToAudioManager(key, group, "PlaySfx");
                audioManager.PlaySfxAsync(key, valume, 0, loop);
            }
        }
        public void StopSfx(string key, string group)
        {
            var item = GetAudioItem(key, group);

            if (item.Clip != null)
            {
                controller.StopSfx(key, group);
                gameState.currentSfxItems.RemoveAll(i => i.Key == key && i.Group == group);
            }
            else
            {
                LogAndForwardToAudioManager(key, group, "StopSfx");
                audioManager.StopSfxAsync(key);
            }
        }

        public void SaveServiceState(GameStateMap state)
        {
            state.SetState(gameState);
        }
        public UniTask LoadServiceStateAsync(GameStateMap state)
        {
            var data = state.GetState<GameState>() ?? new GameState();

            controller.ResetController();

            if (data.currentBmgItem.Key != "")
            {
                PlayBmg(data.currentBmgItem.Key, data.currentBmgItem.Group);
            }

            foreach (var item in data.currentSfxItems)
            {
                if (item.Key != "")
                {
                    PlaySfx(item.Key, item.Group, item.Loop, item.Valume);
                }
            }

            return UniTask.CompletedTask;
        }
    }
}