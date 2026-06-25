using System.Collections;
using UnityEngine;

namespace Runtime.Customers
{
    public class CustomerMovement : MonoBehaviour
    {
        public Vector3 barPosition;
        public Vector3 exitPosition;
        
        [Tooltip("The time it takes for the customer to walk")]
        [SerializeField] [Min(0)] private float movementTime;

        [Tooltip("The time the customer will remain at the bar before leaving")]
        [SerializeField] [Min(0)] private float timeBeforeExit = 1.5f;

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
                transform.position = Vector3.Lerp(startPosition, barPosition, elapsedTime/movementTime);
                
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
                transform.position = Vector3.Lerp(startPosition, exitPosition, elapsedTime/movementTime);
                
                yield return new WaitForFixedUpdate();
            }
            
            Destroy(gameObject);
        }
    }
}