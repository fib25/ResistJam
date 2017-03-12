using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkerTalker : AbstractIdealist
{
	protected Animator anim;

	protected int idleId;
	protected int walkId;
	protected int talkId;
	protected bool isWalking;
	protected bool isTalking;

	protected bool walkingLeft = true;
	[SerializeField]
	protected float walkSpeed = 1f;
	protected float talkTimer;

	protected override void Awake()
	{
		base.Awake();

		anim = GetComponentInChildren<Animator>(true);
		idleId = Animator.StringToHash("Idle");
		walkId = Animator.StringToHash("Walk");
		talkId = Animator.StringToHash("Talk");
	}

	protected override void Start()
	{
		base.Start();

		WalkLeft();
	}

	protected override void Update()
	{
		base.Update();

		if (isWalking)
		{
			this.transform.position += walkSpeed * Time.deltaTime * (walkingLeft ? Vector3.left : Vector3.right);

			if (this.transform.localPosition.x <= -6.8f)
			{
				WalkRight();
			}
			if (this.transform.localPosition.x >= -3.6f)
			{
				WalkLeft();
			}
		}
		else if (isTalking)
		{
			talkTimer -= Time.deltaTime;

			if (talkTimer <= 0f)
			{
				SetWalk();
			}
		}
	}

	public void DoAnnounce()
	{
		SetTalk();
	}

	/*protected void SetIdle()
	{
		isWalking = false;
		anim.SetTrigger(idleId);
	}*/

	protected void SetWalk()
	{
		isWalking = true;
		isTalking = false;
		anim.SetTrigger(walkId);
	}

	protected void SetTalk()
	{
		isWalking = false;
		isTalking = true;
		anim.SetTrigger(talkId);
		talkTimer = 2f;
	}

	protected void WalkLeft()
	{
		if (!isWalking)
		{
			SetWalk();
		}

		walkingLeft = true;

		Vector3 scale = this.transform.localScale;
		scale.x = Mathf.Abs(scale.x);
		this.transform.localScale = scale;
	}

	protected void WalkRight()
	{
		if (!isWalking)
		{
			SetWalk();
		}

		walkingLeft = false;

		Vector3 scale = this.transform.localScale;
		scale.x = -Mathf.Abs(scale.x);
		this.transform.localScale = scale;
	}
}
