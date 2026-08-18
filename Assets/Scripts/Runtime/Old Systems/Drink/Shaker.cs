using System;
using System.Linq;
using FMODUnity;
using Runtime.Audio;
using Runtime.Drinks;
using UnityEngine;

namespace Runtime.Old_Systems.Drink {
    [Serializable]
    public struct ShakerConversion {
        public Ingredient from;
        public Ingredient to;
    }
    
    [Obsolete]
    public class Shaker : DrinkObject {
        public float shakeDuration = 0.5f;
        public ShakerConversion[] conversions;

        [Header("Audio")] 
        public EventReference liquid;
        public EventReference ice;
        public Ingredient[] iceIngredients;

        private const string _audioKey = "shaker_shake";

        private void OnEnable() {
            Debug.Log("OnEnable");
            if (currentContents.Contains(iceIngredients)) {
                SfxManager.StartAudio(_audioKey, ice, transform.position);
            }
            else {
                SfxManager.StartAudio(_audioKey, liquid, transform.position);
            }
        }

        private void OnDisable() {
            Debug.Log("OnDisable");
            SfxManager.StopAudio(_audioKey);
        }

        private void Start() {
            enabled = false;
        }

        private void Update() {
            ShakeDuration += Time.deltaTime;
            if (ShakeDuration >= shakeDuration) {
                GroupIngredients();
            }
        }

        private void GroupIngredients() {
            IngredientGroup shakerGroup = IngredientGroup
                .CreateNewGroup(currentContents.ingredientGroups.SelectMany(x => x.ingredients).ToArray());
            
            currentContents.Clear();
            currentContents.Add(shakerGroup);
            
            foreach (ShakerConversion conversion in conversions) {
                if (shakerGroup.ingredients.Contains(conversion.from)) {
                    shakerGroup.ingredients.Remove(conversion.from);
                    shakerGroup.ingredients.Add(conversion.to);
                }
            }
        }
    }
}