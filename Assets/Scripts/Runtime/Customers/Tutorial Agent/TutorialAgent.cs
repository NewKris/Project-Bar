using System;
using Runtime.Interact;
using UnityEngine;

namespace Runtime.Customers.Tutorial_Agent {
    [RequireComponent(typeof(CustomerBase))]
    public class TutorialAgent : MonoBehaviour {
        [SerializeField] private TutorialAgentStep[] tutorialSteps;
        
        private CustomerBase _base;

        private void OnEnable() {
            _base = GetComponent<CustomerBase>();
        }
    }
}