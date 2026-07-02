using System;
using Runtime.Satisfaction;
using Runtime.Scene_Handling;
using UnityEngine;

namespace Runtime.Customers
{
    public class CustomerManager : MonoBehaviour
    {
        [SerializeField] private SatisfactionEvents satisfactionEvents;
        [SerializeField] private Level startingLevelData;
        [SerializeField] private CustomerSlot[] customerSlots;
        [SerializeField] private GameObject customerPrefab;
        
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

        private void UnlockCustomerSlot(int slot)
        {
            
        }

        private void LockCustomerSlot(int slot)
        {
            
        }

        private void ToggleTarget(bool unlocked)
        {
            
        }
    }
}
