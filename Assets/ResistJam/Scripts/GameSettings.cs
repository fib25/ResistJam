using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : ScriptableObject
{
	[Header("Settings")]
	public int CrowdSize;
	public float PersonMaxSpeed;
	public float RoundTime;
	[Header("Debug")]
	public bool ShowPersonDebug;


	protected static GameSettings _gameSettings;
	public static GameSettings Instance
	{
		get
		{
			if (_gameSettings == null) return Get();
			return _gameSettings;
		}
	}

	/// <summary>
	/// Loads the GameSettings asset from the Resources folder.
	/// </summary>
	public static GameSettings Get()
	{
		_gameSettings = Resources.Load<GameSettings>("GameSettings");
		return _gameSettings;
	}
}
