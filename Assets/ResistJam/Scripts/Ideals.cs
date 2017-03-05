using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum IdealType
{
	Immigration,
	PublicSpending,
	CivilRights
}

public class Ideals
{
	public float immigration;
	public float publicSpending;
	public float civilRights;

	public IdealType keyIdeal;

	protected const float WEIGHT = 2f;

	public static float CompareIdeals(Ideals a, Ideals b)
	{
		float chi = Mathf.Abs((Mathf.Pow((b.immigration - a.immigration), 2f) / a.immigration) +
			(Mathf.Pow((b.civilRights - a.civilRights), 2f) / a.civilRights) +
			(Mathf.Pow((b.publicSpending - a.publicSpending), 2f) / a.publicSpending));

		return chi;
	}

	public void Randomise()
	{
		// imm = randomNo. pS = rand, CR = Rand, choose 1-3 , choicex2 + nonchosens choice x2/sum, nonchosens/sum
		immigration = UnityEngine.Random.Range(0f, 1f);
		publicSpending = UnityEngine.Random.Range(0f, 1f);
		civilRights = UnityEngine.Random.Range(0f, 1f);

		float max = Mathf.Max(immigration,publicSpending,civilRights);
		Debug.Log(max);

		if (immigration == max)
		{
			keyIdeal = IdealType.Immigration;
		}
		else if (publicSpending == max)
		{
			keyIdeal = IdealType.PublicSpending;
		}
		else if (civilRights == max)
		{
			keyIdeal = IdealType.CivilRights;
		}

		float keyVal = GetValue(keyIdeal);

		float sum = immigration + publicSpending + civilRights + (keyVal * (WEIGHT-1f));

		if (keyIdeal == IdealType.Immigration)
		{
			immigration = (immigration * WEIGHT) / sum;
		}
		else
		{
			immigration = immigration / sum;
		}

		if (keyIdeal == IdealType.PublicSpending)
		{
			publicSpending = (publicSpending * WEIGHT) / sum;
		}
		else
		{
			publicSpending = publicSpending / sum;
		}

		if (keyIdeal == IdealType.CivilRights)
		{
			civilRights = (civilRights * WEIGHT) / sum;
		}
		else
		{
			civilRights = civilRights / sum;
		}

		immigration 	*= UnityEngine.Random.value > 0.5f ? -1f : 1f;
		publicSpending 	*= UnityEngine.Random.value > 0.5f ? -1f : 1f;
		civilRights 	*= UnityEngine.Random.value > 0.5f ? -1f : 1f;

		Debug.Log("Key: " + keyIdeal.ToString() + " - " + immigration + " " + publicSpending + " " + civilRights);
	}

	public void SetValue(IdealType type, float val)
	{
		switch (type)
		{
		case IdealType.Immigration:
			immigration = val;
			break;

		case IdealType.PublicSpending:
			publicSpending = val;
			break;

		case IdealType.CivilRights:
			civilRights = val;
			break;
		}
	}

	public float GetValue(IdealType type)
	{
		switch (type)
		{
		case IdealType.Immigration:
			return immigration;

		case IdealType.PublicSpending:
			return publicSpending;

		case IdealType.CivilRights:
			return civilRights;
		}

		return 0f;
	}
}
