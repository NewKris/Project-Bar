using System;
using Runtime.Customers;
using Runtime.Drink;
using Runtime.Interact;
using Runtime.Items;
using UnityEngine;

namespace Runtime.Player {
    public class PlayerHand : MonoBehaviour {
        private ItemPickup _heldItem;

        public void TryGrabItem(Interaction interaction) {
            if (interaction == null) {
                return;
            }
            
            if (interaction.TryGetComponent(out ItemPickup pickup)) {
                PickUpItem(pickup);
            } else if (interaction.TryGetComponent(out ItemSource source)) {
                PickUpItem(source.SpawnItem());
            }
        }
        
        public void ReleaseHeldItem(Interaction interaction) {
            if (interaction == null || _heldItem == null) {
                DropItem();
                return;
            }
            
            if (interaction.TryGetComponent(out ItemDock dock) && dock.CanPlaceItem()) {
                dock.PlaceItem(_heldItem);
                _heldItem = null;
            }
            else if (interaction.TryGetComponent(out Customer customer) && (_heldItem.TryGetComponent(out DrinkObject drink)))
            {
                customer.ServeDrink(drink.currentContents);
            }
            else {
                DropItem();
            }
        }

        private void Awake() {
            PlayerController.OnAddIngredient += TryAddIngredient;
        }

        private void OnDestroy() {
            PlayerController.OnAddIngredient -= TryAddIngredient;
        }

        private void TryAddIngredient(string ingredientKey) {
            Debug.Log($"Trying to add ingredient {ingredientKey}");
        }

        private void DropItem() {
            _heldItem?.Unpin();
            _heldItem = null;
        }

        private void PickUpItem(ItemPickup item) {
            if (_heldItem) return;
            
            _heldItem = item;
            item.Pin(transform);
        }

        private void OnDrawGizmos() {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(transform.position, 0.25f);
        }
    }
}