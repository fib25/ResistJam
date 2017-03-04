using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BoxColliderGizmo : MonoBehaviour
{
	public Color colour = Color.green;

	protected BoxCollider boxCollider;
	protected BoxCollider2D boxCollider2D;

	protected void Awake()
	{
		boxCollider = GetComponent<BoxCollider>();
		boxCollider2D = GetComponent<BoxCollider2D>();
	}

	protected void OnDrawGizmos()
	{
		Gizmos.color = colour;

		if (boxCollider != null)
		{
			Gizmos.DrawWireCube(boxCollider.bounds.center, boxCollider.bounds.size);
		}
		if (boxCollider2D != null)
		{
			Gizmos.DrawWireCube(boxCollider2D.bounds.center, boxCollider2D.bounds.size);
		}
	}
}
