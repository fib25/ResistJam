using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionsScreen : MonoBehaviour
{
	public void OnNextButtonPressed()
	{
		Navigation.GoToScreen(NavScreen.Game);
	}
}
