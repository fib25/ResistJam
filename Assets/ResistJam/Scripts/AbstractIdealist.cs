using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractIdealist : MonoBehaviour
{
	public float policyA;
	public float policyB;
	public float policyC;

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
		_ideals.SetIdealValue(IdealType.A, this.policyA);
		_ideals.SetIdealValue(IdealType.B, this.policyB);
		_ideals.SetIdealValue(IdealType.C, this.policyC);
	}

	public virtual void RandomiseIdeals()
	{
		_ideals.Randomise();

		UpdatePublicVars();
	}

	protected void UpdatePublicVars()
	{
		this.policyA = _ideals.GetIdealValue(IdealType.A);
		this.policyB = _ideals.GetIdealValue(IdealType.B);
		this.policyC = _ideals.GetIdealValue(IdealType.C);
	}
}
