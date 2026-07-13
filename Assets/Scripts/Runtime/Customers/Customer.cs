using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using Runtime.Dialogue;
using Runtime.Drink;
using Runtime.Interact;
using Runtime.Satisfaction;
using UnityEngine;
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
            _customerDialogue.PatienceTimeOut();
            satisfactionPort.DecreaseSatisfaction(_satisfactionMissedOrder);
            _customerBase.LeaveBar();
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

            if (drink.DrinkIsAccepted(_acceptableDrinks))
            {
                Debug.Log("Drink accepted!");
                _customerDialogue.Success();
                satisfactionPort.IncreaseSatisfaction(_satisfactionSuccess);
            }
            else
            {
                Debug.Log("Drink rejected");
                _customerDialogue.Failure();
                satisfactionPort.DecreaseSatisfaction(_satisfactionFailure);
            }

            if (drink.ContainsPoison()) {
                customerEventPort.RaiseCustomerDiedEvent(_isTarget);
            }
            
            _customerBase.LeaveBar();
        }

        private void OnOrder()
        {
            if (!_hasOrdered) {
                _hasOrdered = true;
                _customerDialogue.Order();
            }
            else {
                _customerDialogue.RepeatOrder();
                satisfactionPort.DecreaseSatisfaction(_satisfactionRepeatOrder);
                _customerPatience.AddTime(-_timePenaltyRepeatOrder);
            }
            
        }

        public void KickOut()
        {
            _customerDialogue.KickOut();

            if (loseSatisfactionWhenKickedOut)
            {
                satisfactionPort.DecreaseSatisfaction(_satisfactionKickedOut);
            }
            
            _customerBase.LeaveBar();
        }

        private void OnEnterBar()
        {
            _customerDialogue.Attention();
        }
    }
}
