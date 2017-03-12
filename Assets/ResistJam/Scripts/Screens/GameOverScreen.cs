using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
	public Image llamaWon;
	public Image wolfWon;
	public Image hintImage;
	public Button endButton;

	protected void Awake()
	{
		#if !UNITY_STANDALONE
		endButton.gameObject.SetActive(false);
		#endif
	}

	protected void OnEnable()
	{
		if (Globals.llamaWon)
		{
			llamaWon.gameObject.SetActive(true);
			wolfWon.gameObject.SetActive(false);
			hintImage.gameObject.SetActive(false);
		}
		else
		{
			llamaWon.gameObject.SetActive(false);
			wolfWon.gameObject.SetActive(true);
			hintImage.gameObject.SetActive(true);

			Color hintColour = hintImage.color;
			hintColour.a = 0f;
			hintImage.color = hintColour;

			this.PerformAction(GameSettings.Instance.HintFadeInDelay, () => {
				StartCoroutine(FadeInHint());
			});
		}
	}

	protected IEnumerator FadeInHint()
	{
		Color hintColour = hintImage.color;
		float inc = 1f / GameSettings.Instance.HintFadeInTime;

		while (hintColour.a < 1f)
		{
			yield return null;

			hintColour.a += inc * Time.deltaTime;
			hintImage.color = hintColour;
		}

		hintColour.a = 1f;
		hintImage.color = hintColour;
	}

	public void OnReplayPressed()
	{
		Navigation.GoToScreen(NavScreen.Title);
	}

	public void OnEndPressed()
	{
		#if UNITY_STANDALONE
		Application.Quit();
		#endif
	}
}
