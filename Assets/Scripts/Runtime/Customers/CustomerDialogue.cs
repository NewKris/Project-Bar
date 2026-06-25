using NaughtyAttributes;
using Runtime.Dialogue;
using UnityEngine;
using UnityEngine.Serialization;

namespace Runtime.Customers
{
    public class CustomerDialogue : MonoBehaviour
    {
        public bool IsSpeaking => dialogueDisplay.showingDialogue;
        
        [Tooltip("The name that will be displayed as the customer name")]
        [SerializeField] private string customerName;
        
        [Tooltip("The DialogueDisplay component attached to the customers dialogue boxes")]
        [SerializeField] private DialogueDisplay dialogueDisplay;
        
        
        [Foldout("Dialogues")][ResizableTextArea]
        [Tooltip("The dialogue displayed when the customer enters the bar")]
        [SerializeField] private string attentionDialogue;
        
        [Foldout("Dialogues")][ResizableTextArea]
        [Tooltip("The dialogue displayed when the customer first orders")]
        [SerializeField] private string orderDialogue;
        
        [Foldout("Dialogues")][ResizableTextArea]
        [Tooltip("The dialogue displayed when the customer is asked to repeat the order")]
        [SerializeField] private string repeatOrderDialogue;

        [Foldout("Dialogues")][ResizableTextArea]
        [Tooltip("The dialogue displayed when the customer has been served the correct drink")]
        [SerializeField] private string successDialogue;

        [Foldout("Dialogues")][ResizableTextArea]
        [Tooltip("The dialogue displayed when the customer has been served an incorrect drink")]
        [SerializeField] private string failureDialogue;

        [Foldout("Dialogues")][ResizableTextArea]
        [Tooltip("The dialogue displayed when the customer starts to get impatient")]
        [FormerlySerializedAs("gettingImpatientDialogue")]
        [SerializeField] private string patienceTimerTickDialogue;
        
        [Foldout("Dialogues")][ResizableTextArea]
        [Tooltip("The dialogue displayed when the customer's patience has run out")]
        [SerializeField] private string patienceTimeOutDialogue;
        
        
        [Foldout("Timers")]
        [Tooltip("The time the dialogue will remain visible upon activation")]
        [SerializeField] private float dialoguePopUpTimer;

        public void SetName()
        {
            dialogueDisplay.SetCharacterName(customerName);
        }

        public void Attention()
        {
            ShowDialogue(attentionDialogue);
        }

        public void Order()
        {
            ShowDialogue(orderDialogue);
        }

        public void RepeatOrder()
        {
            ShowDialogue(repeatOrderDialogue);
        }

        public void Success()
        {
            ShowDialogue(successDialogue);
        }

        public void Failure()
        {
            ShowDialogue(failureDialogue);
        }

        public void PatienceTick()
        {
            ShowDialogue(patienceTimerTickDialogue);
        }

        public void PatienceTimeOut()
        {
            ShowDialogue(patienceTimeOutDialogue);
        }
        
        private void ShowDialogue(string dialogue)
        {
            dialogueDisplay.ShowDialogueTimed(dialogue, dialoguePopUpTimer);
        }
    }
}