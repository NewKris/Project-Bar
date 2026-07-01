using System;
using Runtime.Animations;
using Runtime.Customers;
using Runtime.Drink;
using Runtime.Interact;
using Runtime.Items;
using UnityEngine;

namespace Runtime.Player {
    public class PlayerHand : MonoBehaviour {
        public Transform itemPivot;
        public HandShakeAnimation  handShake;
        
        private ItemPickup _heldItem;

        public bool ShakeDrink { get; set; }
        
        public void TryGrabItem(HandInteraction handInteraction) {
            if (handInteraction == null) {
                return;
            }
            
            if (handInteraction.TryGetComponent(out ItemPickup pickup)) {
                PickUpItem(pickup);
            } else if (handInteraction.TryGetComponent(out ItemSource source)) {
                PickUpItem(source.SpawnItem());
            }
        }
        
        public void ReleaseHeldItem(HandInteraction handInteraction) {
            if (handInteraction == null || _heldItem == null) {
                _heldItem?.Unpin();
                RemoveItemFromHand();
                return;
            }
            
            if (handInteraction.TryGetComponent(out ItemDock dock) && dock.CanPlaceItem()) {
                dock.PlaceItem(_heldItem);
            }
            else if (handInteraction.TryGetComponent(out Customer customer) && (_heldItem.TryGetComponent(out DrinkObject drink)))
            {
                customer.ServeDrink(drink.currentContents);
                _heldItem.Despawn();
                _heldItem = null;
                return;
            }
            else {
                _heldItem?.Unpin();
            }
            
            RemoveItemFromHand();
        }

        public void PourDrink(HandInteraction handInteraction) {
            if (!_heldItem || !_heldItem.TryGetComponent(out DrinkObject heldDrink)) return;

            if (handInteraction?.TryGetComponent(out DrinkObject targetDrink) ?? false) {
                targetDrink.AddContents(heldDrink.currentContents);
            }
            
            heldDrink.EmptyContents();
        }

        private void OnGUI() {
            if (_heldItem?.TryGetComponent(out DrinkObject drink) ?? false) {
                GUILayout.BeginArea(new Rect(10, 10, 500, 500));
                
                GUILayout.Label($"Container: {drink.currentContents.container.name}");
                GUILayout.Label($"Mix: {drink.currentContents.mixType}");
                
                foreach (Ingredient ingredient in drink.currentContents.ingredients) {
                    GUILayout.Label(ingredient.name);
                }
                
                GUILayout.EndArea();
            }
        }

        private void Awake() {
            PlayerController.OnAddIngredient += TryAddIngredient;
        }

        private void OnDestroy() {
            PlayerController.OnAddIngredient -= TryAddIngredient;
        }

        private void Update() {
            TryShakeDrink();
        }

        private void TryAddIngredient(string ingredientAction) {
            if (!_heldItem) return;

            Ingredient ingredient = IngredientList.GetIngredient(ConvertActionToKey(ingredientAction));
            if (ingredient != null && _heldItem.TryGetComponent(out DrinkObject drink)) {
                drink.AddIngredient(ingredient);
            }
        }

        private string ConvertActionToKey(string action) {
            return action[^1].ToString().ToUpper();
        }

        private void RemoveItemFromHand() {
            _heldItem?.SetFrontRender(false);
            _heldItem = null;
        }

        private void PickUpItem(ItemPickup item) {
            if (_heldItem) return;
            
            _heldItem = item;
            _heldItem.SetFrontRender(true);
            item.Pin(itemPivot);
        }

        private void TryShakeDrink() {
            if (ShakeDrink && _heldItem && _heldItem.TryGetComponent(out Shaker shaker)) {
                shaker.TickShake();
                handShake.Shaking = true;
            }
            else {
                handShake.Shaking = false;
            }
        }

        private void OnDrawGizmos() {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(transform.position, 0.25f);
        }
    }
}