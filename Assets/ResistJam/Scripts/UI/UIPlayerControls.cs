using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerControls : MonoBehaviour
{
	public Slider immigrationSlider;
	public Slider publicSpendingSlider;
	public Slider civilRightsSlider;

	protected Player player;

	protected void Awake()
	{
		player = FindObjectOfType<Player>();
	}

	public void Update()
	{
		OnGoButton();
	}

	public void OnGoButton()
	{
		player.immigration = immigrationSlider.value;
		player.publicSpending = publicSpendingSlider.value;
		player.civilRights = civilRightsSlider.value;
	}
}
