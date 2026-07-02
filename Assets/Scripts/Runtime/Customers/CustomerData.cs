using System.Collections.Generic;
using NaughtyAttributes;
using Runtime.Drink;
using UnityEngine;

namespace Runtime.Customers
{
    [CreateAssetMenu(fileName = "Customer", menuName = "Customer", order = 0)]
    public class CustomerData : ScriptableObject
    {
        [Tooltip("The name that will be displayed as the customer name")]
        public string customerName;
        
        [Tooltip("Is used to know whether customer is the target")]
        public bool isTarget;
        
        [Tooltip("The possible drinks that when served to the customer will result in a successful order")]
        public List<Recipe> acceptableDrinks;
        
        
        [Foldout("Timers")]
        [Tooltip("The time that will be removed from the patience timer when order is repeated")]
        [Min(0)] public float timePenaltyRepeatOrder;
        
        [Foldout("Timers")]
        [Tooltip("The time it takes before the customer leaves")]
        [Min(0)] public float patienceTimer;
        
        [Foldout("Timers")]
        [Tooltip("The time it takes before the customer asks the player to hurry up")]
        [Min(0)] public float patienceTickTime;
        
        
        [Foldout("Satisfaction settings")]
        [Tooltip("The amount of satisfaction that the player will gain on a successful order")]
        [Min(0)] public int satisfactionSuccess;
        
        [Foldout("Satisfaction settings")]
        [Tooltip("The amount of satisfaction that the player will lose on if they fail the order")]
        [Min(0)] public int satisfactionFailure;
        
        [Foldout("Satisfaction settings")]
        [Tooltip("The amount of satisfaction that the player will lose if the customer isn't served")]
        [Min(0)] public int satisfactionMissedOrder;
        
        [Foldout("Satisfaction settings")]
        [Tooltip("The amount of satisfaction that the player will lose if they repeat the customers order")]
        [Min(0)] public int satisfactionRepeatOrder;
        
        
        [Foldout("Dialogues")][ResizableTextArea]
        [Tooltip("The dialogue displayed when the customer enters the bar")]
        public string attentionDialogue;
        
        [Foldout("Dialogues")][ResizableTextArea]
        [Tooltip("The dialogue displayed when the customer first orders")]
        public string orderDialogue;
        
        [Foldout("Dialogues")][ResizableTextArea]
        [Tooltip("The dialogue displayed when the customer is asked to repeat the order")]
        public string repeatOrderDialogue;

        [Foldout("Dialogues")][ResizableTextArea]
        [Tooltip("The dialogue displayed when the customer has been served the correct drink")]
        public string successDialogue;

        [Foldout("Dialogues")][ResizableTextArea]
        [Tooltip("The dialogue displayed when the customer has been served an incorrect drink")]
        public string failureDialogue;

        [Foldout("Dialogues")][ResizableTextArea]
        [Tooltip("The dialogue displayed when the customer starts to get impatient")]
        public string patienceTimerTickDialogue;
        
        [Foldout("Dialogues")][ResizableTextArea]
        [Tooltip("The dialogue displayed when the customer's patience has run out")]
        public string patienceTimeOutDialogue;
        
    }
}