using Runtime.Audio;
using UnityEngine;
using UnityEngine.Events;

namespace Runtime.Satisfaction
{
    [CreateAssetMenu(fileName = "Satisfaction Port", menuName = "Satisfaction/Satisfaction Port")]
    public class SatisfactionPort : ScriptableObject {
        public int overflowPenalty;
        public int splashPenalty;
        public int dropPenalty;
        public int dirtyContainerPenalty;
        
        public UnityAction<int> OnSatisfactionChange;
        public UnityAction<int> OnSatisfactionSet;

        public void IncreaseSatisfaction(int value)
        {
            OnSatisfactionChange?.Invoke(Mathf.Abs(value));
            SfxManager.PlaySuccess();
        }

        public void DecreaseSatisfaction(int value)
        {
            OnSatisfactionChange?.Invoke(-(Mathf.Abs(value)));
            SfxManager.PlayFailure();
        }

        public void SetSatisfaction(int value)
        {
            OnSatisfactionSet?.Invoke(value);
        }
    }
}