using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

	public bool interact = false;
	public bool grounded = false;
	public Transform lineStart, lineEnd, groundedEnd;

	RaycastHit2D whatIHit;

	public float speed = 6.0f;

	public Direction direction;

	Animator anim;

	// Use this for initialization
	void Start ()
	{
		anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		Movement();
		Raycasting ();
		animationSetter ();
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

		anim.SetFloat ("speed", Mathf.Abs (Input.GetAxis ("Vertical")));

		if (Input.GetAxisRaw ("Horizontal") > 0) 
		{
			anim.SetBool ("runReleased", false);
			this.direction = Direction.EAST;

			transform.Translate (Vector2.right * 4f * Time.deltaTime);
			transform.eulerAngles = new Vector2(0, 0); // this sets the rotation of the gameobject

		}

		if (Input.GetAxisRaw("Horizontal") < 0) 
		{
			anim.SetBool ("runReleased", false);
			this.direction = Direction.WEST;

			transform.Translate (Vector2.right * 4f * Time.deltaTime);
			transform.eulerAngles = new Vector2(0, 180);  // this sets the rotation of the gamebject
		}

		if (Input.GetAxisRaw ("Vertical") < 0) 
		{
			anim.SetBool ("runReleased", false);
			this.direction = Direction.SOUTH;

			transform.Translate (-Vector2.up * 4f * Time.deltaTime);
		}

		if (Input.GetAxisRaw ("Vertical") > 0) 
		{
			anim.SetBool ("runReleased", false);
			this.direction = Direction.NORTH;

			transform.Translate (Vector2.up * 4f * Time.deltaTime);
		}

		if(Input.GetKeyDown (KeyCode.Space) && grounded == true)
		{
			rigidbody2D.AddForce(Vector2.up * 200f);
		}

	}

	void animationSetter()
	{
		if (this.direction == Direction.NORTH)
		{
			anim.SetInteger ("Direction", 1);
		}

		if (this.direction == Direction.SOUTH)
		{
			anim.SetInteger ("Direction", 5);
		}

		if (Input.GetKeyUp (KeyCode.W) || Input.GetKeyUp (KeyCode.S) || Input.GetKeyUp (KeyCode.D) || Input.GetKeyUp (KeyCode.A))
		{
			anim.SetBool ("runReleased", true);
		} 
	
	}

	public enum Direction
	{
		NORTH, SOUTH, EAST, WEST, NORTHEAST, NORTHWEST, SOUTHEAST, SOUTHWEST
	}
	
}

