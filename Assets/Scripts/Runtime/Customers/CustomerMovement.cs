using System.Collections;
using Runtime.Utility;
using UnityEngine;

namespace Runtime.Customers
{
    public class CustomerMovement : MonoBehaviour
    {
        private Vector3 _barPosition;
        private Vector3 _exitPosition;
        private CustomerEventPort _customerEventPort;
        
        [Tooltip("The time it takes for the customer to walk")]
        [SerializeField] [Min(0)] private float movementTime;

        [Tooltip("The time the customer will remain at the bar before leaving")]
        [SerializeField] [Min(0)] private float timeBeforeExit = 1.5f;

        public void Setup(Vector3 barPosition, Vector3 exitPosition, CustomerEventPort port)
        {
            _barPosition = barPosition;
            _exitPosition = exitPosition;
            _customerEventPort = port;
        }
        
        public void EnterBar()
        {
            StartCoroutine(WalkToBar());
        }

        public void ExitBar()
        {
            StartCoroutine(WalkToExit());
        }
        
        private IEnumerator WalkToBar()
        {
            float elapsedTime = 0;
            Vector3 startPosition = transform.position;

            while (elapsedTime < movementTime)
            {
                elapsedTime += Time.fixedDeltaTime;
                transform.position = Vector3.Lerp(startPosition, _barPosition, elapsedTime/movementTime);
                
                yield return new WaitForFixedUpdate();
            }
        }
        
        private IEnumerator WalkToExit()
        {
            float elapsedTime = 0;
            Vector3 startPosition = transform.position;
            
            yield return new WaitForSeconds(timeBeforeExit);
            
            while (elapsedTime < movementTime)
            {
                elapsedTime += Time.fixedDeltaTime;
                transform.position = Vector3.Lerp(startPosition, _exitPosition, elapsedTime/movementTime);
                
                yield return new WaitForFixedUpdate();
            }
            
            _customerEventPort.RaiseCustomerEvent();
            Destroy(gameObject);
        }
        
        private void OnDrawGizmos() {
            HandlesProxy.DrawDisc(transform.position, Vector3.up, 0.25f, false, Color.white);
            HandlesProxy.DrawDisc(_barPosition, Vector3.up, 0.25f, false, Color.white);
            HandlesProxy.DrawDisc(_exitPosition, Vector3.up, 0.25f, false, Color.white);
            HandlesProxy.DrawLine(transform.position, _barPosition, 1, true, Color.red);
            HandlesProxy.DrawLine(_barPosition, _exitPosition, 1, true, Color.red);
        }
    }
}