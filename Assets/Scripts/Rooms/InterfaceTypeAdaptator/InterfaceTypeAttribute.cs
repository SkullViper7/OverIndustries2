using UnityEngine;

public class InterfaceTypeAttribute : PropertyAttribute
{
    public System.Type Type;

    public InterfaceTypeAttribute(System.Type type)
    {
        Type = type;
    }
}