using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardCollection : ScriptableObject
{
	private static CardCollection _cardCollection;
	public static CardCollection Instance
	{
		get
		{
			if (_cardCollection == null)
			{
				_cardCollection = Resources.Load<CardCollection>("CardCollection");
				_cardCollection.FillCardPool();
			}

			return _cardCollection;
		}
	}

	public List<Card> cards = new List<Card>();

	protected List<Card> cardPool = new List<Card>();

	public void FillCardPool()
	{
		cardPool.Clear();

		for (int i = 0; i < cards.Count; i++)
		{
			cardPool.Add(cards[i]);
		}
	}

	public Card GetRandomCardFromPool()
	{
		Card card = cardPool[UnityEngine.Random.Range(0, cardPool.Count)];
		cardPool.Remove(card);
		return card;
	}

	public int NumberOfCardsInPool()
	{
		return cardPool.Count;
	}

	public bool HasCardsInPool()
	{
		return cardPool.Count > 0;
	}
}
