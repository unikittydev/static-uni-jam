using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    public class SMPTEAFK : MonoBehaviour
    {
        [SerializeField] private float afkTime;
        private float afkCounter;
        
        [SerializeField] private UnityEvent onAFKStart;
        [SerializeField] private UnityEvent onAFKEnd;

        private bool afk;
        private Vector3 oldMousePosition;
        
        
        private void Update()
        {
            if (afk)
            {
                if (oldMousePosition == Input.mousePosition) return;
                afk = false;
                onAFKEnd?.Invoke();
                return;
            }
            
            afkCounter += Time.unscaledDeltaTime;

            if (afkCounter > afkTime)
            {
                afk = true;
                afkCounter = 0f;
                onAFKStart?.Invoke();
            }

            oldMousePosition = Input.mousePosition;
        }
    }
}
