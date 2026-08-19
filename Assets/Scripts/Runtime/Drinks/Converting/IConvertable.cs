using UnityEngine;

namespace Runtime.Drinks.Converting {
    public interface IConvertable {
        public static bool IsConvertable(Component obj, out IConvertable convertable) {
            return IsConvertable(obj.gameObject, out convertable);
        }
        
        public static bool IsConvertable(GameObject obj, out IConvertable convertable) {
            convertable = null;
            return obj?.TryGetComponent(out convertable) ?? false;
        }
        
        public void ConvertIngredients(Conversion[]  conversions);
    }
}