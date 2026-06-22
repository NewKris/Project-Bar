using System;
using System.Collections.Generic;

namespace Runtime.Drink {
    [Serializable]
    public struct DrinkContents {
        public Ingredient container;
        public List<Ingredient> prep;
        public List<Ingredient> liquids;
        public List<Ingredient> garnish;
        public MixType mixType;
    }
}