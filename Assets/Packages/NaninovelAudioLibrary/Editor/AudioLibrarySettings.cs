using System.Collections.Generic;
using System;
using UnityEditor;

namespace Naninovel.U.LibraryAudio
{
    public class AudioLibrarySettings : OrthoActorManagerSettings<AudioLibraryConfiguration, IAudioLibraryActor, AudioLibraryMetadata>
    {
        protected override MetadataEditor<IAudioLibraryActor, AudioLibraryMetadata> MetadataEditor { get; } = new AudioLibraryMetadataEditor();

        private static bool editMainRequested;

        public override void OnGUI(string searchContext)
        {
            if (editMainRequested)
            {
                editMainRequested = false;
            }

            base.OnGUI(searchContext);
        }

        protected override Dictionary<string, Action<SerializedProperty>> OverrideConfigurationDrawers()
        {
            var drawers = base.OverrideConfigurationDrawers();
            drawers[nameof(CharactersConfiguration.SharedPoses)] = ActorPosesEditor.Draw;
            return drawers;
        }

        [MenuItem("Naninovel/Resources/AudioLibrary")]
        private static void OpenResourcesWindow()
        {
            // Automatically open main background editor when opened via resources context menu.
            editMainRequested = true;
            OpenResourcesWindowImpl();
        }
    }
}
