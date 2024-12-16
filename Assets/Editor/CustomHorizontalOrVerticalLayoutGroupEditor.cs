using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor.UI;
using UnityEditor;
#endif

#if UNITY_EDITOR
[CustomEditor(typeof(HorizontalOrVerticalLayoutGroup), true)]
[CanEditMultipleObjects]
public class CustomHorizontalOrVerticalLayoutGroupEditor : HorizontalOrVerticalLayoutGroupEditor
{
    SerializedProperty m_Padding;
    SerializedProperty m_Spacing;
    SerializedProperty m_ChildAlignment;
    SerializedProperty m_ChildControlWidth;
    SerializedProperty m_ChildControlHeight;
    SerializedProperty m_ChildScaleWidth;
    SerializedProperty m_ChildScaleHeight;
    SerializedProperty m_ChildForceExpandWidth;
    SerializedProperty m_ChildForceExpandHeight;
    SerializedProperty m_ReverseArrangement;

    HorizontalOrVerticalLayoutGroup _horizontalOrVertical;
    CustomLayoutGroup _customLayoutGroup;

    protected override void OnEnable()
    {
        m_Padding = serializedObject.FindProperty("m_Padding");
        m_Spacing = serializedObject.FindProperty("m_Spacing");
        m_ChildAlignment = serializedObject.FindProperty("m_ChildAlignment");
        m_ChildControlWidth = serializedObject.FindProperty("m_ChildControlWidth");
        m_ChildControlHeight = serializedObject.FindProperty("m_ChildControlHeight");
        m_ChildScaleWidth = serializedObject.FindProperty("m_ChildScaleWidth");
        m_ChildScaleHeight = serializedObject.FindProperty("m_ChildScaleHeight");
        m_ChildForceExpandWidth = serializedObject.FindProperty("m_ChildForceExpandWidth");
        m_ChildForceExpandHeight = serializedObject.FindProperty("m_ChildForceExpandHeight");
        m_ReverseArrangement = serializedObject.FindProperty("m_ReverseArrangement");

        _horizontalOrVertical = (HorizontalOrVerticalLayoutGroup)target;
        _customLayoutGroup = _horizontalOrVertical.GetComponent<CustomLayoutGroup>();
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // If the CustomLayoutGroup exists, show message
        if (_customLayoutGroup != null)
        {
            EditorGUILayout.HelpBox("Some properties are controlled by CustomLayoutGroup.", MessageType.Info);
        }

        // If the CustomLayoutGroup exists, disable fields
        if (_customLayoutGroup != null)
        {
            GUI.enabled = false;
        }

        EditorGUILayout.PropertyField(m_Padding, true);
        EditorGUILayout.PropertyField(m_Spacing, true);

        GUI.enabled = true;

        EditorGUILayout.PropertyField(m_ChildAlignment, true);
        EditorGUILayout.PropertyField(m_ReverseArrangement, true);

        // If the CustomLayoutGroup exists, disable fields
        if (_customLayoutGroup != null)
        {
            GUI.enabled = false;
        }

        Rect rect = EditorGUILayout.GetControlRect();
        rect = EditorGUI.PrefixLabel(rect, -1, EditorGUIUtility.TrTextContent("Control Child Size"));
        rect.width = Mathf.Max(50, (rect.width - 4) / 3);
        EditorGUIUtility.labelWidth = 50;
        ToggleLeft(rect, m_ChildControlWidth, EditorGUIUtility.TrTextContent("Width"));
        rect.x += rect.width + 2;
        ToggleLeft(rect, m_ChildControlHeight, EditorGUIUtility.TrTextContent("Height"));
        EditorGUIUtility.labelWidth = 0;

        rect = EditorGUILayout.GetControlRect();
        rect = EditorGUI.PrefixLabel(rect, -1, EditorGUIUtility.TrTextContent("Use Child Scale"));
        rect.width = Mathf.Max(50, (rect.width - 4) / 3);
        EditorGUIUtility.labelWidth = 50;
        ToggleLeft(rect, m_ChildScaleWidth, EditorGUIUtility.TrTextContent("Width"));
        rect.x += rect.width + 2;
        ToggleLeft(rect, m_ChildScaleHeight, EditorGUIUtility.TrTextContent("Height"));
        EditorGUIUtility.labelWidth = 0;

        rect = EditorGUILayout.GetControlRect();
        rect = EditorGUI.PrefixLabel(rect, -1, EditorGUIUtility.TrTextContent("Child Force Expand"));
        rect.width = Mathf.Max(50, (rect.width - 4) / 3);
        EditorGUIUtility.labelWidth = 50;
        ToggleLeft(rect, m_ChildForceExpandWidth, EditorGUIUtility.TrTextContent("Width"));
        rect.x += rect.width + 2;
        ToggleLeft(rect, m_ChildForceExpandHeight, EditorGUIUtility.TrTextContent("Height"));
        EditorGUIUtility.labelWidth = 0;

        // If the CustomLayoutGroup exists, set values as false
        if (_customLayoutGroup != null)
        {
            m_ChildControlWidth.boolValue = false;
            m_ChildControlHeight.boolValue = false;
            m_ChildScaleWidth.boolValue = false;
            m_ChildScaleHeight.boolValue = false;
            m_ChildForceExpandWidth.boolValue = false;
            m_ChildForceExpandHeight.boolValue = false;
        }

        serializedObject.ApplyModifiedProperties();
    }

    void ToggleLeft(Rect position, SerializedProperty property, GUIContent label)
    {
        bool toggle = property.boolValue;
        EditorGUI.BeginProperty(position, label, property);
        EditorGUI.BeginChangeCheck();
        int oldIndent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;
        toggle = EditorGUI.ToggleLeft(position, label, toggle);
        EditorGUI.indentLevel = oldIndent;
        if (EditorGUI.EndChangeCheck())
        {
            property.boolValue = property.hasMultipleDifferentValues ? true : !property.boolValue;
        }
        EditorGUI.EndProperty();
    }
}
#endif