using System;
using System.Collections;
using System.Collections.Generic;
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
}
