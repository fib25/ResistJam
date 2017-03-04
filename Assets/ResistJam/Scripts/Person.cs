using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person : MonoBehaviour
{
	public float speed = 1f;
	[Range(0f, 1f)]
	public float lean = 0.5f;

	protected Vector3 targetPos;
	protected BoundBox maxBounds;
	protected BoundBox localBounds;

	protected Ideals _ideals;
	public Ideals Ideals { get { return _ideals; } }

	protected void Awake()
	{
		BoxCollider2D personArea = GameObject.Find("CrowdArea").GetComponent<BoxCollider2D>();
		maxBounds = new BoundBox(personArea.bounds.center, personArea.bounds.size);

		_ideals = new Ideals();
		_ideals.Randomise();

		localBounds = CalculateLocalBounds(lean);
	}

	protected void Start()
	{
		targetPos = this.transform.position;
	}

	protected void FixedUpdate()
	{
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
	}

	protected void OnDrawGizmosSelected()
	{
		// Local bounds.
		Gizmos.color = Color.blue;
		Gizmos.DrawWireCube(localBounds.Center, localBounds.Size);
	}
}
