using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person : AbstractIdealist
{
	public float speed = 1f;
	[Range(0f, 1f)]
	public float lean = 0.5f;
	[Header("Debug")]
	public bool showDebug = false;
	public GameObject debugDisplay;

	protected Vector3 targetPos;
	protected BoundBox maxBounds;
	protected BoundBox localBounds;

	[HideInInspector]
	public float driftAmount;
	[HideInInspector]
	public float driftSpeed;

	protected override void Awake()
	{
		base.Awake();

		_ideals.Randomise();
		immigration = _ideals.immigration;
		publicSpending = _ideals.publicSpending;
		civilRights = _ideals.civilRights;

		BoxCollider2D personArea = GameObject.Find("CrowdArea").GetComponent<BoxCollider2D>();
		maxBounds = new BoundBox(personArea.bounds.center, personArea.bounds.size);

		localBounds = CalculateLocalBounds(lean);
	}

	protected override void Start()
	{
		base.Start();

		targetPos = this.transform.position;
	}

	protected override void Update()
	{
		base.Update();

		lean = Mathf.Clamp01(lean);

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

		float maxChi = GameSettings.Instance.MaxChi;

		float dictatorDrift = -Mathf.Clamp(maxChi - dictatorResult, 0f, maxChi);
		float playerDrift = Mathf.Clamp(maxChi - playerResult, 0f, maxChi);
		// DEBUG testing the numbers.
		//dictatorDrift = -7f;
		//playerDrift = 9f;
		//Debug.Log("Dicator drift: " + dictatorDrift);
		//Debug.Log("Player drift: " + playerDrift);
		driftAmount = dictatorDrift + playerDrift;
		//Debug.Log("Drift = " + driftAmount);

		driftSpeed = (GameSettings.Instance.MaxDriftSpeed / maxChi) * driftAmount;

		lean += driftSpeed * Time.deltaTime;
	}

	protected BoundBox CalculateLocalBounds(float lean)
	{
		BoundBox newBounds = new BoundBox();
		newBounds.Center = new Vector3(maxBounds.Center.x, Mathf.Lerp(maxBounds.Top, maxBounds.Bottom, lean), 0f);
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
