using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using Runtime.Dialogue;
using Runtime.Drink;
using Runtime.Interact;
using Runtime.Satisfaction;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Runtime.Customers
{
    [RequireComponent(typeof(CustomerBase))]
    [RequireComponent(typeof(CustomerMovement))]
    [RequireComponent(typeof(CustomerDialogue))]
    [RequireComponent(typeof(CustomerPatience))]
    [RequireComponent(typeof(Interactable))]
    public class Customer : MonoBehaviour
    {
        [Tooltip("The scriptable object satisfaction port")]
        [SerializeField] private SatisfactionPort satisfactionPort;
        
        [Tooltip("The scriptable object customer event port")]
        [SerializeField] private CustomerEvents customerEventPort;
        
        [Tooltip("Determines whether the player should lose satisfaction when the customer gets kicked out")]
        [SerializeField] private bool loseSatisfactionWhenKickedOut;

        public UnityEvent onServe;
        public UnityEvent onCorrectDrink;
        public UnityEvent onWrongDrink;
        public UnityEvent onPatienceTimeOut;
        public UnityEvent onPoisoned;
        public UnityEvent onOrder;
        public UnityEvent onRepeatOrder;
        public UnityEvent onKickedOut;
        public UnityEvent onEnter;
        public UnityEvent onExit;
        
        private List<Recipe> _acceptableDrinks;
        
        private float _timePenaltyRepeatOrder;
        
        private int _satisfactionSuccess;
        private int _satisfactionFailure;
        private int _satisfactionMissedOrder;
        private int _satisfactionRepeatOrder;
        private int _satisfactionKickedOut;

        private CustomerBase _customerBase;
        private CustomerDialogue _customerDialogue;
        private CustomerPatience _customerPatience;
        
        private bool _isTarget;
        private bool _hasOrdered;
        private bool _isLeaving;

        private void OnEnable()
        {
            _customerBase ??= GetComponent<CustomerBase>();
            _customerPatience ??= GetComponent<CustomerPatience>();
            _customerDialogue ??= GetComponent<CustomerDialogue>();
            
            _customerBase.onServeDrink += ServeDrink;
            _customerBase.onOrder += OnOrder;
            _customerBase.onEnterBar += OnEnterBar;
            
            
            _customerPatience.OnPatienceTick += _customerDialogue.PatienceTick;
            _customerPatience.OnPatienceTimeOut += HandlePatienceTimeOut;
        }

        private void OnDisable()
        {
            _customerPatience.OnPatienceTick -= _customerDialogue.PatienceTick;
            _customerPatience.OnPatienceTimeOut -= HandlePatienceTimeOut;
            _customerBase.onServeDrink -= ServeDrink;
            _customerBase.onOrder -= OnOrder;
            _customerBase.onEnterBar -= OnEnterBar;
        }

        private void HandlePatienceTimeOut()
        {
            onPatienceTimeOut?.Invoke();
            _customerDialogue.PatienceTimeOut();
            satisfactionPort.DecreaseSatisfaction(_satisfactionMissedOrder);
            LeaveBar();
        }

        
        public void CustomerSetup(CustomerData data, CustomerEventPort port, Vector3 barPosition, Vector3 exitPosition)
        {
            _customerBase.Setup(data.mesh, barPosition, exitPosition, port);
            
            _isTarget = data.isTarget;
            _acceptableDrinks = data.acceptableDrinks;
            _timePenaltyRepeatOrder = data.timePenaltyRepeatOrder;
            _satisfactionSuccess = data.satisfactionSuccess;
            _satisfactionFailure = data.satisfactionFailure;
            _satisfactionMissedOrder = data.satisfactionMissedOrder;
            _satisfactionRepeatOrder = data.satisfactionRepeatOrder;
            _satisfactionKickedOut = data.satisfactionKickedOut;
            
            _customerPatience.Setup(data.patienceTimer, data.patienceTickTime);
            _customerDialogue.Setup(
                data.customerName,
                data.attentionDialogue,
                data.orderDialogue,
                data.repeatOrderDialogue,
                data.successDialogue,
                data.failureDialogue,
                data.patienceTimerTickDialogue,
                data.patienceTimeOutDialogue,
                data.kickedOutDialogue
            );
            
            _customerBase.EnterBar();
        }

        private void ServeDrink(DrinkContents drink)
        {
            // Compare contents with accepted drinks
            Debug.Log("Serving 💅");
            onServe?.Invoke();

            if (drink.DrinkIsAccepted(_acceptableDrinks))
            {
                onCorrectDrink?.Invoke();
                Debug.Log("Drink accepted!");
                _customerDialogue.Success();
                satisfactionPort.IncreaseSatisfaction(_satisfactionSuccess);
                
                if (drink.ContainsPoison()) {
                    customerEventPort.RaiseCustomerDiedEvent(_isTarget);
                    onPoisoned.Invoke();
                }
            }
            else {
                onWrongDrink?.Invoke();
                Debug.Log("Drink rejected");
                _customerDialogue.Failure();
                satisfactionPort.DecreaseSatisfaction(_satisfactionFailure);
            }
            
            LeaveBar();
        }

        private void OnOrder()
        {
            if (!_hasOrdered) {
                onOrder?.Invoke();
                _hasOrdered = true;
                _customerDialogue.Order();
            }
            else {
                onRepeatOrder?.Invoke();
                _customerDialogue.RepeatOrder();
                satisfactionPort.DecreaseSatisfaction(_satisfactionRepeatOrder);
                _customerPatience.AddTime(-_timePenaltyRepeatOrder);
            }
            
        }

        public void KickOut() {
            onKickedOut?.Invoke();
            _customerDialogue.KickOut();

            if (loseSatisfactionWhenKickedOut)
            {
                satisfactionPort.DecreaseSatisfaction(_satisfactionKickedOut);
            }
            
            LeaveBar();
        }

        private void LeaveBar() {
            onExit?.Invoke();
            _customerBase.LeaveBar();
        }

        private void OnEnterBar()
        {
            onEnter?.Invoke();
            _customerDialogue.Attention();
        }
    }
}
