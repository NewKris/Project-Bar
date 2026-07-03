using UnityEngine;
using UnityEngine.Events;

namespace Runtime.Customers
{
    [CreateAssetMenu(fileName = "Customer Event Port", menuName = "Event Ports/Customer Event Port")]
    public class CustomerEventPort : ScriptableObject
    {
        public UnityAction OnCustomerEvent;

        public void RaiseCustomerEvent()
        {
            OnCustomerEvent?.Invoke();
        }
    }
}