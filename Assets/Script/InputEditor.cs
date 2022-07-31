#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEditor.ShortcutManagement;
using UnityEngine;

public class InputEditor : MonoBehaviour
{
    [MenuItem("GameObject/My object/Input", false, -1)]
    public static void ClearInput(MenuCommand menuCommand)
    {
        GameObject newGO = Resources.Load("Input") as GameObject;
        var qwe = Instantiate(newGO, Vector3.zero, Quaternion.identity);
        qwe.transform.SetParent((menuCommand.context as GameObject).transform, false);
    }
}
#endif