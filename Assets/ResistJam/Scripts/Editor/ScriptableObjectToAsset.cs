using System;
using UnityEngine;
using UnityEditor;

public class ScriptableObjectToAsset : Editor
{
	[MenuItem("Utility/Create asset file from selection")]
	public static void ScriptableObjectToAsset_Static()
	{
		foreach (UnityEngine.Object obj in Selection.objects)
		{
			MonoScript ms = obj as MonoScript;
			Type type = ms.GetClass();

			if (type != null && type.IsSubclassOf(typeof(ScriptableObject)) && !type.IsSubclassOf(typeof(Editor)))
			{
				ScriptableObject asset = ScriptableObject.CreateInstance(type);

				string selectionPath = AssetDatabase.GetAssetPath(obj);
				selectionPath = selectionPath.Substring(0, selectionPath.LastIndexOf('/'));

				string path = AssetDatabase.GenerateUniqueAssetPath(System.IO.Path.Combine(selectionPath, string.Format("{0}.asset", type.Name)));
				
				AssetDatabase.CreateAsset(asset, path);
				EditorGUIUtility.PingObject(asset);

				Debug.Log("Created asset file at " + path);
			}
			else
			{
				Debug.LogWarning("Could not create asset file! " + obj.name + " is not a subclass of ScriptableObject.");
			}
		}
	}
}