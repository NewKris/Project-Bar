using System;
using System.Collections.Generic;

namespace Runtime.Drink {
    [Serializable]
    public struct DrinkContents {
        public DrinkContainer container;
        public List<Ingredient> ingredients;
        public MixType mixType;
        public bool autoFail;
    }
}