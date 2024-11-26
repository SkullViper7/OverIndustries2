using UnityEngine;

/// <summary>
/// A structure to stock a component and a quantity.
/// </summary>
[System.Serializable]
public struct Ingredient
{
    /// <summary>
    /// A component needed in a recipe.
    /// </summary>
    [field: SerializeField, Tooltip("A component needed in a recipe.")]
    public ComponentType Component { get; private set; }

    /// <summary>
    /// Quantity of this component needed in the recipe.
    /// </summary>
    [field: SerializeField, Tooltip("Quantity of this component needed in the recipe.")]
    public int Quantity { get; private set; }
}