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
	[SerializeField]
	protected UINewsHeadline newsHeadline;

	protected float roundTimer;
	protected bool gameRunning = false;

	protected void Awake()
	{
		dictator = FindObjectOfType<Dictator>();
		player = FindObjectOfType<Player>();
		playerControls = FindObjectOfType<UIPlayerControls>();
		playerControls.CardSelectedEvent += OnCardSelected;
		//newsHeadline = FindObjectOfType<UINewsHeadline>();
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
		List<string> names = new List<string>(new string[]{"Jim", "Michael", "Jules", "Nat", "Joe"});
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
		/*IdealType[] ideals = Utility.GetEnumValues<IdealType>();
		IdealLean headlineLean = UnityEngine.Random.value > 0.5f ? IdealLean.Positive : IdealLean.Negative;
		IdealType headlineIdeal = ideals[UnityEngine.Random.Range(0, ideals.Length)];
		bool validHeadline = false;
		while (!validHeadline)
		{
			headlineIdeal = ideals[UnityEngine.Random.Range(0, ideals.Length)];

			if (player.Ideals.GetIdealValue(headlineIdeal) != 0f)
			{
				validHeadline = true;
				continue;
			}
		}
		string headline = "Headline is " + headlineLean.ToString() + " " + headlineIdeal.ToString() + "!";

		newsHeadline.InitHeadline(headlineIdeal, headlineLean, headline);
		newsHeadline.Show();*/

		IdealType[] allIdeals = Utility.GetEnumValues<IdealType>();
		List<IdealType> validIdeals = new List<IdealType>();
		for (int i = 0; i < allIdeals.Length; i++)
		{
			if (player.Ideals.GetIdealValue(allIdeals[i]) != 0f)
			{
				validIdeals.Add(allIdeals[i]);
			}
		}

		NewsHeadline headline = NewsHeadlineCollection.Instance.GetRandomByIdealType(validIdeals[UnityEngine.Random.Range(0, validIdeals.Count)]);;
		newsHeadline.InitHeadline(headline);
		newsHeadline.Show();

		this.PerformAction(GameSettings.Instance.HeadlineOnScreenTime, () => {
			newsHeadline.Hide();
		});

		for (int i = 0; i < allSheep.Count; i++)
		{
			allSheep[i].SetState(SheepState.Newspaper);
		}

		// Remove cards display.
		playerControls.Hide();
	}

	protected void UpdateTimerDisplay()
	{
		//timerText.text = Mathf.FloorToInt(roundTimer / 60f) + ":" + Mathf.FloorToInt(roundTimer % 60f).ToString("00"); // Minutes and seconds.
		timerText.text = Mathf.FloorToInt(roundTimer).ToString("00"); // Seconds.
	}

	/*protected void UpdateAllSheepLean()
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
	}*/

	protected void OnCardSelected(Card card)
	{
		player.Ideals.AddToIdealValue(card.idealType, card.value);
		dictator.Ideals.AddToIdealValue(card.idealType, -card.value);
	}

	protected void OnHeadlineResolved(IdealType idealType, IdealLean lean)
	{
		List<Sheep> flippingSheep = new List<Sheep>();

		// Get sheep's whose lean are opposite to the headline idealType.
		for (int i = 0; i < allSheep.Count; i++)
		{
			Sheep sheep = allSheep[i];
			IdealLean sheepLean = sheep.Ideals.GetIdealLean(idealType);

			if (lean == IdealLean.Positive && sheepLean == IdealLean.Negative)
			{
				flippingSheep.Add(sheep);
			}
			else if (lean == IdealLean.Negative && sheepLean == IdealLean.Positive)
			{
				flippingSheep.Add(sheep);
			}

			sheep.SetState(SheepState.Idle);
		}

		// Choose an amount to flip and flip 'em!
		int amountToFlip = (int)((float)flippingSheep.Count * UnityEngine.Random.Range(0.35f, 0.65f));

		for (int i = 0; i < amountToFlip; i++)
		{
			Sheep flipSheep = flippingSheep[i];

			//float prevLean = flipSheep.Lean;
			flipSheep.Ideals.SetIdealValue(idealType, -flippingSheep[i].Ideals.GetIdealValue(idealType));
			//flipSheep.UpdateLean(player, dictator);

			/*if (flipSheep.Lean != prevLean)
			{
				flipSheep.SetState(SheepState.Newspaper);
			}*/
		}

		newsHeadline.Hide();

		StartNewsHeadlineCountdown();

		playerControls.Show();
		playerControls.ShowNewCards();
	}
}
