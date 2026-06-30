using System;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime.Drink {
    [Serializable]
    public struct DrinkContents {
        public DrinkContainer container;
        public List<Ingredient> ingredients;
        public MixType mixType;
        [HideInInspector] public bool autoFail;
    }
}