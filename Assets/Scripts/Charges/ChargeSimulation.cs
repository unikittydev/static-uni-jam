using UnityEngine;

namespace Game
{
    public class ChargeSimulation : PuzzleBase
    {
        [SerializeField] private float forceFactor;

        [SerializeField] private Charge[] charges;

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
    }
}