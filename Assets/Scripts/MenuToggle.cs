using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    public class MenuToggle : MonoBehaviour
    {
        private const string onState = "ВКЛ";
        private const string offState = "ВЫКЛ";

        [SerializeField] private UnityEvent<bool> onValueChanged;
        [SerializeField] private bool state;

        [SerializeField] private TMP_Text text;

        private void Awake()
        {
            UpdateText();
        }

        public void SetValue(bool value)
        {
            state = value;
            UpdateText();
            onValueChanged?.Invoke(state);
        }
        
        public void Toggle()
        {
            state = !state;
            UpdateText();
            onValueChanged?.Invoke(state);
        }

        private void UpdateText()
        {
            text.text = state ? onState : offState;
        }
    }
}