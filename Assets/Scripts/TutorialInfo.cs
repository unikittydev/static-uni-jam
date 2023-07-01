using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "Game/Level Tutorial", fileName = "New Tutorial")]
    public class TutorialInfo : ScriptableObject
    {
        public string[] labels;
    }
}