using FMODUnity;
using Runtime.Audio;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Runtime.UI.Audio {
    public class HoverSound : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
        public EventReference beginHoverSound;
        public EventReference endHoverSound;
        
        public void OnPointerEnter(PointerEventData eventData) {
            SfxManager.PlayOneShot(beginHoverSound);
        }

        public void OnPointerExit(PointerEventData eventData) {
            SfxManager.PlayOneShot(endHoverSound);
        }
    }
}