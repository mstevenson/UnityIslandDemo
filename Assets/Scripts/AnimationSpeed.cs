using UnityEngine;

public class AnimationSpeed : MonoBehaviour
{
	public string statename = "defaulto";
	public float speed = 0.5f;
	public Animation _animation;

	void Start () 
	{
		_animation.GetComponent(typeof(Animation));
		_animation[statename].speed = speed;
	}
}
