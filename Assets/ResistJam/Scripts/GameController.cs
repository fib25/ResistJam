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
				people[i].UpdateLean(player, dictator);
			}

			roundTimer -= Time.deltaTime;
			timerText.text = roundTimer.ToString("00");

			if (roundTimer <= 0f)
			{
				CompleteGame();
			}
		}
	}

	protected void CompleteGame()
	{
		//gameRunning = false;
	}
}
