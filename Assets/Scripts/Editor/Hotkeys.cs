using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using System.Collections;
using Object = UnityEngine.Object;

public class InspectorLockToggle
{
	private static EditorWindow _mouseOverWindow;
	
	[MenuItem("Stuff/Select Inspector under mouse cursor (use hotkey) #&q")]
	static void SelectLockableInspector()
	{
		if (EditorWindow.mouseOverWindow.GetType().Name == "InspectorWindow")
		{
			_mouseOverWindow = EditorWindow.mouseOverWindow;
			Type type = Assembly.GetAssembly(typeof(Editor)).GetType("UnityEditor.InspectorWindow");
			Object[] findObjectsOfTypeAll = Resources.FindObjectsOfTypeAll(type);
			int indexOf = findObjectsOfTypeAll.ToList().IndexOf(_mouseOverWindow);
			EditorPrefs.SetInt("LockableInspectorIndex", indexOf);
		}
	}
	
	[MenuItem("Stuff/Toggle Lock &q")]
	static void ToggleInspectorLock()
	{
		if (_mouseOverWindow == null)
		{
			if (!EditorPrefs.HasKey("LockableInspectorIndex"))
				EditorPrefs.SetInt("LockableInspectorIndex", 0);
			int i = EditorPrefs.GetInt("LockableInspectorIndex");
			
			Type type = Assembly.GetAssembly(typeof(Editor)).GetType("UnityEditor.InspectorWindow");
			Object[] findObjectsOfTypeAll = Resources.FindObjectsOfTypeAll(type);
			_mouseOverWindow = (EditorWindow)findObjectsOfTypeAll[i];
		}
		
		if ((_mouseOverWindow != null) && (_mouseOverWindow.GetType().Name == "InspectorWindow"))
		{
			Type type = Assembly.GetAssembly(typeof(Editor)).GetType("UnityEditor.InspectorWindow");
			PropertyInfo propertyInfo = type.GetProperty("isLocked");
			bool value = (bool)propertyInfo.GetValue(_mouseOverWindow, null);
			propertyInfo.SetValue(_mouseOverWindow, !value, null);
			_mouseOverWindow.Repaint();
		}
	}
	
	[MenuItem("Stuff/Clear Console #&c")]
	static void ClearConsole()
	{
		Type type = Assembly.GetAssembly(typeof(Editor)).GetType("UnityEditorInternal.LogEntries");
		type.GetMethod("Clear").Invoke(null,null);
	}
     [MenuItem("Stuff/Revert to Prefab #&r")]
 static void Revert() {
     var selection = Selection.gameObjects;
     //Debug.Log(selection[0]);

     if (selection.Length > 0) {
         for (int i = 0; i < selection.Length; i++) {
             var go = selection[i];
             var name = go.name;
             PrefabUtility.DisconnectPrefabInstance(go);

             var children = go.GetComponentsInChildren<Transform>();
             foreach (var child in children)
                 if (child != go.transform)
                     GameObject.DestroyImmediate(child.gameObject);
             
             PrefabUtility.ReconnectToLastPrefab(go);
             PrefabUtility.RevertPrefabInstance(go);

             //PrefabUtility.ResetToPrefabState(go);

             //Keep the name
             go.name = name;
             //PrefabUtility.RevertPrefabInstance(go);
         }
     } else {
         Debug.Log("Cannot revert to prefab - nothing selected");
     }
 }

}

