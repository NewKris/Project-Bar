using UnityEngine;

namespace Runtime.Drinks.Converting {
    public interface IConverter {
        public static bool IsConverter(Component obj, out IConverter converter) {
            return IsConverter(obj.gameObject, out converter);
        }
        
        public static bool IsConverter(GameObject obj, out IConverter converter) {
            converter = null;
            return obj?.TryGetComponent(out converter) ?? false;
        }
        
        public void Convert(IConvertable convertable);
    }
}