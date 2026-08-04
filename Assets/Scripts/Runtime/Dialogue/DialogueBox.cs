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
            if (_textAnimationCoroutine != null) StopCoroutine(_textDisplayingCoroutine);
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
            
            int characterCount = dialogue.Length;
            int currentIndex = 0;
            string currentMessage = "";
            
            float timePerLetter = timeUntilFullyDisplayed / characterCount;

            while (currentIndex < characterCount)
            {
                currentMessage += dialogue[currentIndex];
                
                textComponent.text = currentMessage;
                currentIndex++;
                yield return new WaitForSeconds(timePerLetter);
            }
        }
    }
}
