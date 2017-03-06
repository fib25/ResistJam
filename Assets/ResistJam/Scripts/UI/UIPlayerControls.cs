using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerControls : MonoBehaviour
{
	public event System.Action<Card> CardSelectedEvent = delegate { };

	//public Slider aSlider;
	//public Slider bSlider;
	//public Slider cSlider;

	protected UICard[] cardUis;
	protected List<Card> currentCards = new List<Card>();
	protected Player player;
	protected List<Person> allPeople;

	protected void Awake()
	{
		player = FindObjectOfType<Player>();
		cardUis = FindObjectsOfType<UICard>();

		for (int i = 0; i < cardUis.Length; i++)
		{
			cardUis[i].cardIndex = i;
			cardUis[i].CardPressed += OnCardPressed;
		}
	}

	protected void Start()
	{
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
		//player.policyA = aSlider.value;
		//player.policyB = bSlider.value;
		//player.policyC = cSlider.value;
	}

	public void SetAllPeople(List<Person> allPeople)
	{
		this.allPeople = allPeople;
	}

	protected void ShowNewCards()
	{
		currentCards.Clear();

		for (int i = 0; i < cardUis.Length; i++)
		{
			if (!CardCollection.Instance.HasCardsInPool())
			{
				CardCollection.Instance.FillCardPool();
			}

			Card card = CardCollection.Instance.GetRandomCardFromPool();

			int agreeCount = 0;
			for (int j = 0; j < allPeople.Count; j++)
			{
				Person p = allPeople[j];
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

			int agreePercent = (int)(((float)agreeCount / (float)allPeople.Count) * 100f);

			currentCards.Add(card);
			cardUis[i].SetCardDetails(card, agreePercent);
		}
	}

	public void OnCardPressed(UICard cardUi)
	{
		CardSelectedEvent(currentCards[cardUi.cardIndex]);

		ShowNewCards();
	}
}
