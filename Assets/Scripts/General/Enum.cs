/// <summary>
/// All job types.
/// </summary>
public enum Job
{
    Boilermaker, //Chaudronnier
    Welder, //Soudeur
    MachiningTechnician, //Technicien d'usinage
    DraftsMan, //Dessinateur industriel
    MaintenanceTechnician, //Technicien de Maintenance
    ElectronicsTechnician //Technicien en électronique
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
    Elevator,
    RawMaterialStorage
}

/// <summary>
/// All component types.
/// </summary>
public enum ComponentType
{
    Gear,
    Screw,
    Rim,
    Fork,
    Handlebar
}

/// <summary>
/// All object types.
/// </summary>
public enum ObjectType
{
    Scooter,
    Jack,
    Bike,
    MilkingRobot,
    Tractor,
    Cylinder
}