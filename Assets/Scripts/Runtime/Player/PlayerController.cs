using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Runtime.Player {
    public class PlayerController : MonoBehaviour {
        public static event Action OnGrab;
        public static event Action OnRelease;
        public static event Action OnInteract;
        public static event Action<string> OnAddIngredient;
        public static event Action OnPour;
        
        public static Vector2 DeltaMouse { get; private set; }

        public string[] ingredientKeys;
        
        private InputAction _lookAction;

        private InputActionMap ActionMap => InputSystem.actions.actionMaps[0];
        
        private void Awake() {
            _lookAction = ActionMap["Look"];
            
            ActionMap["Grab"].performed += _ => OnGrab?.Invoke();
            ActionMap["Grab"].canceled += _ => OnRelease?.Invoke();
            ActionMap["Interact"].performed += _ => OnInteract?.Invoke();
            ActionMap["Pour"].performed += _ => OnPour?.Invoke();
            
            // Får problem med att subscribe lambda funktioner i den här loopen
            // där de inte blir disposed och därför skulle få duplicerade anrop.
            // pga. det skapades AddIngredientPerformed() istället för lamda funktion
            foreach (string ingredientKey in ingredientKeys) {
                ActionMap[$"Ingredient {ingredientKey}"].performed += AddIngredientPerformed;
            }
            
            ActionMap.Enable();
        }

        private void OnDestroy() {
            foreach (string ingredientKey in ingredientKeys) {
                ActionMap[$"Ingredient {ingredientKey}"].performed -= AddIngredientPerformed;
            }
            
            ActionMap.Dispose();
        }

        private void Update() {
            DeltaMouse = _lookAction.ReadValue<Vector2>();
        }
        
        private void AddIngredientPerformed(InputAction.CallbackContext context) {
            if (context.performed) {
                OnAddIngredient?.Invoke(context.action.name);
            }
        }
    }
}