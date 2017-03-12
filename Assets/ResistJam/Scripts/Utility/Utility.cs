using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using UnityEngine;

public static class Utility
{
	public static void PerformAction(this MonoBehaviour attach, float time, Action action)
	{
		attach.StartCoroutine(PerformActionRoutine(time, action));
	}

	private static IEnumerator PerformActionRoutine(float time, Action action)
	{
		yield return new WaitForSeconds(time);

		if (action != null)
		{
			action();
		}
	}

	public static T[] GetEnumValues<T>(params T[] excludeValues)
	{
		T[] values = (T[])Enum.GetValues(typeof(T));
		List<T> output = new List<T>();

		for (int i = 0; i < values.Length; i++)
		{
			bool excluded = false;
			for (int j = 0; j < excludeValues.Length; j++)
			{
				if (values[i].Equals(excludeValues[j]))
				{
					excluded = true;
					break;
				}
			}

			if (!excluded) output.Add(values[i]);
		}

		return output.ToArray();
	}

	public static string GetDescription(this Enum enumValue)
	{
		string enumValueAsString = enumValue.ToString();

		var type = enumValue.GetType();
		FieldInfo fieldInfo = type.GetField(enumValueAsString);
		object[] attributes = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

		if (attributes.Length > 0)
		{
			var attribute = (DescriptionAttribute)attributes[0];
			return attribute.Description;
		}

		return enumValueAsString;
	}
}
