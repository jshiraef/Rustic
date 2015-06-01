using UnityEngine;
using System.Collections;

public class GuardBehavior : MonoBehaviour {

	public Transform sightStart, sightEnd;

	public bool spotted = false;
	public bool facingLeft = true;

	public GameObject arrow;

	// Use this for initialization
	void Start () 
	{
		InvokeRepeating ("Patrol", 0f, Random.Range (2,4));
	}
	
	// Update is called once per frame
	void Update () 
	{
		Raycasting ();
		Behaviours ();
	}

	void Raycasting()
	{
		Debug.DrawLine (sightStart.position, sightEnd.position, Color.green);
		spotted = Physics2D.Linecast (sightStart.position, sightEnd.position, 1 << LayerMask.NameToLayer ("Player")); 
	}

	void Behaviours()
	{
		if (spotted == true) 
		{
			arrow.SetActive (true);
		} 
		else 
		{
			arrow.SetActive (false);
		}
	}

	void Patrol()
	{
		facingLeft = !facingLeft;
		if (facingLeft == true) 
		{
			transform.eulerAngles = new Vector2 (0, 0);
		}
		else 
		{
			transform.eulerAngles = new Vector2 (0, 180);
		}


	}
}
