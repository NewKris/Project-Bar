using System;
using System.Collections.Generic;

namespace Runtime.Drink {
    [Serializable]
    public struct DrinkContents {
        public List<Ingredient> ingredients;
        public MixType mixType;
    }
}