using System;
using Runtime.Drinks;
using Runtime.Drinks.Pouring;
using UnityEngine;

namespace Runtime.Old_Systems.Drink {
    [Obsolete]
    public class Sink : MonoBehaviour, IPourReceiver {
        public void AddContents(DrinkContents contents) { }

        public void AddContents(IngredientGroup group) { }
    }
}