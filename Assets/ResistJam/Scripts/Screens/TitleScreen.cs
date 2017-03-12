using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreen : MonoBehaviour
{
	
	public void OnEnable()
	{
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
		Navigation.GoToScreen(NavScreen.Instruction);
	}
}
