using System;

namespace Runtime.Drinks.Converting {
    [Serializable]
    public struct Reaction {
        public Ingredient catalyst;
        public Ingredient result;
    }
}