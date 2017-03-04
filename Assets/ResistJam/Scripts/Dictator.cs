using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dictator : MonoBehaviour
{
	[Range(-1f, 1f)]
	public float immigration;
	[Range(-1f, 1f)]
	public float publicSpending;
	[Range(-1f, 1f)]
	public float civilRights;

	protected Ideals _ideals;
	public Ideals Ideals { get { return _ideals; } }

	protected void Start()
	{
		_ideals = new Ideals();
	}

	public void Update()
	{
		_ideals.immigration = this.immigration;
		_ideals.publicSpending = this.publicSpending;
		_ideals.civilRights = this.civilRights;
	}
}
