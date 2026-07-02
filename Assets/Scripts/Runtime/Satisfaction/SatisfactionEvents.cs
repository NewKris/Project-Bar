using UnityEngine;
using UnityEngine.Events;

namespace Runtime.Satisfaction
{
    [CreateAssetMenu(fileName = "Satisfaction Events", menuName = "Satisfaction/Satisfaction Events", order = 0)]
    public class SatisfactionEvents : ScriptableObject
    {
        public UnityAction<int> OnCustomerSlotUnlocked;
        public UnityAction<int> OnCustomerSlotLocked;
        /// <summary>
        /// The <c>OnToggleTarget</c> event should send true when the target should be added to the pool and false when it should be removed from the customer pool.
        /// </summary>
        public UnityAction<bool> OnToggleTarget;
        public UnityAction OnGameOver;

        public void ChangeCustomerSlotState(int slot, bool lockSlot)
        {
            if (lockSlot)
            {
                OnCustomerSlotLocked.Invoke(slot);
            }
            else
            {
                OnCustomerSlotUnlocked.Invoke(slot);
            }
        }

        /// <summary>
        /// <c>ToggleTarget</c> is used to invoke the event used to handle target availability
        /// </summary>
        /// <param name="state"> When state is true the target will be set to be available </param>
        public void ToggleTarget(bool state)
        {
            OnToggleTarget.Invoke(state);
        }

        public void GameOver()
        {
            OnGameOver.Invoke();
        }
    }
}