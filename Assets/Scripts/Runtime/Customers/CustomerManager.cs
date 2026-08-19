using System;
using System.Collections.Generic;
using NaughtyAttributes;
using Runtime.Satisfaction;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Runtime.Customers
{
    public class CustomerManager : MonoBehaviour
    {
        [SerializeField] private SatisfactionEvents satisfactionEvents;
        [SerializeField] private CustomerEventPort tutorialFinishedPort;
        [SerializeField] private CustomerEventPort generalCustomerEventPort;
        
        [Tooltip("If true then customer slots will not be unlocked until tutorial is finished.")]
        [SerializeField] private bool waitForTutorial = true;
        
        [Foldout("Customers")]
        [SerializeField] private CustomerSlot[] customerSlots;
        [Foldout("Customers")]
        [SerializeField] private Customer customerPrefab;

        private bool[] _customersToUnlockOnTutorialFinished;
        private bool _isTutorialFinished;
        
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

        public float TargetSpawnChance => _targetSpawnChance;

        private void OnEnable()
        {
            _customersToUnlockOnTutorialFinished = new bool[customerSlots.Length];
            
            satisfactionEvents.OnToggleTarget += ToggleTarget;
            satisfactionEvents.OnCustomerSlotUnlocked += UnlockCustomerSlot;
            satisfactionEvents.OnCustomerSlotLocked += LockCustomerSlot;
            satisfactionEvents.OnUpdateCustomers += UpdateCustomers;
            
            tutorialFinishedPort.onCustomerEvent += OnTutorialFinished;
            generalCustomerEventPort.onCustomerEventWithData += HandleCustomerLeaving;
        }

        private void OnDisable()
        {
            satisfactionEvents.OnToggleTarget -= ToggleTarget;
            satisfactionEvents.OnCustomerSlotUnlocked -= UnlockCustomerSlot;
            satisfactionEvents.OnCustomerSlotLocked -= LockCustomerSlot;
            satisfactionEvents.OnUpdateCustomers -= UpdateCustomers;
            
            tutorialFinishedPort.onCustomerEvent -= OnTutorialFinished;
        }

        private void Start() {
            _activeCustomers = new HashSet<CustomerData>();
            _servedCustomers = new HashSet<CustomerData>();
            foreach (CustomerSlot slot in customerSlots)
            {
                slot.customerManager = this;
                slot.customerEventPort = generalCustomerEventPort;
                slot.Setup();
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
            CustomerData data = null;
            
            CustomerData[] customers = new CustomerData[_activeCustomers.Count];

            _activeCustomers.CopyTo(customers);

            if (customers.Length > 0) {
                foreach (CustomerData customer in customers) {
                    if (!customer) continue;
                    Debug.Log(customer.customerName);
                }
            }
            

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

            Customer newCustomer = null;

            if (!_activeCustomers.Contains(data)) {
                _activeCustomers.Add(data);
                newCustomer = Instantiate(customerPrefab, spawnPosition, Quaternion.identity);
                newCustomer.CustomerSetup(data, port, barPosition, exitPosition);
                newCustomer.gameObject.name = data.name;
            }
            
            return newCustomer;
        }

        private void HandleCustomerLeaving(CustomerData data) {
            _activeCustomers.Remove(data);
            _lastCustomer = data;
        }

        private void UnlockCustomerSlot(int slot)
        {
            if (waitForTutorial && !_isTutorialFinished) {
                _customersToUnlockOnTutorialFinished[slot] = true;
            }
            else {
                customerSlots[slot].Enable();
            }
        }

        private void LockCustomerSlot(int slot)
        {
            if (waitForTutorial && !_isTutorialFinished) {
                _customersToUnlockOnTutorialFinished[slot] = false;
            }
            else {
                customerSlots[slot].Disable();
            }
            
        }

        private void ToggleTarget(bool state)
        {
            _targetUnlocked = state;

            if (!_targetUnlocked && _shouldTargetReset)
            {
                _targetSpawnChance = _targetBaseChance;
            }
        }

        private void OnTutorialFinished() {
            _isTutorialFinished = true;
            for (int i = 0; i < _customersToUnlockOnTutorialFinished.Length; i++) {
                if (_customersToUnlockOnTutorialFinished[i]) {
                    UnlockCustomerSlot(i);
                }
            }
        }
    }
}
