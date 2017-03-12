using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionsScreen : MonoBehaviour
{
	public void OnNextButtonPressed()
	{
		AudioManager.PlaySFX("ui-select");
		Navigation.GoToScreen(NavScreen.Game);
	}
}
