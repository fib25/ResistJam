using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIIdealsDebug : MonoBehaviour
{
	public AbstractIdealist idealist;
	public Text xText;
	public Text yText;
	public Text zText;

	protected void Update()
	{
		xText.text = "X: " + idealist.Ideals.immigration.ToString("N3");
		yText.text = "Y: " + idealist.Ideals.publicSpending.ToString("N3");
		zText.text = "Z: " + idealist.Ideals.civilRights.ToString("N3");
	}
}
