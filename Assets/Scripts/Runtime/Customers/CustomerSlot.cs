using UnityEngine;

namespace Runtime.Customers
{
    public class CustomerSlot : MonoBehaviour
    {
        [SerializeField] private float timeBetweenCustomers = 5f;
        
        
        private bool _enabled = false;

        public void Enable() => _enabled = true;
        
        public void Disable() => _enabled = false;
    }
}
