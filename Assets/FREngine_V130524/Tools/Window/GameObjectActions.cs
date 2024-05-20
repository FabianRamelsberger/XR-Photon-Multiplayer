using UnityEngine;
using UnityEditor;

/// <summary>
/// Contains static methods for performing various actions on GameObjects, such as renaming, clearing children, and more.
/// </summary>
public static class GameObjectActions
{
    public static void RenameChildren(GameObject parent, string prefix, bool includeNumbering)
    {
        if (parent != null)
        {
            Undo.SetCurrentGroupName("Rename Children");
            int counter = 1;
            foreach (Transform child in parent.transform)
            {
                Undo.RecordObject(child.gameObject, "Rename Child");
                child.gameObject.name = ConstructName(prefix, counter, includeNumbering);
                counter++;
            }
            Undo.CollapseUndoOperations(Undo.GetCurrentGroup());
        }
        else
        {
            Debug.LogWarning("No GameObject selected. Please select a parent GameObject to rename its children.");
        }
    }

    public static void RenameSelected(GameObject[] selectedObjects, string prefix, bool includeNumbering)
    {
        if (selectedObjects.Length > 0)
        {
            Undo.SetCurrentGroupName("Rename Selected");
            int counter = 1;
            foreach (GameObject obj in selectedObjects)
            {
                Undo.RecordObject(obj, "Rename Object");
                obj.name = ConstructName(prefix, counter, includeNumbering);
                counter++;
            }
            Undo.CollapseUndoOperations(Undo.GetCurrentGroup());
        }
        else
        {
            Debug.LogWarning("No GameObjects selected. Please select one or more GameObjects to rename.");
        }
    }

    public static void ClearChildren(GameObject parent)
    {
        if (parent != null)
        {
            Undo.SetCurrentGroupName("Clear Children");
            while (parent.transform.childCount > 0)
            {
                Undo.DestroyObjectImmediate(parent.transform.GetChild(0).gameObject);
            }
            Undo.CollapseUndoOperations(Undo.GetCurrentGroup());
        }
        else
        {
            Debug.LogWarning("No GameObject selected. Please select a parent GameObject to clear its children.");
        }
    }

    public static void RemoveSelected(GameObject[] selectedObjects)
    {
        if (selectedObjects.Length > 0)
        {
            Undo.SetCurrentGroupName("Remove Selected");
            foreach (GameObject obj in selectedObjects)
            {
                Undo.DestroyObjectImmediate(obj);
            }
            Undo.CollapseUndoOperations(Undo.GetCurrentGroup());
        }
        else
        {
            Debug.LogWarning("No GameObjects selected. Please select one or more GameObjects to remove.");
        }
    }

    public static void DuplicateSelected(GameObject[] selectedObjects)
    {
        if (selectedObjects.Length > 0)
        {
            Undo.SetCurrentGroupName("Duplicate Selected");
            foreach (GameObject obj in selectedObjects)
            {
                GameObject duplicate = Object.Instantiate(obj, obj.transform.parent);
                duplicate.name = obj.name + " (Copy)";
                Undo.RegisterCreatedObjectUndo(duplicate, "Duplicate Object");
            }
        }
        else
        {
            Debug.LogWarning("No GameObjects selected. Please select one or more GameObjects to duplicate.");
        }
    }

    public static void SetSameParent(GameObject[] selectedObjects)
    {
        if (selectedObjects.Length > 0)
        {
            Transform parent = selectedObjects[0].transform.parent;
            Undo.SetCurrentGroupName("Set Same Parent");
            foreach (GameObject obj in selectedObjects)
            {
                Undo.RecordObject(obj.transform, "Set Parent");
                obj.transform.SetParent(parent, true);
            }
            Undo.CollapseUndoOperations(Undo.GetCurrentGroup());
        }
        else
        {
            Debug.LogWarning("No GameObjects selected. Please select GameObjects to set their parent.");
        }
    }

    public static void UnparentSelected(GameObject[] selectedObjects)
    {
        if (selectedObjects.Length > 0)
        {
            Undo.SetCurrentGroupName("Unparent Selected");
            foreach (GameObject obj in selectedObjects)
            {
                Undo.RecordObject(obj.transform, "Unparent Object");
                obj.transform.SetParent(null, true);
            }
            Undo.CollapseUndoOperations(Undo.GetCurrentGroup());
        }
        else
        {
            Debug.LogWarning("No GameObjects selected. Please select GameObjects to unparent.");
        }
    }

    private static string ConstructName(string prefix, int counter, bool includeNumbering)
    {
        string newName = prefix;
        if (includeNumbering)
        {
            newName += counter.ToString();
        }
        return newName;
    }
}
