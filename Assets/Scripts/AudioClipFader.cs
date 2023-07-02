using System.Collections;
using UnityEngine;

namespace Game
{
    public class AudioClipFader : MonoBehaviour
    {
        [SerializeField] private AudioSource source;
        [SerializeField] private float fadeTime;
        
        public IEnumerator Fade(bool value)
        {
            float from = value ? 0f : 1f, to = 1f - from;
            float counter = 0f;

            source.volume = from;
            if (value)
                source.Play();
            
            while (counter < fadeTime)
            {
                float t = Mathf.Clamp01(counter / fadeTime);
                source.volume = Mathf.Lerp(from, to, t);
                
                counter += Time.unscaledDeltaTime;
                yield return null;
            }

            if (!value)
                source.Stop();
            source.volume = to;
        }
    }
}
