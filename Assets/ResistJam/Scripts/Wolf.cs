using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : WalkerTalker
{
	protected override void Start()
	{
		base.Start();

		WalkRight();
	}
}
