using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.UI.NewRecipeBook {
    public class RecipeRow : MonoBehaviour {
        public TMP_Text description;
        public Image icon;

        public void SetInfo(string recipeDescription, Sprite recipeIcon) {
            description.text = recipeDescription;
            icon.sprite = recipeIcon;
        }
    }
}