using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using Runtime.Dialogue;
using Runtime.Drinks;
using Runtime.Old_Systems.Drink;
using Runtime.Old_Systems.Interact;
using Runtime.Satisfaction;
using Runtime.UI;
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
    [RequireComponent(typeof(CustomerActionEvents))]
    public class Customer : MonoBehaviour
    {
        [Tooltip("The scriptable object satisfaction port")]
        [SerializeField] private SatisfactionPort satisfactionPort;
        
        [Tooltip("The scriptable object customer event port")]
        [SerializeField] private CustomerEvents customerEventPort;
        
        [Tooltip("Determines whether the player should lose satisfaction when the customer gets kicked out")]
        [SerializeField] private bool loseSatisfactionWhenKickedOut;

        [SerializeField] private bool disableAttentionDialogue;

        public Transform barkSpawn;
        
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
            _customerBase.customerEventHandler.TimeOutPatience(gameObject);
            _customerDialogue.PatienceTimeOut();
            satisfactionPort.DecreaseSatisfaction(_satisfactionMissedOrder);
            LeaveBar();
        }

        
        public void CustomerSetup(CustomerData data, CustomerEventPort port, Vector3 barPosition, Vector3 exitPosition)
        {
            _customerBase.Setup(data.mesh, barPosition, exitPosition, port, data);
            
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
            _customerBase.customerEventHandler.Serve(gameObject);

            if (drink.DrinkIsAccepted(_acceptableDrinks, BarkComplaint))
            {
                _customerBase.customerEventHandler.CorrectDrinkServed(gameObject);
                Debug.Log("Drink accepted!");
                _customerDialogue.Success();
                satisfactionPort.IncreaseSatisfaction(_satisfactionSuccess);
                
                if (drink.ContainsPoison()) {
                    customerEventPort.RaiseCustomerDiedEvent(_isTarget);
                    _customerBase.customerEventHandler.PoisonServed(gameObject);
                }
            }
            else {
                _customerBase.customerEventHandler.WrongDrinkServed(gameObject);
                Debug.Log("Drink rejected");
                _customerDialogue.Failure();
                satisfactionPort.DecreaseSatisfaction(_satisfactionFailure);
            }
            
            LeaveBar();
        }
        
        private void OnOrder()
        {
            if (!_hasOrdered) {
                _customerBase.customerEventHandler.Order(gameObject);
                _hasOrdered = true;
                _customerDialogue.Order();
            }
            else {
                _customerBase.customerEventHandler.RepeatOrder(gameObject);
                _customerDialogue.RepeatOrder();
                satisfactionPort.DecreaseSatisfaction(_satisfactionRepeatOrder);
                _customerPatience.AddTime(-_timePenaltyRepeatOrder);
            }
            
        }
 
        public void KickOut() {
            _customerBase.customerEventHandler.KickOut(gameObject);
            _customerDialogue.KickOut();

            if (loseSatisfactionWhenKickedOut)
            {
                satisfactionPort.DecreaseSatisfaction(_satisfactionKickedOut);
            }
            
            LeaveBar();
        }

        private void BarkComplaint(string complaint) {
            WorldSpaceCanvas.SpawnBarkText(complaint, barkSpawn.position, barkSpawn.rotation);
        }
        
        private void LeaveBar() {
            _customerBase.customerEventHandler.Exit(gameObject);
            _customerBase.LeaveBar();
        }

        private void OnEnterBar()
        {
            _customerBase.customerEventHandler.Enter(gameObject);
            if (!disableAttentionDialogue) _customerDialogue.Attention();
        }
    }
}
