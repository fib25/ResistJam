using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreen : MonoBehaviour
{
	
	public void OnEnable()
	{
		AudioManager.PlayMusic("intro-help music loop", true);

		this.PerformAction(5f, NextScreen);
	}

	public void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			NextScreen();
		}
	}

	protected void NextScreen()
	{
		AudioManager.PlaySFX("ui-select");
		Navigation.GoToScreen(NavScreen.Instruction);
	}
}
