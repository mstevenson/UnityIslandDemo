using System.Collections.Generic;
using UnityEngine;

public class WaveAnimation : MonoBehaviour
{
    public GameObject[] siblings = new GameObject[0];
	public int index = 0;
	public float offset = 0.00f;
	public float slideMin = -0.1f;
	public float slideMax = 0.4f;
	public float slideSpeed = 0.5f;
	public float slideSharpness = 1.00f;
	public float scaleMin = 1.00f;
	public float scaleMax = 0.40f;
	public float scaleSpeed = 0.50f;
	public float scaleSharpness = 0.50f;

	public float fadeSpeed = 0.00f;

	public Vector3 baseScroll = new Vector3(0.1f, 0, 0.3547f);
	public float baseRotation = 0.00f;
	public Vector3 baseScale = new Vector3 (10.0f, 10, 10.0f);

	private Material theMaterial;
	private float slide = 0.00f;
	private float slideInertia = 0.00f;
	private float scale = 0.00f;
	private float scaleInertia = 0.00f;
	private Vector3 basePos;
	private Vector3 texScale;
	private float lastSlide = 0.00f;
	private float fade = 1.00f;
	private Color color;
	private Color fadeColor;
	public WaveAnimation original;

	void Start ()
	{
		CheckHWSupport();
		
		var waves = GetComponents<WaveAnimation>();
		if(waves.Length == 1 && original == null)
		{
			original = this;	
		}
		
		foreach (GameObject s in siblings)
		{
			AddCopy(s, original, false);	
		}
		if(waves.Length < GetComponent<Renderer>().materials.Length)
		{
			AddCopy(gameObject, original, true);
		}
		theMaterial = GetComponent<Renderer>().materials[index];
		color = theMaterial.GetColor("_Color");
		fadeColor = color;
		fadeColor.a = 0;
		texScale = theMaterial.GetTextureScale("_MainTex");	
	}

	private void CheckHWSupport()
	{
		var supported = GetComponent<Renderer>().sharedMaterial.shader.isSupported;
		foreach(GameObject s in siblings)
			s.GetComponent<Renderer>().enabled = supported;
		GetComponent<Renderer>().enabled = supported;
	}


	void Update ()
	{
		CheckHWSupport();
		
		slideInertia = Mathf.Lerp(slideInertia, Mathf.PingPong((Time.time * scaleSpeed) + offset, 1), slideSharpness * Time.deltaTime);
		slide = Mathf.Lerp(slide, slideInertia, slideSharpness * Time.deltaTime);
		theMaterial.SetTextureOffset("_MainTex", new Vector3(index * 0.35f, Mathf.Lerp(slideMin, slideMax, slide) * 2, 0));
		theMaterial.SetTextureOffset("_Cutout", new Vector3(index * 0.79f, Mathf.Lerp(slideMin, slideMax, slide) / 2, 0));
		
		fade = Mathf.Lerp(fade, slide - lastSlide > 0 ? 0.3f : 0, Time.deltaTime * fadeSpeed); /**/
		lastSlide = slide;
		theMaterial.SetColor("_Color", Color.Lerp(fadeColor, color, fade));
		
		scaleInertia = Mathf.Lerp(scaleInertia, Mathf.PingPong((Time.time * scaleSpeed) + offset, 1), scaleSharpness * Time.deltaTime);
		scale = Mathf.Lerp(scale, scaleInertia, scaleSharpness * Time.deltaTime);
		theMaterial.SetTextureScale("_MainTex", new Vector3(texScale.x, Mathf.Lerp(scaleMin,scaleMax, scale), texScale.z));
		
		basePos += baseScroll * Time.deltaTime;
		var inverseScale = new Vector3 (1 / baseScale.x, 1 / baseScale.y, 1 / baseScale.z);
		var uvMat = Matrix4x4.TRS (basePos, Quaternion.Euler (baseRotation,90,90), inverseScale);
		theMaterial.SetMatrix ("_WavesBaseMatrix", uvMat);
	}


	void AddCopy (GameObject ob, WaveAnimation original, bool copy)
	{
		WaveAnimation newWave = ob.AddComponent<WaveAnimation>();
		newWave.original = original;
		if(copy) newWave.index = index + 1;
		else newWave.index = index;
		newWave.offset = original.offset + (2.00f / GetComponent<Renderer>().materials.Length);
		newWave.slideMin = original.slideMin;
		newWave.slideMax = original.slideMax;
		newWave.slideSpeed = original.slideSpeed + Random.Range(-original.slideSpeed / 5, original.slideSpeed / 5);
		newWave.slideSharpness = original.slideSharpness + Random.Range(-original.slideSharpness / 5, original.slideSharpness / 5);
		newWave.scaleMin = original.scaleMin;
		newWave.scaleMax = original.scaleMax;
		newWave.scaleSpeed = original.scaleSpeed + Random.Range(-original.scaleSpeed / 5, original.scaleSpeed / 5);
		newWave.scaleSharpness = original.scaleSharpness + Random.Range(-original.scaleSharpness / 5, original.scaleSharpness / 5);
		
		newWave.fadeSpeed = original.fadeSpeed;
			
		Vector3 randy = Random.onUnitSphere; 
		randy.y = 0;
		newWave.baseScroll = randy.normalized * original.baseScroll.magnitude;
		newWave.baseRotation = Random.Range(0,360);
		newWave.baseScale = original.baseScale * Random.Range(0.8f, 1.2f);	
	}
}