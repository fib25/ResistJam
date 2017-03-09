﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum IdealType
{
	A,
	B,
	C
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
		idealsDict.Add(IdealType.A, 0);
		idealsDict.Add(IdealType.B, 0);
		idealsDict.Add(IdealType.C, 0);
	}

	public void RandomiseKeyIdeals()
	{
		KeyIdeal = (IdealType)UnityEngine.Random.Range(0, 3);

		RandomiseCurrentIdeals();
	}

	public void RandomiseCurrentIdeals()
	{
		RandomiseIdealValue(IdealType.A);
		RandomiseIdealValue(IdealType.B);
		RandomiseIdealValue(IdealType.C);

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
