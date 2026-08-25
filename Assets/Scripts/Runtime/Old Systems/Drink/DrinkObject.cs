using System;
using System.Collections.Generic;
using System.Linq;
using FMODUnity;
using Runtime.Audio;
using Runtime.Drinks;
using Runtime.Drinks.Converting;
using Runtime.Drinks.Pouring;
using Runtime.Satisfaction;
using Runtime.Utility;
using UnityEngine;

namespace Runtime.Old_Systems.Drink {
    [Obsolete]
    public class DrinkObject : MonoBehaviour, IPourable, IPourReceiver, IConvertable {
        [HideInInspector] public bool isDirty;
        
        public DrinkContents currentContents;
        public EventReference pourAudio;
        
        [Header("Overflow")]
        public int maxIngredients;
        public SatisfactionPort satisfactionPort;
        
        protected float ShakeDuration { get; set; }
        private Dictionary<int, float> StationDurations { get; set; }

        public bool HasContent => currentContents.IngredientCount > 0;

        public float GetStationCompletion(int stationKey, float maxDuration) {
            return StationDurations[stationKey] / maxDuration;
        }

        public bool ShouldReact(Reaction[] reactions, out Ingredient result) {
            result = null;
            
            if (reactions == null) return false;
            
            foreach (Reaction reaction in reactions) {
                if (currentContents.Contains(reaction.catalyst)) {
                    result = reaction.result;
                    return true;
                }
            }

            return false;
        }
        
        public void ConvertIngredients(Conversion[] conversions) {
            if (conversions == null) return;
            
            foreach (IngredientGroup group in currentContents.ingredientGroups) {
                foreach (Conversion conversion in conversions) {
                    if (group.ingredients.Contains(conversion.from)) {
                        group.ingredients.Remove(conversion.from);
                        group.ingredients.Add(conversion.to);
                    }
                }
            }
        }
        
        public void EmptyContents() {
            if (currentContents.ContainsLiquid()) {
                SfxManager.PlayOneShot(new OneShotConfig() {
                    eventReference = pourAudio,
                    attachedGameObject = gameObject,
                    parameters = new [] {
                        FmodParameter.NoLooping
                    }
                });
            }
            
            currentContents.Clear();
            ResetDurations();
        }
        
        public void GiveContent(IPourReceiver receiver) {
            receiver.AddContents(currentContents);
        }

        public void AddContents(DrinkContents contents) {
            if (contents.IngredientCount > 0) {
                isDirty = true;
            }
            
            currentContents.ingredientGroups.AddRange(contents.ingredientGroups);
            ResetDurations();
        }

        public void AddContents(IngredientGroup group) {
            if (group.ingredients.Count > 0) {
                isDirty = true;
            }
            
            currentContents.ingredientGroups.Add(group);
            ResetDurations();
        }
        
        public void AddIngredient(Ingredient ingredient) {
            if (currentContents.IngredientCount >= maxIngredients) {
                VerboseDebug.Log("Cannot add ingredient: Container is full!");
                satisfactionPort.DecreaseSatisfaction(satisfactionPort.overflowPenalty);
                return;
            }
            
            VerboseDebug.Log("Adding ingredient " + ingredient.name);

            if (ShouldReact(ingredient.ingredientReactions, out Ingredient replacement)) {
                ingredient = replacement;
                VerboseDebug.Log("Reaction! Adding ingredient " + ingredient.name + " instead!");
            }
            
            isDirty = true;
            currentContents.ingredientGroups.Add(IngredientGroup.CreateNewGroup(ingredient));
            DecreaseStationDurations();
            ConvertIngredients(ingredient.ingredientInteractions);
            PlayIngredientOneShot(ingredient);
        }

        private void PlayIngredientOneShot(Ingredient ingredient) {
            if (!ingredient.ingredientSound.IsNull) {
                SfxManager.PlayOneShot(new OneShotConfig() {
                    eventReference = ingredient.ingredientSound,
                    attachedGameObject = gameObject,
                    parameters = new [] {
                        FmodParameter.NoLooping
                    }
                });
            }
        }

        private void ResetDurations() {
            ShakeDuration = 0f;
            StationDurations.Clear();
        }
        
        private void Awake() {
            StationDurations = new Dictionary<int, float>();
        }

        private void DecreaseStationDurations() {
            if(StationDurations.Count == 0) return;
            
            int[] keys = StationDurations.Keys.ToArray();
            
            foreach (int key in keys) {
                StationDurations[key] *= 0.5f;
            }

            ShakeDuration *= 0.5f;
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