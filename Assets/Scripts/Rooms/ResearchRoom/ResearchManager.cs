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
    [field: SerializeField]
    public ItemProgressionData ItemProgressionData { get; private set; }

    /// <summary>
    /// All components that the room can manufacture.
    /// </summary>
    public List<ComponentData> ManufacturableComponents { get; private set; } = new();

    /// <summary>
    /// All objects that the room can manufacture.
    /// </summary>
    public List<ObjectData> ManufacturableObjects { get; private set; } = new();

    /// <summary>
    /// The list of researchable components by a research rooms at lvl 1.
    /// </summary>
    public List<ComponentData> ResearchableComponentsAtLvl1 { get; private set; } = new();

    /// <summary>
    /// The list of researchable components by a research rooms at lvl 2.
    /// </summary>
    public List<ComponentData> ResearchableComponentsAtLvl2 { get; private set; } = new();

    /// <summary>
    /// The list of researchable components by a research rooms at lvl 3.
    /// </summary>
    public List<ComponentData> ResearchableComponentsAtLvl3 { get; private set; } = new();

    /// <summary>
    /// The list of researchable objects by a research rooms at lvl 1.
    /// </summary>
    public List<ObjectData> ResearchableObjectsAtLvl1 { get; private set; } = new();

    /// <summary>
    /// The list of researchable objects by a research rooms at lvl 2.
    /// </summary>
    public List<ObjectData> ResearchableObjectsAtLvl2 { get; private set; } = new();

    /// <summary>
    /// The list of researchable objects by a research rooms at lvl 3.
    /// </summary>
    public List<ObjectData> ResearchableObjectsAtLvl3 { get; private set; } = new();

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
        ManufacturableComponents.AddRange(ItemProgressionData.ManufacturableComponentsByDefault);
        ManufacturableObjects.AddRange(ItemProgressionData.ManufacturableObjectsByDefault);

        // Set researchable components for a room at lvl 1
        ResearchableComponentsAtLvl1.AddRange(ItemProgressionData.ComponentsToUnlockAtLvl1);

        // Set researchable components for a room at lvl 2
        ResearchableComponentsAtLvl2.AddRange(ItemProgressionData.ComponentsToUnlockAtLvl1);
        ResearchableComponentsAtLvl2.AddRange(ItemProgressionData.ComponentsToUnlockAtLvl2);

        // Set researchable components for a room at lvl 3
        ResearchableComponentsAtLvl3.AddRange(ItemProgressionData.ComponentsToUnlockAtLvl1);
        ResearchableComponentsAtLvl3.AddRange(ItemProgressionData.ComponentsToUnlockAtLvl2);
        ResearchableComponentsAtLvl3.AddRange(ItemProgressionData.ComponentsToUnlockAtLvl3);

        // Set researchable objects for a room at lvl 1
        ResearchableObjectsAtLvl1.AddRange(ItemProgressionData.ObjectsToUnlockAtLvl1);

        // Set researchable objects for a room at lvl 2
        ResearchableObjectsAtLvl2.AddRange(ItemProgressionData.ObjectsToUnlockAtLvl1);
        ResearchableObjectsAtLvl2.AddRange(ItemProgressionData.ObjectsToUnlockAtLvl2);

        // Set researchable objects for a room at lvl 3
        ResearchableObjectsAtLvl3.AddRange(ItemProgressionData.ObjectsToUnlockAtLvl1);
        ResearchableObjectsAtLvl3.AddRange(ItemProgressionData.ObjectsToUnlockAtLvl2);
        ResearchableObjectsAtLvl3.AddRange(ItemProgressionData.ObjectsToUnlockAtLvl3);
    }

    /// <summary>
    /// Called when a new research room is create so the manager listens events of the room.
    /// </summary>
    /// <param name="newResearchRoom"> New research room created. </param>
    public void InitNewResearchRoomListeners(ResearchRoom newResearchRoom)
    {
        // Init listeners for components
        newResearchRoom.ComponentResearchStarted += AddComponentBeingSearched;
        newResearchRoom.ComponentResearchCanceled += RemoveComponentBeingSearched;
        newResearchRoom.ComponentResearchCompleted += AddManufacturableComponent;
        newResearchRoom.ComponentResearchCompleted += RemoveResearchableComponent;

        // Init listeners for objects
        newResearchRoom.ObjectResearchStarted += AddObjectBeingSearched;
        newResearchRoom.ObjectResearchCanceled += RemoveObjectBeingSearched;
        newResearchRoom.ObjectResearchCompleted += AddManufacturableObject;
        newResearchRoom.ObjectResearchCompleted += RemoveResearchableObject;
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
        _componentsBeingSearched.Add(newComponentBeingSearched);
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
        _objectsBeingSearched.Add(newObjectBeingSearched);
    }

    /// <summary>
    /// Called to remove an object being searched from the list.
    /// </summary>
    /// <param name="objectNoLongerResearched"> The object which is no longer researched. </param>
    private void RemoveObjectBeingSearched(ObjectData objectNoLongerResearched)
    {
        _objectsBeingSearched.Remove(objectNoLongerResearched);
    }

    /// <summary>
    /// Called to remove a researchable component from lists.
    /// </summary>
    /// <param name="componentNoLongerResearchable"> The component which is no longer researchable. </param>
    private void RemoveResearchableComponent(ComponentData componentNoLongerResearched)
    {
        ResearchableComponentsAtLvl1.Remove(componentNoLongerResearched);
        ResearchableComponentsAtLvl2.Remove(componentNoLongerResearched);
        ResearchableComponentsAtLvl3.Remove(componentNoLongerResearched);
    }

    /// <summary>
    /// Called to remove a researchable object from lists.
    /// </summary>
    /// <param name="objectNoLongerResearchable"> The object which is no longer researchable. </param>
    private void RemoveResearchableObject(ObjectData objectNoLongerResearched)
    {
        ResearchableObjectsAtLvl1.Remove(objectNoLongerResearched);
        ResearchableObjectsAtLvl2.Remove(objectNoLongerResearched);
        ResearchableObjectsAtLvl3.Remove(objectNoLongerResearched);
    }
}