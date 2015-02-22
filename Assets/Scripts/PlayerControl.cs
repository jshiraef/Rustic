using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

	public bool interact = false;
	public Transform lineStart, lineEnd;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		Movement();
		Raycasting ();
	}

	void Raycasting()
	{
		Debug.DrawLine (lineStart.position, lineEnd.position, Color.green);

	}

	void Movement()
	{
		if (Input.GetKey (KeyCode.D)) 
		{
			transform.Translate (Vector2.right * 4f * Time.deltaTime);
		}

		if (Input.GetKey (KeyCode.A)) 
		{
			transform.Translate (-Vector2.right * 4f * Time.deltaTime);
		}

		if (Input.GetKey (KeyCode.S)) 
		{
			transform.Translate (-Vector2.up * 4f * Time.deltaTime);
		}

		if (Input.GetKey (KeyCode.W)) 
		{
			transform.Translate (Vector2.up * 4f * Time.deltaTime);
		}
	}
}
