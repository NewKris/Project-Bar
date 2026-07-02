using System;
using Runtime;
using Runtime.Scene_Handling;
using UnityEngine;
using UnityEngine.Events;

namespace Runtime.Satisfaction
{
    public class SatisfactionManager : MonoBehaviour
    {
        public Level currentLevel;
        public SatisfactionPort satisfactionPort;

        public int currentSatisfaction;
        
        [SerializeField] private SatisfactionEvents satisfactionEvents;
        
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
                satisfactionEvents.GameOver();
            }
        }

        private void HandleTargetUnlock()
        {
            if (currentSatisfaction >= currentLevel.targetSatisfaction && !_targetUnlocked)
            {
                _targetUnlocked = true;
                satisfactionEvents.ToggleTarget(_targetUnlocked);
            }
            else if (currentSatisfaction < currentLevel.targetSatisfaction && _targetUnlocked)
            {
                _targetUnlocked = false;
                satisfactionEvents.ToggleTarget(_targetUnlocked);
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
                satisfactionEvents.ChangeCustomerSlotState(index, false);
            }

            if (currentSatisfaction < unlock && _availableCustomers[index])
            {
                _availableCustomers[index] = false;
                satisfactionEvents.ChangeCustomerSlotState(index, true);
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
