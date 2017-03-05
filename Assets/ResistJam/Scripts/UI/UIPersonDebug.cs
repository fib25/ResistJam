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
	public Text driftText;
	public Text speed;
	public Text keyIdeal;

	protected void Start()
	{
		Color randomColour = new Color(UnityEngine.Random.RandomRange(0.5f, 1f),
									   UnityEngine.Random.RandomRange(0.5f, 1f),
									   UnityEngine.Random.RandomRange(0.5f, 1f));
		xText.color = yText.color = zText.color = driftText.color = speed.color = keyIdeal.color = randomColour;
	}

	protected void Update()
	{
		xText.text = "X: " + person.Ideals.immigration.ToString("N3");
		yText.text = "Y: " + person.Ideals.publicSpending.ToString("N3");
		zText.text = "Z: " + person.Ideals.civilRights.ToString("N3");
		driftText.text = "Drift: " + person.driftAmount.ToString("N3");
		speed.text = "Speed: " + person.driftSpeed.ToString("N3");
		keyIdeal.text = "Key: " + person.Ideals.keyIdeal.ToString();
	}
}
