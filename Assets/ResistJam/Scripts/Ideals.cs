using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public enum IdealType
{
	[Description("Environment")]
	Environment,
	[Description("Civil Rights")]
	CivilRights,
	[Description("Public Services")]
	PublicServices,
	[Description("Foreign Policy")]
	ForeignPolicy,
	[Description("Economy")]
	Economy,
	[Description("Science & Culture")]
	ScienceAndCulture
}

public enum IdealLean
{
	Negative = -1,
	Neutral = 0,
	Positive = 1
}

public class Ideals
{
	public Dictionary<IdealType, float> idealsDict = new Dictionary<IdealType, float>();

	public IdealType KeyIdeal { get; private set; }

	public static float CompareIdeals(Ideals sheep, Ideals other)
	{
		float result = 0;

		foreach (IdealType idealType in sheep.idealsDict.Keys)
		{
			if (sheep.idealsDict[idealType] > 0)
			{
				if (other.idealsDict[idealType] >= sheep.idealsDict[idealType])
				{
					result++;
				}
			}
			else
			{
				if (other.idealsDict[idealType] <= sheep.idealsDict[idealType])
				{
					result++;
				}
			}
		}

		return result;
	}

	public Ideals()
	{
		// Initialise the ideals dictionary.
		IdealType[] allIdealTypes = Utility.GetEnumValues<IdealType>();
		for (int i = 0; i < allIdealTypes.Length; i++)
		{
			idealsDict.Add(allIdealTypes[i], 0f);
		}
	}

	public void RandomiseKeyIdeals()
	{
		int idealsCount = Utility.GetEnumValues<IdealType>().Length;
		KeyIdeal = (IdealType)UnityEngine.Random.Range(0, idealsCount);

		RandomiseCurrentIdeals();
	}

	public void RandomiseCurrentIdeals()
	{
		IdealType[] allIdealTypes = Utility.GetEnumValues<IdealType>();
		for (int i = 0; i < allIdealTypes.Length; i++)
		{
			RandomiseIdealValue(allIdealTypes[i]);
		}

		//Debug.Log("Key: " + keyIdeal.ToString() + " - " + idealsDict[IdealType.A] + " " + idealsDict[IdealType.B] + " " + idealsDict[IdealType.C]);
	}

	public void RandomiseIdealValue(IdealType idealType)
	{
		float newVal = 0f;

		if (KeyIdeal == idealType)
		{
			newVal = RandomValue(4, 5);
		}
		else
		{
			newVal = RandomValue(1, 3);
		}

		idealsDict[idealType] = newVal;
	}

	public float GetIdealValue(IdealType idealType)
	{
		return idealsDict[idealType];
	}

	public void SetIdealValue(IdealType idealType, float value)
	{
		idealsDict[idealType] = Mathf.Clamp(value, -5f, 5f);
	}

	public IdealLean GetIdealLean(IdealType idealType)
	{
		float val = idealsDict[idealType];

		if (val == 0f) return IdealLean.Neutral;
		else if (val > 0f) return IdealLean.Positive;
		else return IdealLean.Negative;
	}

	public void AddToIdealValue(IdealType idealType, float value)
	{
		idealsDict[idealType] += value;
		idealsDict[idealType] = Mathf.Clamp(idealsDict[idealType], -5f, 5f);
	}

	protected float RandomValue(int min, int max)
	{
		return (float)UnityEngine.Random.Range(min, max + 1) * (UnityEngine.Random.value > 0.5f ? 1f : -1f);
	}
}
