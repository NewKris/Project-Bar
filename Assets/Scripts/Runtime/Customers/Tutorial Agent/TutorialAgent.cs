using System;
using System.Collections.Generic;
using System.Linq;
using Runtime.Drink;
using Runtime.Highlighting;
using Runtime.Interact;
using UnityEngine;

namespace Runtime.Customers.Tutorial_Agent {
    [RequireComponent(typeof(CustomerBase))]
    [RequireComponent(typeof(TutorialDialogueRunner))]
    public class TutorialAgent : MonoBehaviour {
        [SerializeField] private TutorialAgentStep[] tutorialSteps;
        
        private CustomerBase _base;
        private TutorialDialogueRunner _dialogueRunner;

        // Needs to start at negative one so that next step function can be called on start
        private int _currentStep = -1;
        private float _timer;

        private List<Highlightable> _objectsClicked;
        
        private TutorialAgentStep CurrentStep => tutorialSteps[_currentStep];
        private List<Recipe> AcceptableDrinks => CurrentStep.acceptedDrinks.Array.ToList();
        
        private void OnEnable() {
            _base = GetComponent<CustomerBase>();
            _dialogueRunner = GetComponent<TutorialDialogueRunner>();

            _base.onOrder += OnOrder;
            _base.onServeDrink += OnServeDrink;
        }

        private void OnDisable() {
            _base.onOrder += OnOrder;
            _base.onServeDrink += OnServeDrink;
        }

        private void Start() {
            NextStep();
        }

        private void NextStep() {
            _currentStep += 1;
            _dialogueRunner.ShowDialogue(CurrentStep.stepStartedDialogue);
            _timer = CurrentStep.reminderTimer;

            if (CurrentStep.progressType == TutorialProgressType.ClickObjects) {
                foreach (Highlightable obj in CurrentStep.objectsToHighlight.Array) {
                    obj.Highlight();
                    obj.onClicked += () => {
                        if (!_objectsClicked.Contains(obj)) _objectsClicked.Add(obj);
                    };
                }
            }
        }

        private void OnOrder() {
            if (CurrentStep == null) return;

            if (CurrentStep.progressType == TutorialProgressType.ClickAgent) {
                NextStep();
            }

            if (CurrentStep.progressType == TutorialProgressType.ServeDrink) {
                _dialogueRunner.ShowDialogue(CurrentStep.repeatOrderDialogue);
            }
        }

        private void OnServeDrink(DrinkContents drink) {
            if (CurrentStep == null) return;
            
            if (CurrentStep.progressType != TutorialProgressType.ServeDrink) return;

            if (drink.DrinkIsAccepted(AcceptableDrinks))
            {
                Debug.Log("Drink accepted!");
                NextStep();
            }
            else
            {
                Debug.Log("Drink rejected");
                _dialogueRunner.ShowDialogue(CurrentStep.wrongDrinkDialogue);
            }
        }

        private void Update() {
            _timer -= Time.deltaTime;

            if (_timer <= 0) {
                _dialogueRunner.ShowDialogue(CurrentStep.reminderDialogue);
                _timer = CurrentStep.reminderTimer;
            }
        }
    }
}