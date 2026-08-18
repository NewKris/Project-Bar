using Runtime.Drinks;
using Runtime.Old_Systems.Drink;
using Runtime.Old_Systems.Interact;
using UnityEngine;
using UnityEngine.Events;

namespace Runtime.Customers {
    [RequireComponent(typeof(CustomerMovement))]
    [RequireComponent((typeof(Interactable)))]
    public class CustomerBase : MonoBehaviour {
        public UnityAction onOrder;
        public UnityAction<DrinkContents> onServeDrink;
        public UnityAction onEnterBar;

        public CustomerEventHandler customerEventHandler;
        
        [Tooltip("The mesh renderer used for the customers model")]
        public MeshFilter customerMeshFilter;
        
        private CustomerMovement _customerMovement;
        
        public bool isLeaving;

        public void NoMeshSetup(Vector3 barPosition, Vector3 exitPosition, CustomerEventPort port, CustomerData data = null) {
            _customerMovement.Setup(barPosition, exitPosition, port, data);
            
        }

        public void Setup(Mesh mesh, Vector3 barPosition, Vector3 exitPosition, CustomerEventPort port, CustomerData data = null) {
            _customerMovement ??= GetComponent<CustomerMovement>();
            if (mesh) customerMeshFilter.mesh = mesh;
            _customerMovement.Setup(barPosition, exitPosition, port, data);
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
            if (isLeaving) return;
            _customerMovement.ExitBar();
            isLeaving = true;
        }

        public void ServeDrink(DrinkContents drink) {
            Debug.Log("Serving 💅");
            
            if (isLeaving) return;
            
            onServeDrink?.Invoke(drink);
        }

        public void Order()
        {
            if (isLeaving) return;
            onOrder?.Invoke();
        }
    }
}