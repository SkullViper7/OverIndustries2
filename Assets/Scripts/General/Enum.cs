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
    ElectronicsTechnician, //Technicien en électronique
    IndustrialElectrician, //Electricien industriel
    SoftwareEngineer //Ingénieur logiciel embarqué
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
    Gear, //Engrenage
    Rim, //Jante de roue
    Fork, //Fourche
    Handlebar, //Guidon
    Chassis, //Chassis
    ElectronicBox, //Boiter electronique
    Cable, //Cable
    Tray, //Plateau
    Engine, //Moteur
    Sensor, //Capteur
    Tube, //Tube
    CylinderBody, //Vérin
    Tank, //Cuve
    ArticulatedArm, //Bras articulé
    ElectroHydraulicCabine, //Capteur électro hydraulique
    DiscBrake, //Frein à disque
}

/// <summary>
/// All object types.
/// </summary>
public enum ObjectType
{
    Scooter, //Trottinette
    Bike, 
    MilkingRobot, //robot de traite
    Tractor, //tracteur agricole
    Cylinder, //vérin électro hydraulique
    Skateboard,
    Forklift, //Charriot élévateur
    BackhoeLoader, //Tractopelle
    ElectricWheelchair, //Fauteuil roulant électrique
    HandPalletTruck, //Transpalette manuel
}