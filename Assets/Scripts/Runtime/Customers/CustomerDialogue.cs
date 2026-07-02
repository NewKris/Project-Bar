using NaughtyAttributes;
using Runtime.Dialogue;
using UnityEngine;
using UnityEngine.Serialization;

namespace Runtime.Customers
{
    public class CustomerDialogue : MonoBehaviour
    {
        public bool IsSpeaking => dialogueDisplay.showingDialogue;
        
        
        [Tooltip("The DialogueDisplay component attached to the customers dialogue boxes")]
        [SerializeField] private DialogueDisplay dialogueDisplay;
        
        private string _customerName;
        private string _attentionDialogue;
        private string _orderDialogue;
        private string _repeatOrderDialogue;
        private string _successDialogue;
        private string _failureDialogue;
        private string _patienceTimerTickDialogue;
        private string _patienceTimeOutDialogue;
        
        [Tooltip("The time the dialogue will remain visible upon activation")]
        [SerializeField] private float dialoguePopUpTimer;

        public void Setup(string customerName, string attention, string order, string repeatOrder, string success,
            string failure, string timerTick, string timeOut)
        {
            _customerName = customerName;
            SetName();
            
            _attentionDialogue = attention;
            _orderDialogue = order;
            _repeatOrderDialogue = repeatOrder;
            _successDialogue = success;
            _failureDialogue = failure;
            _patienceTimerTickDialogue = timerTick;
            _patienceTimeOutDialogue = timeOut;
        }
        
        private void SetName()
        {
            dialogueDisplay.SetCharacterName(_customerName);
        }

        public void Attention()
        {
            ShowDialogue(_attentionDialogue);
        }

        public void Order()
        {
            ShowDialogue(_orderDialogue);
        }

        public void RepeatOrder()
        {
            ShowDialogue(_repeatOrderDialogue);
        }

        public void Success()
        {
            ShowDialogue(_successDialogue);
        }

        public void Failure()
        {
            ShowDialogue(_failureDialogue);
        }

        public void PatienceTick()
        {
            ShowDialogue(_patienceTimerTickDialogue);
        }

        public void PatienceTimeOut()
        {
            ShowDialogue(_patienceTimeOutDialogue);
        }
        
        private void ShowDialogue(string dialogue)
        {
            dialogueDisplay.ShowDialogueTimed(dialogue, dialoguePopUpTimer);
        }
    }
}