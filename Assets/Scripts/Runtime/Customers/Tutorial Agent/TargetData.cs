using System;
using NaughtyAttributes;
using UnityEngine;

namespace Runtime.Customers.Tutorial_Agent {
    [Serializable]
    public class TargetData {
        public CustomerData customerData;
        [Tooltip("The base percentage of target spawning when available")]
        [Range(0f, 100f)] public float baseChance;
        [Tooltip("The percentage units that the target spawn chance will increase by when other customers are spawned while the target is available")]
        [Range(0f, 100f)] public float chanceIncrease;
        [Tooltip("If true, the target spawn chance will reset when target becomes unavailable")]
        public bool shouldSpawnChanceReset = true;

    }
}