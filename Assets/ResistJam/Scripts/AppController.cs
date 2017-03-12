using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppController : MonoBehaviour
{
	public void Awake()
	{
		Navigation.GoToScreen(NavScreen.Team);
	}
}
