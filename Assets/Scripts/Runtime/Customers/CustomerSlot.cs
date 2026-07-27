using NaughtyAttributes;
using UnityEngine;

namespace Runtime.Customers
{
    public class CustomerSlot : MonoBehaviour
    {
        [HideInInspector] public CustomerManager customerManager;

        [SerializeField] private CustomerEventPort customerEventPort;
        [SerializeField] private float timeBetweenCustomers = 5f;
        
        [Tooltip("Determines whether the slot will force active customer to leave when disabled")]
        [SerializeField] private bool kickOutCustomerWhenDisabled = true;
        
        [Foldout("Positions")]
        [Tooltip("The position where the customer will spawn at")]
        public Vector3 customerSpawnPosition;
        [Tooltip("The colors that will be used for the custom handles for the spawn position")]
        public CustomHandleColors spawnPositionHandleColors;
        [Foldout("Positions")]
        [Tooltip("The position the customer should stand at while ordering.")]
        public Vector3 customerOrderPosition;
        [Tooltip("The colors that will be used for the custom handles for the order position")]
        public CustomHandleColors orderPositionHandleColors;
        [Foldout("Positions")]
        [Tooltip("The position the customer should leave.")]
        public Vector3 customerExitPosition;
        [Tooltip("The colors that will be used for the custom handles for the exit position")]
        public CustomHandleColors exitPositionHandleColors;
        
        private Customer _currentCustomer;
        private bool _enabled = false;
        private float _spawnTimer;
        
        public void Enable()
        {
            _enabled = true;
            _spawnTimer = timeBetweenCustomers;
        }
        
        public void Disable()
        {
            _enabled = false;

            if (kickOutCustomerWhenDisabled && _currentCustomer)
            {
                _currentCustomer.KickOut();
            }
        }


        private void OnEnable()
        {
            customerEventPort.OnCustomerEvent += EmptySlot;
        }

        private void OnDisable()
        {
            customerEventPort.OnCustomerEvent -= EmptySlot;
        }

        private void Start()
        {
            EmptySlot();
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
                _spawnTimer = timeBetweenCustomers;
            }
        }
    }
}
