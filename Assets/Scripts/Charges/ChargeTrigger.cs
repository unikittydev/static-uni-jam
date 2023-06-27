using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class ChargeTrigger : MonoBehaviour
    {
        [SerializeField] private GameObject activeModel, inactiveModel;
        
        private List<Charge> chargesInTrigger = new();

        public bool activated => chargesInTrigger.Count > 0;
        
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.TryGetComponent(out Charge charge))
                chargesInTrigger.Add(charge);
            SetTriggerActive(true);
            
            ChargeSimulation.Instance.CheckWin();
        }

        private void OnTriggerExit2D(Collider2D col)
        {
            if (col.TryGetComponent(out Charge charge))
                chargesInTrigger.Remove(charge);
            if (chargesInTrigger.Count != 0) return;
            
            ChargeSimulation.Instance.CheckWin();
            SetTriggerActive(false);
        }

        private void SetTriggerActive(bool active)
        {
            activeModel.SetActive(active);
            inactiveModel.SetActive(!active);
        }
    }
}
