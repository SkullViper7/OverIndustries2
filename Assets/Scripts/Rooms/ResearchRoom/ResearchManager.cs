using System.Collections.Generic;
using UnityEngine;

public class ResearchManager : MonoBehaviour
{
    // Singleton
    private static ResearchManager _instance = null;
    public static ResearchManager Instance => _instance;

    /// <summary>
    /// Data which contains items progression during the game.
    /// </summary>
    [SerializeField]
    private ItemProgressionData _itemProgressionData;

    /// <summary>
    /// All components that a machining room cans manufacture.
    /// </summary>
    public List<ComponentData> ManufacturableComponents { get; private set; } = new();

    /// <summary>
    /// All objects that the an assembly room cans manufacture.
    /// </summary>
    public List<ObjectData> ManufacturableObjects { get; private set; } = new();

    /// <summary>
    /// The list of all researchable components in the game.
    /// </summary>
    public List<ComponentData> AllResearchableComponents { get; private set; } = new();

    /// <summary>
    /// The list of all researchable objects in the game.
    /// </summary>
    public List<ObjectData> AllResearchableObjects { get; private set; } = new();

    /// <summary>
    /// Components currently being searched.
    /// </summary>
    private List<ComponentData> _componentsBeingSearched = new();

    /// <summary>
    /// Objects currently being searched.
    /// </summary>
    private List<ObjectData> _objectsBeingSearched = new();

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

    private void Start()
    {
        // Set manufacturable components and objects by default
        ManufacturableComponents.AddRange(_itemProgressionData.ManufacturableComponentsByDefault);
        ManufacturableObjects.AddRange(_itemProgressionData.ManufacturableObjectsByDefault);

        // Set all researchable components in the game
        AllResearchableComponents.AddRange(_itemProgressionData.ComponentsResearchableAtLvl3);

        // Set all researchable objects in the game
        AllResearchableObjects.AddRange(_itemProgressionData.ObjectsResearchableAtLvl3);
    }

    /// <summary>
    /// Called when a new research room is create so the manager listens events of the room.
    /// </summary>
    /// <param name="newResearchRoom"> New research room created. </param>
    public void InitNewResearchRoomListeners(ResearchRoom newResearchRoom)
    {
        // Init listeners for components
        newResearchRoom.ComponentResearchStarted += AddComponentBeingSearched;
        newResearchRoom.ComponentResearchStoped += RemoveComponentBeingSearched;
        newResearchRoom.ComponentResearchCompleted += AddManufacturableComponent;

        // Init listeners for objects
        newResearchRoom.ObjectResearchStarted += AddObjectBeingSearched;
        newResearchRoom.ObjectResearchStoped += RemoveObjectBeingSearched;
        newResearchRoom.ObjectResearchCompleted += AddManufacturableObject;
    }

    /// <summary>
    /// Return true if the component given is already searched.
    /// </summary>
    /// <param name="componentToCheck"> Component to check. </param>
    /// <returns></returns>
    public bool IsThisComponentAlreadySearched(ComponentData componentToCheck)
    {
        return _componentsBeingSearched.Contains(componentToCheck);
    }

    /// <summary>
    /// Return true if the object given is already searched.
    /// </summary>
    /// <param name="objectToCheck"> Object to check. </param>
    /// <returns></returns>
    public bool IsThisObjectAlreadySearched(ObjectData objectToCheck)
    {
        return _objectsBeingSearched.Contains(objectToCheck);
    }

    /// <summary>
    /// Return true if the component given has already been searched.
    /// </summary>
    /// <param name="componentToCheck"> Component to check. </param>
    /// <returns></returns>
    public bool HasThisComponentAlreadyBeenSearched(ComponentData componentToCheck)
    {
        return ManufacturableComponents.Contains(componentToCheck);
    }

    /// <summary>
    /// Return true if the object given has already been searched.
    /// </summary>
    /// <param name="objectToCheck"> Object to check. </param>
    /// <returns></returns>
    public bool HasThisObjectAlreadyBeenSearched(ObjectData objectToCheck)
    {
        return ManufacturableObjects.Contains(objectToCheck);
    }

