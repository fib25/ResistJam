using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum IdealType
{
	A,
	B,
	C
}

public class Ideals
{
	public Dictionary<IdealType, float> idealsDict = new Dictionary<IdealType, float>();

	public IdealType keyIdeal;

	public static float CompareIdeals(Ideals person, Ideals other)
	{
		float result = 0;

		foreach (IdealType idealType in person.idealsDict.Keys)
		{
			if (person.idealsDict[idealType] > 0)
			{
				if (other.idealsDict[idealType] >= person.idealsDict[idealType])
				{
					result++;
				}
			}
			else
			{
				if (other.idealsDict[idealType] <= person.idealsDict[idealType])
				{
					result++;
				}
			}
		}

		return result;
	}

	protected void InitIdealValue(IdealType idealType)
	{
		float newVal = 0f;

		if (keyIdeal == idealType)
		{
			newVal = Rand(4, 5);
		}
		else
		{
			newVal = Rand(1, 3);
		}

		if (idealsDict.ContainsKey(idealType)) idealsDict[idealType] = newVal;
		else idealsDict[idealType] = newVal;
	}

	public void Randomise()
	{
		keyIdeal = (IdealType)UnityEngine.Random.Range(0, 3);

		InitIdealValue(IdealType.A);
		InitIdealValue(IdealType.B);
		InitIdealValue(IdealType.C);

		Debug.Log("Key: " + keyIdeal.ToString() + " - " + idealsDict[IdealType.A] + " " + idealsDict[IdealType.B] + " " + idealsDict[IdealType.C]);
	}

	public void SetIdealValue(IdealType idealType, float value)
	{
		if (idealsDict.ContainsKey(idealType)) idealsDict[idealType] = value;
		else idealsDict[idealType] = value;
	}

	public float GetIdealValue(IdealType idealType)
	{
		if (idealsDict.ContainsKey(idealType)) return idealsDict[idealType];
		else return 0f;
	}

	protected float Rand(int min, int max)
	{
		return (float)UnityEngine.Random.Range(min, max + 1) * (UnityEngine.Random.value > 0.5f ? 1f : -1f);
	}
}
