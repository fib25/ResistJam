using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamScreen : MonoBehaviour
{
	public void OnEnable()
	{
		this.PerformAction(2f, () => {
			Navigation.GoToScreen(NavScreen.Title);
		});
	}
}
