using System.Collections;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

namespace Runtime.Dialogue
{
    public class DialogueDisplay : MonoBehaviour
    {
        [SerializeField] private bool hasName;
        [EnableIf("hasName")] [SerializeField] private TMP_Text nameObject;
        [SerializeField] private bool alwaysDisplayName;
        [SerializeField] private TMP_Text dialogueBox;
        [SerializeField] private float timeToDisplayDialogue = 1;

        private string _name;
        private Coroutine _dialogueCoroutine;
        
        public void ShowDialogue(string dialogue)
        {
            if (hasName)
            {
                nameObject.gameObject.SetActive(true);
                nameObject.text = _name;
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

        public void SetCharacterName(string characterName)
        {
            if (!hasName) return;
            _name = characterName;
            if (alwaysDisplayName)
            {
                nameObject.gameObject.SetActive(true);
                nameObject.text = characterName;
            }
        }

        private IEnumerator DisplayDialogue(string dialogue)
        {
            if (timeToDisplayDialogue <= 0)
            {
                dialogueBox.text = dialogue;
                yield break;
            }
            
            int characterCount = dialogue.Length;
            int currentIndex = 0;
            string currentMessage = "";
            
            float timePerLetter = timeToDisplayDialogue / characterCount;

            while (currentIndex < characterCount)
            {
                currentMessage += dialogue[currentIndex];
                dialogueBox.text = currentMessage;
                currentIndex++;
                yield return new WaitForSeconds(timePerLetter);
            }
            
            dialogueBox.text = dialogue;
        }
        
    }
}