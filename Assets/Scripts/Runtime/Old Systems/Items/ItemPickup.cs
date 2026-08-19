using System;
using FMODUnity;
using NaughtyAttributes;
using Runtime.Audio;
using Runtime.Satisfaction;
using UnityEngine;

namespace Runtime.Old_Systems.Items {
    [Obsolete]
    public class ItemPickup : MonoBehaviour {
        [Required] public SatisfactionPort satisfactionPort;
        [HideInInspector] public ItemSource source;

        [Header("Respawning")] 
        public bool canRespawn;
        public ItemDock startDock;
        
        [Header("Audio")]
        public EventReference pickUpAudio;
        public EventReference putDownAudio;
        public EventReference breakAudio;
        public ContainerMaterialType glassMaterialLabel;
        
        public event Action OnPinned;

        private FmodParameter _containerMaterial;
        private Transform _pin;

        public void PlayPickupSound() {
            SfxManager.PlayOneShot(new OneShotConfig() {
                eventReference = pickUpAudio,
                attachedGameObject = gameObject,
                parameters = new [] {
                    _containerMaterial,
                    
                }
            });
        }

        public void PlayPutDownSound(string surfaceLabel) {
            SfxManager.PlayOneShot(new OneShotConfig() {
                eventReference = putDownAudio,
                attachedGameObject = gameObject,
                parameters = new [] {
                    _containerMaterial,
                    new FmodParameter() { parameterName = "SurfaceMaterial",  value = surfaceLabel },
                }
            });
        }
        
        public void BreakItem() {
            SfxManager.PlayOneShot(new OneShotConfig() {
                eventReference = breakAudio, 
                position = transform.position,
                parameters = new [] {
                    _containerMaterial
                }
            });
            
            satisfactionPort.DecreaseSatisfaction(satisfactionPort.dropPenalty);
            Despawn();
        }

        public void Despawn() {
            if (canRespawn) {
                startDock.PlaceItem(this);
            }
            else {
                Destroy(gameObject);
            }
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
            
            _pin = pinPoint;
            SnapToPin();
            
            rb.isKinematic = true;
            rb.position = pinPoint.position;
            rb.rotation = pinPoint.rotation;
        }

        public void Unpin() {
            GetComponent<Rigidbody>().isKinematic = false;
            transform.SetParent(null);
            _pin = null;
        }

        private void Awake() {
            _containerMaterial = new FmodParameter() { parameterName = "GlassMaterial", value = glassMaterialLabel.ToString() };
        }

        private void Start() {
            if (startDock) {
                startDock.PlaceItem(this);
            }
        }

        private void Update() {
            if (_pin) SnapToPin();
        }

        private void SnapToPin() {
            transform.position = _pin.position;
            transform.rotation = _pin.rotation;
        }

        private void OnDrawGizmos() {
            if (startDock != null) {
                Gizmos.color = Color.cyan;
                Gizmos.DrawSphere(startDock.itemPivot.position, 0.01f);
                Gizmos.DrawLine(transform.position, startDock.itemPivot.position);
            }
        }
    }
}
