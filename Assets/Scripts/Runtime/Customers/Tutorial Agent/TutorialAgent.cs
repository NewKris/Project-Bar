using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using Runtime.Drinks;
using Runtime.Highlighting;
using Runtime.Old_Systems.Drink;
using Runtime.Old_Systems.Player.Hand;
using UnityEngine;
using UnityEngine.Events;

namespace Runtime.Customers.Tutorial_Agent {
    [RequireComponent(typeof(CustomerBase))]
    [RequireComponent(typeof(TutorialDialogueRunner))]
    [RequireComponent(typeof(Highlightable))]
    [RequireComponent(typeof(HandInteraction))]
    public class TutorialAgent : MonoBehaviour {
        public bool skipTutorial;
        [SerializeField] private string characterName;
        
        [SerializeField] private CustomerEventPort tutorialFinishedPort;
        [SerializeField] private UnlockRecipesEventPort unlockRecipesPort;
        [SerializeField] private Vector3 exitPosition;
        [SerializeField] private TutorialAgentStep[] tutorialSteps;
        [SerializeField] private string tutorialCompletedDialogue;

        [Foldout("Events")] [SerializeField] private UnityEvent onAnyStepStarted;
        [Foldout("Events")] [SerializeField] private UnityEvent onAnyStepCompleted;
        
        private CustomerBase _base;
        private TutorialDialogueRunner _dialogueRunner;
        private Highlightable _agentHighlightable;

        // Needs to start at negative one so that next step function can be called on start
        private int _currentStep = -1;
        private float _timer;
        private float _timeSinceStepChanged;

        private HashSet<Highlightable> _objectsClicked;
        
        // Avoids index out of range exception by using Mathf.Min
        private TutorialAgentStep CurrentStep => tutorialSteps[Mathf.Min(_currentStep, tutorialSteps.Length - 1)];
        private List<Recipe> AcceptableDrinks => CurrentStep.acceptedDrinks.Array.ToList();

        private void OnValidate() {
            if (!gameObject.activeInHierarchy) return;
            if (!_base) Debug.LogError("Customer base is null!", this);
            if (!tutorialFinishedPort) Debug.LogError("Tutorial finished port is missing", this);
        }
        
        private void OnEnable() {
            _base = GetComponent<CustomerBase>();
            _dialogueRunner = GetComponent<TutorialDialogueRunner>();
            _agentHighlightable = GetComponent<Highlightable>();

            _base.onOrder += OnOrder;
            _base.onServeDrink += OnServeDrink;
        }

        private void OnDisable() {
            _base.onOrder += OnOrder;
            _base.onServeDrink += OnServeDrink;
        }

        private void Start() {
            _base.Setup(null, transform.position, exitPosition, tutorialFinishedPort);
            NextStep();
            _dialogueRunner.SetCharacterName(characterName);

            if (skipTutorial) {
                Recipe[] recipesToUnlock = tutorialSteps
                    .SelectMany(x => x.recipesToUnlockAtStart)
                    .Union(tutorialSteps.SelectMany(x => x.recipesToUnlockAtEnd))
                    .ToArray();
                
                unlockRecipesPort.UnlockRecipes(recipesToUnlock);
                FinishTutorial();
            }
        }

        private void NextStep() {
            _dialogueRunner.HideDialogue();
            
            HandleStepEndActions();
            _currentStep += 1;

            if (_currentStep >= tutorialSteps.Length) {
                FinishTutorial();
                return;
            }

            HandleStepStartUpActions();
            

            _timer = CurrentStep.reminderTimer;
            if (CurrentStep.ImitateCustomer) {
                _dialogueRunner.ShowDialogueTimed(CurrentStep.stepStartedDialogue);
            }
            else {
                _dialogueRunner.ShowDialogueNonTimed(CurrentStep.stepStartedDialogue);
            }

            if (CurrentStep.ClickAgent) {
                _agentHighlightable.TutorialHighlight();
            }

            if (CurrentStep.ClickObjects) {
                _objectsClicked = new HashSet<Highlightable>();
                foreach (Highlightable obj in CurrentStep.objectsToHighlight.Array) {
                    obj.TutorialHighlight();
                    obj.onClicked += () => {
                        HandleObjectClicked(obj);
                    };
                }
            }
        }

        private void HandleStepStartUpActions() {
            CurrentStep.onStepStarted?.Invoke();
            onAnyStepStarted?.Invoke();
            if (CurrentStep.recipesToUnlockAtStart.Length > 0) {
                unlockRecipesPort.UnlockRecipes(CurrentStep.recipesToUnlockAtStart);
            }
            
        }

        private void HandleStepEndActions() {
            if (_currentStep >= 0) {
                CurrentStep.onStepCompleted?.Invoke();
                onAnyStepCompleted?.Invoke();
                if (CurrentStep.recipesToUnlockAtEnd.Length > 0) {
                    unlockRecipesPort.UnlockRecipes(CurrentStep.recipesToUnlockAtEnd);
                }
            }
        }

        private void OnOrder() {
            if (_currentStep >= tutorialSteps.Length) return;
            if (CurrentStep == null) return;

            if (CurrentStep.ClickAgent) {
                NextStep();
            } 
            else if (CurrentStep.ImitateCustomer) {
                _dialogueRunner.ShowDialogueTimed(CurrentStep.repeatOrderDialogue);
            }
        }

        private void OnServeDrink(DrinkContents drink) {
            if (CurrentStep == null) return;
            
            if (!CurrentStep.ImitateCustomer && !CurrentStep.ServeDrink) return;

            if (drink.DrinkIsAccepted(AcceptableDrinks))
            {
                NextStep();
            }
            else
            {
                _dialogueRunner.ShowDialogueTimed(CurrentStep.wrongDrinkDialogue);
            }
        }

        private void Update() {
            _timer -= Time.deltaTime;
            _timeSinceStepChanged += Time.deltaTime;

            if (!CurrentStep.ImitateCustomer) return;
            
            if (_timer <= 0 && !_base.isLeaving) {
                _dialogueRunner.ShowDialogueTimed(CurrentStep.reminderDialogue);
                _timer = CurrentStep.reminderTimer;
            }
        }

        private void FinishTutorial() {
            _dialogueRunner.ShowDialogueTimed(tutorialCompletedDialogue);
            _base.LeaveBar();
        }

        private void HandleObjectClicked(Highlightable obj) {
            _objectsClicked.Add(obj);
            if (_objectsClicked.Count >= CurrentStep.objectsToHighlight.Array.Length) {
                NextStep();
            }
        }
    }
}