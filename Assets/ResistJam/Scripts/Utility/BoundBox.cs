using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundBox
{
	protected Bounds bounds;

	public Vector3 Center
	{
		get { return bounds.center; }
		set { bounds.center = value; }
	}
	public Vector3 Size
	{
		get { return bounds.size; }
		set { bounds.size = value; }
	}
	public float Top { get { return bounds.center.y + bounds.extents.y; } }
	public float Bottom { get { return bounds.center.y - bounds.extents.y; } }
	public float Left { get { return bounds.center.x - bounds.extents.x; } }
	public float Right { get { return bounds.center.x + bounds.extents.x; } }
	public float Front { get { return bounds.center.z - bounds.extents.z; } }
	public float Back { get { return bounds.center.z + bounds.extents.z; } }

	public BoundBox()
	{
		bounds = new Bounds(Vector3.zero, Vector3.zero);
	}

	public BoundBox(Vector3 center, Vector3 size)
	{
		bounds = new Bounds(center, size);
	}
}
