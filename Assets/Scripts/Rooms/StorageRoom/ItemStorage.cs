using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemStorage : MonoBehaviour
{
    // Singleton
    private static ItemStorage _instance = null;
    public static ItemStorage Instance => _instance;

    /// <summary>
    /// Capacity of the storage;
    /// </summary>
    [SerializeField]
    private int _storageCapacity;

    /// <summary>
    /// Gets the capacity of the storage.
    /// </summary>
    public int StorageCapacity { get { return _storageCapacity; } private set { } }

    /// <summary>
    /// Dictionnary wich stocks components.
    /// </summary>
    public Dictionary<ComponentData, int> ComponentStorage { get; private set; } = new();

    /// <summary>
    /// Dictionnary wich stocks objects.
    /// </summary>
    public Dictionary<ObjectData, int> ObjectStorage { get; private set; } = new();

    /// <summary>
    /// Current amount of items in the storage.
    /// </summary>
    [SerializeField]
    private int _currentStorage;

    /// <summary>
    /// Gets the amount of items in storage.
    /// </summary>
    public int CurrentStorage { get { return _currentStorage; } private set { } }

    /// <summary>
    /// Events to indicate changes about the storage.
    /// </summary>
    public delegate void ItemsDelegate();
    public event ItemsDelegate ItemStorageHasChanged;
    public event Action<int> AmountHasChanged, CapacityHasChanged;

    /// <summary>
    /// Event for update the quest advancement 
    /// </summary>
    public event Action<ObjectType, int> StorageObjectChanged;
    public event Action<ComponentType, int> StorageComponentChanged;

    private void Awake()
    {
        // Singleton
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            _instance = this;
        }
    }

    /// <summary>
    /// Called to add components in storage.
    /// </summary>
    /// <param name="component"> Type of the component. </param>
    /// <param name="amount"> Amount of components to add. </param>
    public void AddComponents(ComponentData component, int amount)
    {
        if (_currentStorage + amount <= _storageCapacity)
        {
            if (ComponentStorage.ContainsKey(component))
            {
                ComponentStorage[component] += amount;
            }
            else
            {
                ComponentStorage.Add(component, amount);
            }

            // Update storage
            _currentStorage += amount;

            ItemStorageHasChanged?.Invoke();
            AmountHasChanged?.Invoke(_currentStorage);
            StorageComponentChanged?.Invoke(component.ComponentType, _currentStorage);
        }
    }

    /// <summary>
    /// Called to substract an amount of component in the storage.
    /// </summary>
    /// <param name="component"> Type of the component. </param>
    /// <param name="amountToSubstract"> Amount to substract. </param>
    public void SubstractComponents(ComponentData component, int amountToSubstract)
    {
        if (ComponentStorage.ContainsKey(component))
        {
            if (ComponentStorage[component] - amountToSubstract > 0)
            {
                ComponentStorage[component] -= amountToSubstract;

                // Update storage
                _currentStorage -= amountToSubstract;

                ItemStorageHasChanged?.Invoke();
                AmountHasChanged?.Invoke(_currentStorage);
                StorageComponentChanged?.Invoke(component.ComponentType, _currentStorage);
            }
            else
            {
                ComponentStorage.Remove(component);

                _currentStorage -= amountToSubstract;
                AmountHasChanged?.Invoke(_currentStorage);
            }
        }
    }

    /// <summary>
    /// Called to add an amount of component in the storage from a recipe.
    /// </summary>
    /// <param name="recipe"> The recipe to add in the storage. </param>
    public void AddRecipe(List<Ingredient> recipe)
    {
        for (int i = 0; i < recipe.Count; i++)
        {
            AddComponents(recipe[i].ComponentData, recipe[i].Quantity);
        }
    }

    /// <summary>
    /// Called to substract an amount of component in the storage from a recipe.
    /// </summary>
    /// <param name="recipe"> The recipe to remove from the storage. </param>
    public void SubstractRecipe(List<Ingredient> recipe)
    {
        for (int i = 0; i < recipe.Count; i++)
        {
            SubstractComponents(recipe[i].ComponentData, recipe[i].Quantity);
        }
    }

    /// <summary>
    /// Called to add objects in storage.
    /// </summary>
    /// <param name="objectType"> Type of the object. </param>
    /// <param name="amount"> Amount of objects to add. </param>
    public void AddObjects(ObjectData objectType, int amount)
    {
        if (_currentStorage + amount <= _storageCapacity)
        {
            if (ObjectStorage.ContainsKey(objectType))
            {
                ObjectStorage[objectType] += amount;
            }
            else
            {
                ObjectStorage.Add(objectType, amount);
            }

            // Update storage
            _currentStorage += amount;

            ItemStorageHasChanged?.Invoke();
            AmountHasChanged?.Invoke(_currentStorage);
            StorageObjectChanged?.Invoke(objectType.ObjectType, _currentStorage);
        }
    }

    /// <summary>
    /// Called to substract an amount of object in the storage.
    /// </summary>
    /// <param name="objectType"> Type of the object. </param>
    /// <param name="amountToSubstract"> Amount to substract. </param>
    public void SubstractObjects(ObjectData objectType, int amountToSubstract)
    {
        if (ObjectStorage.ContainsKey(objectType))
        {
            if (ObjectStorage[objectType] - amountToSubstract > 0)
            {
                ObjectStorage[objectType] -= amountToSubstract;

                // Update storage
                _currentStorage -= amountToSubstract;

                ItemStorageHasChanged?.Invoke();
                AmountHasChanged?.Invoke(_currentStorage);
                StorageObjectChanged?.Invoke(objectType.ObjectType, _currentStorage);
            }
            else
            {
                ObjectStorage.Remove(objectType);

                _currentStorage -= amountToSubstract;
                AmountHasChanged?.Invoke(_currentStorage);
            }
        }
    }

    /// <summary>
    /// Called to know if an amount added to the storage will exceed the capacity.
    /// </summary>
    /// <param name="amountToCheck"> Amount to compare with the stock. </param>
    /// <returns></returns>
    public bool WillExceedCapacity(int amountToCheck)
    {
        return _currentStorage + amountToCheck > _storageCapacity;
    }

    /// <summary>
    /// Called to get the remaining storage.
    /// </summary>
    /// <returns></returns>
    public int GetRemainingStorage()
    {
        return _storageCapacity - _currentStorage;
    }

    /// <summary>
    /// Return true if the storage is full.
    /// </summary>
    /// <returns></returns>
    public bool IsStorageFull()
    {
        if (_storageCapacity != 0)
        {
            return _currentStorage == _storageCapacity;
        }
        else
        {
            return true;
        }
    }

    /// <summary>
    /// Called to know if there is enough of a component type in storage.
    /// </summary>
    /// <param name="component"> Type of the component. </param>
    /// <param name="amountToCheck"> Amount to compare with the stock. </param>
    /// <returns></returns>
    public bool ThereIsEnoughComponentsInStorage(ComponentData component, int amountToCheck)
    {
        if (ComponentStorage.ContainsKey(component))
        {
            return amountToCheck <= ComponentStorage[component];
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Called to know if there is enough components from a recipe in the storage.
    /// </summary>
    /// <param name="ingredients"></param>
    /// <returns></returns>
    public bool ThereIsEnoughIngredientsInStorage(List<Ingredient> ingredients)
    {
        bool recipeIsPossible = true;

        for (int i = 0; i < ingredients.Count; i++)
        {
            if (!ThereIsEnoughComponentsInStorage(ingredients[i].ComponentData, ingredients[i].Quantity))
            {
                recipeIsPossible = false;
                break;
            }
        }

        return recipeIsPossible;
    }

    /// <summary>
    /// Called to know if there is enough of an object type in storage.
    /// </summary>
    /// <param name="objectType"> Type of the object. </param>
    /// <param name="amountToCheck"> Amount to compare with the stock. </param>
    /// <returns></returns>
    public bool ThereIsEnoughObjectsInStorage(ObjectData objectType, int amountToCheck)
    {
        if (ObjectStorage.ContainsKey(objectType))
        {
            return amountToCheck >= ObjectStorage[objectType];
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Called to know the number of this object type in the storage.
    /// </summary>
    /// <param name="objectType"> Type of the object. </param>
    /// <returns></returns>
    public int ReturnNumberOfThisObject(ObjectData objectType)
    {
        if (ObjectStorage.ContainsKey(objectType))
        {
            return ObjectStorage[objectType];
        }
        else
        {
            return 0;
        }
    }

    /// <summary>
    /// Called to know the number of this component type in the storage.
    /// </summary>
    /// <param name="ComponentType"> Type of the object. </param>
    /// <returns></returns>
    public int ReturnNumberOfThisComponent(ComponentData componentType)
    {
        if (ComponentStorage.ContainsKey(componentType))
        {
            return ComponentStorage[componentType];
        }
        else
        {
            return 0;
        }
    }

    /// <summary>
    /// Called to add capacity when a storage room is build or upgraded;
    /// </summary>
    /// <param name="capacityToAdd"> Capacity to add. </param>
    public void IncreaseCapacity(int capacityToAdd)
    {
        _storageCapacity += capacityToAdd;

        ItemStorageHasChanged?.Invoke();
        CapacityHasChanged?.Invoke(_storageCapacity);
    }
}
