using NaughtyAttributes;
using UnityEngine;

namespace Runtime.Scene_Handling
{
    [CreateAssetMenu(fileName = "Game Scene", menuName = "Scene Handling/Game Scene", order = 1)]
    public class GameScene : ScriptableObject
    {
        [Scene]
        public string Name;
    }
}