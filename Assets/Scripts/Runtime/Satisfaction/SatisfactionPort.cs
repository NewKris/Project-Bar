using UnityEngine;
using UnityEngine.Events;

namespace Runtime.Satisfaction
{
    [CreateAssetMenu(fileName = "Satisfaction Port", menuName = "Event Ports/Satisfaction Port")]
    public class SatisfactionPort : ScriptableObject
    {
        public UnityAction<int> OnSatisfactionChange;
        public UnityAction<int> OnSatisfactionSet;

        public void IncreaseSatisfaction(int value)
        {
            OnSatisfactionChange?.Invoke(Mathf.Abs(value));
        }

        public void DecreaseSatisfaction(int value)
        {
            OnSatisfactionChange?.Invoke(-Mathf.Abs(value));
        }

        public void SetSatisfaction(int value)
        {
            OnSatisfactionSet?.Invoke(value);
        }
    }
}