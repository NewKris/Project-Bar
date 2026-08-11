using Runtime.Dialogue;
using UnityEngine;

namespace Runtime.Customers.Tutorial_Agent {
    public class TutorialDialogueRunner : MonoBehaviour {
        [Tooltip("The DialogueDisplay component attached to the customers dialogue boxes")]
        [SerializeField] private DialogueDisplay dialogueDisplay;
        [Tooltip("The time the dialogue will remain visible upon activation")]
        [SerializeField] private float dialoguePopUpTimer;

        public void ShowDialogueTimed(string dialogue) {
            dialogueDisplay.ShowDialogueTimed(dialogue, dialoguePopUpTimer);
        }

        public void HideDialogue() {
            dialogueDisplay.HideDialogue();
        }

        public void ShowDialogueNonTimed(string dialogue) {
            dialogueDisplay.ShowDialogueNonTimed(dialogue);
        }

        public void SetCharacterName(string characterName) {
            dialogueDisplay.SetCharacterName(characterName);
        }
    }
}