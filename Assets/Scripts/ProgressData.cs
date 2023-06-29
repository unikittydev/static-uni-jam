using System.Collections.Generic;

namespace Game
{
    [System.Serializable]
    public class ProgressData
    {
        public List<LevelData> completedLevels = new();
    }
}