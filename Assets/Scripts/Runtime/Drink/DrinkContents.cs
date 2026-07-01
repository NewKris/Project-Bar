using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Runtime.Drink {
    [Serializable]
    public struct DrinkContents {
        public List<Ingredient> ingredients;
        public MixType mixType;
        [HideInInspector] public bool autoFail;

        public DrinkContainer GetContainer() {
            return ingredients.First(x => x is DrinkContainer) as DrinkContainer;
        }
    }
}