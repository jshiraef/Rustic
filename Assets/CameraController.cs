using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public GameObject target;

	public GameObject player;

	public float LockedY = -10.5f; public float LockedZ = -20f;

	public float distance;

	public float height;

	public Transform targetPosition;

	// Use this for initialization
	void Start () 
	{
		player = GameObject.Find ("player");

	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.position = new Vector3 (player.transform.position.x, player.transform.position.y, -10f);
	}
}
