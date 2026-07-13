using System;
using NaughtyAttributes;
using Runtime.Drink;
using Runtime.Highlighting;
using UnityEngine;
// HideNestedArrayAttribute from https://github.com/dbrizov/NaughtyAttributes/issues/142#issuecomment-1993967793
#if UNITY_EDITOR
using UnityEditor;
#endif
/// <summary>
/// ____DESC:      
/// </summary>
[AttributeUsage(AttributeTargets.Field, Inherited = true)]
public class HideNestedArrayAttribute : PropertyAttribute
{

}
#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(HideNestedArrayAttribute))]
public class HideNestedArrayDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var array = property.FindPropertyRelative("Array");
        EditorGUI.PropertyField(position, array, new GUIContent(property.displayName));
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var array = property.FindPropertyRelative("Array");
        // These numbers are tried out. I don't know how to get it.
        if (array.isExpanded)
        {
            return 70 + Mathf.Clamp((array.arraySize-1) * 20, 0, float.MaxValue);
        }
        else
            return 20;
    }
}
#endif

// Nested Array from https://github.com/dbrizov/NaughtyAttributes/issues/142#issuecomment-1780009091
[System.Serializable]
public class NestedArray<T>
{
    public T[] Array;
}

namespace Runtime.Customers.Tutorial_Agent {
    [Serializable]
    public class TutorialAgentStep {
        [Tooltip("The dialogue that will play when this step starts")]
        public string stepStartedDialogue;

        public float reminderTimer;
        public string reminderDialogue;
        
        [Header("Progress settings")]
        public TutorialProgressType progressType;
        
        public bool ServeDrink => progressType == TutorialProgressType.ServeDrink;
        public bool ClickObject => progressType == TutorialProgressType.ClickObjects;
        
        [ShowIf("ServeDrink"), AllowNesting, HideNestedArray]
        public NestedArray<Recipe> acceptedDrinks;

        [ShowIf("ServeDrink"), AllowNesting]
        public string repeatOrderDialogue;

        [ShowIf("ServeDrink"), AllowNesting]
        public string wrongDrinkDialogue;
        
        [ShowIf("ClickObject"), AllowNesting, HideNestedArray]
        public NestedArray<TutorialHighlightable> objectsToHighlight;
    }
}