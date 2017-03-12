using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UINewsHeadline : MonoBehaviour
{
	public event System.Action<IdealType, IdealLean> HeadlineResolvedEvent = delegate { };

	[SerializeField]
	protected Text headlineText;

	protected IdealType currentIdeal;
	protected IdealLean idealLean;

	protected bool isHeadlineInitialised = false;
	protected bool responded = false;

	public void InitHeadline(NewsHeadline newsHeadline)
	{
		this.InitHeadline(newsHeadline.idealType, newsHeadline.lean, newsHeadline.headline);
	}

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
		if (this.gameObject.activeSelf) return;

		this.gameObject.SetActive(true);
	}

	public void Hide()
	{
		if (!this.gameObject.activeSelf) return;

		this.gameObject.SetActive(false);

		if (!responded && isHeadlineInitialised)
		{
			ResolveHeadline();
		}
	}

	protected void ResolveHeadline()
	{
		responded = true;
		HeadlineResolvedEvent(currentIdeal, idealLean);
	}
}
