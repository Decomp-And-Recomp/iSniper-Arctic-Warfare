using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class NavMeshTools : EditorWindow
{
    private static void AddTransform(List<Transform> children, Transform root)
    {
        children.Add(root);

        for (int i = 0; i < root.childCount; i++)
        {
            AddTransform(children, root.GetChild(i));
        }
    }

    public static List<GameObject> SelectAll(int layer)
    {
        GameObject[] rootObjects = SceneManager.GetActiveScene().GetRootGameObjects();

        List<Transform> children = new List<Transform>();
        List<GameObject> selection = new List<GameObject>();

        foreach (GameObject obj in rootObjects)
        {
            AddTransform(children, obj.transform);
        }

        foreach (Transform child in children)
        {
            if (child.gameObject.layer == layer) selection.Add(child.gameObject);
        }

        Selection.objects = selection.ToArray();

        return selection;
    }

    [MenuItem("GameObject/NavMeshing/Fix All")]
    public static void FixAll()
    {
        SerializedObject settings = new SerializedObject(UnityEditor.AI.NavMeshBuilder.navMeshSettingsObject);

        settings.FindProperty("m_BuildSettings.agentRadius").floatValue = 0.25f;
        settings.FindProperty("m_BuildSettings.manualCellSize").boolValue = false;

        settings.ApplyModifiedProperties();
		settings.Update();

        List<GameObject> walkable = SelectAll(10);

        List<GameObject>
        notWalkable =        SelectAll(11) ;
        notWalkable.AddRange(SelectAll(12));
        notWalkable.AddRange(SelectAll(13));
        notWalkable.AddRange(SelectAll(14));
        notWalkable.AddRange(SelectAll(15));

        settings.ApplyModifiedProperties();
		settings.Update();

        foreach (GameObject walkableObject in walkable)
        {
            GameObjectUtility.SetStaticEditorFlags(walkableObject, StaticEditorFlags.NavigationStatic);
            GameObjectUtility.SetNavMeshArea(walkableObject, 0);
        }

        foreach (GameObject notWalkableObject in notWalkable)
        {
            GameObjectUtility.SetStaticEditorFlags(notWalkableObject, StaticEditorFlags.NavigationStatic);
            GameObjectUtility.SetNavMeshArea(notWalkableObject, 1);
        }

        Selection.objects = new Object[0];
    }

    [MenuItem("GameObject/NavMeshing/Select Grounds")]
    public static void SelectGrounds() { SelectAll(10); }

    [MenuItem("GameObject/NavMeshing/Select Obstacles")]
    public static void SelectObstacles() { SelectAll(11); }

    [MenuItem("GameObject/NavMeshing/Select Platforms")]
    public static void SelectPlatforms() { SelectAll(12); }

    [MenuItem("GameObject/NavMeshing/Select Shows")]
    public static void SelectShows() { SelectAll(13); }

    [MenuItem("GameObject/NavMeshing/Select Nils")]
    public static void SelectNils() { SelectAll(14); }

    [MenuItem("GameObject/NavMeshing/Select Editors")]
    public static void SelectEditors() { SelectAll(15); }
}
