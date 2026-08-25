using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Drinks {
    public class IngredientInfoPanel : MonoBehaviour {
        private static Dictionary<IngredientInfoTrigger, float> LookedAtTriggers;

        public GameObject panelBox;
        public TMP_Text title;
        public TMP_Text description;
        public Image icon;
        public float lookTime;
        
        public static void BeginTriggerLook(IngredientInfoTrigger trigger) {
            LookedAtTriggers.Add(trigger, Time.time);
        }

        public static void RemoveTriggerLook(IngredientInfoTrigger trigger) {
            LookedAtTriggers.Remove(trigger);
        }

        private void Awake() {
            LookedAtTriggers = new Dictionary<IngredientInfoTrigger, float>(10);
        }

        private void Update() {
            bool hasLookTarget = false;
            
            foreach (KeyValuePair<IngredientInfoTrigger, float> lookedAtTrigger in LookedAtTriggers) {
                float t = Time.time - lookedAtTrigger.Value;
                if (t > lookTime) {
                    hasLookTarget = true;
                    DrawInfo(lookedAtTrigger.Key.ingredient);
                    break;
                }
            }
            
            if (!hasLookTarget) DisablePanel();
        }

        private void DisablePanel() {
            panelBox.SetActive(false);
        }

        private void DrawInfo(Ingredient ingredient) {
            panelBox.SetActive(true);
            title.text = ingredient.DisplayName;
            description.text = ingredient.description;
            icon.sprite = ingredient.icon;
        }
    }
}