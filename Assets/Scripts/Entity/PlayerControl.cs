using UnityEngine;
using System.Collections;
//using XInputDotNetPure;
using UnityEngine.PS4;



public class PlayerControl : Entity
{

    // movement
    public bool isRunning = false;
    public bool isWalking = false;
    public bool isIdle = false;

    private float knockBackCoolDown;
    private float knockBackTimeLength = 2.1f;
    public bool knockBack = false;

    public bool rolling = false;
    private float rollingCoolDown;
    private float afterRollCoolDown;

    // items
    public bool swinging = false;
    public bool holdingThrowableItem;
    public bool grabItem;
    public float grabItemTimer;
    public bool setItemDown;
    public bool throwing;
    public int throwTimer;

    // elements
    public bool inWater = false;

    private CinemachineCameraShaker screenShake;

    public Transform lineStart, lineEnd, groundedEnd;

    public RaycastHit2D itemInView;

    public float rollSpeed = 8f;
    public float v, h;

    //private PlayerIndex playerIndex;
    private Projector playerBlobShadow;
    private bool restoreBlobShadowToNormal;

    public RunDirection runDirection;
    public RunDirection lastRecordedRunDirection;

    private bool shortFall = false;
    private float shortFallCoolDown;

    private bool rumble = false;
    private float rumbleCoolDown;

    private GameObject player;
    private GameObject renderMask;
    private GameObject renderMaskOutliner;
    private GameObject throwableItem;
    private FieldOfView vision;

    // barrel variables (variables similar to these can help keep track of relations between player and world objects)
    public GameObject[] barrels;
    public Vector2 distanceToBarrel;
    private GameObject nearestBarrel;
    private float actualBarrelSeparation;

    private float analogAxesAngle;
    private float analogAxesAngle360;
    private bool freezeForAnimation;

    float barrelCooldown;
    bool barrelSwitch;

    private static readonly int IDLE = 0;
    private static readonly int RUNNING = 1;
    private static readonly int WALKING = 2;
    private static readonly int ROLLING = 3;
    private static readonly int KNOCKBACK = 4;
    private static readonly int SWINGING = 5;
    private static readonly int ITEMGRAB = 6;
    private static readonly int WALKWITHITEM = 7;
    private static readonly int THROWING = 8;


    // Use this for initialization
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        maxVelocity = 77f;

        moveSpeed = 4f;

        anim = GetComponent<Animator>();
        currentAction = IDLE;

        this.direction = Direction.NULL;

        player = GameObject.Find("player");
        boxCollider2D = player.GetComponent<BoxCollider2D>();
        renderMask = GameObject.Find("renderMask");
        renderMaskOutliner = GameObject.Find("renderMaskOutliner");
        playerBlobShadow = player.GetComponentInChildren<Projector>();
        player.GetComponent<SpriteRenderer>().receiveShadows = true;
        screenShake = GameObject.Find("CM vcam1").GetComponent<CinemachineCameraShaker>();
        vision = GetComponent<FieldOfView>();

