using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : ScriptableObject
{
	protected static GameSettings _gameSettings;
	public static GameSettings Instance
	{
		get
		{
			if (_gameSettings == null) return Get();
			return _gameSettings;
		}
	}

	[Header("Settings")]
	public int CrowdSize;
	public float RoundTime;
	public float CardSelectTime;
	[Header("Headline")]
	public float HeadlineRepeatTime;
	public float HeadlineOnScreenTime;
	[Header("Sheep")]
	public float IdleTime;
	public float WanderSpeed;
	public float MoveToLeanSpeed;
	public float WanderRange;
	public float NewspaperReadTime;
	[Header("Debug")]
	public bool ShowSheepDebug;
	public bool ShowValuesOnCards;

	/// <summary>
	/// Loads the GameSettings asset from the Resources folder.
	/// </summary>
	public static GameSettings Get()
	{
		_gameSettings = Resources.Load<GameSettings>("GameSettings");
		return _gameSettings;
	}
}
