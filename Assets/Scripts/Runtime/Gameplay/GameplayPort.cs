using UnityEngine;
using UnityEngine.Events;

namespace Runtime.Gameplay {
    [CreateAssetMenu(menuName = "Gameplay/Gameplay Port")]
    public class GameplayPort : ScriptableObject {
        public UnityAction OnGameplayOver;
        public UnityAction OnGameplayStart;

        public void StartGameplay() {
            OnGameplayStart?.Invoke();
        }
        
        public void EndGameplay() {
            OnGameplayOver?.Invoke();
        }
    }
}