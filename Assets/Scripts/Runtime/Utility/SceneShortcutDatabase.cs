using System;
using NaughtyAttributes;
using UnityEngine;

namespace Runtime.Utility {
    [CreateAssetMenu(menuName = "Scene Shortcuts")]
    public class SceneShortcutDatabase : ScriptableObject {
        public Shortcut[] shortcuts;
    }
    
    [Serializable]
    public struct Shortcut {
        public string buttonText;
        [Scene] public string[] scenes;
    }
}