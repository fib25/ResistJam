using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	protected static AudioManager _instance;
	public static AudioManager Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = FindObjectOfType<AudioManager>();
				_instance.InitialiseInternal();
			}

			return _instance;
		}
	}

	[SerializeField]
	protected AudioClip[] audioClips;

	protected List<AudioSource> sfxSources = new List<AudioSource>();
	protected AudioSource musicSource;

	protected const int SFX_SOURCE_COUNT = 16;

	public static void Initialise()
	{
		_instance = FindObjectOfType<AudioManager>();
		_instance.InitialiseInternal();
	}

	public static void PlaySFX(string clipName)
	{
		Instance.PlaySFXInternal(clipName);
	}

	public static void PlaySFXRandom(string clipPrefix)
	{
		Instance.PlaySFXRandomInternal(clipPrefix);
	}

	public static void PlayMusic(string clipName, bool loop = false)
	{
		Instance.PlayMusicInternal(clipName, loop);
	}

	public static void StopMusic()
	{
		Instance.StopMusicInternal();
	}

	protected void InitialiseInternal()
	{
		for (int i = 0; i < SFX_SOURCE_COUNT; i++)
		{
			GameObject sourceObj = new GameObject("sfx_source_" + i.ToString("00"));
			sourceObj.transform.SetParent(this.transform);
			AudioSource source = sourceObj.AddComponent<AudioSource>();
			sfxSources.Add(source);
		}

		GameObject musicSourceObj = new GameObject("music_source");
		musicSourceObj.transform.SetParent(this.transform);
		musicSource = musicSourceObj.AddComponent<AudioSource>();
	}

	protected void PlaySFXInternal(string clipName)
	{
		AudioSource source = null;

		for (int i = 0; i < sfxSources.Count; i++)
		{
			if (!sfxSources[i].isPlaying)
			{
				source = sfxSources[i];
				break;
			}
		}

		source.clip = GetClip(clipName);
		source.Play();
	}

	protected void PlaySFXRandomInternal(string clipPrefix)
	{
		List<string> validClipNames = new List<string>();

		for (int i = 0; i < audioClips.Length; i++)
		{
			if (audioClips[i].name.Contains(clipPrefix))
			{
				validClipNames.Add(audioClips[i].name);
			}
		}

		PlaySFXInternal(validClipNames[UnityEngine.Random.Range(0, validClipNames.Count)]);
	}

	protected void PlayMusicInternal(string clipName, bool loop)
	{
		musicSource.Stop();
		musicSource.clip = GetClip(clipName);
		musicSource.loop = loop;
		musicSource.Play();
	}

	protected void StopMusicInternal()
	{
		musicSource.Stop();
	}

	protected AudioClip GetClip(string clipName)
	{
		for (int i = 0; i < audioClips.Length; i++)
		{
			if (audioClips[i].name == clipName)
			{
				return audioClips[i];
			}
		}

		return null;
	}
}
