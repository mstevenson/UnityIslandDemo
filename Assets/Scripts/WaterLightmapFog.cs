using UnityEngine;

public class WaterLightmapFog : MonoBehaviour
{
	public float fogDensity = 0.00f;
	public Color fogColor;
	public Color baseColor;
	public float baseMultBlurPixels = 0.00f;
	public float blurOverDrive = 0.00f;
	public float depthAmbient = 1.50f;

	public Vector3 terrainSize;
	public Collider terrainCollider;
	public Texture2D texture;

	[ContextMenu ("Apply Fog")]
	void ApplyFog ()
	{
		var bColorTex = new Texture2D(texture.width, texture.height);
		var x = 0.00f;
		var y = 0.00f;
		while(x < texture.width)
		{
			y = 0.00f;
			while(y < texture.height)
			{
				var vect = new Vector3((x / texture.width) * terrainSize.x, 400.00f, (y / texture.height) * terrainSize.y);
				RaycastHit hit;
				if(terrainCollider.Raycast(new Ray(vect, Vector3.up * -500), out hit, 500))
				{
					float depth = 35.35f - hit.point.y;
					if(x == 256) print(vect);
					if(depth > 0)
					{
						var lightCol = texture.GetPixel((int)x,(int)y);
						var curCol = Color.Lerp(lightCol, Color.gray, depthAmbient * depth * fogDensity);
						var fog = new Vector3(Mathf.Pow(fogColor.r, depth * fogDensity), Mathf.Pow(fogColor.g, depth * fogDensity), Mathf.Pow(fogColor.b, depth * fogDensity));
						texture.SetPixel((int)x,(int)y, new Color(curCol.r * fog.x * lightCol.a, curCol.g * fog.y * lightCol.a, curCol.b * fog.z * lightCol.a, curCol.a));
						bColorTex.SetPixel((int)x,(int)y, new Color(baseColor.r, baseColor.g, baseColor.b, 1));
 					}
 					else
 					{
 						bColorTex.SetPixel((int)x,(int)y, Color.white);	
 					}
				}
				y++;
			}
			x++;	
		}
		
		//bColorTex.Apply();
		
		x = 0.00f;
		var pix = 0.0f;
		while(x < texture.width)
		{
			y = 0.00f;
			while(y < texture.height)
			{
				var curCol = texture.GetPixel((int)x,(int)y);
				
				float lerp;
				if(baseMultBlurPixels > 0)
				{
					lerp = (1.00f / (4.00f * baseMultBlurPixels)) * (1 + blurOverDrive);
					pix = baseMultBlurPixels;
				}
				else
				{
					lerp = 1.00f;
					pix = baseMultBlurPixels;
				}
				
				var temp = bColorTex.GetPixel((int)Mathf.Clamp(x, 0, texture.width - 1), (int)Mathf.Clamp(y, 0, texture.width - 1));
				curCol = Color.Lerp(curCol, new Color(curCol.r * temp.r, curCol.g * temp.g, curCol.b * temp.b, curCol.a), lerp);
				while(pix > 0)
				{
					temp = bColorTex.GetPixel((int)Mathf.Clamp(x+pix, 0, texture.width - 1), (int)Mathf.Clamp(y, 0, texture.width - 1));
					curCol = Color.Lerp(curCol, new Color(curCol.r * temp.r, curCol.g * temp.g, curCol.b * temp.b, curCol.a), lerp);
					
					temp = bColorTex.GetPixel((int)Mathf.Clamp(x-pix, 0, texture.width - 1), (int)Mathf.Clamp(y, 0, texture.width - 1));
					curCol = Color.Lerp(curCol, new Color(curCol.r * temp.r, curCol.g * temp.g, curCol.b * temp.b, curCol.a), lerp);
					
					temp = bColorTex.GetPixel((int)Mathf.Clamp(x, 0, texture.width - 1), (int)Mathf.Clamp(y+pix, 0, texture.width - 1));
					curCol = Color.Lerp(curCol, new Color(curCol.r * temp.r, curCol.g * temp.g, curCol.b * temp.b, curCol.a), lerp);
					
					temp = bColorTex.GetPixel((int)Mathf.Clamp(x, 0, texture.width - 1), (int)Mathf.Clamp(y-pix, 0, texture.width - 1));
					curCol = Color.Lerp(curCol, new Color(curCol.r * temp.r, curCol.g * temp.g, curCol.b * temp.b, curCol.a), lerp);
					pix --;
				}
				texture.SetPixel((int)x,(int)y, curCol);
				
				y++;
			}
			x++;	
		}
		texture.Apply();
		DestroyImmediate(bColorTex);
	}	
}