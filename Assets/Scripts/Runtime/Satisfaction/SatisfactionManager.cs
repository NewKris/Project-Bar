using System;
using Runtime;
using Runtime.Scene_Handling;
using UnityEngine;

namespace Runtime.Satisfaction
{
    public class SatisfactionManager : MonoBehaviour
    {
        public Level currentLevel;
        public SatisfactionPort satisfactionPort;

        public int currentSatisfaction;

        private void OnEnable()
        {
            satisfactionPort.OnSatisfactionChange += HandleSatisfactionChange;
            satisfactionPort.OnSatisfactionSet += SetSatisfaction;
        }
        
        private void OnDisable()
        {
            satisfactionPort.OnSatisfactionChange -= HandleSatisfactionChange;
            satisfactionPort.OnSatisfactionSet -= SetSatisfaction;
        }

        private void Start()
        {
            currentSatisfaction = currentLevel.startSatisfaction;
        }

        private void HandleSatisfactionChange(int value)
        {
            currentSatisfaction = Mathf.Min(currentSatisfaction + value, currentLevel.maximumSatisfaction);

            if (currentSatisfaction >= currentLevel.targetSatisfaction)
            {
                // HANDLE ACTIVATE TARGET ALSO MAKE IT SO THAT TARGET IS REMOVED FROM SPAWN POOL WHEN SATISFACTION GOES BELOW TARGET
            }
            
            if (currentSatisfaction <= 0)
            {
                // HANDLE GAME OVER
            }
        }

        private void SetSatisfaction(int value)
        {
            currentSatisfaction = value;
        }

        public void SetLevel(Level level)
        {
            currentLevel = level;
            Restart();
        }

        public void Restart()
        {
            currentSatisfaction = currentLevel.startSatisfaction;
        }
    }
}
