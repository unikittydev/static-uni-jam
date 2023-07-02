using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "New World", menuName = "Game/Level World")]
    public class LevelWorld : ScriptableObject
    {
        public string worldName;
        public LevelData[] requirements;
        public LevelData[] levelIndices;
        public TutorialInfo tutorial;
        public Color plateTint;
        
        public bool IsUnlocked(List<LevelData> completedLevels)
        {
            foreach (LevelData level in requirements)
                if (!completedLevels.Contains(level))
                    return false;

            return true;
        }
    }
}