using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NavScreen
{
	Team,
	Title,
	Instruction,
	Game,
	GameOver
}

public class Navigation : MonoBehaviour
{
	protected static Navigation _instance;
	public static Navigation Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = FindObjectOfType<Navigation>();
			}

			return _instance;
		}
	}

	public static NavScreen CurrentScreen { get { return Instance._currentScreen; } }

	public static void GoToScreen(NavScreen screen)
	{
		Instance.GoToScreenInternal(screen);
	}

	public GameObject[] screens;

	protected NavScreen _currentScreen;

	protected void GoToScreenInternal(NavScreen screen)
	{
		for (int i = 0; i < screens.Length; i++)
		{
			screens[i].SetActive(false);
		}

		screens[(int)screen].SetActive(true);

		_currentScreen = screen;
	}
}
