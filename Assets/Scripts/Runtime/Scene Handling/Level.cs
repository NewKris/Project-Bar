using UnityEngine;

namespace Runtime.Scene_Handling
{
    [CreateAssetMenu(fileName = "Level", menuName = "Scene Handling/Level", order = 2)]
    public class Level : GameScene
    {
        public int startSatisfaction;
        [Tooltip("The amount of satisfaction needed to unlock each customer (array length determines amount customer slots)")]
        public int[] customerUnlocks;
        [Tooltip("The amount of satisfaction needed for the target to be able to spawn")]
        public int targetSatisfaction;
        public int maximumSatisfaction;
    }
}