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
	protected List<Person> allPeople = new List<Person>();
	protected UIPlayerControls playerControls;

	protected float roundTimer;
	protected bool gameRunning = false;

	protected void Awake()
	{
		dictator = FindObjectOfType<Dictator>();
		player = FindObjectOfType<Player>();
		playerControls = FindObjectOfType<UIPlayerControls>();
		playerControls.CardSelectedEvent += OnCardSelected;

		crowdTransform = GameObject.Find("Crowd").transform;
	}

	protected void Start()
	{
		InitCrowd();
		InitDictator();
		InitPlayer();

		playerControls.SetAllPeople(allPeople);

		roundTimer = GameSettings.Instance.RoundTime;
		gameRunning = true;
	}

	protected void InitCrowd()
	{
		for (int i = 0; i < GameSettings.Instance.CrowdSize; i++)
		{
			Person inst = GameObject.Instantiate<Person>(personPrefab);
			inst.transform.SetParent(crowdTransform);
			inst.transform.localPosition = Vector3.zero;

			inst.lean = UnityEngine.Random.Range(0.4f, 0.6f);

			allPeople.Add(inst);
		}
	}

	protected void InitDictator()
	{
		dictator.RandomiseKeyIdeals();
	}

	protected void InitPlayer()
	{
		//player.RandomiseIdeals();
	}

	protected void Update()
	{
		if (gameRunning)
		{
			for (int i = 0; i < allPeople.Count; i++)
			{
				allPeople[i].UpdateLean(player, dictator);
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

	protected void OnCardSelected(Card card)
	{
		player.Ideals.AddToIdealValue(card.idealType, card.value);
	}
}
