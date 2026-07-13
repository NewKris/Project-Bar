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
    [RequireComponent(typeof(TutorialHighlightable))]
    public class TutorialAgent : MonoBehaviour {
        [SerializeField] private CustomerEventPort tutorialFinishedPort;
        [SerializeField] private Vector3 exitPosition;
        [SerializeField] private TutorialAgentStep[] tutorialSteps;
        [SerializeField] private string tutorialCompletedDialogue;
        
        private CustomerBase _base;
        private TutorialDialogueRunner _dialogueRunner;
        private TutorialHighlightable _agentHighlightable;

        // Needs to start at negative one so that next step function can be called on start
        private int _currentStep = -1;
        private float _timer;

        private HashSet<TutorialHighlightable> _objectsClicked;
        
        // Avoids index out of range exception by using Mathf.Min
        private TutorialAgentStep CurrentStep => tutorialSteps[Mathf.Min(_currentStep, tutorialSteps.Length - 1)];
        private List<Recipe> AcceptableDrinks => CurrentStep.acceptedDrinks.Array.ToList();
        
        private void OnEnable() {
            _base = GetComponent<CustomerBase>();
            _dialogueRunner = GetComponent<TutorialDialogueRunner>();
            _agentHighlightable = GetComponent<TutorialHighlightable>();

            _base.onOrder += OnOrder;
            _base.onServeDrink += OnServeDrink;
        }

        private void OnDisable() {
            _base.onOrder += OnOrder;
            _base.onServeDrink += OnServeDrink;
        }

        private void Start() {
            if (!_base) Debug.LogError("Customer base is null!");
            if (!tutorialFinishedPort) Debug.LogError("Tutorial finished port");
            
            _base.Setup(null, transform.position, exitPosition, tutorialFinishedPort);
            NextStep();
        }

        private void NextStep() {
            _currentStep += 1;

            if (_currentStep >= tutorialSteps.Length) {
                FinishTutorial();
                return;
            }
            
            _dialogueRunner.ShowDialogue(CurrentStep.stepStartedDialogue);
            _timer = CurrentStep.reminderTimer;

            if (CurrentStep.progressType == TutorialProgressType.ClickAgent) {
                _agentHighlightable.Highlight();
            }

            if (CurrentStep.progressType == TutorialProgressType.ClickObjects) {
                foreach (TutorialHighlightable obj in CurrentStep.objectsToHighlight.Array) {
                    obj.Highlight();
                    obj.onClicked += () => {
                        HandleObjectClicked(obj);
                    };
                }
            }
        }

        private void OnOrder() {
            if (_currentStep >= tutorialSteps.Length) return;
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

        private void FinishTutorial() {
            _dialogueRunner.ShowDialogue(tutorialCompletedDialogue);
            _base.LeaveBar();
        }

        private void HandleObjectClicked(TutorialHighlightable obj) {
            _objectsClicked.Add(obj);
            if (_objectsClicked.Count >= CurrentStep.objectsToHighlight.Array.Length) {
                NextStep();
            }
        }
    }
}