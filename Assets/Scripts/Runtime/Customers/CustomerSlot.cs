using NaughtyAttributes;
using UnityEngine;

namespace Runtime.Customers
{
    public class CustomerSlot : MonoBehaviour
    {
        [HideInInspector] public CustomerManager customerManager;

        [SerializeField] private CustomerEventPort customerEventPort;
        [SerializeField] private float timeBetweenCustomers = 5f;
        
        [Foldout("Positions")]
        [Tooltip("The position where the customer will spawn at")]
        public Vector3 customerSpawnPosition;
        [Foldout("Positions")]
        [Tooltip("The position the customer should stand at while ordering.")]
        public Vector3 customerOrderPosition;
        [Foldout("Positions")]
        [Tooltip("The position the customer should leave.")]
        public Vector3 customerExitPosition;
        
        private Customer _currentCustomer;
        private bool _enabled = true;
        private float _spawnTimer;
        
        public void Enable()
        {
            _enabled = true;
            _spawnTimer = timeBetweenCustomers;
        }
        
        public void Disable()
        {
            _enabled = false;
        }


        private void OnEnable()
        {
            customerEventPort.OnCustomerEvent += EmptySlot;
        }

        private void OnDisable()
        {
            
        }

        private void Start()
        {
            _spawnTimer = timeBetweenCustomers;
            _currentCustomer = null;
        }

        private void EmptySlot()
        {
            _currentCustomer = null;
            _spawnTimer = timeBetweenCustomers;
        }

        private void Update()
        {
            if (!_enabled) return;

            if (_currentCustomer)
            {
                
            }
            else
            {
                HandleNewCustomer();
            }
        }

        private void HandleNewCustomer()
        {
            _spawnTimer -= Time.deltaTime;

            if (_spawnTimer <= 0)
            {
                _currentCustomer = customerManager.SpawnCustomer(
                    customerEventPort,
                    customerSpawnPosition, 
                    customerOrderPosition,
                    customerExitPosition
                );
            }
        }
    }
}
