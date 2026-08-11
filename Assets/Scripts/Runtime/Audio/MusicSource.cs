using System;
using FMOD.Studio;
using FMODUnity;
using NaughtyAttributes;
using Runtime.Gameplay;
using Runtime.Satisfaction;
using UnityEngine;

namespace Runtime.Audio {
    public class MusicSource : MonoBehaviour {
        public EventReference music;

        [Header("Satisfaction")]
        
        [Required]
        public SatisfactionPort satisfactionPort;
        
        [Required]
        public GameplayPort gameplayPort;
        
        public string parameterName;
        
        [ValidateInput(nameof(HasValidAmountsOfLabels), "Invalid amount of labels!")]
        public string[] labels;
        
        [ValidateInput(nameof(HasValidAmountsOfThresholds), "Invalid amount of thresholds!")]
        public int[] thresholds;

        public string endLabel;

        private bool _inEpilogue;

        private void OnDestroy() {
            satisfactionPort.OnSatisfactionChange -= UpdateSatisfactionParameter;
            satisfactionPort.OnSatisfactionSet -= UpdateSatisfactionParameter;
            gameplayPort.OnGameplayOver -= SetEpilogueLabel;
        }

        private void Start() {
            satisfactionPort.OnSatisfactionChange += UpdateSatisfactionParameter;
            satisfactionPort.OnSatisfactionSet += UpdateSatisfactionParameter;
            gameplayPort.OnGameplayOver += SetEpilogueLabel;
            
            MusicManager.PlayMusic(music);
            //MusicManager.SetParameter("MainMenu", "ToTutorial");
            UpdateSatisfactionParameter(0);
        }

        private void SetEpilogueLabel() {
            MusicManager.SetParameter(parameterName, endLabel);
            _inEpilogue = true;
        }
        
        private void UpdateSatisfactionParameter(int currentSatisfaction) {
            if (_inEpilogue) return;
            
            string label = labels[0];
            
            for (int i = 0; i < labels.Length; i++) {
                if (i >= thresholds.Length || currentSatisfaction < thresholds[i]) break;

                label = labels[i];
            }
            
            MusicManager.SetParameter(parameterName, label);
        }

        private int CalculateCorrectAmountOfThresholds(int labelCount) {
            return labelCount - 1;
        }

        private bool HasValidAmountsOfLabels(string[] currentLabels) {
            return currentLabels is { Length: > 0 };
        }

        private bool HasValidAmountsOfThresholds(int[] currentThresholds) {
            return currentThresholds.Length == CalculateCorrectAmountOfThresholds(labels.Length);
        }

        private bool HasValue(string currentName) {
            return !string.IsNullOrEmpty(currentName);
        }
    }
}