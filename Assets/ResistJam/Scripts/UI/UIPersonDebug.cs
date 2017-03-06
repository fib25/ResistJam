using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPersonDebug : MonoBehaviour
{
	public Person person;
	public Text xText;
	public Text yText;
	public Text zText;
	public Text leanText;
	public Text speed;
	public Text keyIdeal;

	protected void Start()
	{
		Color randomColour = new Color(UnityEngine.Random.Range(0.5f, 1f),
									   UnityEngine.Random.Range(0.5f, 1f),
									   UnityEngine.Random.Range(0.5f, 1f));
		xText.color = yText.color = zText.color = leanText.color = speed.color = keyIdeal.color = randomColour;
	}

	protected void Update()
	{
		xText.text = GetString(IdealType.A);
		yText.text = GetString(IdealType.B);
		zText.text = GetString(IdealType.C);
		leanText.text = "Lean: " + person.lean.ToString("N3");
		speed.text = "Speed: " + person.driftSpeed.ToString("N3");
		keyIdeal.text = "Key: " + person.Ideals.keyIdeal.ToString();
	}

	protected string GetString(IdealType idealType)
	{
		return idealType.ToString().Replace("IdealType.", "") + ": " + person.Ideals.GetIdealValue(idealType).ToString("N1");
	}
}
