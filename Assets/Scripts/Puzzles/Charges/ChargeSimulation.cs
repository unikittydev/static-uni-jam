using System.Collections;
using UnityEngine;

namespace Game
{
    public class ChargeSimulation : PuzzleBase
    {
        [SerializeField] private float winCountdown;
        [SerializeField] private float forceFactor;

        [SerializeField] private Charge[] charges;
        [SerializeField] private ChargeTrigger[] triggers;
        
        public static ChargeSimulation Instance { get; private set; }

        private Coroutine countdownCheck;

        private void Awake()
        {
            Instance = this;
        }

        private void FixedUpdate()
        {
            for (int i = 0; i < charges.Length; i++)
            for (int j = i + 1; j < charges.Length; j++)
            {
                if (charges[i].state == ChargeState.Neutral || charges[j].state == ChargeState.Neutral)
                    continue;

                Vector2 delta = charges[i].rb.position - charges[j].rb.position;
                Vector2 direction = delta.normalized;

                float distanceSq = delta.magnitude;
                float forceSign = charges[i].state == charges[j].state ? 1f : -1f;
                float force = forceFactor * forceSign / distanceSq * Time.fixedDeltaTime;
                charges[i].rb.AddForce(direction * force, ForceMode2D.Force);
                charges[j].rb.AddForce(-direction * force, ForceMode2D.Force);
            }
        }
        
        public void CheckWin()
        {
            if (IsWinning() && countdownCheck == null)
                countdownCheck = StartCoroutine(Countdown());
            if (!enabled || IsWinning() || countdownCheck == null) return;
            VHSOverlay.Instance.Stop();
            StopCoroutine(countdownCheck);
            countdownCheck = null;
            CancelWin();
        }

        private bool IsWinning()
        {
            foreach (ChargeTrigger trigger in triggers)
                if (!trigger.activated)
                    return false;
            return true;
        }
        
        private IEnumerator Countdown()
        {
            float counter = winCountdown;

            VHSOverlay.Instance.ShowCountdown();
            while (counter > 0)
            {
                VHSOverlay.Instance.UpdateCountdown(counter);
                counter -= Time.deltaTime;
                yield return null;
            }
            VHSOverlay.Instance.ResetCountdown();
            TryWin();
        }
    }
}