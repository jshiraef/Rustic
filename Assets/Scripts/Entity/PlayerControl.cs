using UnityEngine;
using System.Collections;
//using XInputDotNetPure;
//using UnityEngine.PS4;



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

    // stats
    public int maxHealth;
    public int currentHealth = 9;

    public int maxStamina;
    public float currentStamina;

    // items
    public bool swinging = false;
    private float swingCoolDown;

    public bool holdingThrowableItem;
    public bool grabItem;
    public float grabItemTimer;
    public bool setItemDown;
    public bool throwing;
    public int throwTimer;
    public bool hatAndCoat;


    private AnimatorClipInfo animInfo;

    // elements
    public bool inWater = false;
    private bool shortGrass = false;

    private CinemachineCameraShaker screenShake;
    public bool parallaxTrigger;

    public Transform lineStart, lineEnd, groundedEnd;

    public RaycastHit2D itemInView;

    public float rollSpeed = 8f;
    public float v, h;

    //private PlayerIndex playerIndex;
    private Projector playerBlobShadow;
    private bool restoreBlobShadowToNormal;

    public RunDirection runDirection;
    public RunDirection lastRecordedRunDirection;

    private Direction lastDirection8;

    // leaning against things
    private int leanTimer;
    public bool leaning;
    public bool wallInteract;

    // hopping over things
    private int hopTimer;
    public bool hopping;

    private bool pushing;

    public bool stumbling = false;
    private float stumbleCoolDown;
    private float stumbleTimeLength = 1f;

    private bool shortFall = false;
    private float shortFallCoolDown;

    private bool rumble = false;
    private float rumbleCoolDown;

    private GameObject player;
    private GameObject renderMask;
    private GameObject renderMaskOutliner;
    private GameObject throwableItem;
    private GameObject sickleSwipe;
    private GameObject grassSquiggle;
    private Sprite[] squiggleSprites;
    private Sprite[] runningSquiggleSprites;
    private SpriteRenderer sprite;
    private FieldOfView vision;

    private int toggleTimer;
    private bool flipflopToggle;

    public string originalLayerName;
    public int originalSortingOrderNumber;

    // barrel variables (variables similar to these can help keep track of relations between player and world objects)
    public GameObject[] barrels;
    public Vector2 distanceToBarrel;
    private GameObject nearestBarrel;
    private float actualBarrelSeparation;

    private float analogAxesAngle;
    private float analogAxesAngle360;
    private bool freezeForAnimation;

    private SatchelController inventory;

    public const string OverlapLayer = "Overlap";



    private static readonly int IDLE = 0;
    private static readonly int RUNNING = 1;
    private static readonly int WALKING = 2;
    private static readonly int ROLLING = 3;
    private static readonly int KNOCKBACK = 4;
    private static readonly int SWINGING = 5;
    private static readonly int ITEMGRAB = 6;
    private static readonly int WALKWITHITEM = 7;
    private static readonly int THROWING = 8;
    private static readonly int LEANAGAINST = 9;
    private static readonly int PUSHING = 10;
    private static readonly int HOPPING = 11;
    private static readonly int STUMBLING = 12;


    // Use this for initialization
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        maxVelocity = 77f;

        moveSpeed = 4f;
        currentHealth = 9;
        maxStamina = 7;
        currentStamina = maxStamina;

        anim = GetComponent<Animator>();
        
        currentAction = IDLE;

        this.direction = Direction.NULL;

        player = GameObject.Find("player");
        sprite = GetComponent<SpriteRenderer>();
        boxCollider2D = player.GetComponent<BoxCollider2D>();
        renderMask = transform.Find("renderMask").gameObject;
        renderMaskOutliner = renderMask.transform.Find("renderMaskOutliner").gameObject; 
        playerBlobShadow = player.GetComponentInChildren<Projector>();
        player.GetComponent<SpriteRenderer>().receiveShadows = true;
        screenShake = GameObject.Find("CM vcam1").GetComponent<CinemachineCameraShaker>();
        sickleSwipe = GameObject.Find("sickleSwipe");
        grassSquiggle = transform.Find("grassSquiggle").gameObject;
        squiggleSprites = Resources.LoadAll<Sprite>("grassSquiggle");
        runningSquiggleSprites = Resources.LoadAll<Sprite>("runningFootTrail");
        vision = GetComponent<FieldOfView>();
        inventory = GameObject.Find("inventory").GetComponent<SatchelController>();

        originalLayerName = sprite.sortingLayerName.ToString();
        originalSortingOrderNumber = sprite.sortingOrder;

        hatAndCoat = true;

        //		nearestBarrel = GameObject.Find ("barrel");
    }

    // Update is called once per frame
    void Update()
    {
        // checks every 10 frame to see if direction has changed
        if (toggleTimer == 1)
            lastDirection8 = getDirection8fromAngle();

        if (toggleTimer > 10)
            toggleTimer = 0;

        toggleTimer += Mathf.RoundToInt(Time.deltaTime * 100);

        //if (lastDirection8 != getDirection8fromAngle())
        //{
        //    Debug.Log("the direction changed; it used to be " + lastDirection8 + " but it is now " + getDirection8fromAngle() );
        //}


        if (rumble && rumbleCoolDown <= 0)
        {
            //GamePad.SetVibration(playerIndex, 0, 0);  
            //PS4Input.PadSetVibration(1, 0, 0);
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
            //if (animationHasPlayedOnce())
            //{
            //    swinging = false;
            //}
        }

        // setAction
        if (stumbling)
        {
            if(currentAction != STUMBLING)
            {
                currentAction = STUMBLING;
                moveSpeed = 0;
            }
        }
        if (hopping)
        {
            if(currentAction != HOPPING)
            {
                currentAction = HOPPING;
                moveSpeed = 0;
            }
        }
        else if (leaning)
        {
            if(currentAction != LEANAGAINST)
            {
                currentAction = LEANAGAINST;
                moveSpeed = 0;
            }
        }
        else if (pushing)
        {
            if(currentAction != PUSHING)
            {
                currentAction = PUSHING;
                moveSpeed = 1;
            }
        }
        else if (holdingThrowableItem)
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
                //anim.Play("SwingScythe");
                //lockPosition = true;
                isRunning = false;

                rumble = true;
                //GamePad.SetVibration(playerIndex, .5f, 0f);
                //PS4Input.PadSetVibration(1, 175, 0);
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
                //PS4Input.PadSetVibration(1, 65, 65);
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
                //PS4Input.PadSetVibration(1, 180, 180);

                rumbleCoolDown = .9f;
                screenShake.ShakeCamera(1f);
            }
        }
        // checks to see if analog is only slightly tilted for walk animation
        else if (Input.GetAxisRaw("Vertical") > -.4f && Input.GetAxisRaw("Vertical") < .4f && Input.GetAxisRaw("Horizontal") > -.4f && Input.GetAxisRaw("Horizontal") < .4f && !((h == 0) && (v == 0)))
        {
            if (!stumbling)
            {
                isWalking = true;
                moveSpeed = 4f;
            
            

                if (currentAction != WALKING)
                {
                    currentAction = WALKING;
                }

                if (!hatAndCoat)
                {
                    anim.Play("WalkingNoHat");
                }
                else anim.Play("Walking");

            }

        }
        else if (Input.GetAxisRaw("Horizontal") > 0 || Input.GetAxisRaw("Horizontal") < 0 || Input.GetAxisRaw("Vertical") > 0 || Input.GetAxisRaw("Vertical") < 0)
        {

            if (!lockPosition && !swinging && !rolling && !knockBack && !stumbling)
            {
                isRunning = true;
                moveSpeed = 4f;

                if (currentAction != RUNNING)
                {
                    currentAction = RUNNING;
                }

                if(!hatAndCoat)
                {
                    anim.Play("RunningNoHat");
                }
                else anim.Play("Running");

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
                if (!swinging && !rolling && !knockBack && !stumbling)
                {
                    if (!isIdle)
                    {
                        if (currentAction != IDLE)
                        {
                            currentAction = IDLE;

                            if (!hatAndCoat)
                            {
                                anim.Play("IdleNoHat");
                            }
                            else anim.Play("Idle");
                            
                        }

                    }
                    isIdle = true;
                }
            }
            else isIdle = false;
        }

        // setting the footprint in the short grass for running and idle states
        if ((isIdle || isRunning) && shortGrass)
        {           
            grassSquiggle.SetActive(true);

            if (isIdle)
            {
                grassSquiggle.GetComponent<SpriteRenderer>().sprite = squiggleLoad();
            }
            else if (isRunning)
            {
                if ((getAnimatorNormalizedTime() < .5f && getAnimatorNormalizedTime() > .47f) || (getAnimatorNormalizedTime() < .89f && getAnimatorNormalizedTime() > .86f))
                {
                    //Debug.Log("the animator time info is " + getAnimatorNormalizedTime());
                    GameObject newSquiggle = Instantiate(grassSquiggle);

                    if(getDirection8() == Direction.NORTH)
                    {
                        if (getAnimatorNormalizedTime() < .5f && getAnimatorNormalizedTime() > .47f)
                        {
                            newSquiggle.transform.position = new Vector3(player.transform.position.x, player.transform.position.y - 1.5f, player.transform.position.z);
                        }
                        else newSquiggle.transform.position = new Vector3(player.transform.position.x - .5f, player.transform.position.y - 1.5f, player.transform.position.z);
                    }
                    else if(getDirection8() == Direction.SOUTH)
                    {
                        if (getAnimatorNormalizedTime() < .5f && getAnimatorNormalizedTime() > .47f)
                        {
                            newSquiggle.transform.position = new Vector3(player.transform.position.x, player.transform.position.y - 1.5f, player.transform.position.z);
                        }
                        else newSquiggle.transform.position = new Vector3(player.transform.position.x + .5f, player.transform.position.y - 1.5f, player.transform.position.z);
                    }
                    else newSquiggle.transform.position = new Vector3(player.transform.position.x, player.transform.position.y - 1.5f, player.transform.position.z);


                    newSquiggle.GetComponent<SpriteRenderer>().sortingOrder -= 2;
                    newSquiggle.GetComponent<SpriteRenderer>().sprite = squiggleLoad();
                    newSquiggle.transform.SendMessage("setFadeOut", true);


                    //grassSquiggle.GetComponent<SpriteRenderer>().sprite = squiggleLoad();
                }

                grassSquiggle.GetComponent<SpriteRenderer>().sprite = null;
            }          

        }
        else grassSquiggle.SetActive(false);


        //allows the player to pause the game
        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    if (!inventory.enabled)
        //    {
        //        inventory.enabled = true;
        //    }
        //    else
        //    {
        //        inventory.enabled = false;
        //    }

        //}

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
            //lockPosition = false;
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

        if (knockBackCoolDown > 0)
        {
            knockBackCoolDown -= Time.deltaTime;
        }

        if(stumbleCoolDown > 0)
        {
            stumbleCoolDown -= Time.deltaTime;
        }

        if(currentStamina < maxStamina)
        {
            currentStamina += Time.deltaTime * 1f;
        }


        if (restoreBlobShadowToNormal)
        {
            setBlobShadowToNorm();
        }

        if (!player.GetComponent<SpriteRenderer>().isVisible)
        {
            Debug.Log("the player is hidden!!");
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

        wallInteract = Physics2D.Linecast(lineStart.position, lineEnd.position, 1 << LayerMask.NameToLayer("Wall"));
        
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
            //GetComponent<Rigidbody2D>().isKinematic = true;
            //Debug.Log("this is happening");

            body.MovePosition(Vector2.up * 200f);
            body.isKinematic = true;
            //body.gravityScale = -1f;
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
        if (Input.GetKeyDown(KeyCode.Q) )
        {

            if (!knockBack && !rolling && !throwing && !swinging && !leaning && !stumbling)
            {
                releaseItem();
                swinging = true;
                swingCoolDown = .62f;
            }
                   
        }
        

        //only temporary, this should be Input.GetButton("PS4_Triangle");
        if (Input.GetButton("PS4_Triangle"))
        {
            if (!rolling)
            {
                rollingCoolDown = .6f;
                currentStamina = maxStamina - 4;
            }
            if (afterRollCoolDown <= 0 && !knockBack && !throwing && !stumbling)
            {
                releaseItem();
                leanTimer = 0;
                rolling = true;
            }
        }

        if (swinging)
        {
            anim.Play("SwingScythe");

            //float swipeMultiplier = 58f;

            if(animatorIsPlaying("swingScythe"))
            {
                if(animationHasPlayedOnce())
                {
                    swinging = false;
                }
            }
            

            //swipeMultiplier += 1 / swingCoolDown * 45f;
            //float swipeAngle = startAngle + (swipeMultiplier * swingCoolDown);

            
        }

        if (rolling)
        {
            anim.Play("Rolling");

            // alters the player's collider during a roll
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


        if (knockBack || stumbling)
        {
            if(knockBack)
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
            }

            if (stumbleCoolDown == stumbleTimeLength)
            {
                if (getDirectionNSEW() == Direction.NORTH)
                {
                    anim.Play("stumbleNorth");
                }
                else if (getDirectionNSEW() == Direction.SOUTH)
                {
                    anim.Play("stumbleSouth");
                }
                else if (getDirectionNSEW() == Direction.EAST)
                {
                    anim.Play("stumbleWest");
                    setSpriteFlipX(true);
                }
                else if (getDirectionNSEW() == Direction.WEST)
                {  
                    anim.Play("stumbleWest");
                }
            }

            

            lockPosition = true;

            // knockBack position change
            if (knockBackCoolDown > (knockBackTimeLength - .5f) || stumbleCoolDown > (stumbleTimeLength - .5f))
            {
                if (this.direction == Direction.EAST)
                {
                    if (knockBack){
                        transform.Translate(-2f * Time.deltaTime, 0, 0);
                    }else if (stumbling) {
                        transform.Translate(-1f * Time.deltaTime, 0, 0);
                    }
                    
                }
                else if (this.direction == Direction.NORTHEAST30)
                {
                    if (knockBack){
                        transform.Translate(-2f * Time.deltaTime, -1f * Time.deltaTime, 0);
                    }else if (stumbling) {
                        transform.Translate(-1f * Time.deltaTime, -.5f * Time.deltaTime, 0);
                    }
                }

                else if (this.direction == Direction.NORTHEAST50)
                {
                     if (knockBack){
                        transform.Translate(-1.5f * Time.deltaTime, -1.5f * Time.deltaTime, 0);
                    }else if (stumbling) {
                        transform.Translate(-.75f * Time.deltaTime, -.75f * Time.deltaTime, 0);
                    }
                }

                else if (this.direction == Direction.NORTHEAST70)
                {
                     if (knockBack){
                        transform.Translate(-1f * Time.deltaTime, -1.5f * Time.deltaTime, 0);
                    }else if (stumbling) {
                        transform.Translate(-.5f * Time.deltaTime, -.75f * Time.deltaTime, 0);
                    }
                }

                else if (this.direction == Direction.NORTH)
                {
                     if (knockBack){
                        transform.Translate(0, -1.5f * Time.deltaTime, 0);
                    }else if (stumbling) {
                        transform.Translate(0, -.75f * Time.deltaTime, 0);
                    }
                }

                else if (this.direction == Direction.NORTHWEST110)
                {
                     if (knockBack){
                        transform.Translate(1f * Time.deltaTime, -1.5f * Time.deltaTime, 0);
                    }else if (stumbling) {
                        transform.Translate(.5f * Time.deltaTime, -.75f * Time.deltaTime, 0);
                    }
                }

                else if (this.direction == Direction.NORTHWEST130)
                {
                     if (knockBack){
                        transform.Translate(1f * Time.deltaTime, -1f * Time.deltaTime, 0);
                    }else if (stumbling) {
                        transform.Translate(.5f * Time.deltaTime, -.5f * Time.deltaTime, 0);
                    }
                }

                else if (this.direction == Direction.NORTHWEST150)
                {
                     if (knockBack){
                        transform.Translate(2f * Time.deltaTime, -1f * Time.deltaTime, 0);
                    }else if (stumbling) {
                        transform.Translate(1f * Time.deltaTime, -.5f * Time.deltaTime, 0);
                    }
                }

                else if (this.direction == Direction.WEST)
                {
                     if (knockBack){
                        transform.Translate(2f * Time.deltaTime, 0, 0);
                    }else if (stumbling) {
                        transform.Translate(1f * Time.deltaTime, 0, 0);
                    }
                }

                else if (this.direction == Direction.SOUTHWEST210)
                {
                     if (knockBack){
                        transform.Translate(2f * Time.deltaTime, .5f * Time.deltaTime, 0);
                    }else if (stumbling) {
                        transform.Translate(1f * Time.deltaTime, .25f * Time.deltaTime, 0);
                    }
                }

                else if (this.direction == Direction.SOUTHWEST230)
                {
                     if (knockBack){
                        transform.Translate(1.5f * Time.deltaTime, 1f * Time.deltaTime, 0);
                    }else if (stumbling) {
                        transform.Translate(.75f * Time.deltaTime, .5f * Time.deltaTime, 0);
                    }
                }

                else if (this.direction == Direction.SOUTHWEST250)
                {
                     if (knockBack){
                        transform.Translate(1.5f * Time.deltaTime, 1.5f * Time.deltaTime, 0);
                    }else if (stumbling) {
                        transform.Translate(.75f * Time.deltaTime, .75f * Time.deltaTime, 0);
                    }
                }

                else if (this.direction == Direction.SOUTH)
                {
                     if (knockBack){
                        transform.Translate(0, 1.5f * Time.deltaTime, 0);
                    }else if (stumbling) {
                        transform.Translate(0, .75f * Time.deltaTime, 0);
                    }
                }

                else if (this.direction == Direction.SOUTHEAST290)
                {
                     if (knockBack){
                        transform.Translate(-.5f * Time.deltaTime, 2f * Time.deltaTime, 0);
                    }else if (stumbling) {
                        transform.Translate(-.25f * Time.deltaTime, 1f * Time.deltaTime, 0);
                    }
                }

                else if (this.direction == Direction.SOUTHEAST310)
                {
                     if (knockBack){
                        transform.Translate(-1f * Time.deltaTime, 1f * Time.deltaTime, 0);
                    }else if (stumbling) {
                        transform.Translate(-.5f * Time.deltaTime, .5f * Time.deltaTime, 0);
                    }
                }

                else if (this.direction == Direction.SOUTHEAST330)
                {
                    if (knockBack){
                        transform.Translate(-2f * Time.deltaTime, 1f * Time.deltaTime, 0);
                    }else if (stumbling) {
                        transform.Translate(-1f * Time.deltaTime, .5f * Time.deltaTime, 0);
                    }
                }

            }


            // changes the animation speed
            if (knockBackCoolDown < (knockBackTimeLength - .5f) && knockBackCoolDown > 1f)
            {
                anim.SetFloat("animationSpeed", .3f);
            }
            else anim.SetFloat("animationSpeed", 1f);
        }

        if (knockBack && knockBackCoolDown > 0f && knockBackCoolDown < .1f)
        {
            setSpriteFlipX(false);
            knockBack = false;

            if (!hatAndCoat)
            {
                anim.Play("IdleNoHat");
            }
            else anim.Play("Idle");

            lockPosition = false;
        }

        if(stumbling && stumbleCoolDown > 0f && stumbleCoolDown < .2f)
        {
            setSpriteFlipX(false);
            stumbling = false;

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


                    if (!hatAndCoat)
                    {
                        anim.Play("IdleNoHat");
                    }
                    else anim.Play("Idle");
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
        } else if (throwing)
        {          
            throwableItem.transform.parent = null;


            if (animatorIsPlaying("throwing"))
            {
                freezeForAnimation = true;

                if (animationHasPlayedOnce())
                {
                    throwing = false;
                    moveSpeed = 4;

                    if (!hatAndCoat)
                    {
                        anim.Play("IdleNoHat");
                    }
                    else anim.Play("Idle");

                    freezeForAnimation = false;
                }
            }
        }else if(leaning && !pushing)
        {
            anim.Play("LeanToPush");
        }
        else if (hopping)
        {
            isRunning = false;
            anim.Play("ShortHopUpSouthWest");
            moveSpeed = 4;
        }
        else if (stumbling)
        {
            isRunning = false;
            isWalking = false;
            moveSpeed = 2;
        }
        else if (pushing)
        {
            if(getDirectionNSEW() == Direction.NORTH)
            {
                anim.Play("pushingNorth");
            }
            else if (getDirectionNSEW() == Direction.SOUTH)
            {
                anim.Play("pushingSouth");
            }
            else if (getDirectionNSEW() == Direction.EAST)
            {
                anim.Play("pushingEast");
            }
            else if (getDirectionNSEW() == Direction.WEST)
            {
                anim.Play("pushingWest");
            }
        }



        if (swingCoolDown > 0)
        {
            swingCoolDown -= Time.deltaTime;
        }

        if (swinging && swingCoolDown <= 0)
        {
            swinging = false;
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

        if(hopTimer > 0)
        {
            hopTimer -= Mathf.RoundToInt(Time.deltaTime * 100);           

            if(hopTimer < 50)
            {
                GetComponent<BoxCollider2D>().enabled = false;
            }

            // move the player slighty up
            if(hopTimer > 70)
            {
                transform.Translate(0, .3f, 0);
            }

            hopping = true;
        }
        else if(hopTimer <= 0)
        {
            GetComponent<BoxCollider2D>().enabled = true;
            returSpriteToOriginLayer();
            hopping = false;
            hopTimer = 0;
        }

        if(lastDirection8 != getDirection8fromAngle())
        {
            leanTimer = 0;
            leaning = false;
        }

        if (leanTimer > 25 && Input.GetAxisRaw("Horizontal") != 0 && Input.GetAxisRaw("Vertical") != 0)
        {
            leaning = true;
        }
        else if (leanTimer <= 0)
        {
            leaning = false;
            leanTimer = 0;
        }

        //only temporary, this should be Input.GetButton("PS4_X");
        if (leaning && Input.GetKey(KeyCode.X) && leanTimer > 50)
        {
            pushing = true;
        }
        else pushing = false;


        //Debug.Log("the lock position " + lockPosition);
        //Debug.Log("the hopTimer is " + hopTimer);
        // Debug.Log("the animator is playing" + anim.runtimeAnimatorController.animationClips[24]);

        //Debug.Log("the normalized time of the current animation is " + getAnimatorNormalizedTime());

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
        if (!swinging && !grabItem && !knockBack && !stumbling)
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

            if (barrel.GetComponent<CircleCollider2D>().IsTouching(sickleSwipe.GetComponent<CircleCollider2D>()))
            {
                barrel.GetComponent<Barrel>().setHit(true);
                screenShake.ShakeCamera(.1f);
                Debug.Log("the sickle swipe hit the barrel collider in PlayerControl.cs");
            }
            //			print ("the actual barrel separation" + actualBarrelSeparation);
            //			print ("the barrelCooldown is " + barrelCooldown);
            //			print ("the barrelSwitch is " + barrelSwitch);
            
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
        //Debug.Log("the collision count is: " + collisionCount);

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

                if(coll.gameObject.GetComponent<LeafEmitter>() != null)
                {
                    coll.gameObject.GetComponent<LeafEmitter>().jostled = true;
                }
            }
        }

        if(coll.gameObject.tag == "weapon")
        {
            rolling = false;
            if (!knockBack)
            {
                currentHealth -= 3;
                knockBack = true;
                knockBackCoolDown = knockBackTimeLength;
            }
           
            
            
            
        }

    }

    void OnCollisionStay2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "fence")
        {
            //lockPosition = true;
        }

        if (coll.gameObject.tag == "wall" && !rolling && wallInteract)
        {
            if ((v > .5 || v < -.5 || h > .5 || h < -.5))
            {
                anim.SetFloat("animationSpeed", 1f);
                leanTimer += Mathf.RoundToInt(Time.deltaTime * 100);
            }
            else if(leanTimer > 0)
            {
                if (animatorIsPlaying("LeanToPush") && getAnimatorNormalizedTime() < .01f)
                {
                    leanTimer = 0;
                }
                leanTimer -= Mathf.RoundToInt(Time.deltaTime * 100);
                    anim.SetFloat("animationSpeed", -1.3f);             
            }                        
        }
        else
        {
            leanTimer = 0;
            anim.SetFloat("animationSpeed", 1f); 
        }

        if(coll.gameObject.tag == "hoppableBarrier" && Input.GetKey(KeyCode.X))
        {
            if (!hopping)
            {
                hopTimer = 100;
            }         
            Debug.Log("we are hopping!");
        }

        if (hopping)
        {
            if(hopTimer < 60)
            {
                Overlap(coll.gameObject);
            }
        }
        
    }

    void OnCollisionExit2D(Collision2D coll)
    {
        collisionCount--;

        if(coll.gameObject.tag == "wall")
        {
            leanTimer = 0;
        }
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
            //renderMask.transform.localPosition = new Vector3(.05f, -2.23f, 0f);
            //renderMaskOutliner.transform.localScale = new Vector3(0, 0, 0);
            shortGrass = true;

        }

        if (other.tag == "foregroundParallaxTrigger")
        {
            parallaxTrigger = true;
        }

        if (other.tag == "transparentTrigger")
        {
            other.gameObject.transform.parent.SendMessage("setTempBool", true);
        }

    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "environment")
        {

            environmentCount--;

            if (environmentCount <= 0)
            {
                renderMask.GetComponent<RenderMask>().setPlayerMaskType(RenderMask.MaskType.NULL);
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
            //renderMaskOutliner.transform.localScale = new Vector3(0.76f, 1.09f, 1.09f);
            shortGrass = false;
        }

        if (other.name == "parallaxTrigger")
        {
            parallaxTrigger = false;
        }

        if (other.tag == "transparentTrigger")
        {
            other.gameObject.transform.parent.SendMessage("setTempBool", false);
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

            renderMask.GetComponent<RenderMask>().setPlayerMaskType(RenderMask.MaskType.WATER);
            setBlobShadowForWater();
        }

        if (other.name == "grassEdge")
        {
            restoreBlobShadowToNormal = false;

            renderMask.GetComponent<RenderMask>().setPlayerMaskType(RenderMask.MaskType.GRASS);
            setBlobShadowForGrass();
        }

        if (other.name == "shortGrassEdge")
        {
            restoreBlobShadowToNormal = false;

            playerBlobShadow.farClipPlane = Mathf.Lerp(playerBlobShadow.farClipPlane, 45f, Time.deltaTime * 2);


            //renderMask.GetComponent<RenderMask>().setPlayerMaskType(RenderMask.MaskType.GRASS);
            
            //setBlobShadowForGrass();
        }

        if(other.name == "stairs")
        {
            stumbling = true;
            stumbleCoolDown = stumbleTimeLength;
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
        playerBlobShadow.transform.localPosition = Vector3.Lerp(playerBlobShadow.transform.localPosition, new Vector3(.07f, -.81f, -29f), Time.deltaTime * 2);

    }

    public void setBlobShadowForWater()
    {
        playerBlobShadow.nearClipPlane = Mathf.Lerp(playerBlobShadow.nearClipPlane, 5f, Time.deltaTime * 2);
        playerBlobShadow.farClipPlane = Mathf.Lerp(playerBlobShadow.farClipPlane, 32f, Time.deltaTime * 2);
        playerBlobShadow.fieldOfView = Mathf.Lerp(playerBlobShadow.fieldOfView, 5.5f, Time.deltaTime * 2);
        playerBlobShadow.aspectRatio = Mathf.Lerp(playerBlobShadow.aspectRatio, 1.4f, Time.deltaTime * 2);
        playerBlobShadow.orthographic = false;
        Vector3.Lerp(playerBlobShadow.transform.localPosition, new Vector3(.275f, -.81f, -29f), Time.deltaTime * 2);
        playerBlobShadow.transform.localPosition = Vector3.Lerp(playerBlobShadow.transform.localPosition, new Vector3(.275f, -.81f, -29f), Time.deltaTime * 2);
    }

    public void setBlobShadowToNorm()
    {
        playerBlobShadow.nearClipPlane = Mathf.Lerp(playerBlobShadow.nearClipPlane, 12.5f, Time.deltaTime * 2);
        playerBlobShadow.farClipPlane = Mathf.Lerp(playerBlobShadow.farClipPlane, 60f, Time.deltaTime * 2);
        playerBlobShadow.fieldOfView = Mathf.Lerp(playerBlobShadow.fieldOfView, 3.75f, Time.deltaTime * 2);
        playerBlobShadow.aspectRatio = Mathf.Lerp(playerBlobShadow.aspectRatio, 1f, Time.deltaTime * 2);
        playerBlobShadow.orthographic = false;
        playerBlobShadow.transform.localPosition = Vector3.Lerp(playerBlobShadow.transform.localPosition, new Vector3(-.09f, -1.7f, -29f), Time.deltaTime * 2);
    }

    public float getDirectionAngle360()
    {
        return analogAxesAngle360;
    }

    public int getDirectionAngle180()
    {
        return (int)analogAxesAngle;
    }

    // this will make the player Sprite overlap another object
    public void Overlap(GameObject thingToOverlap)
    {

        GetComponent<SpriteRenderer>().sortingLayerName = OverlapLayer;
        this.GetComponent<SpriteRenderer>().sortingOrder = thingToOverlap.GetComponentInParent<SpriteRenderer>().sortingOrder + 1;

    }

    // this will return the player Sprite back to its original sorting Layer & sortingOrder #
    public void returSpriteToOriginLayer()
    {
        this.GetComponent<SpriteRenderer>().sortingLayerName = originalLayerName;
        this.GetComponent<SpriteRenderer>().sortingOrder = originalSortingOrderNumber;
    }


    public Sprite squiggleLoad()
    {
        if (isIdle)
        {
            if (analogAxesAngle360 < 34 && analogAxesAngle360 > 11.25)
            {
                grassSquiggle.transform.localPosition = new Vector3(-.113f, -1.564f, 0);
                return squiggleSprites[2];
            }
            else if (analogAxesAngle360 < 79 && analogAxesAngle360 > 34)
            {
                grassSquiggle.transform.localPosition = new Vector3(-.049f, -1.532f, 0);
                return squiggleSprites[3];
            }
            else if (analogAxesAngle360 < 101 && analogAxesAngle360 > 79)
            {
                grassSquiggle.transform.localPosition = new Vector3(-.072f, -1.638f, 0);
                return squiggleSprites[1];
            }
            else if (analogAxesAngle360 < 124 && analogAxesAngle360 > 101)
            {
                grassSquiggle.transform.localPosition = new Vector3(-.218f, -1.623f, 0);
                return squiggleSprites[4];
            }
            else if (analogAxesAngle360 < 169 && analogAxesAngle360 > 124)
            {
                grassSquiggle.transform.localPosition = new Vector3(-.129f, -1.734f, 0);
                return squiggleSprites[5];
            }
            else if (analogAxesAngle360 < 191 && analogAxesAngle360 > 169)
            {
                grassSquiggle.transform.localPosition = new Vector3(-.005f, -1.702f, 0);
                return squiggleSprites[11];
            }
            else if (analogAxesAngle360 < 214 && analogAxesAngle360 > 191)
            {
                grassSquiggle.transform.localPosition = new Vector3(-.052f, -1.591f, 0);
                return squiggleSprites[9];
            }
            else if (analogAxesAngle360 < 259 && analogAxesAngle360 > 214)
            {
                grassSquiggle.transform.localPosition = new Vector3(.086f, -1.606f, 0);
                return squiggleSprites[10];
            }
            else if (analogAxesAngle360 < 281 && analogAxesAngle360 > 259)
            {
                grassSquiggle.transform.localPosition = new Vector3(.073f, -1.552f, 0);
                return squiggleSprites[6];
            }
            else if (analogAxesAngle360 < 304 && analogAxesAngle360 > 281)
            {
                grassSquiggle.transform.localPosition = new Vector3(.07f, -1.533f, 0);
                return squiggleSprites[7];
            }
            else if (analogAxesAngle360 < 340 && analogAxesAngle360 > 304)
            {
                grassSquiggle.transform.localPosition = new Vector3(.053f, -1.537f, 0);
                return squiggleSprites[8];
            }
            else if (analogAxesAngle360 < 11.25 && analogAxesAngle360 > 349)
            {
                grassSquiggle.transform.localPosition = new Vector3(.074f, -1.48f, 0);
                return squiggleSprites[0];
            }
            else
            {
                grassSquiggle.transform.localPosition = new Vector3(.074f, -1.48f, 0);
                return squiggleSprites[0];
            }
        }
        else if (isRunning)
        {
            if (getDirection8() == Direction.EAST)
            {
                return runningSquiggleSprites[0];
            }
            else if (getDirection8() == Direction.NORTHEAST50)
            {
                return runningSquiggleSprites[2];
            }
            else if (getDirection8() == Direction.NORTH)
            {
                return runningSquiggleSprites[1];
            }
            else if (getDirection8() == Direction.NORTHWEST130)
            {
                return runningSquiggleSprites[3];
            }
            else if (getDirection8() == Direction.WEST)
            {
                return runningSquiggleSprites[7];
            }
            else if (getDirection8() == Direction.SOUTHWEST230)
            {
                return runningSquiggleSprites[6];
            }
            else if (getDirection8() == Direction.SOUTH)
            {
                return runningSquiggleSprites[4];
            }
            else if (getDirection8() == Direction.SOUTHEAST310)
            {
                return runningSquiggleSprites[5];
            }
            else return null;
            
        }
        else
        {
            return null;
        }
        
    }

    public Direction getDirection8fromAngle()
    {
        if (getDirectionAngle360() >= 0 && getDirectionAngle360() < 21.1)
        {
            return Direction.EAST;
            //East
        }
        else if (getDirectionAngle360() < 63.3)
        {
            return Direction.NORTHEAST50;
            //NorthEast30
        }
        else if (getDirectionAngle360() < 105.5)
        {
            return Direction.NORTH;
            //North
        }
        else if (getDirectionAngle360() < 147.7)
        {
            return Direction.NORTHWEST130;
            //NorthWest130
        }
        else if (getDirectionAngle360() < 189.9)
        {
            return Direction.WEST;           
            //West
        }
        else if (getDirectionAngle360() <= 232.1)
        {
            return Direction.SOUTHWEST230;
            //southWest230
        }
        else if (getDirectionAngle360() < 264.3)
        {
            return Direction.SOUTH;
            //south
        }
        else if (getDirectionAngle360() < 316.5)
        {
            return Direction.SOUTHEAST310;
            //southWest310
        }
        else 
        {
            return Direction.EAST;
            //East
        }
       
    }

    // fading out the alpha of the sprite
    //void fadeOut(GameObject disappearingObject)
    //{

    //    if (faded)
    //        return;

    //    if (!faded)
    //    {
    //        disappearingObject.GetComponent<SpriteRenderer>().alpha -= .005f * Time.deltaTime * 60;
    //        // Debug.Log("it is fading");
    //    }

    //    if (playerInventory.GetComponent<CanvasGroup>().alpha <= .001f)
    //    {
    //        faded = true;

    //    }


    //}

    public bool getIsRunning()
    {
        return isRunning;
    }

    public bool getIsIdle()
    {
        return isIdle;
    }

    public bool getSwinging()
    {
        return swinging;
    }

    public bool getShortGrass()
    {
        return shortGrass;
    }

    public void setSwinging(bool b)
    {
        swinging = b;
    }

    public float getSwingCoolDown()
    {
        return swingCoolDown;
    }

    public int getCurrentHealth()
    {
        return currentHealth;
    }

    public float getCurrentStamina()
    {
        return currentStamina;
    }

    public int getMaxStamina()
    {
        return maxStamina;
    }
	
}

