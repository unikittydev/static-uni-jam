using System;
using UnityEngine;

namespace Game
{
    public enum ChargeState
    {
        Positive,
        Neutral,
        Negative,
    }
    
    public class Charge : MonoBehaviour
    {
        [SerializeField] private ChargeState _state;
        public ChargeState state => _state;
        [SerializeField] private bool interactable = true;

        [SerializeField] private Rigidbody2D _rb;
        public Rigidbody2D rb => _rb;
        [Header("Models")]
        [SerializeField] private GameObject positive;
        [SerializeField] private GameObject neutral;
        [SerializeField] private GameObject negative;
        [SerializeField] private GameObject locked;

        private void Start()
        {
            UpdateState();
        }

        private void UpdateState()
        {
            if (!positive || !neutral || !negative)
                return;
            
            positive.SetActive(state == ChargeState.Positive);
            neutral.SetActive(state == ChargeState.Neutral);
            negative.SetActive(state == ChargeState.Negative);
            locked.SetActive(!interactable);
        }

        private void OnMouseDown()
        {
            if (!interactable)
                return; 
            _state = (ChargeState)(((int)state + 1) % 3);
            UpdateState();
        }
    }
}
