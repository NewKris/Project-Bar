using System;
using System.Collections.Generic;
using System.Linq;
using Runtime.Configuration;
using UnityEngine;

namespace Runtime.Drink {
    public class DrinkObject : MonoBehaviour {
        public DrinkContents currentContents;
        
        public float ShakeDuration { get; protected set; }
        protected Dictionary<int, float> MixDurations { get; set; }

        public void EmptyContents() {
            currentContents.ingredients.Clear();
            currentContents.mixType = MixType.None;
            ShakeDuration = 0f;
        }

        public void AddContents(DrinkContents contents) {
            currentContents.ingredients.AddRange(contents.ingredients); // Exclude container
            currentContents.mixType = contents.mixType;
            ShakeDuration = 0f;
        }
        
        public void AddIngredient(Ingredient ingredient) {
            currentContents.ingredients.Add(ingredient);
            DecreaseMixDurations();
        }

        private void Awake() {
            MixDurations = new  Dictionary<int, float>();
        }

        private void DecreaseMixDurations() {
            int[] keys = MixDurations.Keys.ToArray();
            
            foreach (int key in keys) {
                MixDurations[key] *= 0.5f;
            }
        }
    }
}