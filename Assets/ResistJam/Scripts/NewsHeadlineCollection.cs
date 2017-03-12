using System.Collections.Generic;
using UnityEngine;

public class NewsHeadlineCollection : ScriptableObject
{
	private static NewsHeadlineCollection newsHeadlineCollection;
	public static NewsHeadlineCollection Instance
	{
		get
		{
			if (newsHeadlineCollection == null)
			{
				newsHeadlineCollection = Resources.Load<NewsHeadlineCollection>("NewsHeadlineCollection");
				newsHeadlineCollection.FillPool();
			}

			return newsHeadlineCollection;
		}
	}

	public List<NewsHeadline> items = new List<NewsHeadline>();

	protected List<NewsHeadline> pool = new List<NewsHeadline>();

	public void FillPool()
	{
		pool.Clear();

		for (int i = 0; i < items.Count; i++)
		{
			pool.Add(items[i]);
		}
	}

	public NewsHeadline GetRandomFromPool()
	{
		NewsHeadline item = pool[UnityEngine.Random.Range(0, pool.Count)];
		pool.Remove(item);
		return item;
	}

	public NewsHeadline GetRandomByIdealType(IdealType idealType)
	{
		List<NewsHeadline> validHeadlines = new List<NewsHeadline>();

		for (int i = 0; i < pool.Count; i++)
		{
			if (pool[i].idealType == idealType)
			{
				validHeadlines.Add(pool[i]);
			}
		}

		NewsHeadline returnHeadline = null;

		if (validHeadlines.Count > 0)
		{
			returnHeadline = validHeadlines[UnityEngine.Random.Range(0, validHeadlines.Count)];
		}
		else
		{
			returnHeadline = pool[UnityEngine.Random.Range(0, pool.Count)];
		}

		pool.Remove(returnHeadline);

		return returnHeadline;
	}

	public int NumberOfItemsInPool()
	{
		return pool.Count;
	}

	public bool HasItemsInPool()
	{
		return pool.Count > 0;
	}
}
