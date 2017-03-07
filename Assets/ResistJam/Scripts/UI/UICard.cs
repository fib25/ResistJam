using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UICard : MonoBehaviour
{
	public event System.Action<UICard> CardPressed = delegate { };

	public Text messageText;
	public Text helperText;

	[HideInInspector]
	public int cardIndex;

	protected string helperTextFormat;

	protected void Awake()
	{
		Button button = GetComponent<Button>();
		button.onClick.AddListener(OnCardClicked);

		helperTextFormat = helperText.text;
	}

	protected void OnDestroy()
	{
		Button button = GetComponent<Button>();
		button.onClick.RemoveListener(OnCardClicked);
	}

	public void SetCardDetails(Card card, int agreePercent)
	{
		messageText.text = card.message;

		if (GameSettings.Instance.ShowValuesOnCards)
		{
			string operatorChar = (card.value < 0) ? "-" : "+";
			messageText.text += " (" + card.idealType.ToString() + operatorChar + Mathf.Abs(card.value).ToString("#0") + ")";
		}

		helperText.text = string.Format(helperTextFormat, agreePercent.ToString());
	}

	protected void OnCardClicked()
	{
		CardPressed(this);
	}
}
