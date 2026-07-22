using Runtime.Animations;
using Runtime.Customers;
using Runtime.Drink;
using Runtime.Highlighting;
using Runtime.Items;
using Runtime.Satisfaction;
using UnityEngine;

namespace Runtime.Player.Hand {
    public class PlayerHand : MonoBehaviour {
        public int pourPenalty;
        
        [Header("References")]
        public SatisfactionPort satisfactionPort;
        public Transform itemPivot;
        public HandShakeAnimation  handShake;
        
        public ItemPickup HeldItem { get; private set; }

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
            
            if (handInteraction.TryGetComponent(out Highlightable highlightable)) {
                highlightable.Click();
            }
        }
        
        public void ReleaseHeldItem(HandInteraction handInteraction) {
            if (handInteraction == null || HeldItem == null) {
                HeldItem?.Unpin();
                RemoveItemFromHand();
                return;
            }
            
            if (handInteraction.TryGetComponent(out ItemDock dock) && dock.CanPlaceItem()) {
                dock.PlaceItem(HeldItem);
            }
            else if (handInteraction.TryGetComponent(out MultiDock passiveStation) && passiveStation.CanPlaceItem()) {
                passiveStation.PlaceItem(HeldItem);
            }
            else if (handInteraction.TryGetComponent(out CustomerBase customer) && (HeldItem.TryGetComponent(out DrinkObject drink)))
            {
                customer.ServeDrink(drink.currentContents);
                HeldItem.Despawn();
                HeldItem = null;
                return;
            }
            else {
                HeldItem?.Unpin();
            }
            
            RemoveItemFromHand();
        }

        public void PourDrink(HandInteraction handInteraction) {
            if (!HeldItem || !HeldItem.TryGetComponent(out DrinkObject heldDrink)) return;

            if (handInteraction?.TryGetComponent(out DrinkObject targetDrink) ?? false) {
                targetDrink.AddContents(heldDrink.currentContents);
            } else if (!InteractionIsSink(handInteraction) && heldDrink.HasContents) {
                satisfactionPort.DecreaseSatisfaction(pourPenalty);
            }
            
            heldDrink.EmptyContents();
        }

        private bool InteractionIsSink(HandInteraction interaction) {
            return interaction?.TryGetComponent(out Sink _) ?? false;
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
            if (!HeldItem) return;

            Ingredient ingredient = IngredientList.GetIngredient(ConvertActionToKey(ingredientAction));
            if (ingredient != null && HeldItem.TryGetComponent(out DrinkObject drink)) {
                drink.AddIngredient(ingredient);
            }
        }

        private string ConvertActionToKey(string action) {
            return action[^1].ToString().ToUpper();
        }

        private void RemoveItemFromHand() {
            HeldItem?.SetFrontRender(false);
            HeldItem?.SetInteractable(true);
            HeldItem = null;
        }

        private void PickUpItem(ItemPickup item) {
            if (HeldItem) return;
            
            HeldItem = item;
            HeldItem.SetFrontRender(true);
            HeldItem.SetInteractable(false);
            HeldItem.Pin(itemPivot);
            HeldItem.PlayPickupSound();
            
        }

        private void TryShakeDrink() {
            if (ShakeDrink && HeldItem && HeldItem.TryGetComponent(out Shaker shaker)) {
                shaker.TickShake();
                handShake.Shaking = true;
            }
            else {
                handShake.Shaking = false;
            }
        }

        private void OnDrawGizmos() {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(transform.position, 0.1f);
        }
    }
}