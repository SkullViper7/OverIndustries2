using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Room/Create new assembly room data")]
public class AssemblyRoomData : ScriptableObject, IRoomBehaviourData
{
    /// <summary>
    /// Objects which can be assembled at the start of the game.
    /// </summary>
    [SerializeField, Tooltip("Objects which can be assembled at the start of the game.")]
    private List<ObjectData> _assemblableObjectsByDefault;

    /// <summary>
    /// Gets objects which can be assembled at the start of the game.
    /// </summary>
    public List<ObjectData> AssemblableObjectsByDefault { get { return _assemblableObjectsByDefault; } private set { } }
}
