using System;
using UnityEditor;

namespace Naninovel.U.LibraryAudio
{
    public class AudioLibraryMetadataEditor : OrthoMetadataEditor<IAudioLibraryActor, AudioLibraryMetadata>
    {
        protected override Action<SerializedProperty> GetCustomDrawer(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(CharacterMetadata.Poses): return DrawWhen(false);
                case nameof(CharacterMetadata.Implementation): return DrawWhen(false);

                case nameof(AudioLibraryMetadata.audioLibraryItems): return DrawWhen(true);
            }
            return base.GetCustomDrawer(propertyName);
        }
    }
}
