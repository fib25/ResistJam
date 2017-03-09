using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum HeadlineResponse
{
	NoResponse,
	FakeNews
}

public class UINewsHeadline : MonoBehaviour
{
	public event System.Action<IdealType, IdealLean, HeadlineResponse> HeadlineResolvedEvent = delegate { };

	[SerializeField]
	protected Text headlineText;

	protected IdealType currentIdeal;
	protected IdealLean idealLean;

	protected bool isHeadlineInitialised = false;
	protected bool responded = false;

	public void InitHeadline(IdealType ideal, IdealLean idealLean, string headline)
	{
		this.currentIdeal = ideal;
		this.idealLean = idealLean;

		headlineText.text = headline;

		isHeadlineInitialised = true;
		responded = false;
	}

	public void Show()
	{
		this.gameObject.SetActive(true);
	}

	public void Hide()
	{
		this.gameObject.SetActive(false);

		if (!responded && isHeadlineInitialised)
		{
			ResolveHeadline(HeadlineResponse.NoResponse);
		}
	}

	protected void ResolveHeadline(HeadlineResponse response)
	{
		responded = true;
		HeadlineResolvedEvent(currentIdeal, idealLean, response);
	}

	public void OnFakeNewsButtonPressed()
	{
		ResolveHeadline(HeadlineResponse.FakeNews);
	}
}
