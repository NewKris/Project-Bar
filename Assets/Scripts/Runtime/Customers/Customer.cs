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
        [Tooltip("The scriptable object satisfaction port")]
        [SerializeField] private SatisfactionPort satisfactionPort;
        
        [Tooltip("Is used to know whether customer is the target")]
        [SerializeField] private bool isTarget;
        
        [Tooltip("The possible drinks that when served to the customer will result in a successful order")]
        [SerializeField] private List<Recipe> acceptableDrinks;
        
        
        [Foldout("Timers")]
        [Tooltip("The time that will be removed from the patience timer when order is repeated")]
        [SerializeField] [Min(0)] private float timePenaltyRepeatOrder;
        
        
        [Foldout("Satisfaction settings")]
        [Tooltip("The amount of satisfaction that the player will gain on a successful order")]
        [SerializeField] [Min(0)] private int satisfactionSuccess;
        
        [Foldout("Satisfaction settings")]
        [Tooltip("The amount of satisfaction that the player will lose on if they fail the order")]
        [SerializeField] [Min(0)] private int satisfactionFailure;
        
        [Foldout("Satisfaction settings")]
        [Tooltip("The amount of satisfaction that the player will lose if the customer isn't served")]
        [SerializeField] [Min(0)] private int satisfactionMissedOrder;
        
        [Foldout("Satisfaction settings")]
        [Tooltip("The amount of satisfaction that the player will lose if they repeat the customers order")]
        [SerializeField] [Min(0)] private int satisfactionRepeatOrder;

        private CustomerMovement _customerMovement;
        private CustomerDialogue _customerDialogue;
        private CustomerPatience _customerPatience;
        
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
            
            _customerDialogue.SetName();
            EnterBar();
        }

        private void HandlePatienceTimeOut()
        {
            _customerDialogue.PatienceTimeOut();
            satisfactionPort.DecreaseSatisfaction(satisfactionMissedOrder);
            LeaveBar();
        }

        public void ServeDrink(DrinkContents drink)
        {
            // Compare contents with accepted drinks
            Debug.Log("Serving 💅");

            if (drink.DrinkIsAccepted(acceptableDrinks))
            {
                Debug.Log("Drink accepted!");
                _customerDialogue.Success();
                satisfactionPort.IncreaseSatisfaction(satisfactionSuccess);
            }
            else
            {
                Debug.Log("Drink rejected");
                _customerDialogue.Failure();
                satisfactionPort.DecreaseSatisfaction(satisfactionFailure);
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
                satisfactionPort.DecreaseSatisfaction(satisfactionRepeatOrder);
                _customerPatience.AddTime(-timePenaltyRepeatOrder);
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
