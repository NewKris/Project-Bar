using Runtime.Drink;
using Runtime.Interact;
using UnityEngine;
using UnityEngine.Events;

namespace Runtime.Customers {
    [RequireComponent(typeof(CustomerMovement))]
    [RequireComponent((typeof(Interactable)))]
    public class CustomerBase : MonoBehaviour {
        public UnityAction onOrder;
        public UnityAction onRepeatOrder;
        public UnityAction<DrinkContents> onServeDrink;
        public UnityAction onEnterBar;
        
        [Tooltip("The mesh renderer used for the customers model")]
        public MeshFilter customerMeshFilter;
        
        private CustomerMovement _customerMovement;
        
        private bool _hasOrdered;
        private bool _isLeaving;

        public void Setup(Mesh mesh, Vector3 barPosition, Vector3 exitPosition, CustomerEventPort port) {
            if (mesh) customerMeshFilter.mesh = mesh;
            _customerMovement.Setup(barPosition, exitPosition, port);
        }

        private void OnEnable()
        {
            _customerMovement = GetComponent<CustomerMovement>();
        }


        public void EnterBar() {
            onEnterBar?.Invoke();
            _customerMovement.EnterBar();
        }

        public void LeaveBar()
        {
            if (_isLeaving) return;
            _customerMovement.ExitBar();
            _isLeaving = true;
        }

        public void ServeDrink(DrinkContents drink) {
            Debug.Log("Serving 💅");
            
            if (_isLeaving) return;
            
            onServeDrink?.Invoke(drink);
        }

        public void Order()
        {
            if (_isLeaving) return;
            if (!_hasOrdered) {
                onOrder?.Invoke();
                _hasOrdered = true;
            }
            else {
                onRepeatOrder?.Invoke();
            }
        }
    }
}