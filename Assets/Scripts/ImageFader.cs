using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class ImageFader : MonoBehaviour
    {
        [SerializeField] private RawImage image;
        [SerializeField] private float fadeTime;
        
        public IEnumerator Fade(bool value)
        {
            float from = value ? 0f : 1f, to = 1f - from;
            float counter = 0f;
            
            Color color = image.color;
            color.a = from;
            image.color = color;
            
            if (value)
                gameObject.SetActive(true);

            while (counter < fadeTime)
            {
                float t = Mathf.Clamp01(counter / fadeTime);
                color.a = Mathf.Lerp(from, to, t);
                image.color = color;
                
                counter += Time.unscaledDeltaTime;
                yield return null;
            }

            if (!value)
                gameObject.SetActive(false);

            color.a = 1f;
            image.color = color;
        }

        public void SetImageAlpha(float value)
        {
            Color color = image.color;
            color.a = value;
            image.color = color;
        }
    }
}
