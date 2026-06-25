using System.Collections;
using TMPro;
using UnityEngine;

namespace Runtime.Dialogue
{
    public static class TextMethods
    {
        public static IEnumerator DisplayText(TMP_Text textComponent,string dialogue, float timeUntilFullyDisplayed)
        {
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
            
            textComponent.text = dialogue;
        }
    }
}