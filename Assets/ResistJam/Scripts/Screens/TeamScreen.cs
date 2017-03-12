using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamScreen : MonoBehaviour
{
	protected void OnEnable()
	{
		AudioManager.Initialise();

		this.PerformAction(2f, () => {
			Navigation.GoToScreen(NavScreen.Title);
		});
	}
}
