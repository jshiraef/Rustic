using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class PlayerControl : MonoBehaviour
{

    public bool interact = false;
    public bool grounded = false;
    public bool isRunning = false;
    public bool isWalking = false;
    public bool lockPosition = false;
    public bool swinging = false;
    public bool rolling = false;
    public bool isIdle = false;
    public bool environmentMask = false;

    Rigidbody2D body;
    float moveForce;
    float maxVelocity;

    private float rollingCoolDown;

    public Transform lineStart, lineEnd, groundedEnd;

    RaycastHit2D whatIHit;

    public float moveSpeed = 4f;
    public float v, h;

    private Vector2 RunningMovement;
    private PlayerIndex playerIndex;


    public Direction direction;
    public RunDirection runDirection;
    public RunDirection lastRecordedRunDirection;
    private float rigidbodyAngularDirection;

    Animator anim;
    private bool shortFall = false;
    private float shortFallCoolDown;

    private bool rumble = false;
    private float rumbleCoolDown;

    private GameObject player;
    private GameObject renderMask;

    // barrel variables (variables similar to these can help keep track of relations between player and world objects)
    public GameObject[] barrels;
    public Vector2 distanceToBarrel;
    private GameObject nearestBarrel;
    private float actualBarrelSeparation;

    float barrelCooldown;
    bool barrelSwitch;


    // Use this for initialization
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        maxVelocity = 77f;

        anim = GetComponent<Animator>();

        this.direction = Direction.NULL;

        player = GameObject.Find("player");
        renderMask = GameObject.Find("renderMask");

        //		nearestBarrel = GameObject.Find ("barrel");
    }

    // Update is called once per frame
    void Update()
    {


        if (rumble && rumbleCoolDown <= 0)
        {
            GamePad.SetVibration(playerIndex, 0, 0);
            rumble = false;
        }

        if (rumbleCoolDown > 0)
        {
            rumbleCoolDown -= Time.deltaTime;
        }

        //Debug.Log(getAngularDirection());
        //    Debug.Log(body.angularVelocity);
        //      Debug.Log(grounded);

        //      Debug.Log("the rumble coolDown is: " + rumbleCoolDown);


        Movement();
        Raycasting();
        setRunDirection();
        animationSetter();
        checkDestructibleObjects();
        checkAttack();


        if (shortFallCoolDown > 0)
        {
            shortFallCoolDown -= Time.deltaTime;
        }
        else
        {
            shortFall = false;
            lockPosition = false;
            anim.SetBool("shortFall", shortFall);
            setNonKinematic();
        }

        if (barrelCooldown > 0)
        {
            barrelCooldown -= Time.deltaTime;
        }




        // freeze position until animation is finished
        if (this.anim.GetCurrentAnimatorStateInfo(0).IsName("shortFallRecovery") || this.anim.GetCurrentAnimatorStateInfo(0).IsName("FallingDown"))
        {
            lockPosition = true;
        }
        else
        {
            lockPosition = false;
        }

        //	Debug.Log ("shortFall is: " + shortFall);
        // Debug.Log ("the shortFallCoolDown is: " + shortFallCoolDown);
    }

    void Raycasting()
    {
        Debug.DrawLine(lineStart.position, lineEnd.position, Color.green);
        Debug.DrawLine(this.transform.position, groundedEnd.position, Color.green);

        grounded = Physics2D.Linecast(this.transform.position, groundedEnd.position, 1 << LayerMask.NameToLayer("ground"));

        if (Physics2D.Linecast(lineStart.position, lineEnd.position, 1 << LayerMask.NameToLayer("Guard")))
        {
            whatIHit = Physics2D.Linecast(lineStart.position, lineEnd.position, 1 << LayerMask.NameToLayer("Guard"));
            interact = true;
        }
        else
        {
            interact = false;
        }

        if (Input.GetKeyDown(KeyCode.E) && interact == true)
        {
            Destroy(whatIHit.collider.gameObject);
        }
    }

    void Movement()
    {
        v = Input.GetAxis("Vertical");
        h = Input.GetAxis("Horizontal");

        Vector3 rightMovement = Vector3.right * moveSpeed * Time.deltaTime * h;
        Vector3 upMovement = Vector3.up * moveSpeed * Time.deltaTime * v;

        Vector3 heading = Vector3.Normalize(rightMovement + upMovement);

        transform.position += rightMovement;
        transform.position += upMovement;

        // movement vectors are updated here
        //Vector3 position = transform.position;
        //Vector3 inputvelocity = new Vector3(h, v, 0).normalized;
        //float moveforce = Mathf.Max((maxVelocity - body.velocity.magnitude), 0);
        //if (inputvelocity.magnitude > 0.99f)
        //{
        //    body.drag = 1;
        //    body.AddForce(inputvelocity * moveforce * Time.deltaTime * 60);
        //}
        //else
        //{
        //    body.drag = 4;
        //}

        //        Debug.Log(body.velocity.magnitude);




        isRunning = false;

        anim.SetFloat("VerticalAnalogAxis", (Input.GetAxis("Vertical")));
        anim.SetFloat("HorizontalAnalogAxis", (Input.GetAxis("Horizontal")));

        // checks to see if analog is only slightly tilted for walk animation
        if (Input.GetAxisRaw("Vertical") > -.4f && Input.GetAxisRaw("Vertical") < .4f && Input.GetAxisRaw("Horizontal") > -.4f && Input.GetAxisRaw("Horizontal") < .4f)
        {
            if (!((h == 0) && (v == 0)))
            {
                isWalking = true;
            }

        }
        else isWalking = false;

        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            if (isRunning == false)
            {
                anim.StopPlayback();
            }

            this.direction = Direction.EAST;

            if (!lockPosition && !swinging && !rolling)
            {

                if (!isWalking)
                {
                    isRunning = true;
                    anim.Play("Running");
                }
                else
                {
                    anim.Play("Walking");
                }


                //transform.Translate(h * .018f, 0, 0);

                //transform.Translate (Vector2.right * speed * Time.deltaTime);
            }

            //			transform.eulerAngles = new Vector2(0, 0); // this sets the rotation of the gameobject

        }


        if (Input.GetAxisRaw("Horizontal") < 0)
        {

            if (isRunning == false)
            {
                anim.StopPlayback();
            }

            this.direction = Direction.WEST;

            if (!lockPosition && !swinging && !rolling)
            {

                if (!isWalking)
                {
                    isRunning = true;
                    anim.Play("Running");
                }
                else
                {
                    anim.Play("Walking");
                }


                //transform.Translate(h * .018f, 0, 0);

                //			transform.Translate (-Vector2.right * speed * Time.deltaTime);
            }

            //			transform.eulerAngles = new Vector2(0, 180);  // this sets the rotation of the gamebject
        }


        if (Input.GetAxisRaw("Vertical") < 0)
        {
            if (isRunning == false)
            {
                anim.StopPlayback();
            }

            this.direction = Direction.SOUTH;

            if (!lockPosition && !swinging && !rolling)
            {
                if (!isWalking)
                {
                    isRunning = true;
                    anim.Play("Running");
                }
                else
                {
                    anim.Play("Walking");
                }


                //transform.Translate(0, v * .018f, 0);

                //			transform.Translate (-Vector2.up * speed * Time.deltaTime);
            }
        }


        if (Input.GetAxisRaw("Vertical") > 0)
        {
            if (isRunning == false)
            {
                anim.StopPlayback();
            }


            this.direction = Direction.NORTH;

            if (!lockPosition && !swinging && !rolling)
            {
                if (!isWalking)
                {
                    isRunning = true;
                    anim.Play("Running");
                }
                else
                {
                    anim.Play("Walking");
                }

                //transform.Translate(0, v * .018f, 0);

                //transform.Translate (Vector2.up * speed * Time.deltaTime);
            }
        }

        anim.SetBool("isRunning", isRunning);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //body.AddForce(Vector2.up * 200f);
            body.MovePosition(Vector2.up * 200f);
        }

        if (shortFall)
        {
            isRunning = false;
            //		transform.Translate (-Vector2.up * 4f * Time.deltaTime);
        }
        else
        {
            anim.SetBool("shortFall", false);
        }

        //This sets the isIdle variable
        if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)
        {
            if (!swinging && !rolling && !isIdle)
            {
                anim.Play("Idle");
                isIdle = true;
            }
        }
        else isIdle = false;

        if (swinging)
        {
            lockPosition = true;
            isRunning = false;

            rumble = true;
            GamePad.SetVibration(playerIndex, 1f, 0f);
            rumbleCoolDown = .3f;
        }

        //		Debug.Log ("the shortfallCoolDown is: " + shortFallCoolDown);

        //		Debug.Log ("the player's direction is: " + this.direction);
        //		Debug.Log ("the vertical axis input is " + Input.GetAxis ("Vertical"));
        //		Debug.Log ("the horizontal axis input is " + Input.GetAxis ("Horizontal"));
        //		Debug.Log ("the direction is " + this.direction);
    }

    void checkAttack()
    {
        if (Input.GetButton("PS4_Square")) {

            swinging = true;
        } else
            swinging = false;

        if (swinging)
        {
            anim.Play("SwingScythe");
        }

        if (Input.GetButton("PS4_Triangle"))
        {
            if (!rolling)
            {
                rollingCoolDown = .7f;
            }
            rolling = true;
        }

        if (rolling)
        {
            anim.Play("Rolling");

            if (h == 0 && v == 0)
            {

                if (this.direction == Direction.EAST)
                {
                    transform.Translate(.045f, 0, 0);
                }

                if (this.direction == Direction.NORTHEAST30)
                {
                    transform.Translate(.04f, .015f, 0);
                }

                if (this.direction == Direction.NORTHEAST50)
                {
                    transform.Translate(.03f, .025f, 0);
                }

                if (this.direction == Direction.NORTHEAST70)
                {
                    transform.Translate(.02f, .04f, 0);
                }

                if (this.direction == Direction.NORTH)
                {
                    transform.Translate(0, .045f, 0);
                }

                if (this.direction == Direction.NORTHWEST110)
                {
                    transform.Translate(-.015f, .04f, 0);
                }

                if (this.direction == Direction.NORTHWEST130)
                {
                    transform.Translate(-.025f, .03f, 0);
                }

                if (this.direction == Direction.NORTHWEST150)
                {
                    transform.Translate(-.04f, .02f, 0);
                }

                if (this.direction == Direction.WEST)
                {
                    transform.Translate(-.045f, 0, 0);
                }

                if (this.direction == Direction.SOUTHWEST210)
                {
                    transform.Translate(-.04f, -.015f, 0);
                }

                if (this.direction == Direction.SOUTHWEST230)
                {
                    transform.Translate(-.03f, -.025f, 0);
                }

                if (this.direction == Direction.SOUTHWEST250)
                {
                    transform.Translate(-.04f, -.02f, 0);
                }

                if (this.direction == Direction.SOUTH)
                {
                    transform.Translate(0, -.045f, 0);
                }

                if (this.direction == Direction.SOUTHEAST290)
                {
                    transform.Translate(.01f, -.04f, 0);
                }

                if (this.direction == Direction.SOUTHEAST310)
                {
                    transform.Translate(.025f, -.03f, 0);
                }

                if (this.direction == Direction.SOUTHEAST330)
                {
                    transform.Translate(.04f, -.02f, 0);
                }

            }

            else
                transform.Translate(h * .05f, v * .05f, 0);

        }

        if (rollingCoolDown > 0)
        {
            rollingCoolDown -= Time.deltaTime;
        }

        if (rolling && rollingCoolDown <= 0)
        {
            rolling = false;
        }

        //        Debug.Log("the roll cooldown is " + rollingCoolDown);
    }

    void animationSetter()
    {

        switch ((int)this.direction)
        {
            case 0:
                anim.SetFloat("direction(float)", 0f);
                break;
            case 1:
                anim.SetFloat("direction(float)", (1f / 16f) + .01f);
                break;
            case 2:
                anim.SetFloat("direction(float)", (2f / 16f) + .01f);
                break;
            case 3:
                anim.SetFloat("direction(float)", (3f / 16f) + .01f);
                break;
            case 4:
                anim.SetFloat("direction(float)", (4f / 16f) + .01f);
                break;
            case 5:
                anim.SetFloat("direction(float)", (5f / 16f) + .01f);
                break;
            case 6:
                anim.SetFloat("direction(float)", (6f / 16f) + .01f);
                break;
            case 7:
                anim.SetFloat("direction(float)", (7f / 16f) + .01f);
                break;
            case 8:
                anim.SetFloat("direction(float)", (8f / 16f) + .01f);
                break;
            case 9:
                anim.SetFloat("direction(float)", (9f / 16f) + .01f);
                break;
            case 10:
                anim.SetFloat("direction(float)", (10f / 16f) + .01f);
                break;
            case 11:
                anim.SetFloat("direction(float)", (11f / 16f) + .01f);
                break;
            case 12:
                anim.SetFloat("direction(float)", (12f / 16f) + .01f);
                break;
            case 13:
                anim.SetFloat("direction(float)", (13f / 16f) + .01f);
                break;
            case 14:
                anim.SetFloat("direction(float)", (14f / 16f) + .01f);
                break;
            case 15:
                anim.SetFloat("direction(float)", (15f / 16f) + .01f);
                break;
        }

        //		Debug.Log("the run direction is: " + this.runDirection);
        //		Debug.Log("the direction is: " + this.direction);
        //		Debug.Log ("the animator's direction float is: " + anim.GetFloat ("direction(float)"));


        //		if (Input.GetKeyUp (KeyCode.W) || Input.GetKeyUp (KeyCode.S) || Input.GetKeyUp (KeyCode.D) || Input.GetKeyUp (KeyCode.A))
        //		{
        //			anim.SetBool ("runReleased", true);
        //		} 

        if (Input.GetAxisRaw("Vertical") == 0 && Input.GetAxisRaw("Horizontal") == 0)
        {
            anim.SetBool("runReleased", true);
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
        if (Input.GetAxis("Vertical") > 0 && Input.GetAxis("Horizontal") > 0)
        {

            if (v > 0 && v < .1 || h > .87 && h < .95)
            {
                this.runDirection = RunDirection.NORTHEAST20;


                //				Debug.Log ("it is now northeast 20");
            }

            if (v > .1 && v < .2 || h > .75 && h < .87)
            {
                this.runDirection = RunDirection.NORTHEAST30;

                //				Debug.Log ("it is now northeast 30");
            }

            if (v > .2 && v < .3 || h > .65 && h < .75)
            {
                this.runDirection = RunDirection.NORTHEAST40;

                //				Debug.Log ("it is now northeast 40");
            }

            if (v > .3 && v < .4 || h > .5 && h < .65)
            {
                this.runDirection = RunDirection.NORTHEAST50;

                //				Debug.Log ("it is now northheast 50");

            }

            if (v > .4 && v < .55 || h > .35 && h < .5)
            {
                this.runDirection = RunDirection.NORTHEAST60;

                //				Debug.Log ("it is now northeast 60");
            }

            if (v > .55 && v < .7 || h > .25 && h < .35)
            {
                this.runDirection = RunDirection.NORTHEAST70;

                //				Debug.Log ("it is now northeast 70");
            }

            if (v > .7 && v < .85 || h > 0 && h < .25)
            {
                this.runDirection = RunDirection.NORTHEAST80;

                //				Debug.Log ("it is now northeast 80");
            }
        }

        // Set NorthWest
        if (Input.GetAxis("Vertical") > 0 && Input.GetAxis("Horizontal") < 0)
        {
            if (v < 1 && v > .85 || h < 0 && h > -.15)
            {
                this.runDirection = RunDirection.NORTHWEST110;

                //				Debug.Log ("it is now northwest 110");
            }

            if (v < .85 && v > .7 || h < -.15 && h > -.3)
            {
                this.runDirection = RunDirection.NORTHWEST120;

                //				Debug.Log ("it is now northwest 120");
            }

            if (v < .7 && v > .55 || h < -.3 && h > -.45)
            {
                this.runDirection = RunDirection.NORTHWEST130;

                //				Debug.Log ("it is now northhwest 130");

            }

            if (v < .55 && v > .42 || h < -.45 && h > -.58)
            {
                this.runDirection = RunDirection.NORTHWEST140;

                //				Debug.Log ("it is now northwest 140");
            }

            if (v < .42 && v > .3 || h < -.58 && h > -.7)
            {
                this.runDirection = RunDirection.NORTHWEST150;

                //				Debug.Log ("it is now northwest 150");
            }

            if (v < .3 && v > .2 || h < -.7 && h > -.8)
            {
                this.runDirection = RunDirection.NORTHWEST160;

                //				Debug.Log ("it is now northeast 160");
            }

            if (v < .2 && v > 0 || h < -.8 && h > -.9)
            {
                this.runDirection = RunDirection.NORTHWEST170;

                //				Debug.Log ("it is now northeast 170");
            }
        }

        // Set SouthWest
        if (Input.GetAxis("Vertical") < 0 && Input.GetAxis("Horizontal") < 0)
        {
            if (v < 0 && v > -.15 || h < -.85 && h > -.95)
            {
                this.runDirection = RunDirection.SOUTHWEST200;


                //				Debug.Log ("it is now southwest 200");
            }

            if (v < -.15 && v > -.25 || h < -.75 && h > -.85)
            {
                this.runDirection = RunDirection.SOUTHWEST210;

                //				Debug.Log ("it is now southwest 210");
            }

            if (v < -.25 && v > -.37 || h < -.63 && h > -.75)
            {
                this.runDirection = RunDirection.SOUTHWEST220;

                //				Debug.Log ("it is now southwest 220");
            }

            if (v < -.37 && v > -.5 || h < -.5 && h > -.63)
            {
                this.runDirection = RunDirection.SOUTHWEST230;

                //				Debug.Log ("it is now southwest 230");
            }

            if (v < -.5 && v > -.63 || h < -.38 && h > -.5)
            {
                this.runDirection = RunDirection.SOUTHWEST240;

                //				Debug.Log ("it is now southwest 240");
            }

            if (v < -.63 && v > -.75 || h < -.25 && h > -.38)
            {
                this.runDirection = RunDirection.SOUTHWEST250;

                //				Debug.Log ("it is now southwest 250");
            }

            if (v < -.75 && v > -.87 || h < 0 && h > -.25)
            {
                this.runDirection = RunDirection.SOUTHWEST260;

                //				Debug.Log ("it is now southwest 260");
            }
        }

        // Set SouthEast
        if (Input.GetAxis("Vertical") < 0 && Input.GetAxis("Horizontal") > 0)
        {
            if (v > -.95 && v < -.83 || h > .05 && h < .17)
            {
                this.runDirection = RunDirection.SOUTHEAST290;

                //				Debug.Log ("it is now southeast 290");
            }

            if (v > -.83 && v < -.7 || h > .17 && h < .3)
            {
                this.runDirection = RunDirection.SOUTHEAST300;

                //RunningMovement = new Vector2(.0036f, -.00624f);
                //transform.Translate (RunningMovement);

                //				Debug.Log ("it is now southeast 300");
            }

            if (v > -.7 && v < -.58 || h > .3 && h < .42)
            {
                this.runDirection = RunDirection.SOUTHEAST310;

                //				Debug.Log ("it is now southeast 310");
            }

            if (v > -.58 && v < -.45 || h > .42 && h < .55)
            {
                this.runDirection = RunDirection.SOUTHEAST320;

                //				Debug.Log ("it is now southeast 320");
            }

            if (v > -.45 && v < -.32 || h > .55 && h < .67)
            {
                this.runDirection = RunDirection.SOUTHEAST330;

                //				Debug.Log ("it is now southeast 330");
            }

            if (v > -.32 && v < -.2 || h > .67 && h < .78)
            {
                this.runDirection = RunDirection.SOUTHEAST340;

                //				Debug.Log ("it is now southeast 340");
            }

            if (v > -.2 && v < 0 || h > .78 && h < .9)
            {
                this.runDirection = RunDirection.SOUTHEAST350;

                //				Debug.Log ("it is now southeast 350");
            }
        }


        //		if (Input.GetAxis ("Vertical") == 0 && Input.GetAxis ("Horizontal") == 0) 
        //		{
        //			this.runDirection = RunDirection.NULL;
        //		}

        if (!(this.runDirection == RunDirection.NULL))
        {
            lastRecordedRunDirection = this.runDirection;
        }


        // Set the direction by using the RunDirection
        switch ((int)lastRecordedRunDirection)
        {
            case 0:
                this.direction = Direction.EAST;
                break;
            case 1:
                this.direction = Direction.NORTHEAST30;
                break;
            case 2:
                this.direction = Direction.NORTHEAST30;
                break;
            case 3:
                this.direction = Direction.NORTHEAST30;
                break;
            case 4:
                this.direction = Direction.NORTHEAST50;
                break;
            case 5:
                this.direction = Direction.NORTHEAST50;
                break;
            case 6:
                this.direction = Direction.NORTHEAST70;
                break;
            case 7:
                this.direction = Direction.NORTHEAST70;
                break;
            case 8:
                this.direction = Direction.NORTH;
                break;
            case 9:
                this.direction = Direction.NORTHWEST110;
                break;
            case 10:
                this.direction = Direction.NORTHWEST110;
                break;
            case 11:
                this.direction = Direction.NORTHWEST110;
                break;
            case 12:
                this.direction = Direction.NORTHWEST130;
                break;
            case 13:
                this.direction = Direction.NORTHWEST130;
                break;
            case 14:
                this.direction = Direction.NORTHWEST150;
                break;
            case 15:
                this.direction = Direction.NORTHWEST150;
                break;
            case 16:
                this.direction = Direction.WEST;
                break;
            case 17:
                this.direction = Direction.SOUTHWEST210;
                break;
            case 18:
                this.direction = Direction.SOUTHWEST230;
                break;
            case 19:
                this.direction = Direction.SOUTHWEST230;
                break;
            case 20:
                this.direction = Direction.SOUTHWEST230;
                break;
            case 21:
                this.direction = Direction.SOUTHWEST230;
                break;
            case 22:
                this.direction = Direction.SOUTHWEST250;
                break;
            case 23:
                this.direction = Direction.SOUTHWEST250;
                break;
            case 24:
                this.direction = Direction.SOUTH;
                break;
            case 25:
                this.direction = Direction.SOUTHEAST290;
                break;
            case 26:
                this.direction = Direction.SOUTHEAST290;
                break;
            case 27:
                this.direction = Direction.SOUTHEAST290;
                break;
            case 28:
                this.direction = Direction.SOUTHEAST310;
                break;
            case 29:
                this.direction = Direction.SOUTHEAST310;
                break;
            case 30:
                this.direction = Direction.SOUTHEAST330;
                break;
            case 31:
                this.direction = Direction.SOUTHEAST330;
                break;
        }

        //		Debug.Log ("the last-recorded Run Direction is: " + this.lastRecordedRunDirection);
    }

    void checkDestructibleObjects()
    {
        barrels = GameObject.FindGameObjectsWithTag("barrel");

        foreach (GameObject barrel in barrels)
        {
            this.distanceToBarrel = new Vector2(barrel.transform.position.x - player.transform.position.x, barrel.transform.position.y - player.transform.position.y);

            actualBarrelSeparation = Mathf.Sqrt(distanceToBarrel.x * distanceToBarrel.x + distanceToBarrel.y * distanceToBarrel.y);

            if (actualBarrelSeparation < 1.5)
            {
                if (!barrelSwitch)
                {
                    barrelCooldown = 1f;
                    barrelSwitch = true;
                }

                if (barrelCooldown <= 0)
                {

                    Animator barrelAnimator = barrel.GetComponent<Animator>();

                    if (barrel.name.EndsWith("1"))
                    {
                        barrelAnimator.Play("barrelBreakParticle");
                    }
                    else {
                        barrelAnimator.Play("barrelBreak");
                    }

                }

            }
            //			print ("the actual barrel separation" + actualBarrelSeparation);
            //			print ("the barrelCooldown is " + barrelCooldown);
            //			print ("the barrelSwitch is " + barrelSwitch);

        }

        if (barrelCooldown <= 0)
        {
            barrelSwitch = false;
        }

    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "fence")
        {
            //         Debug.Log("this is happening");
        }

        if (coll.gameObject.tag == "wall")
        {
            Debug.Log("there's a wall there moron!");
        }

    }

    void OnCollisionStay2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "fence")
        {
            lockPosition = true;
        }
    }

    void OnCollisionExit2D(Collision2D coll)
    {
        renderMask.GetComponent<RenderMask>().setMaskType(RenderMask.MaskType.NULL);
    }

    void coolDownMaker(bool coolDownTrigger, float coolDown, int coolDownTime)
    {
        if (!coolDownTrigger)
            return;

        if (coolDownTrigger && coolDown <= 0)
        {
            coolDownTime = (int)coolDown;
        }

        if (coolDown > 0)
        {
            coolDown -= Time.deltaTime;
        }

        if (coolDown <= 0)
        {
            coolDownTrigger = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "fallingLedge")
        {
            setShortFall();
        }

    }


    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "environment")
        {
            renderMask.GetComponent<RenderMask>().setMaskType(RenderMask.MaskType.NULL);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "environment")
        {
            environmentMask = true;
        }

        if (other.name == "waterEdge")
        {
            renderMask.GetComponent<RenderMask>().setMaskType(RenderMask.MaskType.WATER);
        }

        if (other.name == "grassEdge")
        {
            renderMask.GetComponent<RenderMask>().setMaskType(RenderMask.MaskType.GRASS);
        }

    }

    public void setShortFall()
    {
        shortFall = true;
        shortFallCoolDown = .5f;
        anim.SetBool("shortFall", shortFall);
        setKinematic();
    }

    void setKinematic()
    {
        this.player.GetComponent<Rigidbody2D>().isKinematic = true;
    }

    void setNonKinematic()
    {
        this.player.GetComponent<Rigidbody2D>().isKinematic = false;
    }

    public bool getIsRunning()
    {
        return isRunning;
    }

    public bool getIsIdle()
    {
        return isIdle;
    }

    public Direction getDirection()
    {

        return direction;
    }

    public float getAngularDirection()
    {
        Vector2 moveDirection = body.velocity;
        if (moveDirection != Vector2.zero)
        {
             rigidbodyAngularDirection = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;            
        }
        return rigidbodyAngularDirection;
    }

	public enum Direction
	{
			EAST, NORTHEAST30, NORTHEAST50, NORTHEAST70, NORTH, NORTHWEST110, NORTHWEST130, NORTHWEST150, WEST, SOUTHWEST210, SOUTHWEST230, SOUTHWEST250, SOUTH, SOUTHEAST290, SOUTHEAST310, SOUTHEAST330, NULL
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

