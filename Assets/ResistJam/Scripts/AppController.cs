using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppController : MonoBehaviour
{
	protected void Awake()
	{
		#if UNITY_EDITOR
		if (GameSettings.Instance.EnableDebugScreen)
		{
			Navigation.GoToScreen(GameSettings.Instance.DebugScreen);
			return;
		}
		#endif

		if (Globals.replayToTitleScreen)
		{
			Navigation.GoToScreen(NavScreen.Title);
		}
		else
		{
			Navigation.GoToScreen(NavScreen.Team);
		}
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
