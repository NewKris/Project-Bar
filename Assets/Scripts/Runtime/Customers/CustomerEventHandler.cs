using UnityEngine;
using UnityEngine.Events;

namespace Runtime.Customers {
    [CreateAssetMenu(fileName = "Customer Event Handler", menuName = "Customers/Event Handler", order = 2)]
    public class CustomerEventHandler : ScriptableObject {
        public UnityAction<GameObject> onEnter;

        public void Enter(GameObject customer) {
            onEnter?.Invoke(customer);
        }
        
        public UnityAction<GameObject> onOrder;

        public void Order(GameObject customer) {
            onOrder?.Invoke(customer);
        }
        
        public UnityAction<GameObject> onRepeatOrder;

        public void RepeatOrder(GameObject customer) {
            onRepeatOrder?.Invoke(customer);
        }
        
        public UnityAction<GameObject> onServe;
		public void Serve(GameObject customer) {
			onServe?.Invoke(customer);	
		}

        public UnityAction<GameObject> onCorrectDrink;

        public void CorrectDrinkServed(GameObject customer) {
            onCorrectDrink?.Invoke(customer);
        }
        
        public UnityAction<GameObject> onWrongDrink;

        public void WrongDrinkServed(GameObject customer) {
            onWrongDrink?.Invoke(customer);
        }
        
        public UnityAction<GameObject> onPatienceTimeOut;

        public void TimeOutPatience(GameObject customer) {
            onPatienceTimeOut?.Invoke(customer);
        }

        public UnityAction<GameObject> onPoisoned;

        public void PoisonServed(GameObject customer) {
            onPoisoned?.Invoke(customer);
        }

        public UnityAction<GameObject> onKickedOut;

        public void KickOut(GameObject customer) {
            onKickedOut?.Invoke(customer);
        }

        public UnityAction<GameObject> onExit;

        public void Exit(GameObject customer) {
            onExit?.Invoke(customer);
        }
    }
}