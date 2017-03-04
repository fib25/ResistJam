using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
	public Person personPrefab;

	protected Transform crowdTransform;
	protected Dictator dictator;
	protected Player player;
	protected List<Person> people = new List<Person>();

	protected void Awake()
	{
		dictator = FindObjectOfType<Dictator>();
		player = FindObjectOfType<Player>();

		crowdTransform = GameObject.Find("Crowd").transform;
	}

	protected void Start()
	{
		for (int i = 0; i < GameSettings.Values.crowdSize; i++)
		{
			Person inst = GameObject.Instantiate<Person>(personPrefab);
			inst.transform.SetParent(crowdTransform);

			//inst.lean = UnityEngine.Random.Range(0.2f, 0.8f);

			people.Add(inst);
		}
	}

	protected void Update()
	{
		if (Input.GetKeyDown(KeyCode.C))
		{
			for (int i = 0; i < people.Count; i++)
			{
				UpdateLean(people[i]);
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

		float ratio = dictatorResult / playerResult;
		//Debug.Log("Ratio = " + ratio);

		float dictatorDrift = -Mathf.Clamp(1f - dictatorResult, 0f, 1f);
		float playerDrift = Mathf.Clamp(1f - playerResult, 0f, 1f);
		float drift = dictatorDrift + playerDrift;
		//Debug.Log("Drift = " + drift);

		person.lean += 0.1f * drift;
	}

	protected float CompareIdeals(Ideals a, Ideals b)
	{
		float chi = Mathf.Abs((Mathf.Pow((b.immigration - a.immigration), 2f) / a.immigration) +
							  (Mathf.Pow((b.civilRights - a.civilRights), 2f) / a.civilRights) +
							  (Mathf.Pow((b.publicSpending - a.publicSpending), 2f) / a.publicSpending));

		return chi;
	}
}
