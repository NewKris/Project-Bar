using UnityEngine;

namespace Runtime.Drink {
    [CreateAssetMenu(menuName = "Drink/Recipe")]
    public class Recipe : ScriptableObject {
        public DrinkContents contents;

        [Header("UI")] 
        public Sprite icon;
        public string customDisplayName;
        [TextArea] public string description;
        
        public string DisplayName => string.IsNullOrEmpty(customDisplayName) ? name : customDisplayName;
    }
}