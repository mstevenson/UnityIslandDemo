using UnityEngine;

public class WaveMeshAdjustment : MonoBehaviour
{
	public float offset = 0.0f;
	public Collider col;

	void Start ()
	{
		MeshFilter filter = GetComponent<MeshFilter>();
		var mesh = filter.mesh;
		var mTransform = transform;
		var vertices = mesh.vertices;
		var i = 1;
		RaycastHit hit;
		Vector3 dir;
		while(i < vertices.Length - 1) // i - 1 == terrain side        // i == water side
		{
			dir = vertices[i-1] - vertices[i];
			if(mTransform.TransformDirection(dir) != Vector3.zero && col.Raycast(new Ray(mTransform.TransformPoint(vertices[i]), mTransform.TransformDirection(dir)), out hit, 30.00f))
			{
				var hitPoint = mTransform.InverseTransformPoint(hit.point);
				var shorePos = hitPoint + (dir / 3); shorePos.y += 15;
				if(col.Raycast(new Ray(mTransform.TransformPoint(shorePos), -Vector3.up), out hit, 30.00f))
					hitPoint = mTransform.InverseTransformPoint(hit.point);
				hitPoint.y += offset;
				if(hitPoint.y > 1.5) hitPoint.y = 0;
				vertices[i-1] = hitPoint;
			}
			i+=2;
		}
	
		mesh.vertices = vertices;
		filter.mesh = mesh;
	}
}
