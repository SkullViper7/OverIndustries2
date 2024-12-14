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
    HorizontalOrVerticalLayoutGroup _horizontalOrVertical;
    CustomLayoutGroup _customLayoutGroup;

    protected override void OnEnable()
    {
        base.OnEnable();

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
}
#endif