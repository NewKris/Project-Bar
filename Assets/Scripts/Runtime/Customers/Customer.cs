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
    [RequireComponent(typeof(CustomerMovement))]
    [RequireComponent(typeof(CustomerDialogue))]
    [RequireComponent(typeof(CustomerPatience))]
    [RequireComponent((typeof(Interactable)))]
    public class Customer : MonoBehaviour
    {
        [Tooltip("The scriptable object satisfaction port")]
        [SerializeField] private SatisfactionPort satisfactionPort;
        
        [Tooltip("Determines whether the player should lose satisfaction when the customer gets kicked out")]
        [SerializeField] private bool loseSatisfactionWhenKickedOut;
        
        [Tooltip("The mesh renderer used for the customers model")]
        [SerializeField] private MeshFilter customerMeshFilter;
        
        private List<Recipe> _acceptableDrinks;
        
        private float _timePenaltyRepeatOrder;
        
        private int _satisfactionSuccess;
        private int _satisfactionFailure;
        private int _satisfactionMissedOrder;
        private int _satisfactionRepeatOrder;
        private int _satisfactionKickedOut;

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

        // private void Start()
        // {
        //     GameObject cam = FindFirstObjectByType<Camera>().gameObject;
        //     transform.LookAt(cam.transform);
        //     transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y+180, transform.eulerAngles.z);
        // }

        private void HandlePatienceTimeOut()
        {
            _customerDialogue.PatienceTimeOut();
            satisfactionPort.DecreaseSatisfaction(_satisfactionMissedOrder);
            LeaveBar();
        }

        
        public void CustomerSetup(CustomerData data, CustomerEventPort port, Vector3 barPosition, Vector3 exitPosition)
        {
            if (data.mesh != null) customerMeshFilter.mesh = data.mesh;
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
            
            _customerMovement.Setup(barPosition, exitPosition, port);
            
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

        public void KickOut()
        {
            _customerDialogue.KickOut();

            if (loseSatisfactionWhenKickedOut)
            {
                satisfactionPort.DecreaseSatisfaction(_satisfactionKickedOut);
            }
            
            LeaveBar();
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
