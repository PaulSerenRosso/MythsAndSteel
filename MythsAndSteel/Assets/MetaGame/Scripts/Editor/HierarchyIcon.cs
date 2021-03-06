using UnityEditor;
using UnityEngine;
using System.Collections;

[InitializeOnLoad]
public static class HierarchyIcon
{
    const string IgnoreIcons = "GameObject Icon, Prefab Icon";

    static HierarchyIcon()
    {
        EditorApplication.hierarchyWindowItemOnGUI += HandleHierarchyWindowItemOnGUI;
    }

    static void HandleHierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
    {
        var content = EditorGUIUtility.ObjectContent(EditorUtility.InstanceIDToObject(instanceID), null); 
        if(content.image != null && !IgnoreIcons.Contains(content.image.name) && content.image.name != "d_GameObject Icon" && content.image.name!= "d_Prefab Icon" && content.image.name != "d_PrefabVariant Icon") 
            GUI.DrawTexture(new Rect(selectionRect.xMax - 16, selectionRect.yMin, 16, 16), content.image);
    }
}
