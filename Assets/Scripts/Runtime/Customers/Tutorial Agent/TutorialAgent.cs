using System;
using Runtime.Interact;
using UnityEngine;

namespace Runtime.Customers.Tutorial_Agent {
    [RequireComponent(typeof(CustomerMovement))]
    [RequireComponent(typeof(Interactable))]
    public class TutorialAgent : MonoBehaviour
    {
        
        private CustomerMovement _customerMovement;

        private void Start()
        {
            _customerMovement = GetComponent<CustomerMovement>();
        }
    }
}