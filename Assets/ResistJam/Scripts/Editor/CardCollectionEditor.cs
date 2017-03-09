using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CardCollection))]
public class CardCollectionEditor : Editor
{
	protected CardCollection cardCollection;

	protected void OnEnable()
	{
		cardCollection = (CardCollection)target;
	}

	public override void OnInspectorGUI()
	{
		if (GUILayout.Button("Parse"))
		{
			ParseText();
		}

		DrawDefaultInspector();
	}

	protected void ParseText()
	{
		Debug.Log("Parse");
		cardCollection.cards.Clear();


	}
}
