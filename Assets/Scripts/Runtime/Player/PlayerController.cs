using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Runtime.Player {
    public class PlayerController : MonoBehaviour {
        public static event Action OnGrab;
        public static event Action OnRelease;
        public static event Action<string> OnAddIngredient;
        
        public static Vector2 DeltaMouse { get; private set; }

        public string[] ingredientKeys;
        
        private InputAction _lookAction;

        private InputActionMap ActionMap => InputSystem.actions.actionMaps[0];
        
        private void Awake() {
            _lookAction = ActionMap["Look"];
            ActionMap["Grab"].performed += _ => OnGrab?.Invoke();
            ActionMap["Grab"].canceled += _ => OnRelease?.Invoke();
            
            foreach (string ingredientKey in ingredientKeys) {
                ActionMap[$"Ingredient {ingredientKey}"].performed += _ => OnAddIngredient?.Invoke(ingredientKey);
            }
            
            ActionMap.Enable();
        }

        private void OnDestroy() {
            ActionMap.Dispose();
        }

        private void Update() {
            DeltaMouse = _lookAction.ReadValue<Vector2>();
        }
    }
}