using System;
using Runtime.Animations;
using Runtime.Customers;
using Runtime.Drinks;
using Runtime.Drinks.Converting;
using Runtime.Drinks.Pouring;
using Runtime.Highlighting;
using Runtime.Old_Systems.Drink;
using Runtime.Old_Systems.Items;
using Runtime.Player;
using Runtime.Satisfaction;
using UnityEngine;

namespace Runtime.Old_Systems.Player.Hand {
    [Obsolete]
    public class PlayerHand : MonoBehaviour {
        [Header("References")]
        public SatisfactionPort satisfactionPort;
        public Transform itemPivot;
        public HandShakeAnimation  handShake;
        
        public ItemPickup HeldItem { get; private set; }

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
            TryEndShake();
            
            if (handInteraction == null || HeldItem == null) {
                HeldItem?.Unpin();
                RemoveItemFromHand();
                return;
            }

            bool holdingDrink = HeldItem.TryGetComponent(out DrinkObject drink);
            
            if (handInteraction.TryGetComponent(out ItemDock dock) && dock.CanPlaceItem(HeldItem)) {
                dock.PlaceItem(HeldItem);
            }
            else if (handInteraction.TryGetComponent(out MultiDock passiveStation) && passiveStation.CanPlaceItem()) {
                passiveStation.PlaceItem(HeldItem);
            }
            else if (handInteraction.TryGetComponent(out CustomerBase customer) && holdingDrink)
            {
                customer.ServeDrink(drink.currentContents);
                HeldItem.Despawn();
                HeldItem = null;
                return;
            }
            else if (handInteraction.TryGetComponent(out ItemSource source) && HeldItem.TryGetComponent(out ItemPickup pickup) && pickup.source == source) {
                if (holdingDrink && drink.isDirty) {
                    satisfactionPort.DecreaseSatisfaction(satisfactionPort.dirtyContainerPenalty);
                }
                
                HeldItem.Despawn();
                HeldItem = null;
                return;
            } else if (handInteraction.TryGetComponent(out Bin _)) {
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
            if (!HeldItem) return;

            if (IPourable.IsPourable(HeldItem, out IPourable pourable)) {
                TryPour(pourable, handInteraction);
            }

            if (IConverter.IsConverter(HeldItem, out IConverter converter)) {
                TryConvert(converter, handInteraction);
            }
        }

        private void TryConvert(IConverter converter, HandInteraction handInteraction) {
            if (IConvertable.IsConvertable(handInteraction, out IConvertable convertable)) {
                converter.Convert(convertable);
            }
        }

        private void TryPour(IPourable pourable, HandInteraction interaction) {
            if (IPourReceiver.IsReceiver(interaction, out IPourReceiver receiver)) {
                pourable.GiveContent(receiver);
            } else if (pourable.HasContent) {
                satisfactionPort.DecreaseSatisfaction(satisfactionPort.splashPenalty);
            }
            
            pourable.EmptyContents();
        }

        private void Awake() {
            PlayerController.OnAddIngredient += TryAddIngredient;
        }

        private void OnDestroy() {
            PlayerController.OnAddIngredient -= TryAddIngredient;
        }

        private void TryAddIngredient(string ingredientAction) {
            if (!HeldItem) return;

            Ingredient ingredient = IngredientList.GetIngredient(ConvertActionToKey(ingredientAction));
            if (ingredient != null && HeldItem.TryGetComponent(out DrinkObject drink)) {
                drink.AddIngredient(ingredient);
            }
        }

        public void TryBeginShake() {
            if (HeldItem?.TryGetComponent(out Shaker shaker) ?? false) {
                shaker.enabled = true;
                handShake.Shaking = true;
            }
        }

        public void TryEndShake() {
            if (HeldItem?.TryGetComponent(out Shaker shaker) ?? false) {
                shaker.enabled = false;
                handShake.Shaking = false;
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

        private void OnDrawGizmos() {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(transform.position, 0.1f);
        }
    }
}