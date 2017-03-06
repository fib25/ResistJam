using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person : AbstractIdealist
{
	[Range(0f, 1f)]
	public float lean = 0.5f;
	[Header("Debug")]
	public bool showDebug = false;
	public GameObject debugDisplay;

	[HideInInspector]
	public float speed = 1f;

	protected Vector3 targetPos;
	protected BoundBox maxBounds;
	protected BoundBox localBounds;

	protected override void Awake()
	{
		base.Awake();

		RandomiseIdeals();

		BoxCollider2D personArea = GameObject.Find("CrowdArea").GetComponent<BoxCollider2D>();
		maxBounds = new BoundBox(personArea.bounds.center, personArea.bounds.size);

		localBounds = CalculateLocalBounds(lean);

		speed = GameSettings.Instance.PersonMaxSpeed;
	}

	protected override void Start()
	{
		base.Start();

		targetPos = this.transform.position;
	}

	protected override void Update()
	{
		base.Update();

		// Target area.
		localBounds = CalculateLocalBounds(lean);

		// Movement.
		if (targetPos.x > localBounds.Right)
		{
			targetPos.x -= 0.1f;
		}
		else if (targetPos.x < localBounds.Left)
		{
			targetPos.x += 0.1f;
		}
		else
		{
			targetPos.x += UnityEngine.Random.Range(-0.1f, 0.1f);
		}

		if (targetPos.y > localBounds.Top)
		{
			targetPos.y -= 0.1f;
		}
		else if (targetPos.y < localBounds.Bottom)
		{
			targetPos.y += 0.1f;
		}
		else
		{
			targetPos.y += UnityEngine.Random.Range(-0.1f, 0.1f);
		}

		Vector3 dir = (targetPos - this.transform.position).normalized;
		this.transform.position += dir * speed * Time.deltaTime;

		// Choose whether to show debug text.
		debugDisplay.SetActive(showDebug && GameSettings.Instance.ShowPersonDebug);
	}

	public void UpdateLean(Player player, Dictator dictator)
	{
		//Debug.Log("----------");

		float dictatorResult = Ideals.CompareIdeals(this.Ideals, dictator.Ideals);
		//Debug.Log("Dictator chi: " + dictatorResult);

		float playerResult = Ideals.CompareIdeals(this.Ideals, player.Ideals);
		//Debug.Log("Player chi: " + playerResult);

		lean = playerResult - dictatorResult;
	}

	protected BoundBox CalculateLocalBounds(float lean)
	{
		float yPos = Mathf.Lerp(maxBounds.Top, maxBounds.Bottom, Mathf.InverseLerp(-3f, 3f, lean));

		BoundBox newBounds = new BoundBox();
		newBounds.Center = new Vector3(maxBounds.Center.x, yPos, 0f);
		newBounds.Size = new Vector3(maxBounds.Size.x, maxBounds.Size.y * 0.1f, 0f);

		return newBounds;
	}

	protected void OnDrawGizmos()
	{
		// Target.
		Gizmos.color = Color.red;
		Gizmos.DrawSphere(targetPos, 0.1f);
		// Current.
		Gizmos.color = Color.green;
		Gizmos.DrawSphere(this.transform.position, 0.1f);
		// Lean.
		if (localBounds != null)
		{
			Gizmos.color = Color.grey;
			Gizmos.DrawLine(new Vector3(localBounds.Left, localBounds.Center.y, 0f), new Vector3(localBounds.Right, localBounds.Center.y, 0f));
		}
	}

	protected void OnDrawGizmosSelected()
	{
		// Local bounds.
		if (localBounds != null)
		{
			Gizmos.color = Color.blue;
			Gizmos.DrawWireCube(localBounds.Center, localBounds.Size);
		}
	}
}
