using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Holds a library of recipe as of now 1
    public class RecipeBookManager : MonoBehaviour 
    {
        public List<RecipeData> recipes = new List<RecipeData>();

    public int GetTotalRecipeBonus(List<GameObject> plateCards)
    {
        // Quick exit if no recipes or no plate cards
        if (recipes == null || recipes.Count == 0 || plateCards == null || plateCards.Count == 0) return 0;

        // Aggregate all tags/cardtypes on he plate 
        var plateTags = new List<CardType>();
        foreach (var obj in plateCards)
        {
            var disp = obj.GetComponent<CardDisplay>();
            if (disp != null && disp.cardData != null)
                plateTags.AddRange(disp.cardData.tags);
        }

        int totalBonus = 0;
        foreach (var r in recipes)
        {
            if (MatchesRecipe(plateTags, r))
                totalBonus += r.bonusSatisfaction;
        }

        return totalBonus;
    }

    // Checks whether the plate's tags match the recipes one 

    private bool MatchesRecipe(List<CardType> plateTags, RecipeData recipe)
    {
        if (recipe == null || recipe.requiredTags == null || recipe.requiredTags.Count == 0) return false;

        if (recipe.requireAllTags)
        {
            // Every required tag must exist at least once in plateTags
            return recipe.requiredTags.All(t => plateTags.Contains(t));
        }
        else
        {
            return recipe.requiredTags.Any(t => plateTags.Contains(t));
        }
    }
}

