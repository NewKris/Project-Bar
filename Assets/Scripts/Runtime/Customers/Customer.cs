using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using Runtime.Dialogue;
using Runtime.Drink;
using Runtime.Satisfaction;
using UnityEngine;
using UnityEngine.Serialization;

namespace Runtime.Customers
{
    [RequireComponent(typeof(CustomerMovement))] [RequireComponent(typeof(CustomerDialogue))]
    public class Customer : MonoBehaviour
    {
        [Tooltip("The scriptable object satisfaction port")]
        [SerializeField] private SatisfactionPort satisfactionPort;
        
        [Tooltip("Is used to know whether customer is the target")]
        [SerializeField] private bool isTarget;
        
        [Tooltip("The possible drinks that when served to the customer will result in a successful order")]
        [SerializeField] private List<Recipe> acceptableDrinks;
        
        
        [Foldout("Timers")]
        [Tooltip("The time it takes before the customer leaves")]
        [SerializeField] private float patienceTimer;
        
        [Foldout("Timers")]
        [Tooltip("The time that will be removed from the patience timer when order is repeated")]
        [SerializeField] [Min(0)] private float timePenaltyRepeatOrder;
        
        [Foldout("Timers")]
        [Tooltip("The time it takes before the customer asks the player to hurry up")]
        [SerializeField] private float patienceTickTime;
        
        
        [Foldout("Satisfaction settings")]
        [Tooltip("The amount of satisfaction that the player will gain on a successful order")]
        [SerializeField] [Min(0)] private int satisfactionSuccess;
        
        [Foldout("Satisfaction settings")]
        [Tooltip("The amount of satisfaction that the player will lose on if they fail the order")]
        [SerializeField] [Min(0)] private int satisfactionFailure;
        
        [Foldout("Satisfaction settings")]
        [Tooltip("The amount of satisfaction that the player will lose if the customer isn't served")]
        [SerializeField] [Min(0)] private int satisfactionMissedOrder;
        
        [Foldout("Satisfaction settings")]
        [Tooltip("The amount of satisfaction that the player will lose if they repeat the customers order")]
        [SerializeField] [Min(0)] private int satisfactionRepeatOrder;

        private CustomerMovement _customerMovement;
        private CustomerDialogue _customerDialogue;
        
        private bool _hasOrdered;
        private float _patienceTimer;
        private bool _isLeaving;
        private bool _patienceTickDialogueHasTriggered;
        

        private void Start()
        {
            _customerMovement = GetComponent<CustomerMovement>();
            _customerDialogue = GetComponent<CustomerDialogue>();
            
            _customerDialogue.SetName();
            _patienceTimer = patienceTimer;
            EnterBar();
        }

        private void Update()
        {
            _patienceTimer -= Time.deltaTime;

            if (_patienceTimer <= patienceTimer - patienceTickTime && !_patienceTickDialogueHasTriggered)
            {
                _customerDialogue.PatienceTick();
                _patienceTickDialogueHasTriggered = true;
            }

            if (_patienceTimer <= 0 && !_isLeaving)
            {
                _customerDialogue.PatienceTimeOut();
                satisfactionPort.DecreaseSatisfaction(satisfactionMissedOrder);
                LeaveBar();
            }
        }

        public void ServeDrink(DrinkContents drink)
        {
            // Compare contents with accepted drinks
            Debug.Log("Serving 💅");

            if (IsDrinkAccepted(drink))
            {
                Debug.Log("Drink accepted!");
                _customerDialogue.Success();
                satisfactionPort.IncreaseSatisfaction(satisfactionSuccess);
            }
            else
            {
                Debug.Log("Drink rejected");
                _customerDialogue.Failure();
                satisfactionPort.DecreaseSatisfaction(satisfactionFailure);
            }
            
            LeaveBar();
        }

        private bool IsDrinkAccepted(DrinkContents drink)
        {
            List<Recipe> possibleRecipes = acceptableDrinks;
            
            possibleRecipes = CheckForMatchingContainer(drink.container, possibleRecipes);
            if (possibleRecipes.Count == 0)
            {
                Debug.Log("Drink mismatch: Container");
                return false;
            }
            
            possibleRecipes = CheckForMatchingMixType(drink.mixType, possibleRecipes);
            if (possibleRecipes.Count == 0)
            {
                Debug.Log("Drink mismatch: MixType");
                return false;
            }
            
            possibleRecipes = CheckForMatchingIngredients(drink.ingredients, possibleRecipes);
            if (possibleRecipes.Count == 0)
            {
                Debug.Log("Drink mismatch: Ingredients");
                return false;
            }

            return CheckForCorrectOrderOfIngredients(drink.ingredients);
        }

        private List<Recipe> CheckForMatchingContainer(DrinkContainer container, List<Recipe> recipes)
        {
            List<Recipe> possibleRecipes = new List<Recipe>();

            foreach (Recipe recipe in recipes)
            {
                if (recipe.contents.container == container) possibleRecipes.Add(recipe);
            }

            return possibleRecipes;
        }

        private List<Recipe> CheckForMatchingMixType(MixType mixType, List<Recipe> recipes)
        {
            List<Recipe> possibleRecipes = new List<Recipe>();

            foreach (Recipe recipe in recipes)
            {
                if (recipe.contents.mixType == mixType) possibleRecipes.Add(recipe);
            }

            return possibleRecipes;
        }

        private List<Recipe> CheckForMatchingIngredients(List<Ingredient> ingredients, List<Recipe> recipes)
        {
            List<Recipe> possibleRecipes = new List<Recipe>();

            foreach (Recipe recipe in recipes)
            {
                bool isPossible = true;
                foreach (Ingredient ingredient in ingredients)
                {
                    if (!recipe.contents.ingredients.Contains(ingredient))
                    {
                        isPossible = false;
                        break;
                    }
                }
                
                if (isPossible) possibleRecipes.Add(recipe);
            }
            
            return possibleRecipes;
        }

        private bool CheckForCorrectOrderOfIngredients(List<Ingredient> ingredients)
        {
            IngredientType typeOfPreviousIngredient = ingredients[0].type;

            for (int i = 1; i < ingredients.Count; i++)
            {
                if (ingredients[i].type < typeOfPreviousIngredient) return false;
            }

            return true;
        }

        public void Order()
        {
            if (_customerDialogue.IsSpeaking) return;
            if (_isLeaving) return;

            if (!_hasOrdered)
            {
                _customerDialogue.Order();
                _hasOrdered = true;
            }
            else
            {
                _customerDialogue.RepeatOrder();
                satisfactionPort.DecreaseSatisfaction(satisfactionRepeatOrder);
                _patienceTimer -= timePenaltyRepeatOrder;
            }
        }

        private void EnterBar()
        {
            _customerDialogue.Attention();
            _customerMovement.EnterBar();
        }

        private void LeaveBar()
        {
            if (_isLeaving) return;
            _customerMovement.ExitBar();
            _isLeaving = true;
        }
    }
}
