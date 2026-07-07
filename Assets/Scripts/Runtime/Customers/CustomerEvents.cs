using UnityEngine;
using UnityEngine.Events;

namespace Runtime.Customers {
    [CreateAssetMenu(fileName = "Customer Events", menuName = "Event Ports/Customer Events")]
    public class CustomerEvents : ScriptableObject {
        public UnityAction<bool> OnCustomerDied;

        public void RaiseCustomerDiedEvent(bool isTarget) {
            OnCustomerDied?.Invoke(isTarget);
        }
    }
}