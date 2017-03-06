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
		xText.text = GetString(IdealType.A);
		yText.text = GetString(IdealType.B);
		zText.text = GetString(IdealType.C);
	}

	protected string GetString(IdealType idealType)
	{
		return idealType.ToString().Replace("IdealType.", "") + ": " + idealist.Ideals.GetIdealValue(idealType).ToString("N1");
	}

}
