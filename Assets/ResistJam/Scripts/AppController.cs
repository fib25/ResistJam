using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppController : MonoBehaviour
{
	protected void Awake()
	{
		Navigation.GoToScreen(NavScreen.Team);
	}

	protected void Update()
	{
		#if UNITY_STANDALONE
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}
		#endif
	}
}
