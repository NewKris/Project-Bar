using System;
using System.Collections.Generic;
using NaughtyAttributes;
using Runtime.Dialogue;
using Runtime.Drink;
using UnityEngine;

namespace Runtime.Customers
{
    public class Customer : MonoBehaviour
    {
        [SerializeField] private DialogueDisplay dialogueDisplay;
        
        [Tooltip("The name that will be displayed as the customer name")]
        [SerializeField] private string customerName;
        
        [Tooltip("Is used to know whether customer is the target")]
        [SerializeField] private bool isTarget;
        
        [Tooltip("The time it takes before the customer leaves")]
        [SerializeField] private float patienceTimer;
        
        [Tooltip("The time the dialogue will remain visible upon activation")]
        [SerializeField] private float dialogueActiveTimer;
        
        [Tooltip("The time that will be removed from the patience timer when order is repeated")]
        [SerializeField] [Min(0)] private float timePenaltyRepeatOrder;
        
        [Tooltip("The time it takes before the customer asks the player to hurry up")]
        [SerializeField] private float patienceMessageTime;
        
        [Tooltip("The possible drinks that when served to the customer will result in a successful order")]
        [SerializeField] private List<DrinkContents> drinks;
        
        
        [Foldout("Dialogues")][ResizableTextArea]
        [Tooltip("The dialogue displayed when the customer enters the bar")]
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
        [Tooltip("The dialogue displayed when the customer's patience has run out")]
        [SerializeField] private string patienceTimeOutDialogue;
        
        [Foldout("Dialogues")][ResizableTextArea]
        [Tooltip("The dialogue displayed when the customer starts to get impatient")]
        [SerializeField] private string gettingImpatientDialogue;
        
        
        [Foldout("Satisfaction settings")]
        [Tooltip("The amount of satisfaction that the player will gain on a successful order")]
        [SerializeField] [Min(0)] private int satisfactionSuccess;
        
        [Foldout("Satisfaction settings")]
        [Tooltip("The amount of satisfaction that the player will lose on if they fail the order")]
        [SerializeField] [Min(0)] private int satisfactionFailure;
        
        [Foldout("Satisfaction settings")]
        [Tooltip("The amount of satisfaction that the player will lose if the customer isn't served")]
        [SerializeField] [Min(0)] private string satisfactionMissedOrder;
        
        [Foldout("Satisfaction settings")]
        [Tooltip("The amount of satisfaction that the player will lose if they repeat the customers order")]
        [SerializeField] [Min(0)] private int satisfactionRepeatOrder;

        private bool _hasOrdered;
        private float _patienceTimer;
        private float _dialogueActiveTimer;

        private void Start()
        {
            
        }

        private void Update()
        {
            // Update and check timers
        }

        public void ServeDrink(DrinkContents drink)
        {
            // Compare contents with accepted drinks
            
        }

        public void Order()
        {
            if (!_hasOrdered)
            {
                dialogueDisplay.ShowDialogue(orderDialogue);
                _hasOrdered = true;
            }
            else
            {
                dialogueDisplay.ShowDialogue(repeatOrderDialogue);
            }
            
            // If customer has not ordered:
            //  Use base dialogue
            // Else:
            //  Use repeat order dialogue
            //  Make player lose satisfaction
        }
    }
}