using System;
using FMODUnity;
using NaughtyAttributes;
using Runtime.Audio;
using Runtime.Satisfaction;
using UnityEngine;

namespace Runtime.Items {
    public class ItemPickup : MonoBehaviour {
        public SatisfactionPort satisfactionPort;
        public ItemSource source;
        
        [Header("Audio")]
        public EventReference pickUpAudio;
        public EventReference putDownAudio;
        public EventReference breakAudio;
        public string glassMaterialLabel;
        
        public event Action OnPinned;
        
        private FmodParameter[] _parameters;

        public void PlayPickupSound() {
            SfxManager.PlayOneShot(new OneShotConfig() {
                eventReference = pickUpAudio,
                attachedGameObject = gameObject,
                parameters = _parameters
            });
        }

        public void PlayPutDownSound() {
            SfxManager.PlayOneShot(new OneShotConfig() {
                eventReference = putDownAudio,
                attachedGameObject = gameObject,
                parameters = _parameters
            });
        }
        
        public void BreakItem() {
            SfxManager.PlayOneShot(new OneShotConfig() {
                eventReference = breakAudio, 
                position = transform.position,
                parameters = _parameters
            });
            
            satisfactionPort.DecreaseSatisfaction(satisfactionPort.dropPenalty);
            Despawn();
        }

        public void Despawn() {
            Destroy(gameObject);
        }

        public void SetFrontRender(bool renderInFront) {
            int layer = renderInFront ? LayerMask.NameToLayer("Held Item") : LayerMask.NameToLayer("Default");
            foreach (MeshRenderer renderer in GetComponentsInChildren<MeshRenderer>()) {
                renderer.gameObject.layer = layer;
            }
        }

        public void SetInteractable(bool interactable) {
            GetComponentInChildren<Collider>().enabled = interactable;
        }
        
        public void Pin(Transform pinPoint) {
            OnPinned?.Invoke();
            
            Rigidbody rb = GetComponent<Rigidbody>();
            
            transform.SetParent(pinPoint);
            transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            
            rb.isKinematic = true;
            rb.position = pinPoint.position;
            rb.rotation = pinPoint.rotation;
        }

        public void Unpin() {
            GetComponent<Rigidbody>().isKinematic = false;
            transform.SetParent(null);
        }

        private void Awake() {
            _parameters = new[] {
                new FmodParameter() { parameterName = "GlassMaterial", value = glassMaterialLabel },
            };
        }
    }
}
