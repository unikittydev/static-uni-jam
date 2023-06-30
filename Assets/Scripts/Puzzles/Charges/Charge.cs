using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

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

        [Space]
        [SerializeField] private UnityEvent onBecameCharged;
        [SerializeField] private UnityEvent onBecameNeutral;
        
        private void Start()
        {
            UpdateState();
        }

        [ContextMenu("UpdateState")]
        private void UpdateState()
        {
            if (!positive || !neutral || !negative)
                return;
            
            positive.SetActive(state == ChargeState.Positive);
            neutral.SetActive(state == ChargeState.Neutral);
            negative.SetActive(state == ChargeState.Negative);
            locked.SetActive(!interactable);
        }

        private void OnMouseOver()
        {
            bool setPositive = Input.GetMouseButtonDown(0);
            bool setNegative = Input.GetMouseButtonDown(1);

            if (!setPositive && !setNegative)
                return;
            
            if (!interactable || EventSystem.current.IsPointerOverGameObject())
                return;

            if (Application.isMobilePlatform)
                _state = (ChargeState)(((int)state + 1) % 3);
            else
            {
                if (setPositive)
                {
                    _state = _state == ChargeState.Neutral ? ChargeState.Positive : ChargeState.Neutral;
                }
                if (setNegative)
                {
                    _state = _state == ChargeState.Neutral ? ChargeState.Negative : ChargeState.Neutral;
                }
            }
            
            if (_state == ChargeState.Neutral)
                onBecameNeutral?.Invoke();
            else
                onBecameCharged?.Invoke();
            
            UpdateState();
        }
    }
}
