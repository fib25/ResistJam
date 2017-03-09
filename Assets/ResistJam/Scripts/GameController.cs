using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
	public Sheep sheepPrefab;
	public UnityEngine.UI.Text timerText;

	protected Transform crowdTransform;
	protected Dictator dictator;
	protected Player player;
	protected List<Sheep> allSheep = new List<Sheep>();
	protected UIPlayerControls playerControls;
	protected UINewsHeadline newsHeadline;

	protected float roundTimer;
	protected bool gameRunning = false;

	protected void Awake()
	{
		dictator = FindObjectOfType<Dictator>();
		player = FindObjectOfType<Player>();
		playerControls = FindObjectOfType<UIPlayerControls>();
		playerControls.CardSelectedEvent += OnCardSelected;
		newsHeadline = FindObjectOfType<UINewsHeadline>();
		newsHeadline.HeadlineResolvedEvent += OnHeadlineResolved;
		newsHeadline.gameObject.SetActive(false);

		crowdTransform = GameObject.Find("Crowd").transform;
	}

	protected void Start()
	{
		InitDictator();
		InitPlayer();
		InitCrowd();

		playerControls.SetAllSheep(allSheep);

		roundTimer = GameSettings.Instance.RoundTime;
		UpdateTimerDisplay();

		this.PerformAction(2f, StartGame);
	}

	protected void InitCrowd()
	{
		List<string> names = new List<string>(new string[]{"Jim", "Michael", "Jules", "Nat"});
		int gap = GameSettings.Instance.CrowdSize / names.Count;

		for (int i = 0; i < GameSettings.Instance.CrowdSize; i++)
		{
			Sheep newSheep = GameObject.Instantiate<Sheep>(sheepPrefab);
			newSheep.name = "Sheep_" + i.ToString("000");
			if (GameSettings.Instance.CrowdSize >= 10 && i % gap == 0 && names.Count > 0)
			{
				newSheep.name = "Sheep_" + names[0];
				names.RemoveAt(0);
			}

			newSheep.transform.SetParent(crowdTransform);

			newSheep.UpdateLean(player, dictator);

			allSheep.Add(newSheep);
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
		playerControls.StartShowingCards();
		StartNewsHeadlineCountdown();
	}

	protected void CompleteGame()
	{
		//gameRunning = false;
		//playerControls.enabled = false;
	}

	protected void Update()
	{
		if (gameRunning)
		{
			for (int i = 0; i < allSheep.Count; i++)
			{
				if (allSheep[i].allowLeanUpdate)
				{
					allSheep[i].UpdateLean(player, dictator);

					if (allSheep[i].Lean <= -3f || allSheep[i].Lean >= 3f)
					{
						Debug.Log("Stop lean " + allSheep[i].name + "!");
						allSheep[i].allowLeanUpdate = false;
					}
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

	protected void StartNewsHeadlineCountdown()
	{
		this.PerformAction(GameSettings.Instance.HeadlineRepeatTime, ShowNewsHeadline);
	}

	protected void ShowNewsHeadline()
	{
		// Randomise a news headline.
		IdealType[] ideals = Utility.GetEnumValues<IdealType>();
		IdealType headlineIdeal = ideals[UnityEngine.Random.Range(0, ideals.Length)];
		IdealLean headlineLean = UnityEngine.Random.value > 0.5f ? IdealLean.Positive : IdealLean.Negative;
		string headline = "Headline is " + headlineLean.ToString() + " " + headlineIdeal.ToString() + "!";

		newsHeadline.InitHeadline(headlineIdeal, headlineLean, headline);
		newsHeadline.Show();

		StartNewsHeadlineCountdown();

		this.PerformAction(GameSettings.Instance.HeadlineChoiceTime, () => {
			newsHeadline.Hide();
		});
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

	protected void OnHeadlineResolved(IdealType idealType, IdealLean lean, HeadlineResponse response)
	{
		newsHeadline.Hide();

		if (response == HeadlineResponse.FakeNews)
		{
			// Flip all sheep's opinions that match the idealType and lean to their opposite.
			for (int i = 0; i < allSheep.Count; i++)
			{
				Sheep sheep = allSheep[i];
				if (sheep.Ideals.GetIdealLean(idealType) == lean)
				{
					sheep.Ideals.SetIdealValue(idealType, -sheep.Ideals.GetIdealValue(idealType));
				}
			}
		}
	}
}
