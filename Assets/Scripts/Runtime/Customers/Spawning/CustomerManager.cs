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
        private CustomerData _lastCustomer;

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
            generalCustomerEventPort.onCustomerEventWithData -= HandleCustomerLeaving;
        }

        private void Start() {
            _activeCustomers = new HashSet<CustomerData>();
            _servedCustomers = new HashSet<CustomerData>();
            
            // Setup of customer slots
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

        /// <summary>
        /// Tries to spawn a customer at the specified CustomerSlot.
        /// </summary>
        /// <param name="spawnPosition"> The spawn position specified in the CustomerSlot. </param>
        /// <param name="barPosition"> The bar position specified in the CustomerSlot. </param>
        /// <param name="exitPosition"> The exit position specified in the CustomerSlot. </param>
        /// <param name="slotIdentifier"> The CustomerSlotIdentifier used in the CustomerSlot used in order to check if a customer can go there. </param>
        /// <returns> An instantiated Customer if a customer can be spawned otherwise null. </returns>
        public Customer SpawnCustomer(Vector3 spawnPosition, Vector3 barPosition, Vector3 exitPosition, CustomerSlotIdentifier slotIdentifier)
        {
            CustomerData data = TrySpawnTarget(slotIdentifier);
            
            if (!data)
            {
                data = GetCustomerToSpawn(slotIdentifier);
            }

            if (!data) return null;
            if (!CanEnterSlot(data, slotIdentifier)) return null;

            Customer newCustomer = null;
            
            _activeCustomers.Add(data);
            newCustomer = Instantiate(customerPrefab, spawnPosition, Quaternion.identity);
            newCustomer.CustomerSetup(data, generalCustomerEventPort, barPosition, exitPosition);
            newCustomer.gameObject.name = data.name;
            
            return newCustomer;
        }

        /// <summary>
        /// Checks if the target can and/or should be spawned at the specified slot.
        /// </summary>
        /// <param name="slotIdentifier">The specified slot</param>
        /// <returns>The CustomerData for the target if the target should be spawned otherwise null.</returns>
        private CustomerData TrySpawnTarget(CustomerSlotIdentifier slotIdentifier) {
            if (!_targetUnlocked) return null;
            if (!CanCustomerBeSpawned(_target, slotIdentifier)) return null;
            
            if (Random.Range(0, 100) < _targetSpawnChance)
            {
                return _target;
            }

            _targetSpawnChance += _targetChanceIncrease;
            return null;
        }

        /// <summary>
        /// <c>GetCustomerToSpawn</c> gets the next customer to spawn at the specified slot. It is to be used when trying to spawn a generic customer and not the target.
        /// </summary>
        /// <param name="slotIdentifier">The specified slot</param>
        /// <returns>A CustomerData if a customer that can be spawned is found otherwise null.</returns>
        private CustomerData GetCustomerToSpawn(CustomerSlotIdentifier slotIdentifier) {
            // Phase 1: Customer is spawned from a predetermined order
            if (_servedCustomers.Count < _availableCustomers.Length)
            {
                CustomerData data = GetFirstAvailableCustomer(slotIdentifier);
                _servedCustomers.Add(data);
                if (data) return data;
            }
            
            // Phase 2: The customer to spawn is decided randomly out of the spawnable customers
            List<CustomerData> spawnableCustomers = GetSpawnableCustomers(slotIdentifier);
            if (spawnableCustomers.Count > 0) return spawnableCustomers[Random.Range(0, spawnableCustomers.Count)]; 

            return null;
        }

        /// <summary>
        /// <c>GetSpawnableCustomers</c> is used to get all currently available customers that can be spawned at the specified slot
        /// </summary>
        /// <param name="slotIdentifier"> The slot where the customer will be spawned </param>
        /// <returns></returns>
        private List<CustomerData> GetSpawnableCustomers(CustomerSlotIdentifier slotIdentifier) {
            List<CustomerData> spawnableCustomers = new();

            foreach (CustomerData customer in _availableCustomers) {
                if (CanCustomerBeSpawned(customer, slotIdentifier)) spawnableCustomers.Add(customer);
            }

            return spawnableCustomers;
        }

        /// <summary>
        /// <c>CanCustomerBeSpawned</c> is used to check if a specified customer can be spawned at the specified slot and that the customer is not active.
        /// </summary>
        /// <param name="data"> The customer data to be checked </param>
        /// <param name="slotIdentifier"> The customer slot to check </param>
        /// <returns> A bool that determines whether the customer can be spawned</returns>
        private bool CanCustomerBeSpawned(CustomerData data, CustomerSlotIdentifier slotIdentifier) {
            if (!CanEnterSlot(data, slotIdentifier)) return false;
            if (_activeCustomers.Contains(data)) return false;
            if (data == _lastCustomer) return false;
            return true;
        }

        /// <summary>
        /// <c>GetFirstAvailableCustomer</c> finds the first customer that can be spawned at the specified CustomerSlot.
        /// </summary>
        /// <param name="slotIdentifier">The customer slot where the customer should be spawned. </param>
        /// <returns> The CustomerData if one can be spawned, otherwise null</returns>
        private CustomerData GetFirstAvailableCustomer(CustomerSlotIdentifier slotIdentifier) {
            foreach (CustomerData customer in _availableCustomers) {
                if (!_servedCustomers.Contains(customer) && CanCustomerBeSpawned(customer, slotIdentifier)) return customer;
            }
            return null;
        }

        /// <summary>
        /// <c>CanEnterSlot</c> checks whether a specified customer can enter the specified slot.
        /// </summary>
        /// <param name="customer">The specified customer</param>
        /// <param name="slotIdentifier">The specified slot</param>
        /// <returns>True if customer can spawn at the slot</returns>
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
