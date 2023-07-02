using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class NoiseFader : MonoBehaviour
    {
        [SerializeField] private float fadeTime;
        [SerializeField] private Image image;
        [SerializeField] private AudioSource noiseSource;
        [SerializeField] private float maxNoiseVolume;

        public IEnumerator SetFade(bool value)
        {
            float from = value ? 0f : 1f;
            float to = 1f - from;

            Color color = image.color;
            color.a = from;
            image.color = color;
            noiseSource.volume = from * maxNoiseVolume;

            if (value)
            {
                image.gameObject.SetActive(true);
                noiseSource.Play();
                noiseSource.time = Random.Range(0f, noiseSource.clip.length);
            }

            float counter = 0f;
            while (counter / fadeTime <= 1f)
            {
                color.a = Mathf.Lerp(from, to, Mathf.Clamp01(counter / fadeTime));
                image.color = color;
                noiseSource.volume = color.a * maxNoiseVolume;
                counter += Time.unscaledDeltaTime;
                yield return null;
            }
            color.a = to;
            image.color = color;
            noiseSource.volume = to * maxNoiseVolume;

            if (!value)
            {
                noiseSource.Stop();
                image.gameObject.SetActive(false);
            }
        }
    }
}