using FMODUnity;
using Runtime.Audio;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Runtime.UI.Audio {
    public class ClickSound : MonoBehaviour, IPointerClickHandler {
        public EventReference clickSound;
        
        public void OnPointerClick(PointerEventData eventData) {
            SfxManager.PlayOneShot(clickSound);
        }
    }
}