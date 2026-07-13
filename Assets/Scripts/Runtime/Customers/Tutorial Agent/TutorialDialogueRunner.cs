using Runtime.Dialogue;
using UnityEngine;

namespace Runtime.Customers.Tutorial_Agent {
    public class TutorialDialogueRunner : MonoBehaviour {
        [Tooltip("The DialogueDisplay component attached to the customers dialogue boxes")]
        [SerializeField] private DialogueDisplay dialogueDisplay;
        [Tooltip("The time the dialogue will remain visible upon activation")]
        [SerializeField] private float dialoguePopUpTimer;
        
        public void ShowDialogue(string dialogue)
        {
            dialogueDisplay.ShowDialogueTimed(dialogue, dialoguePopUpTimer);
        }
    }
}