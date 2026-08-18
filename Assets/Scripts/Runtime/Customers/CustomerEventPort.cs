using UnityEngine;
using UnityEngine.Events;

namespace Runtime.Customers
{
    [CreateAssetMenu(fileName = "Customer Event Port", menuName = "Event Ports/Customer Event Port")]
    public class CustomerEventPort : ScriptableObject
    {
        public UnityAction onCustomerEvent;
        public UnityAction<CustomerData> onCustomerEventWithData;
        
        public void RaiseCustomerEvent()
        {
            onCustomerEvent?.Invoke();
        }

        public void RaiseCustomerEvent(CustomerData customerData) {
            onCustomerEvent?.Invoke();
            onCustomerEventWithData?.Invoke(customerData);
        }
    }
}