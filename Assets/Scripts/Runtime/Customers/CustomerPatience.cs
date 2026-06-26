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
        
        
        [Foldout("Timers")]
        [Tooltip("The time it takes before the customer leaves")]
        [SerializeField] private float patienceTimer;
        
        [Foldout("Timers")]
        [Tooltip("The time it takes before the customer asks the player to hurry up")]
        [SerializeField] private float patienceTickTime;

        
        private float _patienceTimer;
        private bool _patienceTickDialogueHasTriggered;
        private bool _isLeaving;

        private void Start()
        {
            _patienceTimer = patienceTimer;
        }

        private void Update()
        {
            _patienceTimer -= Time.deltaTime;

            patienceImage.fillAmount = Mathf.Max(_patienceTimer, 0) / patienceTimer;

            if (_patienceTimer <= patienceTimer - patienceTickTime && !_patienceTickDialogueHasTriggered)
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