using UnityEngine;
using UnityEngine.Video;

namespace Game
{
    [CreateAssetMenu(menuName = "Game/Level", fileName = "New Level")]
    public class LevelData : ScriptableObject
    {
        public new string name;
        public int buildIndex;
        //public VideoClip endVideo;
        public string endVideoPath;
    }
}