    /// <summary>
    /// Return true if the component given is researchable by a room at the lvl given.
    /// </summary>
    /// <param name="componentToCheck"> Component to check. </param>
    /// <param name="lvlOfTheRoom"> Lvl of the room. </param>
    /// <returns></returns>
    public bool IsThisComponentResearchableAtThisLvl(ComponentData componentToCheck, int lvlOfTheRoom)
    {
        switch (lvlOfTheRoom)
        {
            case 1:
                return _itemProgressionData.ComponentsResearchableAtLvl1.Contains(componentToCheck);
            case 2:
                return _itemProgressionData.ComponentsResearchableAtLvl2.Contains(componentToCheck);
            case 3:
                return _itemProgressionData.ComponentsResearchableAtLvl3.Contains(componentToCheck);
            default:
                return false;
        }
    }

    /// <summary>
    /// Return true if the object given is researchable by a room at the lvl given.
    /// </summary>
    /// <param name="objectToCheck"> Object to check. </param>
    /// <param name="lvlOfTheRoom"> Lvl of the room. </param>
    /// <returns></returns>
    public bool IsThisObjectResearchableAtThisLvl(ObjectData objectToCheck, int lvlOfTheRoom)
    {
        switch (lvlOfTheRoom)
        {
            case 1:
                return _itemProgressionData.ObjectsResearchableAtLvl1.Contains(objectToCheck);
            case 2:
                return _itemProgressionData.ObjectsResearchableAtLvl2.Contains(objectToCheck);
            case 3:
                return _itemProgressionData.ObjectsResearchableAtLvl3.Contains(objectToCheck);
            default:
                return false;
        }
    }

    /// <summary>
    /// Called to add a new manufacturable component to the list and notifies all already built machining rooms.
    /// </summary>
    /// <param name="newManufacturableComponent"> New manufacturable component. </param>
    private void AddManufacturableComponent(ComponentData newManufacturableComponent)
    {
        ManufacturableComponents.Add(newManufacturableComponent);
    }

    /// <summary>
    /// Called to add a new manufacturable object to the list and notifies all already built assembly rooms.
    /// </summary>
    /// <param name="newManufacturableObject"> New manufacturable object. </param>
    private void AddManufacturableObject(ObjectData newManufacturableObject)
    {
        ManufacturableObjects.Add(newManufacturableObject);
    }

    /// <summary>
    /// Called to add a component being searched to the list.
    /// </summary>
    /// <param name="newComponentBeingSearched"> The new component being searched. </param>
    private void AddComponentBeingSearched(ComponentData newComponentBeingSearched)
    {
        if (!_componentsBeingSearched.Contains(newComponentBeingSearched))
        {
            _componentsBeingSearched.Add(newComponentBeingSearched);
        }
    }

    /// <summary>
    /// Called to remove a component being searched from the list.
    /// </summary>
    /// <param name="componentNoLongerResearched"> The component which is no longer researched. </param>
    private void RemoveComponentBeingSearched(ComponentData componentNoLongerResearched)
    {
        _componentsBeingSearched.Remove(componentNoLongerResearched);
    }

    /// <summary>
    /// Called to add an object being searched to the list.
    /// </summary>
    /// <param name="newObjectBeingSearched"> The new object being searched. </param>
    private void AddObjectBeingSearched(ObjectData newObjectBeingSearched)
    {
        if (!_objectsBeingSearched.Contains(newObjectBeingSearched))
        {
            _objectsBeingSearched.Add(newObjectBeingSearched);
        }
    }

    /// <summary>
    /// Called to remove an object being searched from the list.
    /// </summary>
    /// <param name="objectNoLongerResearched"> The object which is no longer researched. </param>
    private void RemoveObjectBeingSearched(ObjectData objectNoLongerResearched)
    {
        _objectsBeingSearched.Remove(objectNoLongerResearched);
    }
}