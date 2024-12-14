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
    GridLayoutGroup _gridLayoutGroup;
    CustomLayoutGroup _customLayoutGroup;

    protected override void OnEnable()
    {
        base.OnEnable();

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