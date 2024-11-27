using UnityEngine;

[System.Serializable]
public struct Ingredient
{
    [field : SerializeField, Tooltip("A component needed in a recipe.")]
    public ComponentData Component {  get; private set; }

    [field: SerializeField, Tooltip("Quantity of this component needed in the recipe.")]
    public int Quantity { get; private set; }
}
