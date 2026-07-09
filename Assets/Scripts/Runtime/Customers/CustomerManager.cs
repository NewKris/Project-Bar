using System;
using System.Collections.Generic;
using NaughtyAttributes;
using Runtime.Customers.Tutorial_Agent;
using Runtime.Satisfaction;
using Runtime.Scene_Handling;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Runtime.Customers
{
    public class CustomerManager : MonoBehaviour
    {
        [SerializeField] private SatisfactionEvents satisfactionEvents;
        
        [Foldout("Customers")]
        [SerializeField] private CustomerSlot[] customerSlots;
        [Foldout("Customers")]
        [SerializeField] private Customer customerPrefab; 
        
        private CustomerData[] _availableCustomers;
        
        private CustomerData _target;
        private float _targetBaseChance;
        private float _targetChanceIncrease;
        private bool _shouldTargetReset = true;

        private float _targetSpawnChance;
        private bool _targetUnlocked = false;
        private HashSet<CustomerData> _activeCustomers = new HashSet<CustomerData>();
        private HashSet<CustomerData> _servedCustomers = new HashSet<CustomerData>();
        private CustomerData _lastCustomer = null;

        private void OnEnable()
        {
            satisfactionEvents.OnToggleTarget += ToggleTarget;
            satisfactionEvents.OnCustomerSlotUnlocked += UnlockCustomerSlot;
            satisfactionEvents.OnCustomerSlotLocked += LockCustomerSlot;
            satisfactionEvents.OnUpdateCustomers += UpdateCustomers;
        }

        private void OnDisable()
        {
            satisfactionEvents.OnToggleTarget -= ToggleTarget;
            satisfactionEvents.OnCustomerSlotUnlocked -= UnlockCustomerSlot;
            satisfactionEvents.OnCustomerSlotLocked -= LockCustomerSlot;
            satisfactionEvents.OnUpdateCustomers -= UpdateCustomers;
        }

        private void Start()
        {
            foreach (CustomerSlot slot in customerSlots)
            {
                slot.customerManager = this;
            }
        }

        private void UpdateCustomers(CustomerData[] customers, TargetData targetData)
        {
            _availableCustomers = customers;
            _target = targetData.customerData;
            _targetBaseChance = targetData.baseChance;
            _targetChanceIncrease = targetData.chanceIncrease;
            _shouldTargetReset = targetData.shouldSpawnChanceReset;

        }

        public Customer SpawnCustomer(CustomerEventPort port, Vector3 spawnPosition, Vector3 barPosition, Vector3 exitPosition)
        {
            Customer newCustomer = Instantiate(customerPrefab, spawnPosition, Quaternion.identity);

            CustomerData data = null;

            if (_targetUnlocked && !_activeCustomers.Contains(_target) && _lastCustomer != _target)
            {
                if (Random.Range(0, 100) < _targetSpawnChance)
                {
                    data = _target;
                }
                else
                {
                    _targetSpawnChance += _targetChanceIncrease;
                }

            }
            
            if (!data)
            {
                if (_servedCustomers.Count < _availableCustomers.Length)
                {
                    data = _availableCustomers[_servedCustomers.Count];
                    _servedCustomers.Add(data);
                }
                else
                {
                    int maxAttempts = 10;
                    int attempts = 0;
                    
                    data = _availableCustomers[Random.Range(0, _availableCustomers.Length)];
                    
                    while ((_activeCustomers.Contains(data) || data == _lastCustomer) && attempts < maxAttempts)
                    {
                        data = _availableCustomers[Random.Range(0, _availableCustomers.Length)];
                        attempts++;
                    }
                }
                
            }

            _activeCustomers.Add(data);

            port.OnCustomerEvent += () =>
            {
                _activeCustomers.Remove(data);
                _lastCustomer = data;
            };
            
            newCustomer.CustomerSetup(data, port, barPosition, exitPosition);
            
            return newCustomer;
        }

        private void UnlockCustomerSlot(int slot)
        {
            customerSlots[slot].Enable();
        }

        private void LockCustomerSlot(int slot)
        {
            customerSlots[slot].Disable();
        }

        private void ToggleTarget(bool state)
        {
            _targetUnlocked = state;

            if (!_targetUnlocked && _shouldTargetReset)
            {
                _targetSpawnChance = _targetBaseChance;
            }
        }
    }
}
