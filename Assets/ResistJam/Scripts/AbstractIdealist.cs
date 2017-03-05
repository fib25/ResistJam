using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractIdealist : MonoBehaviour
{
	[Range(-1f, 1f)]
	public float immigration;
	[Range(-1f, 1f)]
	public float publicSpending;
	[Range(-1f, 1f)]
	public float civilRights;

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
		_ideals.immigration = this.immigration;
		_ideals.publicSpending = this.publicSpending;
		_ideals.civilRights = this.civilRights;
	}

	public virtual void RandomiseIdeals()
	{
		_ideals.Randomise();

		this.immigration = _ideals.immigration;
		this.publicSpending = _ideals.publicSpending;
		this.civilRights = _ideals.civilRights;
	}
}
