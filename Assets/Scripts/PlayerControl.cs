using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

	public bool interact = false;
	public bool grounded = false;
	public Transform lineStart, lineEnd, groundedEnd;

	RaycastHit2D whatIHit;

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
		Debug.DrawLine (this.transform.position, groundedEnd.position, Color.green);

		grounded = Physics2D.Linecast (this.transform.position, groundedEnd.position, 1 << LayerMask.NameToLayer ("ground"));

		if(Physics2D.Linecast (lineStart.position, lineEnd.position, 1 << LayerMask.NameToLayer ("Guard")))
        {
			whatIHit = Physics2D.Linecast (lineStart.position, lineEnd.position, 1 << LayerMask.NameToLayer ("Guard"));
			interact = true;
		}
		else
		{
			interact = false;
		}

		if(Input.GetKeyDown (KeyCode.E) && interact == true)
		 {
			Destroy (whatIHit.collider.gameObject);
		 }
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

		if(Input.GetKeyDown (KeyCode.Space) && grounded == true)
		{
			rigidbody2D.AddForce(Vector2.up * 200f);
		}
	}
}
