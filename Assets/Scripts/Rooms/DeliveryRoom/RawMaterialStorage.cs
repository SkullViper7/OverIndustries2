using UnityEngine;

public class RawMaterialStorage : MonoBehaviour
{
    // Singleton
    private static RawMaterialStorage _instance = null;
    public static RawMaterialStorage Instance => _instance;

    /// <summary>
    /// Capacity of the storage;
    /// </summary>
    [SerializeField]
    private int _storageCapacity;

    /// <summary>
    /// Amount of raw materials in storage.
    /// </summary>
    [SerializeField]
    private int _amoutOfRawMaterial;

    /// <summary>
    /// Gets the capacity of the storage.
    /// </summary>
    public int StorageCapacity { get { return _storageCapacity; } private set { } }

    /// <summary>
    /// Gets the amount of raw materials in storage.
    /// </summary>
    public int AmoutOfRawMaterial { get { return _amoutOfRawMaterial; } private set { } }

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
    }

    /// <summary>
    /// Called to substract an amount of raw material in the storage.
    /// </summary>
    /// <param name="amountToSubstract"> Amount to substract. </param>
    public void SubstractRawMaterials(int amountToSubstract)
    {
        _amoutOfRawMaterial -= amountToSubstract;
        Mathf.Clamp(_amoutOfRawMaterial, 0, _storageCapacity);
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
    /// Called to add capacity when a delivery room is build or upgraded;
    /// </summary>
    /// <param name="capacityToAdd"> Capacity to add. </param>
    public void IncreaseCapacity(int capacityToAdd)
    {
        _storageCapacity += capacityToAdd;
    }
}
