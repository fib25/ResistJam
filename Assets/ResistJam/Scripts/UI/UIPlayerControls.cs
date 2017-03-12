using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIPlayerControls : MonoBehaviour
{
	public event System.Action<Card> CardSelectedEvent = delegate { };

	//public Slider aSlider;
	//public Slider bSlider;
	//public Slider cSlider;

	[SerializeField]
	protected GameObject getReadyMessage;
	[SerializeField]
	protected GameObject inGame;
	[SerializeField]
	protected GameObject cardSelectTimerDisplay;

	protected UICard[] cardUis;
	protected List<Card> currentCards = new List<Card>();
	protected Llama player;
	protected Wolf dictator;
	protected List<Sheep> allSheep;

	protected bool isActive = false;
	protected UICard selectedCardUi;
	protected float cardSelectTimer;
	protected float cardSelectTimerMaxScale;

	protected void Awake()
	{
		player = FindObjectOfType<Llama>();
		dictator = FindObjectOfType<Wolf>();
		cardUis = GetComponentsInChildren<UICard>(true);

		for (int i = 0; i < cardUis.Length; i++)
		{
			cardUis[i].cardIndex = i;
			cardUis[i].CardPressed += OnCardPressed;
		}

		cardSelectTimerMaxScale = cardSelectTimerDisplay.transform.localScale.x;
	}

	protected void Start()
	{
		getReadyMessage.SetActive(true);
		inGame.SetActive(false);

		ResetCardSelectTimer();
		ShowNewCards();
	}

	protected void OnDestroy()
	{
		for (int i = 0; i < cardUis.Length; i++)
		{
			cardUis[i].CardPressed -= OnCardPressed;
		}
	}

	protected void Update()
	{
		if (!isActive) return;

		cardSelectTimer -= Time.deltaTime;
		if (cardSelectTimer <= 0f)
		{
			ConfirmCardSelection();
			ResetCardSelectTimer();
		}

		UpdateTimerScale();
	}

	public void SetAllSheep(List<Sheep> allSheep)
	{
		this.allSheep = allSheep;
	}

	public void Show()
	{
		this.gameObject.SetActive(true);
	}

	public void Hide()
	{
		this.gameObject.SetActive(false);
	}

	public void StartShowingCards()
	{
		isActive = true;

		getReadyMessage.SetActive(false);
		inGame.SetActive(true);

		ResetCardSelectTimer();
		ShowNewCards();
	}

	public void ResetCardSelectTimer()
	{
		cardSelectTimer = GameSettings.Instance.CardSelectTime;

		Vector3 s = cardSelectTimerDisplay.transform.localScale;
		s.x = cardSelectTimerMaxScale;
		cardSelectTimerDisplay.transform.localScale = s;

		UpdateTimerScale();
	}

	public void ShowNewCards()
	{
		ResetCardSelectTimer();

		if (selectedCardUi != null)
		{
			selectedCardUi.UnHighlight();
			selectedCardUi = null;
		}

		currentCards.Clear();

		for (int i = 0; i < cardUis.Length; i++)
		{
			if (!CardCollection.Instance.HasCardsInPool())
			{
				CardCollection.Instance.FillCardPool();
			}

			Card card = CardCollection.Instance.GetRandomCardFromPool();

			int agreeCount = 0;
			for (int j = 0; j < allSheep.Count; j++)
			{
				Sheep p = allSheep[j];
				float pIdealVal = p.Ideals.GetIdealValue(card.idealType);

				if (card.value > 0 && pIdealVal > 0)
				{
					agreeCount++;
				}
				else if (card.value < 0 && pIdealVal < 0)
				{
					agreeCount++;
				}
			}

			int agreePercent = (int)(((float)agreeCount / (float)allSheep.Count) * 100f);

			currentCards.Add(card);
			cardUis[i].SetCardDetails(card, agreePercent);
		}
	}

	protected void ConfirmCardSelection()
	{
		if (selectedCardUi != null)
		{
			CardSelectedEvent(currentCards[selectedCardUi.cardIndex]);
		}

		ShowNewCards();
	}

	protected void UpdateTimerScale()
	{
		// Update timer scale.
		Vector3 s = cardSelectTimerDisplay.transform.localScale;
		s.x = (cardSelectTimer / GameSettings.Instance.CardSelectTime) * cardSelectTimerMaxScale;
		cardSelectTimerDisplay.transform.localScale = s;
	}

	public void OnCardPressed(UICard cardUi)
	{
		if (selectedCardUi != null)
		{
			selectedCardUi.UnHighlight();
		}

		selectedCardUi = cardUi;
		cardUi.Highlight();

		AudioManager.PlaySFX("ui-select");

		/*Card lowestCard = null;
		float lowestScore = float.MaxValue;

		// Get the lowest card score to add to the dictators score.
		for (int i = 0; i < cardUis.Length; i++)
		{
			if (cardUis[i].cardIndex != cardUi.cardIndex)
			{
				Card card = currentCards[cardUis[i].cardIndex];
				if (card.value < lowestScore)
				{
					lowestScore = card.value;
					lowestCard = card;
				}
			}
		}
		Debug.Log("Update dictator " + lowestCard.idealType.ToString() + " " + lowestCard.value);
		dictator.Ideals.AddToIdealValue(lowestCard.idealType, lowestCard.value);*/
	}
}
