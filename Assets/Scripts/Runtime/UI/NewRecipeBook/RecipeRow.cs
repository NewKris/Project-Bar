using NaughtyAttributes;
using Runtime.Drinks;
using Runtime.Old_Systems.Interact;
using Runtime.Old_Systems.Looking;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.UI.NewRecipeBook {
    [RequireComponent(typeof(BoxCollider), typeof(Interactable))]
    public class RecipeRow : MonoBehaviour {
        public TMP_Text description;
        public Image icon;
        [ReadOnly] public Recipe recipe;

        public void HighlightRecipe() {
            FlashManager.FlashRecipe(recipe);
        }
        
        public void SetInfo(string recipeDescription, Sprite recipeIcon, Recipe targetRecipe) {
            description.text = recipeDescription;
            icon.sprite = recipeIcon;
            recipe = targetRecipe;
        }
    }
}