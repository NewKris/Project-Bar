using System;
using NaughtyAttributes;
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
        [Foldout("Customers")]
        [SerializeField] private CustomerData[] availableCustomers;
        
        [Foldout("Target")]
        [SerializeField] private CustomerData target;
        [Foldout("Target")]
        [Tooltip("The base percentage of target spawning when available")]
        [SerializeField] [Range(0f, 100f)] private float targetBaseChance;
        [Foldout("Target")]
        [Tooltip("The percentage units that the target spawn chance will increase by when other customers are spawned while the target is available")]
        [SerializeField] [Range(0f, 100f)] private float targetChanceIncrease;
        [Foldout("Target")]
        [Tooltip("If true, the target spawn chance will reset when target becomes unavailable")]
        [SerializeField] private bool shouldTargetReset = true;

        private float _targetSpawnChance;
        private bool _targetUnlocked = false;

        private void OnEnable()
        {
            satisfactionEvents.OnToggleTarget += ToggleTarget;
            satisfactionEvents.OnCustomerSlotUnlocked += UnlockCustomerSlot;
            satisfactionEvents.OnCustomerSlotLocked += LockCustomerSlot;
        }

        private void OnDisable()
        {
            satisfactionEvents.OnToggleTarget -= ToggleTarget;
            satisfactionEvents.OnCustomerSlotUnlocked -= UnlockCustomerSlot;
            satisfactionEvents.OnCustomerSlotLocked -= LockCustomerSlot;
        }

        private void Start()
        {
            foreach (CustomerSlot slot in customerSlots)
            {
                slot.customerManager = this;
            }
        }

        public Customer SpawnCustomer(CustomerEventPort port, Vector3 spawnPosition, Vector3 barPosition, Vector3 exitPosition)
        {
            Customer newCustomer = Instantiate(customerPrefab, spawnPosition, Quaternion.identity);

            CustomerData data = null;

            if (_targetUnlocked)
            {
                if (Random.Range(0, 100) < _targetSpawnChance)
                {
                    data = target;
                }
            }
            if (!data) data = availableCustomers[Random.Range(0, customerSlots.Length)];
            
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

        private void ToggleTarget(bool unlocked)
        {
            _targetUnlocked = unlocked;
        }
    }
}
