using Runtime.Old_Systems.Interact;
using UnityEngine;
using UnityEngine.Events;

namespace Runtime.UI.NewRecipeBook {
    [RequireComponent(typeof(BoxCollider), typeof(Interactable))]
    public class RecipeButton : MonoBehaviour {
        public UnityEvent onPressed;

        public void Press() {
            onPressed.Invoke();
        }
    }
}