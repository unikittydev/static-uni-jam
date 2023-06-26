using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class LevelButton : MonoBehaviour
    {
        [field: SerializeField] public Button button { get; private set; }
        [field: SerializeField] public TMP_Text text { get; private set; }
    }
}