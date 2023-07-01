using System;
using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    public class SMPTEAFK : MonoBehaviour
    {
        [SerializeField] private float timeToShow;
        
        
        [SerializeField] private UnityEvent onAFKStart;
        [SerializeField] private UnityEvent onAFKEnd;

        private void Update()
        {
            
        }
    }
}
