using UnityEngine;

namespace Runtime.Drinks.Pouring {
    public interface IPourReceiver {
        public static bool IsReceiver(Component obj, out IPourReceiver receiver) {
            return IsReceiver(obj.gameObject, out receiver);
        }
        
        public static bool IsReceiver(GameObject obj, out IPourReceiver receiver) {
            receiver = null;
            return obj?.TryGetComponent(out receiver) ?? false;
        }
        
        public void AddContents(DrinkContents contents);
        public void AddContents(IngredientGroup group);
    }
}