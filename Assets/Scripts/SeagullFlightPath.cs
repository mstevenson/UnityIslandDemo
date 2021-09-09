using System.Collections;
using UnityEngine;

public class SeagullFlightPath : MonoBehaviour
{
	public float flySpeed = 15.00f;
	public float highFlyHeight = 80.00f;
	public float normalFlyHeight = 40.00f;
	public float lowFlyHeight = 20.00f;
	public float flyDownSpeed = 0.10f;
	public float circleRadius = 60.00f;
	public float circleSpeed = 0.20f;
	public float circleTime = 15.00f;
	public float awayTime = 20.00f;

	public Vector3 offset;

	private Transform myT;
	private Transform player;
	private Vector3 awayDir;
	private float flyHeight = 0.00f;
	private Collider col;
	private RaycastHit hit;
	private float distToTarget = 0.00f;
	private float lastHeight = 0.00f;
	private float height = 0.00f;
	private Vector3 terrainSize;
	private TerrainData terrainData;

	private float dTime = 0.1f;

	void Start ()
	{
		terrainData = Terrain.activeTerrain.terrainData;
		terrainSize = terrainData.size;
		col = Terrain.activeTerrain.GetComponent<Collider>();
		myT = transform;
		player = GameObject.FindWithTag("Player").transform;
		StartCoroutine(MainRoutine());	
	}

	IEnumerator MainRoutine()
	{
		while(true)
		{
			yield return StartCoroutine(ReturnToPlayer());
			yield return StartCoroutine (CirclePlayer());
			yield return StartCoroutine(FlyAway());
		}
	}

	IEnumerator ReturnToPlayer()
	{
		distToTarget = 100.00f;
		while(distToTarget > 10)
		{
			var toPlayer = player.position - myT.position;
			toPlayer.y = 0;
			distToTarget = toPlayer.magnitude;
			Vector3 targetPos;
			if(distToTarget > 0) targetPos = transform.position + ((toPlayer / distToTarget) * 10);
			else targetPos = Vector3.zero;
			
			targetPos.y = terrainData.GetInterpolatedHeight(targetPos.x / terrainSize.x, targetPos.z / terrainSize.z);
			var normal = terrainData.GetInterpolatedNormal(targetPos.x / terrainSize.x, targetPos.z / terrainSize.z);
			offset = new Vector3(normal.x * 40, 0, normal.z * 40);
			
			flyHeight = (distToTarget > 80) ? highFlyHeight : lowFlyHeight;
			if(distToTarget > 0) Move(targetPos - transform.position);
			yield return new WaitForSeconds(dTime);	
		}	
	}

	IEnumerator CirclePlayer()
	{
		var time = 0.00;
		while(time < circleTime)
		{
			var circlingPos = player.position + new Vector3(Mathf.Cos(Time.time * circleSpeed) * circleRadius, 0, Mathf.Sin(Time.time * circleSpeed) * circleRadius);
			circlingPos.y = terrainData.GetInterpolatedHeight(circlingPos.x / terrainSize.x, circlingPos.z / terrainSize.z);
			var normal = terrainData.GetInterpolatedNormal(circlingPos.x / terrainSize.x, circlingPos.z / terrainSize.z);
			offset = new Vector3(normal.x * 40, 0, normal.z * 40);

			flyHeight = normalFlyHeight;
			Move(circlingPos - myT.position);
			time += dTime;
			yield return new WaitForSeconds(dTime);	
		}	
	}

	IEnumerator FlyAway()
	{
		var radians = Random.value * 2 * Mathf.PI;
		awayDir = new Vector3(Mathf.Cos(radians), 0, Mathf.Sin(radians));
		var time = 0.00;
		while(time < awayTime)
		{
			var away = player.position + (awayDir * 1000);
			away.y = 0;
			
			var toAway = away - transform.position;

			Vector3 targetPos;
			distToTarget = toAway.magnitude;
			if(distToTarget > 0) targetPos = transform.position + ((toAway / distToTarget) * 10);
			else targetPos = Vector3.zero;
			
			targetPos.y = terrainData.GetInterpolatedHeight(targetPos.x / terrainSize.x, targetPos.z / terrainSize.z);
			var normal = terrainData.GetInterpolatedNormal(targetPos.x / terrainSize.x, targetPos.z / terrainSize.z);
			offset = new Vector3(normal.x * 40, 0, normal.z * 40);
			
			flyHeight = highFlyHeight;
			Move(targetPos - transform.position);
			time += dTime;
			yield return new WaitForSeconds(dTime);	
		}	
	}

	void Move (Vector3 delta)
	{
		delta.y = 0;
		delta = delta.normalized * flySpeed * dTime;
		var newPos = new Vector3(myT.position.x + delta.x, 1000, myT.position.z + delta.z);
		float newHeight;
		float height = 0;
		if(col.Raycast(new Ray(newPos, -Vector3.up), out hit, 2000)) newHeight = hit.point.y;
		else newHeight = 0.00f;
		if(newHeight < lastHeight) height = Mathf.Lerp(height, newHeight, flyDownSpeed * dTime);
		else height = newHeight;
		lastHeight = newHeight;
		myT.position = new Vector3(newPos.x, Mathf.Clamp(height, 35.28f, 1000.00f) + flyHeight, newPos.z);
	}
}