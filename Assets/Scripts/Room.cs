using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Room : MonoBehaviour
{
	public BoxCollider2D _volume;
	public void Awake()
	{
		_volume = gameObject.GetComponentInChildren<BoxCollider2D> ();
		if(_volume)
		{
			_volume.isTrigger = true;
		}
	}
	public bool Contains(Vector3 pos)
	{
		bool result = _volume && _volume.bounds.Contains (pos);
		return result;
	}
	public bool Contains(Vector2 pos)
	{
		bool result = Contains (new Vector2 (pos.x, pos.y));
		return result;
	}
	public bool Contains(GameObject obj)
	{
		bool result = Contains (obj.transform.position);
		return result;
	}
	public void GetBounds(out Vector2 min, out Vector2 max)
	{
		if(_volume)
		{
			min = _volume.bounds.min;//new Vector2(_volume.bounds.min.x, _volume.bounds.min.y);
			max = _volume.bounds.max;//new Vector2(_volume.bounds.max.x, _volume.bounds.max.y);
		}
		else
		{
			min = new Vector2();
			max = new Vector2();
		}
	}
	public Vector2 GetCenter()
	{
		Vector2 result = new Vector2 ();
		if(_volume)
		{
			result = _volume.bounds.center;
		}
		return result;
	}
}

