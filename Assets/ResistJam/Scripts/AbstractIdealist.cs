using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractIdealist : MonoBehaviour
{
	protected Ideals _ideals;
	public Ideals Ideals { get { return _ideals; } }

	protected virtual void Awake()
	{
		_ideals = new Ideals();
	}

	protected virtual void Start()
	{
		//
	}

	protected virtual void Update()
	{
		//
	}

	public virtual void RandomiseKeyIdeals()
	{
		_ideals.RandomiseKeyIdeals();
	}

	public virtual void RandomiseCurrentIdeals()
	{
		_ideals.RandomiseCurrentIdeals();
	}
}
