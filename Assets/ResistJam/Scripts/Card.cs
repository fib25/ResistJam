using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Card
{
	public IdealType idealType;
	public float value;
	[TextArea]
	public string message;
}
