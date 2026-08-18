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
                onEnter?.Invoke();
            }
        }
        
        private void Order(GameObject customer) {
            if (customer == gameObject) {
                onOrder?.Invoke();
            }
        }
        
        private void RepeatOrder(GameObject customer) {
            if (customer == gameObject) {
                onRepeatOrder?.Invoke();
            }
        }
        
        private void Serve(GameObject customer) {
            if (customer == gameObject) {
                onServe?.Invoke();
            }
        }
        
        private void CorrectDrinkServed(GameObject customer) {
            if (customer == gameObject) {
                onCorrectDrink?.Invoke();
            }
        }
        
        private void WrongDrinkServed(GameObject customer) {
            if (customer == gameObject) {
                onWrongDrink?.Invoke();
            }
        }

        private void PoisonServed(GameObject customer) {
            if (customer == gameObject) {
                onPoisoned?.Invoke();
            }
        }
        
        private void PatienceTimedOut(GameObject customer) {
            if (customer == gameObject) {
                onPatienceTimeOut?.Invoke();
            }
        }
        
        private void KickOut(GameObject customer) {
            if (customer == gameObject) {
                onKickedOut?.Invoke();
            }
        }
        
        private void Exit(GameObject customer) {
            if (customer == gameObject) {
                onExit?.Invoke();
            }
        }
    }
}