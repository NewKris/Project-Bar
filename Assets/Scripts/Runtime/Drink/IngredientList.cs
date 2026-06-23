using System;
using NaughtyAttributes;
using UnityEngine;

namespace Runtime.Drink {
    public class IngredientList : MonoBehaviour {
        private static IngredientList Instance;

        [Foldout("Prep")] public Ingredient ingredient1;
        [Foldout("Prep")] public Ingredient ingredient2;
        [Foldout("Prep")] public Ingredient ingredient3;

        [Foldout("Liquids")] public Ingredient ingredientQ;
        [Foldout("Liquids")] public Ingredient ingredientW;
        [Foldout("Liquids")] public Ingredient ingredientE;
        [Foldout("Liquids")] public Ingredient ingredientR;
        [Foldout("Liquids")]public Ingredient ingredientA;
        [Foldout("Liquids")]public Ingredient ingredientS;
        [Foldout("Liquids")]public Ingredient ingredientD;
        [Foldout("Liquids")]public Ingredient ingredientF;

        [Foldout("Garnish")] public Ingredient ingredientZ;
        [Foldout("Garnish")] public Ingredient ingredientX;
        [Foldout("Garnish")] public Ingredient ingredientC;
        
        public static Ingredient GetIngredient(string key) {
            return Instance.KeyToIngredient(key.Trim());
        }

        private Ingredient KeyToIngredient(string key) {
            return key switch {
                "1" => ingredient1,
                "2" => ingredient2,
                "3" => ingredient3,
                "Q" => ingredientQ,
                "W" => ingredientW,
                "E" => ingredientE,
                "R" => ingredientR,
                "A" => ingredientA,
                "S" => ingredientS,
                "D" => ingredientD,
                "F" => ingredientF,
                "Z" => ingredientZ,
                "X" => ingredientX,
                "C" => ingredientC,
                _ => throw new ArgumentOutOfRangeException(nameof(key), key, null)
            };
        }

        private void Start() {
            Instance = this;
        }
    }
}