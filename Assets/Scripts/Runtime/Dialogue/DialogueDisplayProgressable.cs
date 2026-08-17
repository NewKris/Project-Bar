using NaughtyAttributes;
using Runtime.Old_Systems.Interact;
using TMPro;
using UnityEngine;

namespace Runtime.Dialogue {
    [RequireComponent(typeof(Interactable))]
    public class DialogueDisplayProgressable : MonoBehaviour {
        [SerializeField] private bool hasName;
        [EnableIf("hasName")] [SerializeField] private NameBox nameObject;
        [EnableIf("hasName")] [SerializeField] private bool alwaysDisplayName;
        
        // rework dialogue box back to old and use a separate
        
        [SerializeField] private DialogueBox dialogueBox;
        [Tooltip("The time it takes until all text is shown, if 0 or less all dialogue will be shown at once otherwise one letter at a time")]
        [SerializeField] private float writeTime = 1;

        private string _name;

        [HideInInspector]
        public bool showingDialogue;
        
        
        public void ShowDialogue(string dialogue)
        {
            if (hasName && !alwaysDisplayName)
            {
                nameObject.gameObject.SetActive(true);
                nameObject.textObject.text = _name;
            }
            
            dialogueBox.gameObject.SetActive(true);
            
            dialogueBox.DisplayText(dialogue, writeTime);
        }
        
        public void HideDialogue()
        {
            if (!alwaysDisplayName) nameObject.gameObject.SetActive(false);
            dialogueBox.gameObject.SetActive(false);
            showingDialogue = false;
        }

        public void SetCharacterName(string characterName)
        {
            if (!hasName) return;
            _name = characterName;
            Debug.Log(characterName);
            if (alwaysDisplayName)
            {
                Debug.Log("Hi I need help");
                nameObject.gameObject.SetActive(true);
                nameObject.textObject.text = characterName;
            }
        }

        public void ShowCharacterName(string characterName)
        {
            _name = characterName;
            nameObject.gameObject.SetActive(true);
            nameObject.textObject.text = characterName;
        }

        public void HideCharacterName()
        {
            nameObject.gameObject.SetActive(false);
        }
    }
}