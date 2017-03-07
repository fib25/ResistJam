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
		InitDictator();
		InitPlayer();
		InitCrowd();

		playerControls.SetAllPeople(allPeople);

		roundTimer = GameSettings.Instance.RoundTime;
		UpdateTimerDisplay();

		this.PerformAction(2f, StartGame);
	}

	protected void InitCrowd()
	{
		for (int i = 0; i < GameSettings.Instance.CrowdSize; i++)
		{
			Person newPerson = GameObject.Instantiate<Person>(personPrefab);
			newPerson.name = "Person_" + i.ToString("000");
			newPerson.transform.SetParent(crowdTransform);

			newPerson.UpdateLean(player, dictator);
			newPerson.SetInitialPosition();

			allPeople.Add(newPerson);
		}
	}

	protected void InitDictator()
	{
		//dictator.RandomiseKeyIdeals();
	}

	protected void InitPlayer()
	{
		//player.RandomiseIdeals();
	}

	protected void StartGame()
	{
		gameRunning = true;
	}

	protected void CompleteGame()
	{
		//gameRunning = false;
	}

	protected void Update()
	{
		if (gameRunning)
		{
			for (int i = 0; i < allPeople.Count; i++)
			{
				if (allPeople[i].allowLeanUpdate)
				{
					allPeople[i].UpdateLean(player, dictator);
				}

				if (allPeople[i].lean <= -5f || allPeople[i].lean >= 5f)
				{
					Debug.Log("Stop lean " + allPeople[i].name + "!");
					allPeople[i].allowLeanUpdate = false;
				}
			}

			roundTimer -= Time.deltaTime;
			if (roundTimer < 0f) roundTimer = 0f;

			UpdateTimerDisplay();

			if (roundTimer <= 0f)
			{
				CompleteGame();
			}
		}
	}

	protected void UpdateTimerDisplay()
	{
		timerText.text = Mathf.FloorToInt(roundTimer / 60f) + ":" + Mathf.FloorToInt(roundTimer % 60f).ToString("00");
	}

	protected void OnCardSelected(Card card)
	{
		player.Ideals.AddToIdealValue(card.idealType, card.value);
		dictator.Ideals.AddToIdealValue(card.idealType, -card.value);
	}
}
