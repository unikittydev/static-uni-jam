using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace Game
{
    public class MenuStartup : MonoBehaviour
    {
        [SerializeField] private EventSystem eventSystem;
        //[Header("Volume tint")] private Volume volume;
        [Header("Logo")]
        [SerializeField] private Graphic logo;
        [SerializeField] private float logoDelay = 0f, logoFadeInSpeed = 1f, logoStayTime = 5f, logoFadeOutSpeed = 2f;
        [Header("Graphics")]
        [SerializeField] private Graphic[] graphics;
        [SerializeField] private float graphicsDelay = 7f, graphicsFadeInSpeed = 1f;

        private void Start()
        {
            eventSystem.enabled = false;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            StartCoroutine(FadeLogo());
            StartCoroutine(FadeGraphics());
        }

        private IEnumerator FadeLogo()
        {
            logo.gameObject.SetActive(false);
            yield return new WaitForSecondsRealtime(logoDelay);
            logo.gameObject.SetActive(true);

            Color color = logo.color;
            color.a = 0f;
            logo.color = color;

            float counter = 0f;
            while (counter <= logoFadeInSpeed)
            {
                color.a = Mathf.Clamp01(counter / logoFadeInSpeed);
                logo.color = color;
                
                counter += Time.unscaledDeltaTime;
                yield return null;
            }

            color.a = 1f;
            logo.color = color;

            yield return new WaitForSecondsRealtime(logoStayTime);
            
            counter = 0f;
            while (counter <= logoFadeOutSpeed)
            {
                color.a = 1f - Mathf.Clamp01(counter / logoFadeOutSpeed);
                logo.color = color;
                
                counter += Time.unscaledDeltaTime;
                yield return null;
            }

            color.a = 0f;
            logo.color = color;
            logo.gameObject.SetActive(false);
        }

        private IEnumerator FadeGraphics()
        {
            foreach (Graphic g in graphics)
            {
                Color color = g.color;
                color.a = 0f;
                g.color = color;
            }
            
            yield return new WaitForSecondsRealtime(graphicsDelay);

            float counter = 0f;
            while (counter <= graphicsFadeInSpeed)
            {
                float t = Mathf.Clamp01(counter / graphicsFadeInSpeed);

                foreach (Graphic g in graphics)
                {
                    Color color = g.color;
                    color.a = t;
                    g.color = color;
                }

                counter += Time.unscaledDeltaTime;
                yield return null;
            }

            eventSystem.enabled = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}