using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Naninovel.U.LibraryAudio
{
    [EditInProjectSettings]
    public class AudioLibraryConfiguration : OrthoActorManagerConfiguration<AudioLibraryMetadata>
    {
        public const float defSfxVolume = 0.8f;

        //public List<AudioLibraryGroupItem> AudioGroups = new List<AudioLibraryGroupItem>();

        public override AudioLibraryMetadata DefaultActorMetadata => DefaultMetadata;
        public override ActorMetadataMap<AudioLibraryMetadata> ActorMetadataMap => Metadata;

        [Tooltip("Metadata to use by default when creating background actors and custom metadata for the created actor ID doesn't exist.")]
        public AudioLibraryMetadata DefaultMetadata = new AudioLibraryMetadata();
        [Tooltip("Metadata to use when creating background actors with specific IDs.")]
        public AudioLibraryMetadata.Map Metadata = new AudioLibraryMetadata.Map();

        public List<AudioLibraryMetadata.Pose> SharedPoses = new List<AudioLibraryMetadata.Pose>();

        protected override ActorPose<TState> GetSharedPose<TState>(string poseName) => SharedPoses.FirstOrDefault(p => p.Name == poseName) as ActorPose<TState>;
    }
}