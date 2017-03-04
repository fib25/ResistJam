using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
	public Person personPrefab;
	public UnityEngine.UI.Text timerText;

	protected Transform crowdTransform;
	protected Dictator dictator;
	protected Player player;
	protected List<Person> people = new List<Person>();

	protected float roundTimer;
	protected bool gameRunning = false;

	protected void Awake()
	{
		dictator = FindObjectOfType<Dictator>();
		player = FindObjectOfType<Player>();

		crowdTransform = GameObject.Find("Crowd").transform;
	}

	protected void Start()
	{
		InitCrowd();
		InitDictator();
		InitPlayer();

		roundTimer = GameSettings.Instance.RoundTime;
		gameRunning = true;
	}

	protected void InitCrowd()
	{
		for (int i = 0; i < GameSettings.Instance.CrowdSize; i++)
		{
			Person inst = GameObject.Instantiate<Person>(personPrefab);
			inst.transform.SetParent(crowdTransform);

			inst.lean = UnityEngine.Random.Range(0.4f, 0.6f);

			people.Add(inst);
		}
	}

	protected void InitDictator()
	{
		dictator.RandomiseIdeals();
	}

	protected void InitPlayer()
	{

	}

	protected void Update()
	{
		if (gameRunning)
		{
			for (int i = 0; i < people.Count; i++)
			{
				UpdateLean(people[i]);
			}

			roundTimer -= Time.deltaTime;
			timerText.text = roundTimer.ToString("00");

			if (roundTimer <= 0f)
			{
				CompleteGame();
			}
		}
	}

	protected void UpdateLean(Person person)
	{
		//Debug.Log("----------");

		float dictatorResult = CompareIdeals(person.Ideals, dictator.Ideals);
		//Debug.Log("Dictator chi: " + dictatorResult);

		float playerResult = CompareIdeals(person.Ideals, player.Ideals);
		//Debug.Log("Player chi: " + playerResult);

		float maxChi = GameSettings.Instance.MaxChi;

		float dictatorDrift = -Mathf.Clamp(maxChi - dictatorResult, 0f, maxChi);
		float playerDrift = Mathf.Clamp(maxChi - playerResult, 0f, maxChi);
		// DEBUG testing the numbers.
		//dictatorDrift = -7f;
		//playerDrift = 9f;
		//Debug.Log("Dicator drift: " + dictatorDrift);
		//Debug.Log("Player drift: " + playerDrift);
		float driftAmount = dictatorDrift + playerDrift;
		//Debug.Log("Drift = " + driftAmount);

		person.lean += (GameSettings.Instance.MaxDriftSpeed / maxChi) * driftAmount * Time.deltaTime;
	}

	protected float CompareIdeals(Ideals a, Ideals b)
	{
		float chi = Mathf.Abs((Mathf.Pow((b.immigration - a.immigration), 2f) / a.immigration) +
							  (Mathf.Pow((b.civilRights - a.civilRights), 2f) / a.civilRights) +
							  (Mathf.Pow((b.publicSpending - a.publicSpending), 2f) / a.publicSpending));

		return chi;
	}

	protected void CompleteGame()
	{
		//gameRunning = false;
	}
}
