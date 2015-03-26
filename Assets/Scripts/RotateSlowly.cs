using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Light))] 

public class RotateSlowly : MonoBehaviour 
{
	
	public float minRotation = 1;
	public float maxRotation = 359;
	
	float random;
	
	
	// Use this for initialization
	void Start () 
	{
		random = Random.Range (0.0f, 65535.0f);
		
		
	}
	
	// Update is called once per frame
	void Update () 
	{
	
		light.transform.Rotate (0, (Mathf.Lerp (minRotation, maxRotation, Time.time)) * .0001f, 0);

	}
}

