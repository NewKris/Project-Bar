using System;
using FMODUnity;
using NaughtyAttributes;
using Runtime.Animations;
using Runtime.Audio;
using Runtime.Drink;
using Runtime.Items;
using Runtime.UI;
using Runtime.Utility;
using Runtime.Utility.Attributes;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Stations {
    public abstract class Station : MonoBehaviour {
        [Foldout("References")] public ItemDock itemDock;
        public EventReference stationAudio;
        public Transform fillPosition;
        
        [BoolOptions("Station Type", "Toggle", "Hold")] 
        public bool isToggle;
        
        [Foldout("References")] public RumbleAnimation rumble;
        

        protected DrinkObject currentItem;
        protected int stationKey;
        protected string stationAudioKey;

        private FillCircle _fillCircle;
        
        public abstract void StartStation();
        public abstract void StopStation();

        public bool IsActive => enabled;

        protected abstract float MaxFill();
        
        protected void StartStationTimer() {
            if (itemDock.HeldItem?.TryGetComponent(out currentItem) ?? false) {
                enabled = true;
                itemDock.HeldItem.SetInteractable(false);

                if (!currentItem.HasStationTimer(stationKey)) {
                    currentItem.CreateStationTimer(stationKey);
                }
            }
        }

        private void Reset() {
            gameObject.layer = LayerMask.NameToLayer("Station");
        }

        private void Awake() {
            enabled = false;
            stationKey = gameObject.GetInstanceID();

            stationAudioKey = SfxManager.CreateUniqueKey(this, stationAudio);

            if (fillPosition) {
                WorldSpaceCanvas canvas = FindAnyObjectByType<WorldSpaceCanvas>();
                _fillCircle = canvas.CreateFillCircle(0, fillPosition.position, fillPosition.rotation);
                _fillCircle.gameObject.SetActive(false);
            }
        }
        
        private void OnEnable() {
            rumble.Shaking = true;
            _fillCircle?.gameObject.SetActive(true);
        }

        private void OnDisable() {
            rumble.Shaking = false;
            _fillCircle?.gameObject.SetActive(false);
        }
        
        protected virtual void Update() {
            currentItem.TickStationTimer(stationKey);
            
            if (_fillCircle) _fillCircle.Fill = currentItem.GetStationCompletion(stationKey, MaxFill());
        }

        private void OnDrawGizmos() {
            if (fillPosition) {
                HandlesProxy.DrawDisc(fillPosition.position, fillPosition.forward, 0.05f, true, Color.white, 3);
            }
        }
    }
}