#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(InterfaceTypeAttribute))]
public class InterfaceTypeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        InterfaceTypeAttribute attribute = (InterfaceTypeAttribute)this.attribute;

        // Checks that the field is an ObjectReference
        if (property.propertyType == SerializedPropertyType.ObjectReference)
        {
            EditorGUI.BeginProperty(position, label, property);

            // Only showes compatible objects
            property.objectReferenceValue = EditorGUI.ObjectField(
                position,
                label,
                property.objectReferenceValue,
                attribute.Type, // Filter by attribute type
                false // Prevents selection of scene objects
            );

            EditorGUI.EndProperty();
        }
        else
        {
            EditorGUI.LabelField(position, label.text, "Use [InterfaceType] with ScriptableObjects.");
        }
    }
}
#endif