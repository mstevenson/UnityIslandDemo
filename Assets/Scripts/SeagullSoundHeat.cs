using UnityEngine;

public class SeagullSoundHeat : MonoBehaviour
{
	public static float heat = 0.00f;

	void Update ()
	{
		if(heat > 0) heat -= Time.deltaTime;
	}	
}

