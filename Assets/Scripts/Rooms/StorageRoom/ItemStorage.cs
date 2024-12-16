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
    private Dictionary<ComponentData, int> _componentStorage = new();

    /// <summary>
    /// Dictionnary wich stocks objects.
    /// </summary>
    private Dictionary<ObjectData, int> _objectStorage = new();

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
    public event Action<int> AmountHasChanged, CapacityHasChanged;

    /// <summary>
    /// Event for update the quest advancement 
    /// </summary>
    public event System.Action<ObjectType, int> StorageChanged;

    public event Action<int, int> NewAmountInInternalStorage;

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
            if (_componentStorage.ContainsKey(component))
            {
                _componentStorage[component] += amount;
            }
            else
            {
                _componentStorage.Add(component, amount);
            }

            // Update storage
            _currentStorage += amount;
            AmountHasChanged?.Invoke(_currentStorage);
            NewAmountInInternalStorage?.Invoke(_currentStorage, _storageCapacity);
        }
    }

    /// <summary>
    /// Called to substract an amount of component in the storage.
    /// </summary>
    /// <param name="component"> Type of the component. </param>
    /// <param name="amountToSubstract"> Amount to substract. </param>
    public void SubstractComponents(ComponentData component, int amountToSubstract)
    {
        if (_componentStorage.ContainsKey(component))
        {
            if (_componentStorage[component] - amountToSubstract >= 0)
            {
                _componentStorage[component] -= amountToSubstract;

                // Update storage
                _currentStorage -= amountToSubstract;
                AmountHasChanged?.Invoke(_currentStorage);
                NewAmountInInternalStorage?.Invoke(_currentStorage, _storageCapacity);
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
            if (_objectStorage.ContainsKey(objectType))
            {
                _objectStorage[objectType] += amount;
            }
            else
            {
                _objectStorage.Add(objectType, amount);
            }

            // Update storage
            _currentStorage += amount;
            AmountHasChanged?.Invoke(_currentStorage);
            NewAmountInInternalStorage?.Invoke(_currentStorage, _storageCapacity);
        }
    }

    /// <summary>
    /// Called to substract an amount of object in the storage.
    /// </summary>
    /// <param name="objectType"> Type of the object. </param>
    /// <param name="amountToSubstract"> Amount to substract. </param>
    public void SubstractObjects(ObjectData objectType, int amountToSubstract)
    {
        if (_objectStorage.ContainsKey(objectType))
        {
            if (_objectStorage[objectType] - amountToSubstract >= 0)
            {
                _objectStorage[objectType] -= amountToSubstract;

                // Update storage
                _currentStorage -= amountToSubstract;
                AmountHasChanged?.Invoke(_currentStorage);
                NewAmountInInternalStorage?.Invoke(_currentStorage, _storageCapacity);
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
        return _currentStorage == _storageCapacity;
    }

    /// <summary>
    /// Called to know if there is enough of a component type in storage.
    /// </summary>
    /// <param name="component"> Type of the component. </param>
    /// <param name="amountToCheck"> Amount to compare with the stock. </param>
    /// <returns></returns>
    public bool ThereIsEnoughComponentsInStorage(ComponentData component, int amountToCheck)
    {
        if (_componentStorage.ContainsKey(component))
        {
            return amountToCheck >= _componentStorage[component];
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
            if (!_componentStorage.ContainsKey(ingredients[i].ComponentData))
            {
                recipeIsPossible = false;
                break;
            }
            else
            {
                if (_componentStorage[ingredients[i].ComponentData] < ingredients[i].Quantity)
                {
                    recipeIsPossible = false;
                    break;
                }
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
        if (_objectStorage.ContainsKey(objectType))
        {
            return amountToCheck >= _objectStorage[objectType];
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
        if (_objectStorage.ContainsKey(objectType))
        {
            return _objectStorage[objectType];
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
        CapacityHasChanged?.Invoke(_storageCapacity);
    }
}
