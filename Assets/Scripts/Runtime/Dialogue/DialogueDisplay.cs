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
        [EnableIf("hasName")] [SerializeField] private NameBox nameObject;
        [EnableIf("hasName")] [SerializeField] private bool alwaysDisplayName;
        
        // rework dialogue box back to old and use a separate
        
        [SerializeField] private DialogueBox dialogueBox;
        [Tooltip("The time it takes until all text is shown, if 0 or less all dialogue will be shown at once otherwise one letter at a time")]
        [SerializeField] private float writeTime = 1;

        [SerializeField] private Image timeRemainingImage;

        private string _name;
        private Coroutine _dialogueCoroutine;
        [HideInInspector]
        public bool showingDialogue;

        private bool _breakingActiveDialogue;

        public void ShowDialogueNonTimed(string dialogue) {
            if (_dialogueCoroutine != null) StopCoroutine(_dialogueCoroutine);
            
            ShowDialogue(dialogue);
            timeRemainingImage.gameObject.SetActive(false);
        }
        
        private void ShowDialogue(string dialogue)
        {
            if (hasName && !alwaysDisplayName)
            {
                nameObject.gameObject.SetActive(true);
                nameObject.textObject.text = _name;
            }
            
            dialogueBox.gameObject.SetActive(true);

            // if (_dialogueCoroutine != null) StopCoroutine(_dialogueCoroutine); 
            // _dialogueCoroutine =
            //     StartCoroutine(TextMethods.DisplayText(dialogueBox.textComponent, dialogue, writeTime));
            
            dialogueBox.DisplayText(dialogue, writeTime);
        }
        
        public void HideDialogue()
        {
            if (!alwaysDisplayName) nameObject.gameObject.SetActive(false);
            timeRemainingImage.gameObject.SetActive(true);
            dialogueBox.gameObject.SetActive(false);
            showingDialogue = false;
            _breakingActiveDialogue = false;
        }

        public void ShowDialogueTimed(string dialogue, float timer)
        {
            _dialogueCoroutine = StartCoroutine(HandleTimedDialogue(dialogue, timer));
        }
        
        private IEnumerator HandleTimedDialogue(string dialogue, float timer)
        {
            if (showingDialogue)
            {
                _breakingActiveDialogue = true;
                while (_breakingActiveDialogue)
                {
                    yield return new WaitForFixedUpdate();
                }

                _breakingActiveDialogue = false;
            }
            
            ShowDialogue(dialogue);
            timeRemainingImage.gameObject.SetActive(true);
            showingDialogue = true;
            float elapsedTime = timer;

            while (elapsedTime > 0)
            {
                timeRemainingImage.fillAmount = elapsedTime / timer;
                elapsedTime -= Time.fixedDeltaTime;
                
                if (_breakingActiveDialogue) break;
                
                yield return new WaitForFixedUpdate();
            }
            
            _breakingActiveDialogue = false;
            showingDialogue = false;
            HideDialogue();
        }

        public void SetCharacterName(string characterName)
        {
            if (!hasName) return;
            _name = characterName;
            if (alwaysDisplayName)
            {
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