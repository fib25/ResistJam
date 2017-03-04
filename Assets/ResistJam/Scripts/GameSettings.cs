using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : ScriptableObject
{
	public int CrowdSize;
	public float MaxDriftSpeed;
	public float RoundTime;
	public float MaxChi;


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
