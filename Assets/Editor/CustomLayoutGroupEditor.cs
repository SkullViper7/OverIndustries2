using UnityEngine.UI;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR
[CustomEditor(typeof(CustomLayoutGroup), true)]
[CanEditMultipleObjects]
public class CustomLayoutGroupEditor : Editor
{
    private SerializedProperty _constraint;
    private SerializedProperty _constraintCount;
    private SerializedProperty _expansionMode;
    private SerializedProperty _ratio;
    private SerializedProperty _paddingMode;
    private SerializedProperty _padding;
    private SerializedProperty _spacingMode;
    private SerializedProperty _spacing;
    private SerializedProperty _gridSpacing;

    CustomLayoutGroup _customLayoutGroup;
    LayoutGroup _layoutGroup;

    public void OnEnable()
    {
        _constraint = serializedObject.FindProperty("_constraint");
        _constraintCount = serializedObject.FindProperty("_constraintCount");
        _expansionMode = serializedObject.FindProperty("_expansionMode");
        _ratio = serializedObject.FindProperty("_ratio");
        _paddingMode = serializedObject.FindProperty("_paddingMode");
        _padding = serializedObject.FindProperty("_padding");
        _spacingMode = serializedObject.FindProperty("_spacingMode");
        _spacing = serializedObject.FindProperty("_spacing");
        _gridSpacing = serializedObject.FindProperty("_gridSpacing");

        _customLayoutGroup = (CustomLayoutGroup)target;
        _layoutGroup = _customLayoutGroup.GetComponent<LayoutGroup>();
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        if (_layoutGroup is GridLayoutGroup)
        {
            EditorGUILayout.PropertyField(_constraint, true);

            EditorGUI.indentLevel++;
            _constraintCount.intValue = Mathf.Max(1, EditorGUILayout.IntField(_constraintCount.displayName, _constraintCount.intValue));
            EditorGUI.indentLevel--;
        }

        EditorGUILayout.PropertyField(_expansionMode, true);
        if (_expansionMode.enumValueIndex < 2)
        {
            EditorGUI.indentLevel++;
            _ratio.floatValue = Mathf.Max(0, EditorGUILayout.FloatField(_ratio.displayName, _ratio.floatValue));
            EditorGUI.indentLevel--;
        }

        EditorGUILayout.PropertyField(_paddingMode, true);
        EditorGUILayout.PropertyField(_padding, true);

        EditorGUILayout.PropertyField(_spacingMode, true);
        if (_layoutGroup is HorizontalOrVerticalLayoutGroup)
        {
            _spacing.floatValue = Mathf.Max(0, EditorGUILayout.FloatField(_spacing.displayName, _spacing.floatValue));

        }
        else if (_layoutGroup is GridLayoutGroup)
        {
            Vector2 vector = _gridSpacing.vector2Value;

            vector = EditorGUILayout.Vector2Field(_gridSpacing.displayName, vector);

            vector.x = Mathf.Max(0, vector.x);
            vector.y = Mathf.Max(0, vector.y);

            _gridSpacing.vector2Value = vector;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif