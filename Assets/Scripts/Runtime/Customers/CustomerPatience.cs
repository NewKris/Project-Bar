using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Runtime.Customers
{
    public class CustomerPatience : MonoBehaviour
    {
        public UnityAction OnPatienceTick;
        public UnityAction OnPatienceTimeOut;
        
        [SerializeField] private Image patienceImage;
        
        // _patience is the full length of the patience timer
        private float _patience;
        // _patienceTimer is the timer that counts down
        private float _patienceTimer;
        private float _patienceTickTime;
        
        private bool _patienceTickDialogueHasTriggered;
        private bool _isLeaving;

        private void Start()
        {
            _patienceTimer = _patience;
        }

        public void Setup(float timer, float tickTime)
        {
            _patience = timer;
            _patienceTickTime = tickTime;

            _patienceTimer = _patience;
        }

        private void Update()
        {
            _patienceTimer -= Time.deltaTime;

            patienceImage.fillAmount = Mathf.Max(_patienceTimer, 0) / _patience;

            if (_patienceTimer <= _patience - _patienceTickTime && !_patienceTickDialogueHasTriggered)
            {
                _patienceTickDialogueHasTriggered = true;
                OnPatienceTick?.Invoke();
            }

            if (_patienceTimer <= 0 && !_isLeaving)
            {
                _isLeaving = true;
                OnPatienceTimeOut.Invoke();
            }
        }

        public void AddTime(float value)
        {
            _patienceTimer += value;
        }
        
    }
}