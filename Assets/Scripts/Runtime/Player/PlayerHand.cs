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
        
        private ItemPickup _heldItem;

        public void TryGrabItem(Grabbable grabbable) {
            if (grabbable == null) {
                return;
            }
            
            if (grabbable.TryGetComponent(out ItemPickup pickup)) {
                PickUpItem(pickup);
            } else if (grabbable.TryGetComponent(out ItemSource source)) {
                PickUpItem(source.SpawnItem());
            }
        }
        
        public void ReleaseHeldItem(Grabbable grabbable) {
            if (grabbable == null || _heldItem == null) {
                DropItem();
                return;
            }
            
            if (grabbable.TryGetComponent(out ItemDock dock) && dock.CanPlaceItem()) {
                dock.PlaceItem(_heldItem);
            }
            else if (grabbable.TryGetComponent(out Customer customer) && (_heldItem.TryGetComponent(out DrinkObject drink)))
            {
                customer.ServeDrink(drink.currentContents);
            }
            else {
                DropItem();
            }
            
            _heldItem = null;
        }

        private void Awake() {
            PlayerController.OnAddIngredient += TryAddIngredient;
        }

        private void OnDestroy() {
            PlayerController.OnAddIngredient -= TryAddIngredient;
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

        private void DropItem() {
            _heldItem?.Unpin();
            _heldItem = null;
        }

        private void PickUpItem(ItemPickup item) {
            if (_heldItem) return;
            
            _heldItem = item;
            item.Pin(itemPivot);
        }

        private void OnDrawGizmos() {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(transform.position, 0.25f);
        }
    }
}