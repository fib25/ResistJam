using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

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

		string[] allLines = System.IO.File.ReadAllLines(Application.dataPath + "/ResistJam/StreamingAssets/ResistJamCards.csv");
		List<List<string>> CardTable = new List<List<string>>();
		for (int i = 0; i<allLines.Length; i++)
		{
			CardTable.Add(new List<string>());
			string[] result = allLines[i].Split(new string[]{";"},StringSplitOptions.None);
			foreach (string s in result)
			{
				CardTable[i].Add(s);
			}
		}

		IdealType currentIdealType = IdealType.Environment;

		for (int i = 1; i<CardTable.Count; i++)
		{
			if (CardTable[i][2]!="")
			{
				Card newCard = new Card();
				if (CardTable[i][0] == "ENVIRONMENT")
				{
					currentIdealType = IdealType.Environment;
				}
				else if (CardTable[i][0] == "CIVIL RIGHTS")
				{
					currentIdealType = IdealType.CivilRights;
				}
				else if (CardTable[i][0] == "PUBLIC SERVICES")
				{
					currentIdealType = IdealType.PublicServices;
				}
				else if (CardTable[i][0] == "FOREIGN POLICY")
				{
					currentIdealType = IdealType.ForeignPolicy;
				}
				else if (CardTable[i][0] == "ECONOMY")
				{
					currentIdealType = IdealType.Economy;
				}
				else if (CardTable[i][0] == "SCIENCE & CULTURE")
				{
					currentIdealType = IdealType.ScienceAndCulture;
				}
				newCard.idealType = currentIdealType;
				newCard.value = float.Parse(CardTable[i][3]);
				newCard.message = CardTable[i][2];

				cardCollection.cards.Add(newCard);
			}
		}
		Debug.Log(cardCollection.cards.Count);
	}
}
