/// <summary>
/// All job types.
/// </summary>
public enum Job
{
    Boilermaker, //Chaudronnier
    Welder, //Soudeur
    MetalLocksmith, //Serrurier M�tallier
    Machinist, //Usineur
    MachiningTechnician, //Technicien d'usinage
    Scrapper, //Ferrailleur
    Carpenter, //Charpentier
    TrimmerMechanicFitter, //Ajusteur M�canicien/Monteur
    MetalCuttingAgent, //Agent de D�coupage des m�taux
    DraftsMan, //Dessinateur industriel
    MaintenanceTechnician, //Technicien de Maintenance
    ElectronicsTechnician //Technicien en �lectronique
}

/// <summary>
/// All room types.
/// </summary>
public enum RoomType
{
    Machining,
    Assembly,
    Delivery,
    Storage,
    Rest,
    Director,
    Recycling,
    Research,
    Elevator
}

/// <summary>
/// All component types.
/// </summary>
public enum ComponentType
{
    Gear,
    Screw
}

/// <summary>
/// All object types.
/// </summary>
public enum ObjectType
{
    Scooter,
    Jack
}