﻿using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

	public bool interact = false;
	public bool grounded = false;
	public bool isRunning = false;
	public Transform lineStart, lineEnd, groundedEnd;

	RaycastHit2D whatIHit;

	public float speed = 6.0f; 
	public float v, h;


	public Direction direction;
	public RunDirection runDirection;
	public RunDirection lastRecordedRunDirection;

	public int animationCase;

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
		setRunDirection ();
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
		v = Input.GetAxis ("Vertical");
		h = Input.GetAxis ("Horizontal");

		isRunning = false;

		anim.SetFloat ("VerticalAnalogAxis", (Input.GetAxis ("Vertical")));
		anim.SetFloat ("HorizontalAnalogAxis", (Input.GetAxis ("Horizontal")));

		if (Input.GetAxis ("Horizontal") > 0)
		{
			isRunning = true;
			anim.SetBool ("runReleased", false);
			this.direction = Direction.EAST;

			transform.Translate (Vector2.right * 3f * Time.deltaTime);
			
//			transform.eulerAngles = new Vector2(0, 0); // this sets the rotation of the gameobject

		} 


		if (Input.GetAxis ("Horizontal") < 0) 
		{
			isRunning = true;
			anim.SetBool ("runReleased", false);
//			this.direction = Direction.WEST;


			transform.Translate (-Vector2.right * 3f * Time.deltaTime);
//			transform.eulerAngles = new Vector2(0, 180);  // this sets the rotation of the gamebject
		} 


		if (Input.GetAxis ("Vertical") < 0) 
		{
			isRunning = true;
			anim.SetBool ("runReleased", false);
			this.direction = Direction.SOUTH;

			transform.Translate (-Vector2.up * 3f * Time.deltaTime);
		} 


		if (Input.GetAxis ("Vertical") > 0) 
		{
			isRunning = true;
			anim.SetBool ("runReleased", false);
			this.direction = Direction.NORTH;


			transform.Translate (Vector2.up * 3f * Time.deltaTime);
		} 

		anim.SetBool ("isRunning", isRunning);

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

		Debug.Log ("the isRunning bool is: " + isRunning);


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


