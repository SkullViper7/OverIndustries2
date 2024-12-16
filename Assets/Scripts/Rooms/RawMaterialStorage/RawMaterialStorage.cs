using UnityEngine;
using System;

public class RawMaterialStorage : MonoBehaviour
{
    // Singleton
    private static RawMaterialStorage _instance = null;
    public static RawMaterialStorage Instance => _instance;

    /// <summary>
    /// Capacity of the storage;
    /// </summary>
    private int _storageCapacity;

    /// <summary>
    /// Amount of raw materials in storage.
    /// </summary>
    private int _amoutOfRawMaterial;

    /// <summary>
    /// Gets the capacity of the storage.
    /// </summary>
    public int StorageCapacity { get { return _storageCapacity; } private set { } }

    /// <summary>
    /// Gets the amount of raw materials in storage.
    /// </summary>
    public int AmoutOfRawMaterial { get { return _amoutOfRawMaterial; } private set { } }

    /// <summary>
    /// Get the amount of raw material recycling
    /// </summary>
    public int TotalRecyclingRawMaterial;

    /// <summary>
    /// Events to indicate changes about the raw material storage.
    /// </summary>
    public event Action<int> AmountHasChanged, CapacityHasChanged;

    /// <summary>
    /// For the recycling room
    /// </summary>
    public event Action<int> RawMaterialToRecycle, RawMaterialProduct;

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
    /// Called to add an amount of raw material in the storage.
    /// </summary>
    /// <param name="amountToAdd"> Amount to add. </param>
    public void AddRawMaterials(int amountToAdd)
    {
        _amoutOfRawMaterial += amountToAdd;
        Mathf.Clamp(_amoutOfRawMaterial, 0, _storageCapacity);

        AmountHasChanged?.Invoke(_amoutOfRawMaterial);
        RawMaterialProduct?.Invoke(amountToAdd);
        NewAmountInInternalStorage?.Invoke(_amoutOfRawMaterial, _storageCapacity);
    }

    /// <summary>
    /// Called to substract an amount of raw material in the storage.
    /// </summary>
    /// <param name="amountToSubstract"> Amount to substract. </param>
    public void SubstractRawMaterials(int amountToSubstract)
    {
        _amoutOfRawMaterial -= amountToSubstract;
        Mathf.Clamp(_amoutOfRawMaterial, 0, _storageCapacity);

        AmountHasChanged?.Invoke(_amoutOfRawMaterial);
        NewAmountInInternalStorage?.Invoke(_amoutOfRawMaterial, _storageCapacity);
    }

    /// <summary>
    /// Called to know if there is enough raw material in storage.
    /// </summary>
    /// <param name="amountToCheck"> Amount to compare with the stock. </param>
    /// <returns></returns>
    public bool ThereIsEnoughRawMaterialInStorage(int amountToCheck)
    {
        return _amoutOfRawMaterial >= amountToCheck;
    }

    /// <summary>
    /// Called to get the remaining storage.
    /// </summary>
    /// <returns></returns>
    public int GetRemainingStorage()
    {
        return _storageCapacity - _amoutOfRawMaterial;
    }

    /// <summary>
    /// Return true if the storage is full.
    /// </summary>
    /// <returns></returns>
    public bool IsStorageFull()
    {
        return _amoutOfRawMaterial == _storageCapacity;
    }

    /// <summary>
    /// Called to add capacity when a delivery room is build or upgraded;
    /// </summary>
    /// <param name="capacityToAdd"> Capacity to add. </param>
    public void IncreaseCapacity(int capacityToAdd)
    {
        _storageCapacity += capacityToAdd;

        CapacityHasChanged?.Invoke(_storageCapacity);
        NewAmountInInternalStorage?.Invoke(_amoutOfRawMaterial, _storageCapacity);
    }

    /// <summary>
    /// Call th recycling room to recycle trash
    /// </summary>
    /// <param name="rawMaterialTrash"></param>
    public void TrashMaterial(int rawMaterialTrash)
    {
        RawMaterialToRecycle?.Invoke(rawMaterialTrash);
    }
}