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
    public class Customer : MonoBehaviour
    {
        public Vector3 barPosition;
        public Vector3 exitPosition;
        
        
        [Tooltip("The DialogueDisplay component attached to the customers dialogue boxes")]
        [SerializeField] private DialogueDisplay dialogueDisplay;
        
        [Tooltip("The scriptable object satisfaction port")]
        [SerializeField] private SatisfactionPort satisfactionPort;
        
        [Tooltip("The name that will be displayed as the customer name")]
        [SerializeField] private string customerName;
        
        [Tooltip("Is used to know whether customer is the target")]
        [SerializeField] private bool isTarget;

        [Tooltip("The time it takes for the customer to walk")]
        [SerializeField] [Min(0)] private float movementTime;
        
        [Tooltip("The possible drinks that when served to the customer will result in a successful order")]
        [SerializeField] private List<DrinkContents> drinks;
        
        
        
        [Foldout("Timers")]
        [Tooltip("The time it takes before the customer leaves")]
        [SerializeField] private float patienceTimer;
        
        [Foldout("Timers")]
        [Tooltip("The time the dialogue will remain visible upon activation")]
        [SerializeField] private float dialoguePopUpTimer;
        
        [Foldout("Timers")]
        [Tooltip("The time that will be removed from the patience timer when order is repeated")]
        [SerializeField] [Min(0)] private float timePenaltyRepeatOrder;
        
        [Foldout("Timers")]
        [Tooltip("The time it takes before the customer asks the player to hurry up")]
        [SerializeField] private float patienceTickTime;
        
        
        
        [Foldout("Dialogues")][ResizableTextArea]
        [Tooltip("The dialogue displayed when the customer enters the bar")]
        [SerializeField] private string attentionDialogue;
        
        [Foldout("Dialogues")][ResizableTextArea]
        [Tooltip("The dialogue displayed when the customer first orders")]
        [SerializeField] private string orderDialogue;

        [Foldout("Dialogues")][ResizableTextArea]
        [Tooltip("The dialogue displayed when the customer has been served the correct drink")]
        [SerializeField] private string successDialogue;

        [Foldout("Dialogues")][ResizableTextArea]
        [Tooltip("The dialogue displayed when the customer has been served an incorrect drink")]
        [SerializeField] private string failureDialogue;

        [Foldout("Dialogues")][ResizableTextArea]
        [Tooltip("The dialogue displayed when the customer is asked to repeat the order")]
        [SerializeField] private string repeatOrderDialogue;

        [Foldout("Dialogues")][ResizableTextArea]
        [Tooltip("The dialogue displayed when the customer starts to get impatient")]
        [FormerlySerializedAs("gettingImpatientDialogue")]
        [SerializeField] private string patienceTimerTickDialogue;
        
        [Foldout("Dialogues")][ResizableTextArea]
        [Tooltip("The dialogue displayed when the customer's patience has run out")]
        [SerializeField] private string patienceTimeOutDialogue;
        
        
        
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
        
        private bool _hasOrdered;
        private float _patienceTimer;
        private bool _showingDialogue;
        private bool _isLeaving;
        

        private void Start()
        {
            dialogueDisplay.SetCharacterName(customerName);
            _patienceTimer = patienceTimer;
            EnterBar();
        }

        private void EnterBar()
        {
            ShowDialogue(attentionDialogue);
            StartCoroutine(WalkToBar());
        }

        private void Update()
        {
            _patienceTimer -= Time.deltaTime;

            if (_patienceTimer <= 0 && !_isLeaving)
            {
                LeaveBar();
            }
        }

        public void ServeDrink(DrinkContents drink)
        {
            // Compare contents with accepted drinks
        }

        public void Order()
        {
            if (_showingDialogue) return;
            if (_isLeaving) return;

            if (!_hasOrdered)
            {
                ShowDialogue(orderDialogue);
                _hasOrdered = true;
            }
            else
            {
                ShowDialogue(repeatOrderDialogue);
                satisfactionPort.DecreaseSatisfaction(satisfactionRepeatOrder);
            }
        }

        private void ShowDialogue(string dialogue)
        {
            StartCoroutine(HandleDialogue(dialogue));
        }

        private IEnumerator HandleDialogue(string dialogue)
        {
            dialogueDisplay.ShowDialogue(dialogue);
            _showingDialogue = true;
            yield return new WaitForSeconds(dialoguePopUpTimer);
            dialogueDisplay.HideDialogue();
            _showingDialogue = false;
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

        private void LeaveBar()
        {
            if (_isLeaving) return;
            StartCoroutine(HandleDialogue(patienceTimeOutDialogue));
            StartCoroutine(WalkToExit());
            satisfactionPort.DecreaseSatisfaction(satisfactionMissedOrder);
        }

        private IEnumerator WalkToExit()
        {
            float elapsedTime = 0;
            Vector3 startPosition = transform.position;
            _isLeaving = true;
            
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
