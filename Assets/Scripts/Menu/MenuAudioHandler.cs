using UnityEngine;
using UnityEngine.EventSystems;

namespace Game
{
    public class MenuAudioHandler : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
    {
        [SerializeField] private AudioSource hoverSound;
        [SerializeField] private AudioSource clickSound;
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            hoverSound.Play();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            clickSound.Play();
        }
    }
}