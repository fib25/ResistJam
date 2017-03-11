using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISheepDebug : MonoBehaviour
{
	public Sheep sheep;
	public Text ideal1Text;
	public Text ideal2Text;
	public Text ideal3Text;
	public Text ideal4Text;
	public Text ideal5Text;
	public Text ideal6Text;
	public Text leanText;
	public Text stateText;
	public Text keyIdeal;

	protected void Start()
	{
		Color randomColour = new Color(UnityEngine.Random.Range(0.5f, 1f),
									   UnityEngine.Random.Range(0.5f, 1f),
									   UnityEngine.Random.Range(0.5f, 1f));
		ideal1Text.color = ideal2Text.color = ideal3Text.color = ideal4Text.color = ideal5Text.color = ideal6Text.color = randomColour;
		leanText.color = stateText.color = keyIdeal.color = randomColour;
	}

	protected void Update()
	{
		ideal1Text.text = GetString(IdealType.CivilRights);
		ideal2Text.text = GetString(IdealType.Economy);
		ideal3Text.text = GetString(IdealType.Environment);
		ideal4Text.text = GetString(IdealType.ForeignPolicy);
		ideal5Text.text = GetString(IdealType.PublicServices);
		ideal6Text.text = GetString(IdealType.ScienceAndCulture);
		leanText.text = "Lean: " + sheep.Lean.ToString("N1");
		stateText.text = sheep.State.ToString();
		keyIdeal.text = "Key: " + sheep.Ideals.KeyIdeal.ToString();
	}

	protected string GetString(IdealType idealType)
	{
		return idealType.ToString() + ": " + sheep.Ideals.GetIdealValue(idealType).ToString("N1");
	}
}
