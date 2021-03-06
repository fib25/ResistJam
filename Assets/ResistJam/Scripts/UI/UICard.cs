﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Button))]
public class UICard : MonoBehaviour
{
	public event System.Action<UICard> CardPressed = delegate { };

	public Text headerText;
	public Text messageText;
	public Text helperText;
	public Image headerImage;
	public Sprite[] headerSprites;

	[HideInInspector]
	public int cardIndex;

	protected Button button;
	protected const string PERCENTAGE_TEXT_FORMAT = "{0}% of sheep like this";

	protected void Awake()
	{
		button = GetComponent<Button>();
		button.onClick.AddListener(OnCardClicked);
	}

	protected void OnDestroy()
	{
		Button button = GetComponent<Button>();
		button.onClick.RemoveListener(OnCardClicked);
	}

	public void SetCardDetails(Card card, int agreePercent)
	{
		SetHeaderSprite(card.idealType);

		headerText.text = card.idealType.GetDescription();
		messageText.text = card.message;

		if (GameSettings.Instance.ShowValuesOnCards)
		{
			string operatorChar = (card.value < 0) ? "-" : "+";
			messageText.text += " (" + card.idealType.ToString() + operatorChar + Mathf.Abs(card.value).ToString("#0") + ")";
		}

		helperText.text = string.Format(PERCENTAGE_TEXT_FORMAT, agreePercent.ToString());
	}

	public void Highlight()
	{
		//this.GetComponent<Image>().color = Color.yellow * 1f;
	}

	public void UnHighlight()
	{
		//this.GetComponent<Image>().color = Color.white;

		// Deselect the button.
		BaseEventData dummyEvent = new BaseEventData(EventSystem.current);
		button.OnDeselect(dummyEvent);
	}

	protected void SetHeaderSprite(IdealType idealType)
	{
		headerImage.sprite = headerSprites[(int)idealType];
	}

	protected void OnCardClicked()
	{
		CardPressed(this);
	}
}
