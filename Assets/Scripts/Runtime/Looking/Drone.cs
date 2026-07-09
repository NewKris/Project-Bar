using System;
using Runtime.Utility.CommonObjects;
using Runtime.Utility.Timers;
using UnityEngine;

namespace Runtime.Looking {
    public class Drone : MonoBehaviour {
        public float moveSpeed;
        public LookController lookController;
        public Vector3 restPosition;
        public Vector3 offset;
        public float restTime = 0.5f;

        private DampedVector _position;
        private Timer _restTimer;

        private void Awake() {
            _position = new DampedVector(restPosition);
            _restTimer = TimerManager.CreateTimer();
        }

        private void OnDestroy() {
            TimerManager.RemoveTimer(_restTimer);
        }

        private void Update() {
            bool hasTarget = lookController.Current;
            
            if (hasTarget) _restTimer.SetTimer(restTime);
            
            _position.Target = hasTarget 
                ? lookController.Current.transform.position + offset :
                _restTimer.Elapsed ? restPosition : transform.position;
            
            transform.position = _position.Tick(moveSpeed);
        }
    }
}