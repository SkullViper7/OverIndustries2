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
    ElectronicsTechnician, //Technicien en �lectronique
    IndustrialElectrician, //Electricien industriel
    SoftwareEngineer //Ing�nieur logiciel embarqu�
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
    CylinderBody, //V�rin
    Tank, //Cuve
    ArticulatedArm, //Bras articul�
    ElectroHydraulicCabine, //Capteur �lectro hydraulique
    DiscBrake, //Frein � disque
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
    Cylinder, //v�rin �lectro hydraulique
    Skateboard,
    Forklift, //Charriot �l�vateur
    BackhoeLoader, //Tractopelle
    ElectricWheelchair, //Fauteuil roulant �lectrique
    HandPalletTruck, //Transpalette manuel
}