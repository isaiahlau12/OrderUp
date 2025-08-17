using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  create RecipeData assets and list them in the RecipeBookManager
[CreateAssetMenu(fileName = "NewRecipe", menuName = "Recipe")]
public class RecipeData : ScriptableObject
{
    public string recipeName;

    // List of tags required for this recipe to trigger 
    public List<CardType> requiredTags = new List<CardType>();

    // Flat satisfaction bonus granted if the recipe matches(can be edited in unity)
    public int bonusSatisfaction = 0;

    public bool requireAllTags = true;
}
