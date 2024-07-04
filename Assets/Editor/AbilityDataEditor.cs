using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AbilityData))]
public class AbilityDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Update the serialized object
        serializedObject.Update();

        // Draw all properties except the icon property
        DrawPropertiesExcluding(serializedObject, "icon");

        AbilityData abilityData = (AbilityData)target;

        // Display the sprite with a larger preview
        EditorGUILayout.LabelField("Icon", GUILayout.Width(100));
        abilityData.icon = (Sprite)EditorGUILayout.ObjectField(abilityData.icon, typeof(Sprite), allowSceneObjects: false, GUILayout.Width(100), GUILayout.Height(100));

        // Apply changes to the serialized object
        serializedObject.ApplyModifiedProperties();
    }
}
