using System.Collections;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Dialogue
{
    public class DialogueDisplay : MonoBehaviour
    {
        [SerializeField] private bool hasName;
        [EnableIf("hasName")] [SerializeField] private DialogueBox nameObject;
        [EnableIf("hasName")] [SerializeField] private bool alwaysDisplayName;
        
        [SerializeField] private DialogueBox dialogueBox;
        [Tooltip("The time it takes until all text is shown, if 0 or less all dialogue will be shown at once otherwise one letter at a time")]
        [SerializeField] private float timeUntilDialogueFullyDisplayed = 1;

        [SerializeField] private Image timeRemainingImage;

        private string _name;
        private Coroutine _dialogueCoroutine;
        [HideInInspector]
        public bool showingDialogue;
        
        
        public void ShowDialogue(string dialogue)
        {
            if (hasName && !alwaysDisplayName)
            {
                nameObject.gameObject.SetActive(true);
                nameObject.textComponent.text = _name;
            }
            
            dialogueBox.gameObject.SetActive(true);
            // dialogueBox.text = dialogue;
            _dialogueCoroutine = StartCoroutine(DisplayDialogue(dialogue));
        }
        
        public void HideDialogue()
        {
            if (!alwaysDisplayName) nameObject.gameObject.SetActive(false);
            dialogueBox.gameObject.SetActive(false);
        }

        public void ShowDialogueTimed(string dialogue, float timer)
        {
            StartCoroutine(HandleTimedDialogue(dialogue, timer));
        }
        
        private IEnumerator HandleTimedDialogue(string dialogue, float timer)
        {
            ShowDialogue(dialogue);
            showingDialogue = true;
            float elapsedTime = timer;

            while (elapsedTime > 0)
            {
                timeRemainingImage.fillAmount = elapsedTime / timer;
                elapsedTime -= Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }
            
            HideDialogue();
            showingDialogue = false;
        }

        public void SetCharacterName(string characterName)
        {
            if (!hasName) return;
            _name = characterName;
            if (alwaysDisplayName)
            {
                nameObject.gameObject.SetActive(true);
                nameObject.textComponent.text = characterName;
            }
        }

        public void ShowCharacterName(string characterName)
        {
            _name = characterName;
            nameObject.gameObject.SetActive(true);
            nameObject.textComponent.text = characterName;
        }

        public void HideCharacterName()
        {
            nameObject.gameObject.SetActive(false);
        }

        private IEnumerator DisplayDialogue(string dialogue)
        {
            if (timeUntilDialogueFullyDisplayed <= 0)
            {
                dialogueBox.textComponent.text = dialogue;
                yield break;
            }
            
            int characterCount = dialogue.Length;
            int currentIndex = 0;
            string currentMessage = "";
            
            float timePerLetter = timeUntilDialogueFullyDisplayed / characterCount;

            while (currentIndex < characterCount)
            {
                currentMessage += dialogue[currentIndex];
                dialogueBox.textComponent.text = currentMessage;
                currentIndex++;
                yield return new WaitForSeconds(timePerLetter);
            }
            
            dialogueBox.textComponent.text = dialogue;
        }
        
    }
}