//		if (Input.GetKeyUp (KeyCode.W) || Input.GetKeyUp (KeyCode.S) || Input.GetKeyUp (KeyCode.D) || Input.GetKeyUp (KeyCode.A))
//		{
//			anim.SetBool ("runReleased", true);
//		} 

		if(Input.GetAxisRaw ("Vertical") == 0 && Input.GetAxisRaw ("Horizontal") == 0)
		{
			anim.SetBool ("runReleased", true);
		}
	
	}

	void setRunDirection()
	{
		// North, South, East, West
		if (v == 1 && h == 0) 
		{
			this.runDirection = RunDirection.NORTH;
		}
		
		if (v == -1 && h == 0) 
		{
			this.runDirection = RunDirection.SOUTH;
		}
		
		if (v == 0 && h == 1) 
		{
			this.runDirection = RunDirection.EAST;
		}
		
		if (v == 0 && h == -1) 
		{
			this.runDirection = RunDirection.WEST;
		}

		// Set NorthEast
		if (Input.GetAxis ("Vertical") > 0 && Input.GetAxis ("Horizontal") > 0) 
		{

			if (v > .01 && v < .1|| h > .87 && h < .95) 
			{
				this.runDirection = RunDirection.NORTHEAST20;
			}
			
			if (v > .1 && v < .2 || h > .75 && h < .87) 
			{
				this.runDirection = RunDirection.NORTHEAST30;
			}
			
			if (v > .2 && v < .3 || h > .65 && h < .75) 
			{
				this.runDirection = RunDirection.NORTHEAST40;
			}
			
			if (v > .3 && v < .4 || h > .5 && h < .65) 
			{
				this.runDirection = RunDirection.NORTHEAST50;
			}
			
			if (v > .4 && v < .55 || h > .35 && h < .5) 
			{
				this.runDirection = RunDirection.NORTHEAST60;
			}
			
			if (v > .55 && v < .7 || h > .25 && h < .35) 
			{
				this.runDirection = RunDirection.NORTHEAST70;
			}
			
			if (v > .7 && v < 85 || h > .1 && h < .25) 
			{
				this.runDirection = RunDirection.NORTHEAST80;
			}
		}

		// Set NorthWest
		if (Input.GetAxis ("Vertical") > 0 && Input.GetAxis ("Horizontal") < 0) 
		{
			if (v < .95 && v > .85 || h < -.05 && h > -.15) 
			{
				this.runDirection = RunDirection.NORTHWEST110;
			}

			if (v < .85 && v > .7 || h < -.15 && h > -.3) 
			{
				this.runDirection = RunDirection.NORTHWEST120;
			}

			if (v < .7 && v > .55 || h < -.3 && h > -.45) 
			{
				this.runDirection = RunDirection.NORTHWEST130;
			}

			if (v < .55 && v > .42 || h < -.45 && h > -.58) 
			{
				this.runDirection = RunDirection.NORTHWEST140;
			}

			if (v < .42 && v > .3 || h < -.58 && h > -.7) 
			{
				this.runDirection = RunDirection.NORTHWEST150;
			}

			if (v < .3 && v > .2 || h < -.7 && h > -.8) 
			{
				this.runDirection = RunDirection.NORTHWEST160;
			}

			if (v < .2 && v > .1 || h < -.8 && h > -.9) 
			{
				this.runDirection = RunDirection.NORTHWEST170;
			}
		}

		// Set SouthWest
		if (Input.GetAxis ("Vertical") < 0 && Input.GetAxis ("Horizontal") < 0) 
		{
			if (v < -.05 && v > -.15 || h < -.85 && h > -.95) 
			{
				this.runDirection = RunDirection.SOUTHWEST200;
			}

			if (v < -.15 && v > -.25 || h < -.75 && h > -.85) 
			{
				this.runDirection = RunDirection.SOUTHWEST210;
			}

			if (v < -.25 && v > -.37 || h < -.63 && h > -.75) 
			{
				this.runDirection = RunDirection.SOUTHWEST220;
			}

			if (v < -.37 && v > -.5 || h < -.5 && h > -.63) 
			{
				this.runDirection = RunDirection.SOUTHWEST230;
			}

			if (v < -.5 && v > -.63 || h < -.38 && h > -.5) 
			{
				this.runDirection = RunDirection.SOUTHWEST240;
			}

			if (v < -.63 && v > -.75 || h < -.25 && h > -.38) 
			{
				this.runDirection = RunDirection.SOUTHWEST250;
			}

			if (v < -.75 && v > -.87 || h < -.13 && h > -.25) 
			{
				this.runDirection = RunDirection.SOUTHWEST260;
			}
		}

		// Set SouthEast
		if (Input.GetAxis ("Vertical") < 0 && Input.GetAxis ("Horizontal") > 0) 
		{
			if (v > -.95 && v < -.83 || h > .05 && h < .17) 
			{
				this.runDirection = RunDirection.SOUTHEAST290;
			}

			if (v > -.83 && v < -.7 || h > .17 && h < .3) 
			{
				this.runDirection = RunDirection.SOUTHEAST300;
			}

			if (v > -.7 && v < -.58 || h > .3 && h < .42) 
			{
				this.runDirection = RunDirection.SOUTHEAST310;
			}

			if (v > -.58 && v < -.45 || h > .42 && h < .55) 
			{
				this.runDirection = RunDirection.SOUTHEAST320;
			}

			if (v > -.45 && v < -.32 || h > .55 && h < .67) 
			{
				this.runDirection = RunDirection.SOUTHEAST330;
			}

			if (v > -.32 && v < -.2 || h > .67 && h < .78) 
			{
				this.runDirection = RunDirection.SOUTHEAST340;
			}

			if (v > -.2 && v < -.1 || h > .78 && h < .9) 
			{
				this.runDirection = RunDirection.SOUTHEAST350;
			}
		}






		if (Input.GetAxis ("Vertical") == 0 && Input.GetAxis ("Horizontal") == 0) 
		{
			this.runDirection = RunDirection.NULL;
		}

		if (!(this.runDirection == RunDirection.NULL)) 
		{
			lastRecordedRunDirection = this.runDirection;
		}

//		Debug.Log ("the last-recorded Run Direction is: " + this.lastRecordedRunDirection);
	}

	public enum Direction
	{
		NORTH, SOUTH, EAST, WEST, NORTHEAST30, NORTHEAST50, NORTHEAST70, NORTHWEST110, NORTHWEST130, NORTHWEST150, SOUTHEAST290, SOUTHEAST310, SOUTHEAST330, SOUTHWEST210, SOUTHWEST230, SOUTHWEST250, NULL
	}

	public enum RunDirection
	{
		EAST,  
		NORTHEAST20, NORTHEAST30, NORTHEAST40, NORTHEAST50, NORTHEAST60, NORTHEAST70, NORTHEAST80, 
		NORTH, 
		NORTHWEST110, NORTHWEST120, NORTHWEST130, NORTHWEST140, NORTHWEST150, NORTHWEST160, NORTHWEST170,
		WEST,
		SOUTHWEST200, SOUTHWEST210, SOUTHWEST220, SOUTHWEST230, SOUTHWEST240, SOUTHWEST250, SOUTHWEST260,
		SOUTH,
		SOUTHEAST290, SOUTHEAST300, SOUTHEAST310, SOUTHEAST320, SOUTHEAST330, SOUTHEAST340, SOUTHEAST350,
		NULL
	}
	
}

