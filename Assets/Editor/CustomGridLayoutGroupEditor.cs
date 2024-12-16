using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor.UI;
using UnityEditor;
#endif

#if UNITY_EDITOR
[CustomEditor(typeof(GridLayoutGroup), true)]
[CanEditMultipleObjects]
public class CustomGridLayoutGroupEditor : GridLayoutGroupEditor
{
    SerializedProperty m_Padding;
    SerializedProperty m_CellSize;
    SerializedProperty m_Spacing;
    SerializedProperty m_StartCorner;
    SerializedProperty m_StartAxis;
    SerializedProperty m_ChildAlignment;
    SerializedProperty m_Constraint;
    SerializedProperty m_ConstraintCount;

    GridLayoutGroup _gridLayoutGroup;
    CustomLayoutGroup _customLayoutGroup;

    protected override void OnEnable()
    {
        m_Padding = serializedObject.FindProperty("m_Padding");
        m_CellSize = serializedObject.FindProperty("m_CellSize");
        m_Spacing = serializedObject.FindProperty("m_Spacing");
        m_StartCorner = serializedObject.FindProperty("m_StartCorner");
        m_StartAxis = serializedObject.FindProperty("m_StartAxis");
        m_ChildAlignment = serializedObject.FindProperty("m_ChildAlignment");
        m_Constraint = serializedObject.FindProperty("m_Constraint");
        m_ConstraintCount = serializedObject.FindProperty("m_ConstraintCount");

        _gridLayoutGroup = (GridLayoutGroup)target;
        _customLayoutGroup = _gridLayoutGroup.GetComponent<CustomLayoutGroup>();
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
        EditorGUILayout.PropertyField(m_CellSize, true);
        EditorGUILayout.PropertyField(m_Spacing, true);

        GUI.enabled = true;

        EditorGUILayout.PropertyField(m_StartCorner, true);
        EditorGUILayout.PropertyField(m_StartAxis, true);
        EditorGUILayout.PropertyField(m_ChildAlignment, true);

        // If the CustomLayoutGroup exists, disable fields
        if (_customLayoutGroup != null)
        {
            GUI.enabled = false;
        }

        EditorGUILayout.PropertyField(m_Constraint, true);
        if (m_Constraint.enumValueIndex > 0)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(m_ConstraintCount, true);
            EditorGUI.indentLevel--;
        }

        GUI.enabled = true;

        serializedObject.ApplyModifiedProperties();
    }
}
#endif