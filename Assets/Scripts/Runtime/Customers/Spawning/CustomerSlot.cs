using NaughtyAttributes;
using Runtime;
using Runtime.Customers;
using UnityEngine;

namespace Runtime.Customers.Spawning {
    public class CustomerSlot : MonoBehaviour
    {
        [HideInInspector] public CustomerManager customerManager;
        [HideInInspector] public CustomerEventPort customerEventPort;

        [SerializeField] private CustomerSlotIdentifier identifier;
        
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
        private bool _hasSubscribed = false;

        private void OnValidate() {
            if (identifier == null) {
                Debug.LogWarning($"Customer Slot {name} has no identifier", this);
            }
        }
        
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
            Setup();
        }

        private void OnDisable()
        {
            if (customerEventPort != null && _hasSubscribed) customerEventPort.onCustomerEventWithData -= EmptySlot;
            _hasSubscribed = false;
        }

        public void Setup() {
            if (customerEventPort != null && !_hasSubscribed) {
                customerEventPort.onCustomerEventWithData += EmptySlot;
                _hasSubscribed = true;
            }

        }

        private void Start()
        {
            EmptySlot();
        }

        private void EmptySlot() {
            _currentCustomer = null;
            _spawnTimer = timeBetweenCustomers;
        }

        private void EmptySlot(CustomerData data)
        {
            if (data != _currentCustomer) return;
            EmptySlot();
        }

        private void Update()
        {
            if (!_enabled) return;

            if (!_currentCustomer)
            {
                HandleNewCustomer();
            }
        }

        private void HandleNewCustomer()
        {
            _spawnTimer -= Time.deltaTime;

            if (_spawnTimer <= 0)
            {
                var newCustomer = customerManager.SpawnCustomer(
                    customerEventPort,
                    customerSpawnPosition,
                    customerOrderPosition,
                    customerExitPosition,
                    identifier
                );
                if (newCustomer != null) {
                    _currentCustomer = newCustomer;
                }
                _spawnTimer = timeBetweenCustomers;
            }
        }
    }
}
