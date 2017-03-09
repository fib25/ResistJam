using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISheepDebug : MonoBehaviour
{
	public Sheep sheep;
	public Text aText;
	public Text bText;
	public Text cText;
	public Text leanText;
	public Text stateText;
	public Text keyIdeal;

	protected void Start()
	{
		Color randomColour = new Color(UnityEngine.Random.Range(0.5f, 1f),
									   UnityEngine.Random.Range(0.5f, 1f),
									   UnityEngine.Random.Range(0.5f, 1f));
		aText.color = bText.color = cText.color = leanText.color = stateText.color = keyIdeal.color = randomColour;
	}

	protected void Update()
	{
		aText.text = GetString(IdealType.A);
		bText.text = GetString(IdealType.B);
		cText.text = GetString(IdealType.C);
		leanText.text = "Lean: " + sheep.Lean.ToString("N1");
		stateText.text = sheep.State.ToString();
		keyIdeal.text = "Key: " + sheep.Ideals.KeyIdeal.ToString();
	}

	protected string GetString(IdealType idealType)
	{
		return idealType.ToString() + ": " + sheep.Ideals.GetIdealValue(idealType).ToString("N1");
	}
}
