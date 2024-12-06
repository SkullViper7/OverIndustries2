using System.Collections.Concurrent;
using UnityEngine;
using System.Collections.Generic;

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
    public ComponentData ComponentData { get; private set; }

    /// <summary>
    /// Quantity of this component needed in the recipe.
    /// </summary>
    [field: SerializeField, Tooltip("Quantity of this component needed in the recipe.")]
    public int Quantity { get; private set; }
}

/// <summary>
/// A structure to stock a research cost for an objet.
/// </summary>
[System.Serializable]
public struct ObjectResearchCost
{
    /// <summary>
    /// Quantity of raw material needed for the research.
    /// </summary>
    [field: SerializeField, Tooltip("Quantity of raw material needed for the research.")]
    public int RawMaterialCost { get; private set; }

    /// <summary>
    /// A list of ingredients needed for the research.
    /// </summary>
    [field: SerializeField, Tooltip("A list of ingredients needed for the research.")]
    public List<Ingredient> IngredientsCost { get; private set; }
}

/// <summary>
/// A pool of objects.
/// </summary>
public struct ObjectPool
{
    /// <summary>
    /// Concurrent bag which stocks objects.
    /// </summary>
    public ConcurrentBag<GameObject> Pool { get; private set; }

    /// <summary>
    /// Prefab of the object stocked.
    /// </summary>
    public GameObject ObjectPrefab { get; private set; }

    // Explicit constructor
    public ObjectPool(ConcurrentBag<GameObject> pool, GameObject objectPrefab)
    {
        Pool = pool;
        ObjectPrefab = objectPrefab;
    }
}