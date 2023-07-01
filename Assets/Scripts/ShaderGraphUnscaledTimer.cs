using System;
using UnityEngine;

namespace Game
{
    public class ShaderGraphUnscaledTimer : MonoBehaviour
    {
        private const string UNSCALED_TIME = "_UnscaledTime";

        private static readonly int UNSCALED_TIME_ID = Shader.PropertyToID(UNSCALED_TIME);
        
        [SerializeField] private Material tapes;

        private void OnDestroy()
        {
            tapes.SetFloat(UNSCALED_TIME_ID, 0f);
        }

        private void Update()
        {
            tapes.SetFloat(UNSCALED_TIME_ID, Time.unscaledTime);
        }
    }
}
