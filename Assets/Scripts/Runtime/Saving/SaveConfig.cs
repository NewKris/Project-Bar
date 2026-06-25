using UnityEngine;

namespace Runtime.Saving {
    [CreateAssetMenu(menuName = "Save Config")]
    public class SaveConfig : ScriptableObject {
        public Save save;
    }
}