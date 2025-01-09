using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Naninovel.U.LibraryAudio
{
    [System.Serializable]
    public class AudioLibraryMetadata : OrthoActorMetadata
    {
        [System.Serializable]
        public class Map : ActorMetadataMap<AudioLibraryMetadata> { }
        [System.Serializable]
        public class Pose : ActorPose<AudioLibraryState> { }

        public List<Pose> Poses = new List<Pose>();
        
        public List<AudioLibraryItem> audioLibraryItems;

        public AudioLibraryMetadata()
        {
            Implementation = typeof(SpriteBackground).AssemblyQualifiedName;
            Loader = new ResourceLoaderConfiguration { PathPrefix = BackgroundsConfiguration.DefaultPathPrefix };
            Pivot = new Vector2(.5f, .5f);
        }

        public override ActorPose<TState> GetPose<TState>(string poseName) => Poses.FirstOrDefault(p => p.Name == poseName) as ActorPose<TState>;
    }
}
