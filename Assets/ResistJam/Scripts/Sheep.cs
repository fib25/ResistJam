using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SheepState
{
	Idle,
	Wandering,
	MoveToLean,
	Newspaper
}

public class Sheep : AbstractIdealist
{
	[SerializeField]
	protected GameObject newspaperDisplay;

	[Header("Debug")]
	public bool showDebug = false;
	public GameObject debugDisplay;

	[HideInInspector]
	public bool allowLeanUpdate = true;

	protected float _lean;
	protected float prevLean;
	protected SheepState _state;
	protected SheepState _prevState;
	protected Vector3 targetPos;
	protected BoundBox maxBounds;
	protected BoundBox localBounds;
	protected bool goToNewspaperNext = false;

	protected float idleTimer;
	protected GameSettings settings;
	protected float halfScreenHeight;

	public float Lean { get { return _lean; } }
	public SheepState State { get { return _state; } }

	protected override void Awake()
	{
		base.Awake();

		settings = GameSettings.Instance;

		RandomiseKeyIdeals();

		BoxCollider2D sheepArea = GameObject.Find("CrowdArea").GetComponent<BoxCollider2D>();
		maxBounds = new BoundBox(sheepArea.bounds.center, sheepArea.bounds.size);
		localBounds = CalculateLocalBounds(_lean);

		halfScreenHeight = Camera.main.orthographicSize;
	}

	protected override void Start()
	{
		base.Start();

		// Target area.
		localBounds = CalculateLocalBounds(_lean);

		SetInitialPosition();
		prevLean = _lean;

		SetState(SheepState.Idle);
		idleTimer = UnityEngine.Random.Range(0f, settings.IdleTime);
		newspaperDisplay.SetActive(false);
	}

	protected override void Update()
	{
		base.Update();

		StateMachineUpdate();

		// Update z depth for sheep sorting.
		Vector3 pos = this.transform.position;
		pos.z = Mathf.Lerp(-10f, 0f, Mathf.InverseLerp(-halfScreenHeight, halfScreenHeight, pos.y));
		this.transform.position = pos;

		// Choose whether to show debug text.
		debugDisplay.SetActive(showDebug && GameSettings.Instance.ShowSheepDebug);
	}

	public void SetInitialPosition()
	{
		localBounds = CalculateLocalBounds(_lean);

		Vector3 startPos = Vector3.zero;
		startPos.x = UnityEngine.Random.Range(localBounds.Left * 0.25f, localBounds.Right * 0.25f);
		startPos.y = UnityEngine.Random.Range(localBounds.Top, localBounds.Bottom);

		this.transform.localPosition = startPos;
	}

	public void UpdateLean(Player player, Dictator dictator)
	{
		//Debug.Log("----------");

		float dictatorResult = Ideals.CompareIdeals(this.Ideals, dictator.Ideals);
		//Debug.Log("Dictator chi: " + dictatorResult);

		float playerResult = Ideals.CompareIdeals(this.Ideals, player.Ideals);
		//Debug.Log("Player chi: " + playerResult);

		_lean = playerResult - dictatorResult;
	}

