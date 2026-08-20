using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using Runtime.Customers;
using Runtime.Satisfaction;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Runtime.Customers.Spawning {
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

        private bool[] _customerSlotsToUnlockOnTutorialFinished;
        private bool _isTutorialFinished;
        
        private CustomerData[] _availableCustomers;
        
        private CustomerData _target;
        private float _targetBaseChance;
        private float _targetChanceIncrease;
        private bool _shouldTargetReset = true;

        private float _targetSpawnChance;
        private bool _targetUnlocked;
        private HashSet<CustomerData> _activeCustomers = new();
        private HashSet<CustomerData> _servedCustomers = new();
        private CustomerData _lastCustomer = null;

        public float TargetSpawnChance => _targetSpawnChance;

        private void OnEnable()
        {
            _customerSlotsToUnlockOnTutorialFinished = new bool[customerSlots.Length];
            
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
            foreach (var slot in customerSlots)
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

        public Customer SpawnCustomer(CustomerEventPort port, Vector3 spawnPosition, Vector3 barPosition, Vector3 exitPosition, CustomerSlotIdentifier slotIdentifier)
        {
            CustomerData data = null;

            data = TrySpawnTarget(slotIdentifier);
            
            if (!data)
            {
                data = GetCustomerToSpawn(slotIdentifier);
            }
            
            if (!CanEnterSlot(data, slotIdentifier)) return null;

            Customer newCustomer = null;

            if (!_activeCustomers.Contains(data)) {
                _activeCustomers.Add(data);
                newCustomer = Instantiate(customerPrefab, spawnPosition, Quaternion.identity);
                newCustomer.CustomerSetup(data, port, barPosition, exitPosition);
                newCustomer.gameObject.name = data.name;
            }
            
            return newCustomer;
        }

        private CustomerData TrySpawnTarget(CustomerSlotIdentifier slotIdentifier) {
            if (!CanEnterSlot(_target, slotIdentifier)) return null;
            
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

            return data;
        }

        private CustomerData GetCustomerToSpawn(CustomerSlotIdentifier slotIdentifier) {
            CustomerData data = null;
            
            if (_servedCustomers.Count < _availableCustomers.Length)
            {
                data = GetFirstAvailableCustomer(slotIdentifier);
                _servedCustomers.Add(data);
            }
                
            if (!data) 
            {
                List<CustomerData> spawnableCustomers = GetSpawnableCustomers(slotIdentifier);
                
                data = spawnableCustomers[Random.Range(0, spawnableCustomers.Count)]; 
            }

            return data;
        }

        private List<CustomerData> GetSpawnableCustomers(CustomerSlotIdentifier slotIdentifier) {
            List<CustomerData> spawnableCustomers = new();

            foreach (CustomerData customer in _availableCustomers) {
                if (CanCustomerBeSpawned(customer, slotIdentifier)) spawnableCustomers.Add(customer);
            }

            return spawnableCustomers;
        }

        private bool CanCustomerBeSpawned(CustomerData data, CustomerSlotIdentifier slotIdentifier) {
            if (!CanEnterSlot(data, slotIdentifier)) return false;
            if (_activeCustomers.Contains(data)) return false;
            return true;
        }

        private CustomerData GetFirstAvailableCustomer(CustomerSlotIdentifier slotIdentifier) {
            foreach (CustomerData customer in _availableCustomers) {
                if (!_servedCustomers.Contains(customer) && CanEnterSlot(customer, slotIdentifier)) return customer;
            }
            return null;
        }

        private bool CanEnterSlot(CustomerData customer, CustomerSlotIdentifier slotIdentifier) {
            if (customer.canEnterAnySlot) return true;
            if (customer.allowedSlots.Contains(slotIdentifier)) return true;
            return false;
        }

        private void HandleCustomerLeaving(CustomerData data) {
            _activeCustomers.Remove(data);
            _lastCustomer = data;
        }

        private void UnlockCustomerSlot(int slot)
        {
            if (waitForTutorial && !_isTutorialFinished) {
                _customerSlotsToUnlockOnTutorialFinished[slot] = true;
            }
            else {
                customerSlots[slot].Enable();
            }
        }

        private void LockCustomerSlot(int slot)
        {
            if (waitForTutorial && !_isTutorialFinished) {
                _customerSlotsToUnlockOnTutorialFinished[slot] = false;
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
            for (var i = 0; i < _customerSlotsToUnlockOnTutorialFinished.Length; i++) {
                if (_customerSlotsToUnlockOnTutorialFinished[i]) {
                    UnlockCustomerSlot(i);
                }
            }
        }
    }
}
