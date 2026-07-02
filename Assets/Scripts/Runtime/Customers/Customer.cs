using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using Runtime.Dialogue;
using Runtime.Drink;
using Runtime.Satisfaction;
using UnityEngine;
using UnityEngine.Serialization;

namespace Runtime.Customers
{
    [RequireComponent(typeof(CustomerMovement))] [RequireComponent(typeof(CustomerDialogue))] [RequireComponent(typeof(CustomerPatience))]
    public class Customer : MonoBehaviour
    {
        public CustomerData data;
        
        [Tooltip("The scriptable object satisfaction port")]
        [SerializeField] private SatisfactionPort satisfactionPort;
        
        private List<Recipe> _acceptableDrinks;
        
        private float _timePenaltyRepeatOrder;
        
        private int _satisfactionSuccess;
        private int _satisfactionFailure;
        private int _satisfactionMissedOrder;
        private int _satisfactionRepeatOrder;

        private CustomerMovement _customerMovement;
        private CustomerDialogue _customerDialogue;
        private CustomerPatience _customerPatience;
        
        private bool _isTarget;
        private bool _hasOrdered;
        private bool _isLeaving;

        private void OnEnable()
        {
            _customerPatience = GetComponent<CustomerPatience>();
            _customerMovement = GetComponent<CustomerMovement>();
            _customerDialogue = GetComponent<CustomerDialogue>();
            
            _customerPatience.OnPatienceTick += _customerDialogue.PatienceTick;
            _customerPatience.OnPatienceTimeOut += HandlePatienceTimeOut;
        }

        private void OnDisable()
        {
            _customerPatience.OnPatienceTick -= _customerDialogue.PatienceTick;
            _customerPatience.OnPatienceTimeOut -= HandlePatienceTimeOut;
        }

        private void Start()
        {
            Debug.Log("CUSTOMER SETUP IS STILL CALLED IN CUSTOMER REMOVE WHEN CUSTOMER SPAWNING IS IMPLEMENTED");
            CustomerSetup(data);
        }

        private void HandlePatienceTimeOut()
        {
            _customerDialogue.PatienceTimeOut();
            satisfactionPort.DecreaseSatisfaction(_satisfactionMissedOrder);
            LeaveBar();
        }

        
        public void CustomerSetup(CustomerData data)
        {
            _isTarget = data.isTarget;
            _acceptableDrinks = data.acceptableDrinks;
            _timePenaltyRepeatOrder = data.timePenaltyRepeatOrder;
            _satisfactionSuccess = data.satisfactionSuccess;
            _satisfactionFailure = data.satisfactionFailure;
            _satisfactionMissedOrder = data.satisfactionMissedOrder;
            _satisfactionRepeatOrder = data.satisfactionRepeatOrder;
            
            _customerPatience.Setup(data.patienceTimer, data.patienceTickTime);
            _customerDialogue.Setup(
                data.customerName,
                data.attentionDialogue,
                data.orderDialogue,
                data.repeatOrderDialogue,
                data.successDialogue,
                data.failureDialogue,
                data.patienceTimerTickDialogue,
                data.patienceTimeOutDialogue
            );

            // TODO: Add movement positions to customer movement
            
            
            EnterBar();
        }

        public void ServeDrink(DrinkContents drink)
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
            
            LeaveBar();
        }

        public void Order()
        {
            if (_customerDialogue.IsSpeaking) return;
            if (_isLeaving) return;

            if (!_hasOrdered)
            {
                _customerDialogue.Order();
                _hasOrdered = true;
            }
            else
            {
                _customerDialogue.RepeatOrder();
                satisfactionPort.DecreaseSatisfaction(_satisfactionRepeatOrder);
                _customerPatience.AddTime(-_timePenaltyRepeatOrder);
            }
        }

        private void EnterBar()
        {
            _customerDialogue.Attention();
            _customerMovement.EnterBar();
        }

        private void LeaveBar()
        {
            if (_isLeaving) return;
            _customerMovement.ExitBar();
            _isLeaving = true;
        }
    }
}
