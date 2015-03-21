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

		this.direction = Direction.NULL;
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
		anim.SetFloat ("speed", Mathf.Abs (Input.GetAxis ("Horizontal")));

		if (Input.GetAxisRaw ("Horizontal") > 0) {
			anim.SetBool ("runReleased", false);
			this.direction = Direction.EAST;

			transform.Translate (Vector2.right * 3f * Time.deltaTime);
//			transform.eulerAngles = new Vector2(0, 0); // this sets the rotation of the gameobject

		}

		if (Input.GetAxisRaw ("Horizontal") < 0) {
			anim.SetBool ("runReleased", false);
//			this.direction = Direction.WEST;

			transform.Translate (-Vector2.right * 3f * Time.deltaTime);
//			transform.eulerAngles = new Vector2(0, 180);  // this sets the rotation of the gamebject
		}

		if (Input.GetAxisRaw ("Vertical") < 0) 
		{
			anim.SetBool ("runReleased", false);
			this.direction = Direction.SOUTH;

			transform.Translate (-Vector2.up * 3f * Time.deltaTime);
		}

		if (Input.GetAxisRaw ("Vertical") > 0) {
			anim.SetBool ("runReleased", false);
			this.direction = Direction.NORTH;

			transform.Translate (Vector2.up * 3f * Time.deltaTime);
		}

		if (Input.GetAxisRaw("Vertical") < 0 && Input.GetAxisRaw("Horizontal") < 0) 
		{
			this.direction = Direction.SOUTHWEST250;
		}

//		Debug.Log ("the Horizontal GetAxis is: " + Input.GetAxis ("Horizontal"));

		if (this.direction == Direction.SOUTH && Input.GetKeyUp (KeyCode.A) && Input.GetAxis ("Horizontal") < 0 && Input.GetAxis ("Horizontal") >= -0.2) 
		{
			Debug.Log ("this happened");
			this.direction = Direction.SOUTHWEST250;
		}

		if (this.direction == Direction.SOUTH && Input.GetKeyUp (KeyCode.A) && Input.GetAxis ("Horizontal") < -0.2 && Input.GetAxis ("Horizontal") >= -0.4) 
		{
			Debug.Log ("SouthWest 230 degrees");
			this.direction = Direction.SOUTHWEST230;
		}

		if (this.direction == Direction.SOUTH && Input.GetKeyUp (KeyCode.A) && Input.GetAxis ("Horizontal") < -0.4 && Input.GetAxis ("Horizontal") >= -0.7)  
		{
			Debug.Log ("SouthWest 210 degrees");
			this.direction = Direction.SOUTHWEST210;
		}



//		if ((Input.GetAxisRaw ("Horizontal") < 0 && Input.GetAxis ("Vertical") > .7f) || (Input.GetAxis("Vertical") > 0f && Input.GetAxis ("Horizontal") > -.8f && Input.GetAxis("Horizontal") < -.2f))
//		{
//			this.direction = Direction.NORTHWEST1;
//		}
//
//		if(Input.GetAxis ("Horizontal") < 0f && Input.GetAxis ("Vertical") < 0.8f && Input.GetAxis("Vertical") > 0.3f)
//		{
//			this.direction = Direction.NORTHWEST2;
//		}



		if(Input.GetKeyDown (KeyCode.Space) && grounded == true)
		{
			rigidbody2D.AddForce(Vector2.up * 200f);
		}

//		Debug.Log ("the player's direction is: " + this.direction);
//		Debug.Log ("the vertical axis input is " + Input.GetAxis ("Vertical"));
//		Debug.Log ("the horizontal axis input is " + Input.GetAxis ("Horizontal"));
//		Debug.Log ("the direction is " + this.direction);

	}

	void animationSetter()
	{
		if(this.direction == Direction.SOUTHEAST330)
		{
			anim.SetInteger("Direction", 16);
		}

		if(this.direction == Direction.SOUTHEAST310)
		{
			anim.SetInteger("Direction", 15);
		}

		if(this.direction == Direction.SOUTHEAST290)
		{
			anim.SetInteger("Direction", 14);
		}

		if(this.direction == Direction.SOUTH)
		{
			anim.SetInteger("Direction", 13);
		}

		if (this.direction == Direction.SOUTHWEST250)
		{
			anim.SetInteger ("Direction", 12);
		}

		if (this.direction == Direction.SOUTHWEST230)
		{
			anim.SetInteger ("Direction", 11);
		}

		if (this.direction == Direction.SOUTHWEST210)
		{
			anim.SetInteger ("Direction", 10);
		}

		if (this.direction == Direction.WEST)
		{
			anim.SetInteger ("Direction", 9);
		}

		if (this.direction == Direction.NORTHWEST150)
		{
			anim.SetInteger ("Direction", 8);
		}

		if (this.direction == Direction.NORTHWEST130)
		{
			anim.SetInteger ("Direction", 7);
		}

		if (this.direction == Direction.NORTHWEST110)
		{
			anim.SetInteger ("Direction", 6);
		}

		if (this.direction == Direction.NORTH)
		{
			anim.SetInteger ("Direction", 5);
		}

		if (this.direction == Direction.NORTHEAST70)
		{
			anim.SetInteger ("Direction", 4);
		}

		if (this.direction == Direction.NORTHEAST50)
		{
			anim.SetInteger ("Direction", 3);
		}

		if (this.direction == Direction.NORTHEAST30)
		{
			anim.SetInteger ("Direction", 2);
		}

		if (this.direction == Direction.EAST)
		{
			anim.SetInteger ("Direction", 1);
		}

//		Debug.Log ("the animator's direction int is" + anim.GetInteger ("Direction"));


		if (Input.GetKeyUp (KeyCode.W) || Input.GetKeyUp (KeyCode.S) || Input.GetKeyUp (KeyCode.D) || Input.GetKeyUp (KeyCode.A))
		{
			anim.SetBool ("runReleased", true);
		} 

	
	}

	public enum Direction
	{
		NORTH, SOUTH, EAST, WEST, NORTHEAST30, NORTHEAST50, NORTHEAST70, NORTHWEST110, NORTHWEST130, NORTHWEST150, SOUTHEAST290, SOUTHEAST310, SOUTHEAST330, SOUTHWEST210, SOUTHWEST230, SOUTHWEST250, NULL
	}
	
}

