using System;
using NaughtyAttributes;
using Runtime.Utility.CommonObjects;
using UnityEngine;

namespace Runtime.Player {
    public class FirstPersonCamera : MonoBehaviour {
        public float sensitivity = 1;
        public float mouseSmoothing = 0;
        public float crouchSmoothing = 0;
        
        public float crouchDeadZone = 0.1f;
        
        [MinMaxSlider(-90, 90)] 
        public Vector2 pitchAngleLimits = new Vector2(-90, 90);

        [MinMaxSlider(0, 2)] 
        public Vector2 cameraHeights = new Vector2(0, 2);
        
        public Vector2 axisScaling = Vector2.one;

        private DampedValue _cameraHeight;
        private DampedAngle _pitch;
        private DampedAngle _yaw;
        private Transform _pitchPivot;
        private Transform _yawPivot;

        private void Start() {
            CreateHierarchy();
            ResetDampedAngles();
            SetCursorLock(true);
        }

        public void ChangeCameraHeight(float amount) {
            if (Mathf.Abs(amount) < crouchDeadZone) return;

            _cameraHeight.Target = amount < 0 ? cameraHeights.x : cameraHeights.y;
        }

        public void Look(Vector2 deltaMouse, float dt) {
            Vector2 lookVel = Vector2.Scale(deltaMouse, axisScaling) * sensitivity;
            
            _yaw.Target += lookVel.x;
            _yaw.Target %= 360f;
            
            _pitch.Target -= lookVel.y;
            _pitch.Target = Mathf.Clamp(_pitch.Target, pitchAngleLimits.x, pitchAngleLimits.y);

            _yawPivot.localRotation = Quaternion.Euler(0, _yaw.Tick(mouseSmoothing, dt), 0);
            _pitchPivot.localRotation = Quaternion.Euler(_pitch.Tick(mouseSmoothing, dt), 0, 0);
        }

        private void Update() {
            _yawPivot.localPosition = Vector3.up * _cameraHeight.Tick(crouchSmoothing);
        }

        private void ResetDampedAngles() {
            _cameraHeight = new DampedValue(cameraHeights.y);
            _yaw = new DampedAngle(_yawPivot.localRotation.eulerAngles.y);
            _pitch = new DampedAngle(_pitchPivot.localRotation.eulerAngles.x);
        }
        
        private void CreateHierarchy() {
            _yawPivot = new GameObject("Yaw").transform;
            _yawPivot.SetParent(transform.parent);
            _yawPivot.SetPositionAndRotation(transform.position, transform.rotation);
            
            _pitchPivot = new GameObject("Pitch").transform;
            _pitchPivot.SetParent(_yawPivot);
            _pitchPivot.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            
            transform.SetParent(_pitchPivot);
            transform.SetLocalPositionAndRotation(Vector2.zero, Quaternion.identity);
        }

        private void SetCursorLock(bool lockCursor) {
            Cursor.lockState = lockCursor ? CursorLockMode.Locked : CursorLockMode.None;
        }
    }
}