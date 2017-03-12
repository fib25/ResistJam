using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fox : MonoBehaviour
{
	public Sprite idleSprite;
	public Sprite announceSprite;

	protected SpriteRenderer spriteRenderer;

	protected void Awake()
	{
		spriteRenderer = GetComponentInChildren<SpriteRenderer>();
		spriteRenderer.sprite = idleSprite;
	}

	public void DoAnnounce()
	{
		SetAnnounce();

		this.PerformAction(GameSettings.Instance.HeadlineOnScreenTime, () => {
			SetIdle();
		});
	}

	protected void SetIdle()
	{
		spriteRenderer.sprite = idleSprite;
	}

	protected void SetAnnounce()
	{
		spriteRenderer.sprite = announceSprite;
	}
}
