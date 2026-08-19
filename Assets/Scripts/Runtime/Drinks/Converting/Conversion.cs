using System;
using UnityEngine;

namespace Runtime.Drinks.Converting {
    [Serializable]
    public struct Conversion {
        [HideInInspector] public string name;
        
        public Ingredient from;
        public Ingredient to;

        public void UpdateName() {
            name = $"{from?.name ?? "NULL"} -> {to?.name ?? "NULL"}";
        }
    }
}