        //		nearestBarrel = GameObject.Find ("barrel");
    }

    // Update is called once per frame
    void Update()
    {

        if (rumble && rumbleCoolDown <= 0)
        {
            //GamePad.SetVibration(playerIndex, 0, 0);  
            PS4Input.PadSetVibration(1, 0, 0);
            rumble = false;
        }

        if (rumbleCoolDown > 0)
        {
            rumbleCoolDown -= Time.deltaTime;
        }
        //    Debug.Log("the rumble coolDown is: " + rumbleCoolDown);

        if (!freezeForAnimation)
        {
            anim.SetFloat("normalizedDirection", analogAxesAngle360 / 360);
        }

        setDirection8();
        Movement();
        Raycasting();
        setRunDirection();
        //animationDirectionSetter();
        checkDestructibleObjects();
        checkAttack();


        //Debug.Log("the time of the animator is " + anim.GetCurrentAnimatorStateInfo(0).length);

        if (currentAction == SWINGING)
        {
            if (animationHasPlayedOnce())
            {
                swinging = false;
            }
        }

        // setAction
        if (holdingThrowableItem)
        {
            if (currentAction != WALKWITHITEM)
            {
                currentAction = WALKWITHITEM;
                moveSpeed = 1;
            }

            if (h == 0 && v == 0)
            {
                anim.speed = 0.0f;
            }
            else
            {
                anim.speed = 1f;
            }

            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime % 1 < .5f)
            {
                moveSpeed = 1;
            }
            else moveSpeed = 0;


        }
        else if (grabItem)
        {
            if (currentAction == WALKWITHITEM)
            {
                anim.speed = 1f;
                setItemDown = true;

            }

            if (currentAction != ITEMGRAB)
            {
                currentAction = ITEMGRAB;
            }

            //throwableItem.transform.position = new Vector3(Mathf.Lerp(throwableItem.transform.position.x, player.transform.position.x, Time.deltaTime/2), Mathf.Lerp(throwableItem.transform.position.y, player.transform.position.y + 1.35f, Time.deltaTime/2), 0);
            if (grabItemTimer < .55f && grabItemTimer > .1f && !setItemDown)
            {
                throwableItem.transform.Translate(new Vector3(0, 5f * Time.deltaTime, 0));
                itemInView.collider.gameObject.transform.SetParent(this.gameObject.transform);
            }
        }
        else if (throwing)
        {
            anim.speed = 1f;

            if (currentAction != THROWING)
            {
                throwableItem.transform.SendMessage("setThrown", true);
                anim.Play("throwing", 0, .55f);
                currentAction = THROWING;
            }
        }
        else if (swinging)
        {
            if (currentAction != SWINGING)
            {
                currentAction = SWINGING;
                anim.Play("SwingScythe");
                //lockPosition = true;
                isRunning = false;

                //rumble = true;
                //GamePad.SetVibration(playerIndex, .5f, 0f);
                PS4Input.PadSetVibration(1, 175, 0);
            }
            moveSpeed = 0;
            //rumbleCoolDown = .3f;
        }
        else if (rolling)
        {
            if (currentAction != ROLLING)
            {
                currentAction = ROLLING;
            }
            //lockPosition = true;
            isRunning = false;

            if (rollingCoolDown > .25f && rollingCoolDown < .4f)
            {
                rumble = true;
                //GamePad.SetVibration(playerIndex, .25f, .25f);
                PS4Input.PadSetVibration(1, 65, 65);
                rumbleCoolDown = .2f;
            }
        }
        else if (knockBack)
        {
            //isRunning = false;
            if (currentAction != KNOCKBACK)
            {
                currentAction = KNOCKBACK;
            }
            if (knockBackCoolDown > (knockBackTimeLength - .05f))
            {
                rumble = true;
                //GamePad.SetVibration(playerIndex, .75f, .75f);
                PS4Input.PadSetVibration(1, 180, 180);

                rumbleCoolDown = .3f;
                screenShake.ShakeCamera(1f);
            }
        }
        // checks to see if analog is only slightly tilted for walk animation
        else if (Input.GetAxisRaw("Vertical") > -.4f && Input.GetAxisRaw("Vertical") < .4f && Input.GetAxisRaw("Horizontal") > -.4f && Input.GetAxisRaw("Horizontal") < .4f && !((h == 0) && (v == 0)))
        {

            isWalking = true;
            moveSpeed = 4f;

            if (currentAction != WALKING)
            {
                currentAction = WALKING;
            }

            anim.Play("Walking");
        }
        else if (Input.GetAxisRaw("Horizontal") > 0 || Input.GetAxisRaw("Horizontal") < 0 || Input.GetAxisRaw("Vertical") > 0 || Input.GetAxisRaw("Vertical") < 0)
        {

            if (!lockPosition && !swinging && !rolling && !knockBack)
            {
                isRunning = true;
                moveSpeed = 4f;

                if (currentAction != RUNNING)
                {
                    currentAction = RUNNING;
                }
                anim.Play("Running");

                //transform.Translate(h * .018f, 0, 0);
                //transform.Translate (Vector2.right * speed * Time.deltaTime);
                //transform.eulerAngles = new Vector2(0, 0); // this sets the rotation of the gameobject

            }
        }
        else
        {
            //This sets the isIdle variable
            if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)
            {
                if (!swinging && !rolling && !knockBack)
                {
                    if (!isIdle)
                    {
                        if (currentAction != IDLE)
                        {
                            currentAction = IDLE;
                            anim.Play("Idle");
                        }

                    }
                    isIdle = true;
                }
            }
            else isIdle = false;
        }

        // sets runReleased parameter in Animator
        if (Input.GetAxisRaw("Vertical") == 0 && Input.GetAxisRaw("Horizontal") == 0)
        {
            anim.SetBool("runReleased", true);
        }
        else anim.SetBool("runReleased", false);


        // cool down timers

        if (shortFallCoolDown > 0)
        {
            shortFallCoolDown -= Time.deltaTime;
        }
        else
        {
            shortFall = false;
            //           lockPosition = false;
            anim.SetBool("shortFall", shortFall);
            setNonKinematic();
        }

        // freeze position until animation is finished
        if (this.anim.GetCurrentAnimatorStateInfo(0).IsName("shortFallRecovery") || this.anim.GetCurrentAnimatorStateInfo(0).IsName("FallingDown"))
        {
            lockPosition = true;
        }
        else
        {
            // lockPosition = false;
        }

        if (barrelCooldown > 0)
        {
            barrelCooldown -= Time.deltaTime;
        }

        if (knockBackCoolDown > 0)
        {
            knockBackCoolDown -= Time.deltaTime;
        }


        if (restoreBlobShadowToNormal)
        {
            setBlobShadowToNorm();
        }

        //	Debug.Log ("shortFall is: " + shortFall);
        // Debug.Log ("the shortFallCoolDown is: " + shortFallCoolDown);
    }

    void Raycasting()
    {
        //lineEnd.localPosition = new Vector3(h * 2, v * 2, 0);
        lineEnd.localPosition = new Vector3(Mathf.Cos(analogAxesAngle360 * Mathf.Deg2Rad), Mathf.Sin(analogAxesAngle360 * Mathf.Deg2Rad), 0);
        Debug.DrawLine(lineStart.position, lineEnd.position, Color.green);
        Debug.DrawLine(this.transform.position, groundedEnd.position, Color.green);

        grounded = Physics2D.Linecast(this.transform.position, groundedEnd.position, 1 << LayerMask.NameToLayer("ground"));

        if (Physics2D.Linecast(lineStart.position, lineEnd.position, 1 << LayerMask.NameToLayer("Rock")) && !holdingThrowableItem && !grabItem)
        {
            itemInView = Physics2D.Linecast(lineStart.position, lineEnd.position, 1 << LayerMask.NameToLayer("Rock"));
            interact = true;
            itemInView.transform.SendMessage("hitByRaycast", true);
        }
        else
        {
            interact = false;
        }

        //only temporary, this should be Input.GetButton("PS4_X");
        if (Input.GetKey(KeyCode.X) && interact == true)
        {
            //Destroy(itemInView.collider.gameObject);
            //this.throwableItem = null;
            throwableItem = itemInView.collider.gameObject;
            itemInView.transform.SendMessage("setPickUp", true);
            throwableItem.GetComponent<Collider2D>().isTrigger = true;
            grabItem = true;
        }


        //Debug.Log("the current action is " + currentAction);
        //Debug.Log("interact bool is " + interact);
    }

    void Movement()
    {
        v = Input.GetAxis("Vertical");
        h = Input.GetAxis("Horizontal");

        //Debug.Log("the getClosestTarget() position is returning " + getClosestTarget().position);

        // gets the angle of the left analog stick axes vector
        if (h >= .01f || h <= -.01f || v >= .01f || v <= -.01f)
        {
            analogAxesAngle = Mathf.Atan2(v, h) * Mathf.Rad2Deg;
        }

        if (analogAxesAngle >= 0)
        {
            analogAxesAngle360 = analogAxesAngle;
        }
        else
        {
            analogAxesAngle360 = 360 - Mathf.Abs(analogAxesAngle);
        }

        getNextPosition(v, h);

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
        isWalking = false;
        isIdle = false;

        anim.SetFloat("VerticalAnalogAxis", (Input.GetAxis("Vertical")));
        anim.SetFloat("HorizontalAnalogAxis", (Input.GetAxis("Horizontal")));


        // set animation parameters for transitions
        anim.SetBool("isRunning", isRunning);
        anim.SetBool("isWalking", isWalking);
        anim.SetBool("isRolling", rolling);

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

        //		Debug.Log ("the shortfallCoolDown is: " + shortFallCoolDown);

    }

    void checkAttack()
    {
        // only temporary, this should be Input.GetButton("PS4_Square");
        if (Input.GetKey(KeyCode.Q)) {

            if (!knockBack && !rolling && !throwing)

                swinging = true;

        } else
            swinging = false;

        //only temporary, this should be Input.GetButton("PS4_Triangle");
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (!rolling)
            {
                rollingCoolDown = .6f;
            }
            if (afterRollCoolDown <= 0 && !knockBack && !throwing)
            {
                releaseItem();
                rolling = true;
            }
        }


        if (rolling)
        {
            anim.Play("Rolling");

            if (getDirectionNSEW() == Direction.NORTH || getDirectionNSEW() == Direction.SOUTH)
            {
                boxCollider2D.size = new Vector2(.8f, 1.4f);
                boxCollider2D.offset = new Vector2(0f, -.9f);
            }
            else if (getDirectionNSEW() == Direction.EAST || getDirectionNSEW() == Direction.WEST)
            {
                boxCollider2D.size = new Vector2(1.6f, .95f);
                boxCollider2D.offset = new Vector2(0f, -1f);
            }


            // rolling while idle
            if (h == 0 && v == 0 && afterRollCoolDown <= 0f)
            {

                if (this.direction == Direction.EAST)
                {
                    transform.Translate(4.5f * Time.deltaTime, 0, 0);
                }
                else if (this.direction == Direction.NORTHEAST30)
                {
                    transform.Translate(4f * Time.deltaTime, 1.5f * Time.deltaTime, 0);
                }

                else if (this.direction == Direction.NORTHEAST50)
                {
                    transform.Translate(3f * Time.deltaTime, 2.5f * Time.deltaTime, 0);
                }

                else if (this.direction == Direction.NORTHEAST70)
                {
                    transform.Translate(2f * Time.deltaTime, 4f * Time.deltaTime, 0);
                }

                else if (this.direction == Direction.NORTH)
                {
                    transform.Translate(0, 4.5f * Time.deltaTime, 0);
                }

                else if (this.direction == Direction.NORTHWEST110)
                {
                    transform.Translate(-1.5f * Time.deltaTime, 4f * Time.deltaTime, 0);
                }

                else if (this.direction == Direction.NORTHWEST130)
                {
                    transform.Translate(-2.5f * Time.deltaTime, 3f * Time.deltaTime, 0);
                }

                else if (this.direction == Direction.NORTHWEST150)
                {
                    transform.Translate(-4f * Time.deltaTime, 2f * Time.deltaTime, 0);
                }

                else if (this.direction == Direction.WEST)
                {
                    transform.Translate(-4.5f * Time.deltaTime, 0, 0);
                }

                else if (this.direction == Direction.SOUTHWEST210)
                {
                    transform.Translate(-.4f * Time.deltaTime, -.15f * Time.deltaTime, 0);
                }

                else if (this.direction == Direction.SOUTHWEST230)
                {
                    transform.Translate(-3f * Time.deltaTime, -2.5f * Time.deltaTime, 0);
                }

                else if (this.direction == Direction.SOUTHWEST250)
                {
                    transform.Translate(-3f * Time.deltaTime, -3.5f * Time.deltaTime, 0);
                }

                else if (this.direction == Direction.SOUTH)
                {
                    transform.Translate(0, -4.5f * Time.deltaTime, 0);
                }

                else if (this.direction == Direction.SOUTHEAST290)
                {
                    transform.Translate(1f * Time.deltaTime, -4f * Time.deltaTime, 0);
                }

                else if (this.direction == Direction.SOUTHEAST310)
                {
                    transform.Translate(2.5f * Time.deltaTime, -3f * Time.deltaTime, 0);
                }

                else if (this.direction == Direction.SOUTHEAST330)
                {
                    transform.Translate(4f * Time.deltaTime, -2f * Time.deltaTime, 0);
                }

            }

            else
            {
                if (afterRollCoolDown <= 0f)
                {
                    Vector3 rollVector = new Vector3(h * rollSpeed * Time.deltaTime, v * rollSpeed * Time.deltaTime, 0);
                    transform.position += rollVector;
                }

            }

        }
        else
        {
            boxCollider2D.offset = new Vector2(.1075f, -.37f);
            boxCollider2D.size = new Vector2(.70f, .65f);
        }

        if (knockBack)
        {
            if (knockBackCoolDown == knockBackTimeLength)
            {
                if (getDirectionNSEW() == Direction.NORTH)
                {
                    anim.Play("blowBackSouth");
                }
                else if (getDirectionNSEW() == Direction.SOUTH)
                {
                    anim.Play("blowBackNorth");
                }
                else if (getDirectionNSEW() == Direction.EAST)
                {
                    anim.Play("blowBackWest");
                }
                else if (getDirectionNSEW() == Direction.WEST)
                {
                    setSpriteFlipX(true);
                    anim.Play("blowBackWest");
                }
            }

            lockPosition = true;

            // knockBack position change
            if (knockBackCoolDown > (knockBackTimeLength - .5f))
            {
                if (this.direction == Direction.EAST)
                {
                    transform.Translate(-2f * Time.deltaTime, 0, 0);
                }
                else if (this.direction == Direction.NORTHEAST30)
                {
                    transform.Translate(-2f * Time.deltaTime, -1f * Time.deltaTime, 0);
                }

                else if (this.direction == Direction.NORTHEAST50)
                {
                    transform.Translate(-1.5f * Time.deltaTime, -1.5f * Time.deltaTime, 0);
                }

                else if (this.direction == Direction.NORTHEAST70)
                {
                    transform.Translate(-1f * Time.deltaTime, -1.5f * Time.deltaTime, 0);
                }

                else if (this.direction == Direction.NORTH)
                {
                    transform.Translate(0, -1.5f * Time.deltaTime, 0);
                }

                else if (this.direction == Direction.NORTHWEST110)
                {
                    transform.Translate(1f * Time.deltaTime, -1.5f * Time.deltaTime, 0);
                }

                else if (this.direction == Direction.NORTHWEST130)
                {
                    transform.Translate(1f * Time.deltaTime, -1f * Time.deltaTime, 0);
                }

                else if (this.direction == Direction.NORTHWEST150)
                {
                    transform.Translate(2f * Time.deltaTime, -1f * Time.deltaTime, 0);
                }

                else if (this.direction == Direction.WEST)
                {
                    transform.Translate(2f * Time.deltaTime, 0, 0);
                }

                else if (this.direction == Direction.SOUTHWEST210)
                {
                    transform.Translate(2f * Time.deltaTime, .5f * Time.deltaTime, 0);
                }

                else if (this.direction == Direction.SOUTHWEST230)
                {
                    transform.Translate(1.5f * Time.deltaTime, 1f * Time.deltaTime, 0);
                }

                else if (this.direction == Direction.SOUTHWEST250)
                {
                    transform.Translate(1.5f * Time.deltaTime, 1.5f * Time.deltaTime, 0);
                }

                else if (this.direction == Direction.SOUTH)
                {
                    transform.Translate(0, 1.5f * Time.deltaTime, 0);
                }

                else if (this.direction == Direction.SOUTHEAST290)
                {
                    transform.Translate(-.5f * Time.deltaTime, 2f * Time.deltaTime, 0);
                }

                else if (this.direction == Direction.SOUTHEAST310)
                {
                    transform.Translate(-1f * Time.deltaTime, 1f * Time.deltaTime, 0);
                }

                else if (this.direction == Direction.SOUTHEAST330)
                {
                    transform.Translate(-2f * Time.deltaTime, 1f * Time.deltaTime, 0);
                }

            }


            // changes the animation speed
            if (knockBackCoolDown < (knockBackTimeLength - .5f) && knockBackCoolDown > 1f)
            {
                anim.SetFloat("animationSpeed", .3f);
            }
            else anim.SetFloat("animationSpeed", 1f);
        }

        if (knockBack && knockBackCoolDown > 0f & knockBackCoolDown < .1f)
        {
            setSpriteFlipX(false);
            knockBack = false;
            anim.Play("Idle");
            lockPosition = false;
        }

        if (grabItem)
        {
            moveSpeed = 0;
            //anim.SetFloat("animationOffset", 0);

            if (getDirectionNSEW() == Direction.NORTH)
            {

                if (setItemDown)
                {
                    anim.Play("putDownNorth");
                }
                else
                {
                    anim.Play("pickUpNorth");

                    if (grabItemTimer > .6f)
                    {
                        transform.Translate(new Vector3(0, .01f, 0));
                    }
                }
            }
            else if (getDirectionNSEW() == Direction.SOUTH)
            {

                if (setItemDown)
                {
                    anim.Play("putDownSouth");
                }
                else
                {
                    anim.Play("pickUpSouth");

                }
            }
            else if (getDirectionNSEW() == Direction.EAST)
            {
                if (setItemDown)
                {
                    anim.Play("putDownEast");
                }
                else
                {
                    if (getDirection8() == Direction.NORTHEAST50)
                    {
                        anim.Play("pickUpNorth");

                        if (grabItemTimer > .6f)
                        {
                            transform.Translate(new Vector3(.007f, .01f, 0));
                        }

                    }
                    else
                    {
                        anim.Play("pickUpEast");
                    }

                    if (grabItemTimer > .6f)
                    {
                        transform.Translate(new Vector3(0, .007f, 0));
                    }
                }

            }
            else if (getDirectionNSEW() == Direction.WEST)
            {
                if (setItemDown)
                {
                    anim.Play("putDownWest");
                }
                else
                {
                    anim.Play("pickUpWest");

                    if (grabItemTimer > .6f)
                    {
                        transform.Translate(new Vector3(0, .007f, 0));
                    }
                }
            }


            if (animatorIsPlaying("pickUpSouth") || animatorIsPlaying("pickUpEast") || animatorIsPlaying("pickUpNorth") || animatorIsPlaying("pickUpWest"))
            {
                if (animationHasPlayedOnce())
                {
                    grabItem = false;
                    holdingThrowableItem = true;
                    anim.StopPlayback();
                }

            }

            if (animatorIsPlaying("putDownSouth") || animatorIsPlaying("putDownEast") || animatorIsPlaying("putDownNorth") || animatorIsPlaying("putDownWest"))
            {
                if (animationHasPlayedOnce())
                {
                    throwableItem.transform.parent = null;
                    setItemDown = false;
                    grabItem = false;
                    moveSpeed = 4;
                    anim.Play("Idle");
                }
            }
        }

        if (grabItem && grabItemTimer <= 0)
        {
            grabItemTimer = 1f;
        }

        if (grabItemTimer > 0)
        {
            grabItemTimer -= Time.deltaTime;
        }

        //Debug.Log("the grabItemTimer says " + grabItemTimer);


        if (holdingThrowableItem)
        {
            if (h > 0 || v > 0 || h < 0 || v < 0)
            {
                anim.Play("WalkingWithObjectOverhead");
            }
            else anim.StopPlayback();

            //only temporary, this should be Input.GetButton("PS4_X");
            if (Input.GetKey(KeyCode.X))
            {
                grabItem = true;
                holdingThrowableItem = false;
                throwableItem.transform.SendMessage("setDown", true);
            }

            if (Input.GetButton("PS4_R1"))
            {
                throwing = true;
                holdingThrowableItem = false;
                throwTimer = 100;
            }
        }

        if (throwing)
        {          
            throwableItem.transform.parent = null;


            if (animatorIsPlaying("throwing"))
            {
                freezeForAnimation = true;

                if (animationHasPlayedOnce())
                {
                    throwing = false;
                    moveSpeed = 4;
                    anim.Play("Idle");
                    freezeForAnimation = false;
                }
            }
        }


        if (rollingCoolDown > 0)
        {
            rollingCoolDown -= Time.deltaTime;
        }

        if (rolling && rollingCoolDown <= 0 && afterRollCoolDown <= 0)
        {
            afterRollCoolDown = .4f;
            //lockPosition = true;
            //rolling = false;
        }

        if (afterRollCoolDown > 0f && afterRollCoolDown < .05f)
        {
            rolling = false;
            //lockPosition = false;
            //anim.Play("Idle");
        }

        if (afterRollCoolDown > 0)
        {
            afterRollCoolDown -= Time.deltaTime;
        }

        if(throwTimer > 0)
        {
            throwTimer -= Mathf.RoundToInt(Time.deltaTime * 100);
        }


        //             Debug.Log("the afterRoll cooldown is " + afterRollCoolDown);
        //Debug.Log("the rolling bool is" + rolling);
        //Debug.Log("the rolling cooldown is " + rollingCoolDown);
    }


    //		Debug.Log("the run direction is: " + this.runDirection);
    //		Debug.Log("the direction is: " + this.direction);
    //		Debug.Log ("the animator's direction float is: " + anim.GetFloat ("direction(float)"));


    void setRunDirection()
    {
        // North, South, East, West
        if (analogAxesAngle > 85 && analogAxesAngle < 95)
        {
            this.runDirection = RunDirection.NORTH;
        }

        if (analogAxesAngle > -95 && analogAxesAngle < -85)
        {
            this.runDirection = RunDirection.SOUTH;

        }

        if (analogAxesAngle > -5 && analogAxesAngle < 15)
        {
            this.runDirection = RunDirection.EAST;

        }

        if (analogAxesAngle > 175 || analogAxesAngle < -175)
        {
            this.runDirection = RunDirection.WEST;

        }

        // Set NorthEast
        if (Input.GetAxis("Vertical") > 0 && Input.GetAxis("Horizontal") > 0)
        {

            if (analogAxesAngle > 15 && analogAxesAngle < 25)
            {
                this.runDirection = RunDirection.NORTHEAST20;


                //				Debug.Log ("it is now northeast 20");
            }

            if (analogAxesAngle > 25 && analogAxesAngle < 35)
            {
                this.runDirection = RunDirection.NORTHEAST30;

                //				Debug.Log ("it is now northeast 30");
            }

            if (analogAxesAngle > 35 && analogAxesAngle < 45)
            {
                this.runDirection = RunDirection.NORTHEAST40;

                //				Debug.Log ("it is now northeast 40");
            }

            if (analogAxesAngle > 45 && analogAxesAngle < 55)
            {
                this.runDirection = RunDirection.NORTHEAST50;

                //				Debug.Log ("it is now northheast 50");

            }

            if (analogAxesAngle > 55 && analogAxesAngle < 65)
            {
                this.runDirection = RunDirection.NORTHEAST60;

                //				Debug.Log ("it is now northeast 60");
            }

            if (analogAxesAngle > 65 && analogAxesAngle < 75)
            {
                this.runDirection = RunDirection.NORTHEAST70;

                //				Debug.Log ("it is now northeast 70");
            }

            if (analogAxesAngle > 75 && analogAxesAngle < 85)
            {
                this.runDirection = RunDirection.NORTHEAST80;

                //				Debug.Log ("it is now northeast 80");
            }
        }

        // Set NorthWest
        if (Input.GetAxis("Vertical") > 0 && Input.GetAxis("Horizontal") < 0)
        {
            if (analogAxesAngle > 105 && analogAxesAngle < 115)
            {
                this.runDirection = RunDirection.NORTHWEST110;

                //				Debug.Log ("it is now northwest 110");
            }

            if (analogAxesAngle > 115 && analogAxesAngle < 125)
            {
                this.runDirection = RunDirection.NORTHWEST120;

                //				Debug.Log ("it is now northwest 120");
            }

            if (analogAxesAngle > 125 && analogAxesAngle < 135)
            {
                this.runDirection = RunDirection.NORTHWEST130;

                //				Debug.Log ("it is now northhwest 130");

            }

            if (analogAxesAngle > 135 && analogAxesAngle < 145)
            {
                this.runDirection = RunDirection.NORTHWEST140;

                //				Debug.Log ("it is now northwest 140");
            }

            if (analogAxesAngle > 145 && analogAxesAngle < 155)
            {
                this.runDirection = RunDirection.NORTHWEST150;

                //				Debug.Log ("it is now northwest 150");
            }

            if (analogAxesAngle > 155 && analogAxesAngle < 165)
            {
                this.runDirection = RunDirection.NORTHWEST160;

                //				Debug.Log ("it is now northeast 160");
            }

            if (analogAxesAngle > 165 && analogAxesAngle < 175)
            {
                this.runDirection = RunDirection.NORTHWEST170;

                //				Debug.Log ("it is now northeast 170");
            }
        }

        // Set SouthWest
        if (Input.GetAxis("Vertical") < 0 && Input.GetAxis("Horizontal") < 0)
        {
            if (analogAxesAngle > -165 && analogAxesAngle < -155)
            {
                this.runDirection = RunDirection.SOUTHWEST200;


                //				Debug.Log ("it is now southwest 200");
            }

            if (analogAxesAngle > -155 && analogAxesAngle < -145)
            {
                this.runDirection = RunDirection.SOUTHWEST210;

                //				Debug.Log ("it is now southwest 210");
            }

            if (analogAxesAngle > -145 && analogAxesAngle < -135)
            {
                this.runDirection = RunDirection.SOUTHWEST220;

                //				Debug.Log ("it is now southwest 220");
            }

            if (analogAxesAngle > -135 && analogAxesAngle < -125)
            {
                this.runDirection = RunDirection.SOUTHWEST230;

                //				Debug.Log ("it is now southwest 230");
            }

            if (analogAxesAngle > -125 && analogAxesAngle < -115)
            {
                this.runDirection = RunDirection.SOUTHWEST240;

                //				Debug.Log ("it is now southwest 240");
            }

            if (analogAxesAngle > -115 && analogAxesAngle < -105)
            {
                this.runDirection = RunDirection.SOUTHWEST250;

                //				Debug.Log ("it is now southwest 250");
            }

            if (analogAxesAngle > -105 && analogAxesAngle < -95)
            {
                this.runDirection = RunDirection.SOUTHWEST260;

                //				Debug.Log ("it is now southwest 260");
            }
        }

        // Set SouthEast
        if (Input.GetAxis("Vertical") < 0 && Input.GetAxis("Horizontal") > 0)
        {
            if (analogAxesAngle > -85 && analogAxesAngle < -75)
            {
                this.runDirection = RunDirection.SOUTHEAST290;

                //				Debug.Log ("it is now southeast 290");
            }

            if (analogAxesAngle > -65 && analogAxesAngle < -55)
            {
                this.runDirection = RunDirection.SOUTHEAST300;

                //				Debug.Log ("it is now southeast 300");
            }

            if (analogAxesAngle > -55 && analogAxesAngle < -45)
            {
                this.runDirection = RunDirection.SOUTHEAST310;

                //				Debug.Log ("it is now southeast 310");
            }

            if (analogAxesAngle > -45 && analogAxesAngle < -35)
            {
                this.runDirection = RunDirection.SOUTHEAST320;

                //				Debug.Log ("it is now southeast 320");
            }

            if (analogAxesAngle > -35 && analogAxesAngle < -25)
            {
                this.runDirection = RunDirection.SOUTHEAST330;

                //				Debug.Log ("it is now southeast 330");
            }

            if (analogAxesAngle > -25 && analogAxesAngle < -15)
            {
                this.runDirection = RunDirection.SOUTHEAST340;

                //				Debug.Log ("it is now southeast 340");
            }

            if (analogAxesAngle > -15 && analogAxesAngle < -5)
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
        if (!swinging && !grabItem)
        {
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
                    //barrelCooldown = 1f;
                    //barrelSwitch = true;
                }

                if (barrelCooldown <= 0)
                {

                    Animator barrelAnimator = barrel.GetComponent<Animator>();

                    if (barrel.name.EndsWith("1"))
                    {
                        //barrelAnimator.Play("barrelBreakParticle");
                    }
                    else {
                        //barrelAnimator.Play("barrelBreak");
                    }

                }

            }
            //			print ("the actual barrel separation" + actualBarrelSeparation);
            //			print ("the barrelCooldown is " + barrelCooldown);
            //			print ("the barrelSwitch is " + barrelSwitch);

        }

        if (barrelCooldown <= 0)
        {
            //barrelSwitch = false;
        }

    }

    public AnimationClip GetAnimationClip(string name)
    {
        if (!anim) return null; // no animator

        foreach (AnimationClip clip in anim.runtimeAnimatorController.animationClips)
        {
            if (clip.name == name)
            {
                return clip;
            }
        }
        return null; // no clip by that name
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        collisionCount++;
        //       Debug.Log("the collision count is: " + collisionCount);

        if (coll.gameObject.tag == "fence")
        {

        }

        if (coll.gameObject.tag == "wall")
        {
            if (rolling)
            {
                rolling = false;
                knockBack = true;
                knockBackCoolDown = knockBackTimeLength;
            }
        }

        if (coll.gameObject.tag == "tree")
        {
            if (rolling)
            {
                rolling = false;
                knockBack = true;
                knockBackCoolDown = knockBackTimeLength;

                coll.gameObject.GetComponent<LeafEmitter>().jostled = true;
            }
        }


    }

    void OnCollisionStay2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "fence")
        {
            //lockPosition = true;
        }
    }

    void OnCollisionExit2D(Collision2D coll)
    {
        collisionCount--;

    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "fallingLedge")
        {
            setShortFall();
        }

        if (other.tag == "environment")
        {
            if (collisionCount == 0)
            {
                environmentCount++;
            }
        }

        if (other.name == "waterEdge")
        {
            renderMask.transform.localScale = new Vector3(.75f, .75f, .75f);
            renderMask.transform.localPosition = new Vector3(.05f, -1.44f, 0f);
        }

        if (other.name == "shortGrassEdge")
        {
            renderMask.transform.localPosition = new Vector3(.05f, -2.23f, 0f);
            renderMaskOutliner.transform.localScale = new Vector3(0, 0, 0);

        }

    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "environment")
        {

            environmentCount--;



            if (environmentCount <= 0)
            {
                renderMask.GetComponent<RenderMask>().setMaskType(RenderMask.MaskType.NULL);
            }
        }

        if (other.name == "waterEdge")
        {
            inWater = false;

            renderMask.transform.localScale = new Vector3(1.225f, 1.225f, 1.225f);
            renderMask.transform.localPosition = new Vector3(.05f, -2f, 0f);
        }

        if (other.name == "shortGrassEdge")
        {
            renderMaskOutliner.transform.localScale = new Vector3(0.76f, 1.09f, 1.09f);
        }


        restoreBlobShadowToNormal = true;

    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "environment")
        {

        }

        if (other.name == "waterEdge")
        {
            restoreBlobShadowToNormal = false;

            inWater = true;

            renderMask.GetComponent<RenderMask>().setMaskType(RenderMask.MaskType.WATER);
            setBlobShadowForWater();
        }

        if (other.name == "grassEdge")
        {
            restoreBlobShadowToNormal = false;

            renderMask.GetComponent<RenderMask>().setMaskType(RenderMask.MaskType.GRASS);
            setBlobShadowForGrass();
        }

        if (other.name == "shortGrassEdge")
        {
            restoreBlobShadowToNormal = false;

            renderMask.GetComponent<RenderMask>().setMaskType(RenderMask.MaskType.GRASS);

            setBlobShadowForGrass();
        }

    }

    public void releaseItem()
    {
        if (grabItem || holdingThrowableItem)
        {
            throwableItem.transform.SendMessage("setDropped", true);
            throwableItem.transform.parent = null;
            grabItem = false;
            holdingThrowableItem = false;
            anim.speed = 1f;
            moveSpeed = 4;
        }
    }

    public Transform getClosestTarget()
    {
        Transform closestTarget = GameObject.Find("TestTarget").transform;

            for (int i = 0; i < vision.visibleTargets.Count; i++)
            {

            if (i == 0) closestTarget = vision.visibleTargets[i];

                float dst = Vector3.Distance(vision.visibleTargets[i].position, this.transform.position);

                if (dst < Vector3.Distance(closestTarget.position, this.transform.position))
                {
                    closestTarget = vision.visibleTargets[i];
                }
                
            }

        return closestTarget;
        
        
    }

    public void setShortFall()
    {
        shortFall = true;
        shortFallCoolDown = .5f;
        anim.SetBool("shortFall", shortFall);
        setKinematic();
    }

    public void setBlobShadowForGrass()
    {
        playerBlobShadow.nearClipPlane = Mathf.Lerp(playerBlobShadow.nearClipPlane, 7.5f, Time.deltaTime * 2);
        playerBlobShadow.farClipPlane = Mathf.Lerp(playerBlobShadow.farClipPlane, 39f, Time.deltaTime * 2); 
        playerBlobShadow.fieldOfView = Mathf.Lerp(playerBlobShadow.fieldOfView, 3f, Time.deltaTime * 2);
        playerBlobShadow.aspectRatio = Mathf.Lerp(playerBlobShadow.aspectRatio, 1.4f, Time.deltaTime * 2);
        playerBlobShadow.orthographic = false;
        playerBlobShadow.transform.localPosition = new Vector3(Mathf.Lerp(playerBlobShadow.transform.localPosition.x, .07f, Time.deltaTime), Mathf.Lerp(playerBlobShadow.transform.localPosition.y, -.81f, Time.deltaTime * 2), -29f);
    }

    public void setBlobShadowForWater()
    {
        playerBlobShadow.nearClipPlane = Mathf.Lerp(playerBlobShadow.nearClipPlane, 5f, Time.deltaTime * 2);
        playerBlobShadow.farClipPlane = Mathf.Lerp(playerBlobShadow.farClipPlane, 32f, Time.deltaTime * 2);
        playerBlobShadow.fieldOfView = Mathf.Lerp(playerBlobShadow.fieldOfView, 5.5f, Time.deltaTime * 2);
        playerBlobShadow.aspectRatio = Mathf.Lerp(playerBlobShadow.aspectRatio, 1.4f, Time.deltaTime * 2);
        playerBlobShadow.orthographic = false;
        playerBlobShadow.transform.localPosition = new Vector3(Mathf.Lerp(playerBlobShadow.transform.localPosition.x, .275f, Time.deltaTime), Mathf.Lerp(playerBlobShadow.transform.localPosition.y, -.81f, Time.deltaTime * 2), -29f);
    }

    public void setBlobShadowToNorm()
    {
        playerBlobShadow.nearClipPlane = Mathf.Lerp(playerBlobShadow.nearClipPlane, 12.5f, Time.deltaTime * 2);
        playerBlobShadow.farClipPlane = Mathf.Lerp(playerBlobShadow.farClipPlane, 60f, Time.deltaTime * 2);
        playerBlobShadow.fieldOfView = Mathf.Lerp(playerBlobShadow.fieldOfView, 3.75f, Time.deltaTime * 2);
        playerBlobShadow.aspectRatio = Mathf.Lerp(playerBlobShadow.aspectRatio, 1f, Time.deltaTime * 2);
        playerBlobShadow.orthographic = false;
        playerBlobShadow.transform.localPosition = new Vector3(Mathf.Lerp(playerBlobShadow.transform.localPosition.x, -.09f, Time.deltaTime), Mathf.Lerp(playerBlobShadow.transform.localPosition.y, -1.7f, Time.deltaTime * 2), -29f);
    }

    public float getDirectionAngle360()
    {
        return  analogAxesAngle360;
    }

    public int getDirectionAngle180()
    {
        return (int) analogAxesAngle;
    }

    public bool getIsRunning()
    {
        return isRunning;
    }

    public bool getIsIdle()
    {
        return isIdle;
    }
	
}

