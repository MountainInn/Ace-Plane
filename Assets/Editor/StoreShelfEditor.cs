using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(StoreShelf))]
public class StoreShelfEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var vendibles = serializedObject.FindProperty("vendibles");
        
        RectTransform contentParent = (RectTransform) serializedObject.FindProperty("contentParent").objectReferenceValue;


    }
}
