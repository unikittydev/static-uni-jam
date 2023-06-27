using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "New World", menuName = "Game/Level World")]
    public class LevelWorld : ScriptableObject
    {
        public string worldName;
        public int[] requirements;
        public int[] levelIndices;

        public bool IsUnlocked(List<int> completedLevels)
        {
            foreach (int level in requirements)
                if (!completedLevels.Contains(level))
                    return false;

            return true;
        }
    }
}