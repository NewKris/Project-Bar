using System;
using System.Collections;
using FMODUnity;
using NaughtyAttributes;
using Runtime.Audio;
using Runtime.Player;
using TMPro;
using UnityEngine;

namespace Runtime.Dialogue
{
    public class DialogueBox : MonoBehaviour
    {
        
        public GameObject dialogueBox;
        public TMP_Text textComponent;
        
        [Foldout("Audio")]
        [SerializeField] private EventReference dialogueAudio;
        [Foldout("Audio")]
        [SerializeField] private float timeBetweenAudio;
        
        
        private Coroutine _textDisplayingCoroutine;
        private Coroutine _textAnimationCoroutine;
        
        public void DisplayText(string dialogue, float timeUntilFullyDisplayed) {
            if (_textDisplayingCoroutine != null) StopCoroutine(_textDisplayingCoroutine);
            _textDisplayingCoroutine = StartCoroutine(AnimateTextWithAudio(dialogue, timeUntilFullyDisplayed));
        }

        private IEnumerator AnimateTextWithAudio(string dialogue, float timeUntilFullyDisplayed) {
            if (_textAnimationCoroutine != null) StopCoroutine(_textAnimationCoroutine);
            _textAnimationCoroutine = StartCoroutine(AnimateText(dialogue, timeUntilFullyDisplayed));

            float timeElapsed = 0;

            while (timeElapsed < timeUntilFullyDisplayed) {
                timeElapsed += timeBetweenAudio;
                
                SfxManager.PlayOneShot(dialogueAudio);
                
                yield return new WaitForSeconds(timeBetweenAudio);
            }
        }

        private IEnumerator AnimateText(string dialogue, float timeUntilFullyDisplayed) {
            if (timeUntilFullyDisplayed <= 0)
            {
                textComponent.text = dialogue;
                yield break;
            }
            
            textComponent.text = dialogue;
            textComponent.ForceMeshUpdate();
            textComponent.maxVisibleCharacters = 0;

            int characterCount = textComponent.textInfo.characterCount;
            float timePerLetter = timeUntilFullyDisplayed / characterCount;

            for (int i = 0; i <= characterCount; i++) {
                textComponent.maxVisibleCharacters = i;
                yield return new WaitForSeconds(timePerLetter);
            }
        }
    }
}
