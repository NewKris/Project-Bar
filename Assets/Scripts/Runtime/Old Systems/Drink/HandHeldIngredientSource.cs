using System.Collections.Generic;
using FMODUnity;
using Runtime.Audio;
using Runtime.Drinks;
using Runtime.Drinks.Pouring;
using UnityEngine;

namespace Runtime.Old_Systems.Drink {
    public class HandHeldIngredientSource : MonoBehaviour, IPourable {
        public Ingredient[] ingredients;
        public EventReference pourSound;

        public bool HasContent => ingredients.Length > 0;

        public void EmptyContents() {
            SfxManager.PlayOneShot(new OneShotConfig() {
                eventReference = pourSound,
                attachedGameObject = gameObject
            });
        }

        public void GiveContent(IPourReceiver receiver) {
            receiver.AddContents(new IngredientGroup() {
                ingredients = new List<Ingredient>(ingredients),
            });
        }
    }
}