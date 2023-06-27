using UnityEngine;

namespace Game
{
    public class LevelWorld : ScriptableObject
    {
        public string worldName;
        public int[] requirements;
        public int[] levelIndices;
    }
}