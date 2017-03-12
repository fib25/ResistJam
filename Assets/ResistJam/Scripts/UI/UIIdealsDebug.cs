using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIIdealsDebug : MonoBehaviour
{
	public AbstractIdealist idealist;
	public Text ideal1Text;
	public Text ideal2Text;
	public Text ideal3Text;
	public Text ideal4Text;
	public Text ideal5Text;
	public Text ideal6Text;

	protected void Update()
	{
		ideal1Text.text = GetString(IdealType.CivilRights);
		ideal2Text.text = GetString(IdealType.Economy);
		ideal3Text.text = GetString(IdealType.Environment);
		ideal4Text.text = GetString(IdealType.ForeignPolicy);
		ideal5Text.text = GetString(IdealType.PublicServices);
		ideal6Text.text = GetString(IdealType.ScienceAndCulture);
	}

	protected string GetString(IdealType idealType)
	{
		return idealType.ToString() + ": " + idealist.Ideals.GetIdealValue(idealType).ToString("N1");
	}

}