	public void SetState(SheepState newState)
	{
		//Debug.Log("Set new state: " + newState.ToString());

		_prevState = _state;
		_state = newState;

		if (goToNewspaperNext && _prevState == SheepState.MoveToLean)
		{
			SetState(SheepState.Newspaper);
			goToNewspaperNext = false;
			return;
		}

		if (_prevState == SheepState.Newspaper)
		{
			newspaperDisplay.SetActive(false);
		}

		if (newState == SheepState.Idle)
		{
			if (_prevState == SheepState.MoveToLean || _prevState == SheepState.Newspaper)
			{
				idleTimer = UnityEngine.Random.Range(0f, settings.IdleTime);
			}
			else
			{
				idleTimer = settings.IdleTime;
			}

			// TODO: Change to eat sprite? Or do something.
		}
		else if (newState == SheepState.Wandering)
		{
			float x = UnityEngine.Random.Range(this.transform.position.x - settings.WanderRange, this.transform.position.x + settings.WanderRange);
			float y = UnityEngine.Random.Range(localBounds.Top, localBounds.Bottom);

			if (x > localBounds.Right)
			{
				x = UnityEngine.Random.Range(this.transform.position.x - settings.WanderRange, this.transform.position.x - (settings.WanderRange * 2f));
			}
			else if (x < localBounds.Left)
			{
				x = UnityEngine.Random.Range(this.transform.position.x + settings.WanderRange, this.transform.position.x + (settings.WanderRange * 2f));
			}

			targetPos = new Vector3(x, y, 0f);
		}
		else if (newState == SheepState.MoveToLean)
		{
			// Target area.
			localBounds = CalculateLocalBounds(_lean);

			targetPos = CalculateTargetPosition();

			// TODO: Change to rushing sprite? Or do something.
		}
		else if (newState == SheepState.Newspaper)
		{
			// Change sprite to newspaper reading!
			if (!goToNewspaperNext && _prevState == SheepState.MoveToLean)
			{
				goToNewspaperNext = true;
				_state = SheepState.MoveToLean;
				return;
			}

			newspaperDisplay.SetActive(true);

			/*this.PerformAction(settings.NewspaperReadTime, () => {
				newspaperDisplay.SetActive(false);
				this.SetState(SheepState.MoveToLean);
			});*/
		}
	}

	protected void StateMachineUpdate()
	{
		if (_lean != prevLean)
		{
			SetState(SheepState.MoveToLean);
		}

		switch (_state)
		{
		case SheepState.Idle:
			IdleStateUpdate();
			break;

		case SheepState.Wandering:
			WanderStateUpdate();
			break;

		case SheepState.MoveToLean:
			MoveToLeanStateUpdate();
			break;

		case SheepState.Newspaper:
			//
			break;
		}

		prevLean = _lean;
	}

	protected void IdleStateUpdate()
	{
		idleTimer -= Time.deltaTime;

		if (idleTimer <= 0f)
		{
			SetState(SheepState.Wandering);
		}
	}

	protected void WanderStateUpdate()
	{
		MoveToTargetPos(settings.WanderSpeed);
	}

	protected void MoveToLeanStateUpdate()
	{
		MoveToTargetPos(settings.MoveToLeanSpeed);
	}

	protected void MoveToTargetPos(float speed)
	{
		Vector3 targetVec = targetPos - this.transform.position;
		targetVec.z = 0f;
		Vector3 moveVec = targetVec.normalized * speed * Time.deltaTime;
		moveVec.z = 0f;

		if (targetVec.sqrMagnitude < moveVec.sqrMagnitude)
		{
			this.transform.position = targetPos;
			if (goToNewspaperNext)
			{
				SetState(SheepState.Newspaper);
			}
			else
			{
				SetState(SheepState.Idle);
			}
		}
		else
		{
			this.transform.position += moveVec;
		}
	}

	protected BoundBox CalculateLocalBounds(float lean)
	{
		float yPos = Mathf.Lerp(maxBounds.Top, maxBounds.Bottom, Mathf.InverseLerp(-3f, 3f, lean));

		BoundBox newBounds = new BoundBox();
		newBounds.Center = new Vector3(maxBounds.Center.x, yPos, 0f);
		newBounds.Size = new Vector3(maxBounds.Size.x, maxBounds.Size.y * 0.1f, 0f);

		return newBounds;
	}

	protected Vector3 CalculateTargetPosition()
	{
		float x = UnityEngine.Random.Range(this.transform.position.x - settings.WanderRange, this.transform.position.x + settings.WanderRange);
		float y = UnityEngine.Random.Range(localBounds.Top, localBounds.Bottom);

		if (x > localBounds.Right)
		{
			x = UnityEngine.Random.Range(this.transform.position.x - settings.WanderRange, this.transform.position.x - (settings.WanderRange * 2f));
		}
		else if (x < localBounds.Left)
		{
			x = UnityEngine.Random.Range(this.transform.position.x + settings.WanderRange, this.transform.position.x + (settings.WanderRange * 2f));
		}

		return new Vector3(x, y, 0f);
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
