using UnityEngine;

[CreateAssetMenu(menuName = "Room/Create new assembly room data")]
public class AssemblyRoomData : ScriptableObject, IRoomBehaviourData
{
    /// <summary>
    /// The picto representing the storage.
    /// </summary>
    [SerializeField, Tooltip("The picto representing the storage.")]
    private Sprite _storagePicto;

    /// <summary>
    /// Internal storage at lvl 1.
    /// </summary>
    [Space, Header("Internal storage"), SerializeField, Tooltip("Internal storage at lvl 1.")]
    private int _internalStorageAtLvl1;

    /// <summary>
    /// Internal storage at lvl 2.
    /// </summary>
    [SerializeField, Tooltip("Internal storage at lvl 2.")]
    private int _internalStorageAtLvl2;

    /// <summary>
    /// Internal storage at lvl 3.
    /// </summary> 
    [SerializeField, Tooltip("Internal storage at lvl 3.")]
    private int _internalStorageAtLvl3;

    /// <summary>
    /// Gets the picto representing the storage.
    /// </summary>
    public Sprite StoragePicto { get { return _storagePicto; } private set { } }

    /// <summary>
    /// Gets the internal storage at lvl 1.
    /// </summary>
    public int InternalStorageAtLvl1 { get { return _internalStorageAtLvl1; } private set { } }

    /// <summary>
    /// Gets the internal storage at lvl 2.
    /// </summary>
    public int InternalStorageAtLvl2 { get { return _internalStorageAtLvl2; } private set { } }

    /// <summary>
    /// Gets the internal storage at lvl 3.
    /// </summary>
    public int InternalStorageAtLvl3 { get { return _internalStorageAtLvl3; } private set { } }
}
