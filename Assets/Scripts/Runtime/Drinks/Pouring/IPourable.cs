using UnityEngine;

namespace Runtime.Drinks.Pouring {
    public interface IPourable {
        public static bool IsPourable(Component obj, out IPourable pourable) {
            return IsPourable(obj.gameObject, out pourable);
        }
        
        public static bool IsPourable(GameObject obj, out IPourable pourable) {
            pourable = null;
            return obj?.TryGetComponent(out pourable) ?? false;
        }
        
        bool HasContent { get; }
        void EmptyContents();
        void GiveContent(IPourReceiver receiver);
    }
}