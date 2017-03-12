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
	public int FlockSize;
	public float RoundTime;
	public float CardSelectTime;
	public float CardSelectTimerSpeedUpModifier;
	public float HintFadeInTime;
	public float HintFadeInDelay;
	[Header("Headline")]
	public float HeadlineRepeatTime;
	public float HeadlineOnScreenTime;
	[Header("Sheep")]
	public float IdleTime;
	public float WanderSpeed;
	public float MoveToLeanSpeed;
	public float WanderRange;
	[Header("Debug")]
	public bool ShowSheepDebug;
	public bool ShowValuesOnCards;
	public bool EnableDebugScreen;
	public NavScreen DebugScreen;

	/// <summary>
	/// Loads the GameSettings asset from the Resources folder.
	/// </summary>
	public static GameSettings Get()
	{
		_gameSettings = Resources.Load<GameSettings>("GameSettings");
		return _gameSettings;
	}
}
