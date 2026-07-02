using System;
using System.Collections.Generic;
using System.Linq;
using Runtime.Configuration;
using UnityEngine;

namespace Runtime.Drink {
    public class DrinkObject : MonoBehaviour {
        public DrinkContents currentContents;
        
        protected float ShakeDuration { get; set; }
        private Dictionary<int, float> StationDurations { get; set; }

        public void EmptyContents() {
            currentContents.ingredients.Clear();
            currentContents.mixType = MixType.None;
            ResetDurations();
        }

        public void AddContents(DrinkContents contents) {
            currentContents.ingredients.AddRange(contents.ingredients);
            currentContents.mixType = contents.mixType;
            ResetDurations();
        }
        
        public void AddIngredient(Ingredient ingredient) {
            currentContents.ingredients.Add(ingredient);
            DecreaseStationDurations();
        }

        private void ResetDurations() {
            ShakeDuration = 0f;
            StationDurations.Clear();
        }
        
        private void Awake() {
            StationDurations = new Dictionary<int, float>();
        }

        private void DecreaseStationDurations() {
            int[] keys = StationDurations.Keys.ToArray();
            
            foreach (int key in keys) {
                StationDurations[key] *= 0.5f;
            }
        }
        
        public void CreateStationTimer(int key) {
            StationDurations.Add(key, 0);
        }

        public bool HasStationTimer(int key) {
            return  StationDurations.ContainsKey(key);
        }

        public void TickStationTimer(int key) {
            if (StationDurations.ContainsKey(key)) {
                StationDurations[key] += Time.deltaTime;
            }
        }

        public void RemoveStationKey(int key) {
            StationDurations.Remove(key);
        }

        public float GetStationTime(int key) {
            return  StationDurations.GetValueOrDefault(key, 0);
        }
    }
}