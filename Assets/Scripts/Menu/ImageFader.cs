using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class ImageFader : MonoBehaviour
    {
        [SerializeField] private float fadeTime;
        [SerializeField] private Image image;

        public IEnumerator SetFade(bool value)
        {
            float from = value ? 0f : 1f;
            float to = 1f - from;

            Color color = image.color;
            color.a = from;
            image.color = color;
            
            if (value)
                gameObject.SetActive(true);
            
            float counter = 0f;
            while (counter / fadeTime <= 1f)
            {
                color.a = Mathf.Lerp(from, to, Mathf.Clamp01(counter / fadeTime));
                image.color = color;
                counter += Time.unscaledDeltaTime;
                yield return null;
            }
            color.a = to;
            image.color = color;
            
            if (!value)
                gameObject.SetActive(false);
        }
    }
}