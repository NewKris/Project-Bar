using UnityEngine;
using UnityEngine.Events;

namespace Runtime.Customers {
    public class CustomerActionEvents : MonoBehaviour {
        public CustomerEventHandler customerEventHandler;
        
        public UnityEvent onEnter;
        public UnityEvent onOrder;
        public UnityEvent onRepeatOrder;
        public UnityEvent onServe;
        public UnityEvent onCorrectDrink;
        public UnityEvent onWrongDrink;
        public UnityEvent onPoisoned;
        public UnityEvent onPatienceTimeOut;
        public UnityEvent onKickedOut;
        public UnityEvent onExit;


        private void OnEnable() {
            customerEventHandler.onEnter += Enter;
            customerEventHandler.onOrder += Order;
            customerEventHandler.onRepeatOrder += RepeatOrder;
            customerEventHandler.onServe += Serve;
            customerEventHandler.onCorrectDrink += CorrectDrinkServed;
            customerEventHandler.onWrongDrink += WrongDrinkServed;
            customerEventHandler.onPoisoned += PoisonServed;
            customerEventHandler.onPatienceTimeOut += PatienceTimedOut;
            customerEventHandler.onKickedOut += KickOut;
            customerEventHandler.onExit += Exit;
        }


        private void OnDisable() {
            customerEventHandler.onEnter -= Enter;
            customerEventHandler.onOrder -= Order;
            customerEventHandler.onRepeatOrder -= RepeatOrder;
            customerEventHandler.onServe -= Serve;
            customerEventHandler.onCorrectDrink -= CorrectDrinkServed;
            customerEventHandler.onWrongDrink -= WrongDrinkServed;
            customerEventHandler.onPoisoned -= PoisonServed;
            customerEventHandler.onPatienceTimeOut -= PatienceTimedOut;
            customerEventHandler.onKickedOut -= KickOut;
            customerEventHandler.onExit -= Exit;
        }

        private void Enter(GameObject customer) {
            if (customer == gameObject) {
                Debug.Log("Enter event called");
                onEnter?.Invoke();
            }
        }
        
        private void Order(GameObject customer) {
            if (customer == gameObject) {
                Debug.Log("Order event called");
                onOrder?.Invoke();
            }
        }
        
        private void RepeatOrder(GameObject customer) {
            if (customer == gameObject) {
                Debug.Log("Repeat order event called");
                onRepeatOrder?.Invoke();
            }
        }
        
        private void Serve(GameObject customer) {
            if (customer == gameObject) {
                Debug.Log("Serve event called");
                onServe?.Invoke();
            }
        }
        
        private void CorrectDrinkServed(GameObject customer) {
            if (customer == gameObject) {
                Debug.Log("Correct drink served event called");
                onCorrectDrink?.Invoke();
            }
        }
        
        private void WrongDrinkServed(GameObject customer) {
            if (customer == gameObject) {
                Debug.Log("Wrong drink served event called");
                onWrongDrink?.Invoke();
            }
        }

        private void PoisonServed(GameObject customer) {
            if (customer == gameObject) {
                Debug.Log("Poisoned event called");
                onPoisoned?.Invoke();
            }
        }
        
        private void PatienceTimedOut(GameObject customer) {
            if (customer == gameObject) {
                Debug.Log("Patience timed out event called");
                onPatienceTimeOut?.Invoke();
            }
        }
        
        private void KickOut(GameObject customer) {
            if (customer == gameObject) {
                Debug.Log("Kicked out event called");
                onKickedOut?.Invoke();
            }
        }
        
        private void Exit(GameObject customer) {
            if (customer == gameObject) {
                Debug.Log("Exit event called");
                onExit?.Invoke();
            }
        }
    }
}