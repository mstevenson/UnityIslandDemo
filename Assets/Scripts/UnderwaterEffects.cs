using UnityEngine;

public class UnderwaterEffects : MonoBehaviour
{
	public UnityStandardAssets.Water.Water water;
	public float waterLevel;
	public AudioClip uAudio;
	public AudioClip aAudio;

	public Color uColor = new Color(1,1,1,1);
	public float uDensity = .05f;

	public Color aColor = new Color(1,1,1,1);
	public float aDensity = .008f;

	public Renderer waterSurface;
	public Renderer underwaterSurface;

	private bool below;
	//private var glow : GlowEffectIsland;
	//private var blur : BlurEffectIsland;

	void Awake() {
		/*if(!waterLevel)
		{
			water = FindObjectOfType(Water);
			if(water) waterLevel = water.transform.position.y; //.gameobject; /**/ /*
		}*/
		waterLevel = water.transform.position.y; //.gameobject; /**/ /*
		aColor = RenderSettings.fogColor;
		aDensity = RenderSettings.fogDensity;
		
		/*glow = GetComponent(GlowEffectIsland);
		blur = GetComponent(BlurEffectIsland);
		if( !glow || !blur )
		{
			Debug.LogError("no right Glow/Blur assigned to camera!");
			enabled = false;
		}*/
		if( !waterSurface || !underwaterSurface )
		{
			Debug.LogError("assign water & underwater surfaces");
			enabled = false;
		}
		if( underwaterSurface != null )
			underwaterSurface.enabled = false; // initially underwater is disabled
	}

	void Update ()
	{
		if (waterLevel < transform.position.y && below)
		{
			GetComponent<AudioSource>().clip = aAudio;
			GetComponent<AudioSource>().Play();
			RenderSettings.fogDensity = aDensity;
			RenderSettings.fogColor = aColor;
			
			below = false;
			
			//glow.enabled = !below && glow.IsSupported();
			//blur.enabled = below && blur.IsSupported();
			waterSurface.enabled = true;
			underwaterSurface.enabled = false;
		}
		
		if (waterLevel > transform.position.y && !below)
		{
			GetComponent<AudioSource>().clip = uAudio;
			GetComponent<AudioSource>().Play();
			RenderSettings.fogDensity = uDensity;
			RenderSettings.fogColor = uColor;
			
			below = true;
			
			//glow.enabled = !below && glow.IsSupported();
			//blur.enabled = below && blur.IsSupported();
			waterSurface.enabled = false;
			underwaterSurface.enabled = true;
		}
	}

	
}
