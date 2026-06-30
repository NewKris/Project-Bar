using System;
using Runtime;
using Runtime.Scene_Handling;
using UnityEngine;
using UnityEngine.Events;

namespace Runtime.Satisfaction
{
    public class SatisfactionManager : MonoBehaviour
    {
        public UnityAction<int> OnCustomerSlotUnlocked;
        public UnityAction<int> OnCustomerSlotLocked;
        public UnityAction<bool> OnToggleTarget;
        public UnityAction OnGameOver;
        
        
        public Level currentLevel;
        public SatisfactionPort satisfactionPort;

        public int currentSatisfaction;
        
        private bool _targetUnlocked = false;
        private bool[] _availableCustomers;

        private void OnEnable()
        {
            satisfactionPort.OnSatisfactionChange += HandleSatisfactionChange;
            satisfactionPort.OnSatisfactionSet += SetSatisfaction;
        }
        
        private void OnDisable()
        {
            satisfactionPort.OnSatisfactionChange -= HandleSatisfactionChange;
            satisfactionPort.OnSatisfactionSet -= SetSatisfaction;
        }

        private void Start()
        {
            Restart();
        }

        private void HandleSatisfactionChange(int value)
        {
            currentSatisfaction = Mathf.Min(currentSatisfaction + value, currentLevel.maximumSatisfaction);

            HandleTargetUnlock();

            HandleCustomerUnlocks();
            
            if (currentSatisfaction <= 0)
            {
                OnGameOver?.Invoke();
            }
        }

        private void HandleTargetUnlock()
        {
            if (currentSatisfaction >= currentLevel.targetSatisfaction && !_targetUnlocked)
            {
                _targetUnlocked = true;
                OnToggleTarget?.Invoke(_targetUnlocked);
            }
            else if (currentSatisfaction < currentLevel.targetSatisfaction && _targetUnlocked)
            {
                _targetUnlocked = false;
                OnToggleTarget?.Invoke(_targetUnlocked);
            }
        }

        private void HandleCustomerUnlocks()
        {
            for (int i = 0; i < currentLevel.customerUnlocks.Length; i++)
            {
                HandleCustomerUnlock(i);
            }
        }

        private void HandleCustomerUnlock(int index)
        {
            int unlock = currentLevel.customerUnlocks[index];
                
            if (currentSatisfaction >= unlock && !_availableCustomers[index])
            {
                _availableCustomers[index] = true;
                OnCustomerSlotUnlocked?.Invoke(index);
            }

            if (currentSatisfaction < unlock && _availableCustomers[index])
            {
                _availableCustomers[index] = false;
                OnCustomerSlotLocked?.Invoke(index);
            }
        }

        private void SetSatisfaction(int value)
        {
            currentSatisfaction = value;
        }

        public void SetLevel(Level level)
        {
            currentLevel = level;
            Restart();
        }

        public void Restart()
        {
            currentSatisfaction = currentLevel.startSatisfaction;
            _targetUnlocked = false;
            _availableCustomers = new bool[currentLevel.customerUnlocks.Length];
        }
    }
}
