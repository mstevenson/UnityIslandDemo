using UnityEngine;

public class TimedObjectDstructor : MonoBehaviour
{
	public float timeOut = 1.0f;
	public bool detachChildren = false;

	void Awake ()
	{
		Invoke ("DestroyNow", timeOut);
	}

	void DestroyNow ()
	{
		if (detachChildren) {
			transform.DetachChildren ();
		}
		DestroyObject (gameObject);
	}
}