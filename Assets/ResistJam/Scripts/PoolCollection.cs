using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolCollection<T, U> : ScriptableObject where U : PoolCollection<T, U>
{
	//public static virtual string ResourcePath { get { return string.Empty; } }

	private static U _collection;
	public static U Instance
	{
		get
		{
			if (_collection == null)
			{
				//_collection = Resources.Load<T>(ResourcePath);
				_collection.FillPool();
			}

			return _collection;
		}
	}

	public List<T> items = new List<T>();

	protected List<T> pool = new List<T>();

	public void FillPool()
	{
		pool.Clear();

		for (int i = 0; i < items.Count; i++)
		{
			pool.Add(items[i]);
		}
	}

	public T GetRandomFromPool()
	{
		T item = pool[UnityEngine.Random.Range(0, pool.Count)];
		pool.Remove(item);
		return item;
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
