using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WellController : Entity
{
    private GameObject wellCrank;
    private GameObject wellCrankOutline;
    private bool glowUp;
    private Animator wellCrankAnim;
    private Animator wellBucketAnim;
    public RuntimeAnimatorController mainAnimatorController, extraPlayerController;
    private int wellCrankTimer;
    private bool wellCranking, wellFullyCranked, crankReleased, playerSkippedOff;
    private GameObject wellRope;
    private GameObject wellCollider1, wellCollider2;
    private GameObject wellLogRoll;
    private GameObject wellRopeClump;
    private GameObject innerWellCollider;
    private GameObject wellBucket, wellBucketOutline;
    private PlayerControl playerControl;
    private Vector3 wellRopeOriginalPosition, wellCrankPosition, wellCrankPosition2;

    private float distanceToPlayer;

    private GameObject AlertIcon;

    private waterBob waterBob;

    private bool _withinTalkingRange;

    public TextAsset dialogue;
    private Dialogue dialogueUI;

    public bool dialogueIsShowing;

    private CircleCollider2D dialogueCollider;

    public AudioSource audioSource;

    [SerializeField] AnimatorSound animatorSound;

    // Start is called before the first frame update
    void Start()
    {
        wellCrank = transform.Find("Well_Crank").gameObject;
        wellRope = transform.Find("ropeTemp").GetChild(0).gameObject;

        wellRopeClump = transform.Find("ropeClump").gameObject;
        wellRopeClump.GetComponent<Animator>().Play("wellRopeClump");
        wellRopeClump.GetComponent<Animator>().enabled = false;

        wellCollider1 = transform.Find("collider1").gameObject;
        wellCollider2 = transform.Find("collider2").gameObject;
        wellCollider2.SetActive(false);
        wellLogRoll = transform.Find("wellLogRoll").gameObject;
        wellLogRoll.SetActive(false);

        wellRopeOriginalPosition = wellRope.transform.position;
        innerWellCollider = transform.Find("innerWellCollider").gameObject;
        innerWellCollider.SetActive(false);

        AlertIcon = GameObject.Find("AlertIcon");

        wellCrankOutline = wellCrank.transform.Find("Outline").gameObject;
        wellCrankOutline.GetComponent<SpriteRenderer>().enabled = false;
        wellCrankAnim = wellCrank.GetComponent<Animator>();
        wellCrankAnim.enabled = false;

        wellCrankPosition = transform.Find("wellCrankPosition").transform.position;
        wellCrankPosition2 = transform.Find("wellCrankPosition").gameObject.transform.GetChild(0).transform.position;

        audioSource = GetComponent<AudioSource>();

        // find the well bucket deep in the hierarchy
        Transform[] children = GetComponentsInChildren<Transform>();
        
        foreach(Transform child in children)
        {
            if(child.transform.name == "tempBucket")
            {
                wellBucket = child.transform.gameObject;
                wellBucketAnim = wellBucket.GetComponent<Animator>();
            }
        }

        playerControl = GameObject.Find("player").GetComponent<PlayerControl>();

        dialogueUI = GameObject.Find("Well Dialogue Text").gameObject.GetComponent<Dialogue>();
        dialogueCollider = GetComponent<CircleCollider2D>();

        // for some reason the waterbob script must be turned off then back on in order for it to work
        waterBob = GetComponentInChildren<waterBob>();
        waterBob.enabled = false;
        waterBob.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        // this makes the outline around the wellCrank glow or appear to be glowing
        if (wellCrankOutline.GetComponent<SpriteRenderer>().enabled)
        {
            if (!glowUp)
            {
                if (wellCrank.GetComponent<SpriteOutline2>().blurAlphaMultiplier > .2f)
                {
                    wellCrank.GetComponent<SpriteOutline2>().blurAlphaMultiplier -= .05f;
                }

                if (wellCrank.GetComponent<SpriteOutline2>().blurAlphaMultiplier < .2f)
                {
                    glowUp = true;
                }
            }

            if (glowUp)
            {
                wellCrank.GetComponent<SpriteOutline2>().blurAlphaMultiplier += .05f;

                if (wellCrank.GetComponent<SpriteOutline2>().blurAlphaMultiplier > .98f)
                {
                    glowUp = false;
                }
            }
        }

        // makes the rope come up when the crank is turning
        if (wellCranking)
        {
            if (wellRope.transform.GetChild(0).transform.position.y <= -35)
            {
                wellCrankTimer += Mathf.RoundToInt(Time.deltaTime * 100);
                wellRope.transform.Translate(-.6f *Time.deltaTime, 0, 0);

                wellRopeClump.GetComponent<Animator>().SetFloat("reverseAnimationMultiplier", 1f);
                wellRopeClump.GetComponent<Animator>().Play("wellRopeClump");

            }

            if (wellRope.transform.GetChild(0).transform.position.y > -35)
            {
                //Debug.Log("the rope reached the choke point");
                wellFullyCranked = true;
                wellCranking = false;
            }

            wellRopeClump.GetComponent<Animator>().enabled = true;
        }

        if (wellCrankTimer > 0)
        {
            //wellCollider1.SetActive(false);
            //wellCollider2.SetActive(true);

            if (!wellFullyCranked)
            {
                wellLogRoll.SetActive(true);
            }
            else wellLogRoll.SetActive(false);
        }

        // this is what makes the rope and bucket go back down into the well
        if (wellCrankTimer > 0 && !wellCranking)
        {
            if (!wellFullyCranked)
            {
                wellCrankTimer -= Mathf.RoundToInt(Time.deltaTime * 100);
            }

            if (wellRope.transform.position.x < wellRopeOriginalPosition.x && !wellFullyCranked)
            {
                wellRope.transform.Translate(+.6f * Time.deltaTime, 0f , 0);

                wellRopeClump.GetComponent<Animator>().SetFloat("reverseAnimationMultiplier", -1);

                if (wellRopeClump.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 1f)
                {
                    wellRopeClump.GetComponent<Animator>().Play("wellRopeClump", 0, 1f);
                }

                wellRopeClump.GetComponent<Animator>().Play("wellRopeClump");
            }
        }

        // forces the animation to the begginning if the rope is completely let all the way down
        if(wellCrankTimer <= 0 && wellRope.transform.position.x >= wellRopeOriginalPosition.x)
        {
            wellRopeClump.GetComponent<Animator>().Play("wellRopeClump", 0, 0f);
        }

        // makes the original well lever disappear while the player animation plays 
        if (playerControl.animatorIsPlaying("wellCrank"))
        {
            wellCrank.GetComponent<SpriteRenderer>().enabled = false;
            wellCrank.GetComponent<SpriteOutline2>().enabled = false;
        }
        else
        {
            wellCrank.GetComponent<SpriteRenderer>().enabled = true;
            wellCrank.GetComponent<SpriteOutline2>().enabled = true;
            playerControl.setLockPosition(false);
        }

        // this allows the player to release the crank and pause and resume cranking if necessary
        if (!wellCranking &&
        playerControl.GetComponent<BoxCollider2D>().IsTouching(innerWellCollider.GetComponent<PolygonCollider2D>()))
        {
            crankReleased = true;
            wellCollider2.SetActive(true);
        }

        // this detects if the player has chosen to move away from the well crank instead of continuing cranking
        // this also invokes the player jumpdown animation from the well ledge
        if (crankReleased &&
        !playerControl.GetComponent<BoxCollider2D>().IsTouching(innerWellCollider.GetComponent<PolygonCollider2D>()))
        {
            playerSkippedOff = true;
            crankReleased = false;
            wellCollider2.SetActive(false);
            playerControl.GetComponent<Animator>().runtimeAnimatorController = extraPlayerController;
            playerControl.GetComponent<Animator>().Play("wellDismount");
            playerControl.setLockPosition(true);

        }


        
        // this reinitializes and resumes the player back to its regular Animator Controller.
        if (playerSkippedOff && playerControl.animatorIsPlaying("wellDismount"))
        {
            if (playerControl.v == 0 && playerControl.h == 0)
            {
                playerControl.transform.Translate(.005f, .007f, 0);
            }
            else playerControl.setMoveSpeed(.07f);

            if (playerControl.animationHasPlayedOnce())
            {
                playerSkippedOff = false;
                playerControl.setLockPosition(false);
                playerControl.transform.Translate(0, -.75f, 0);
                //Debug.Log("we transform dot translated");
                wellCollider1.GetComponent<PolygonCollider2D>().isTrigger = false;
                playerControl.GetComponent<Animator>().runtimeAnimatorController = mainAnimatorController;
            }
        }

        if (wellBucketOutline == null && wellBucket.transform.childCount > 0)
        {
            wellBucketOutline = wellBucket.transform.GetChild(0).gameObject;
        }

        if (Vector3.Distance(wellBucket.transform.position, playerControl.transform.position) < 2)
        {
            wellBucketOutline.SetActive(true);

            if (Input.GetKey(KeyCode.X) && playerControl.currentAncientWater < playerControl.maxWater)
            {
                playerControl.setCurrentAncientWater(.05f);

                if(playerControl.transform.position.y > wellBucket.transform.position.y)
                {
                    playerControl.GetComponent<Animator>().Play("pushingSouth");
                }
                else
                {
                    playerControl.GetComponent<Animator>().Play("pushingNorth");
                }             
                
            }

            if (Input.GetKeyUp(KeyCode.X))
            {
                playerControl.GetComponent<Animator>().Play("Idle");
            }

        }
        else wellBucketOutline.SetActive(false);

        // this uses the animation to trigger the wellCranking bool
        if (playerControl.animatorIsPlaying("wellCrank"))
        {
            wellCranking = true;
        }


        if (wellCrankTimer <= 0)
        {
            wellCrankTimer = 0;

            wellLogRoll.SetActive(false);
            wellRopeClump.GetComponent<Animator>().enabled = false;
        }



        //Dialogue Practice pay no attention to the next 50 lines of code here
        if (_withinTalkingRange && !dialogueUI.getIsDialoguePlaying())
        {
            AlertIcon.SetActive(true);
        }
        else
        {
            AlertIcon.SetActive(false);
        }

        if (!_withinTalkingRange || !wellFullyCranked)
        {
            
            dialogueUI.transform.parent.transform.parent.GetComponent<CanvasGroup>().alpha = 0;
        }

        if (!dialogueUI.getIsDialoguePlaying())
        {

        }
        else if (dialogueUI.getIsDialoguePlaying())
        {
            //anim.Play("showInventorySouth");
        }


        distanceToPlayer = Vector3.Distance(transform.position, playerControl.transform.position);

        // dialogue text box display

        if (wellFullyCranked)
        {
            if (distanceToPlayer < 10)
            {
                if (!dialogueIsShowing)
                {
                    dialogueUI.LoadDialogueAsset(dialogue);
                    dialogueUI.setIsDialoguePlaying(false);

                    _withinTalkingRange = true;
                    dialogueIsShowing = true;

                }
            }
            else
            {
                if (dialogueIsShowing)
                {

                    dialogueUI.UnloadDialogueAsset();

                    dialogueIsShowing = false;
                    _withinTalkingRange = false;

                }

            }
        }


        if (wellFullyCranked || wellRope.transform.GetChild(0).transform.position.y > -38)
        {
            wellBucketAnim.enabled = true;
        }
        else wellBucketAnim.enabled = false;


        //Debug.Log("the wellRope position is " + wellRope.transform.GetChild(0).transform.position.y);
        //Debug.Log("the well crank Timer is " + wellCrankTimer);
        //Debug.Log("the well rope clump animator time is " + wellRopeClump.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime);      

    }



    void personSensor(Collider2D other)
    {
        if (!Input.GetKey(KeyCode.X))
        {
            wellCrankOutline.GetComponent<SpriteRenderer>().enabled = true;
        }

        if (Input.GetKey(KeyCode.X))
        {
            if (other.gameObject.name == "player")
            {
                //other.GetComponent<PlayerControl>().setForcePlayer(true, .2f, .2f, 200);
                if (!wellFullyCranked)
                {
                    wellCollider1.GetComponent<PolygonCollider2D>().isTrigger = true;
                    other.transform.position = Vector3.MoveTowards(other.transform.position, wellCrankPosition2, Time.deltaTime * 2.5f);
                    other.GetComponent<Animator>().runtimeAnimatorController = extraPlayerController;
                    playerControl.setLockPosition(true);
                    playerControl.Overlap(this.transform.gameObject, 2);
                }
                    
                //}              
                //Debug.Log("the distance between the player and the wellCrankPosition1 is " + Vector3.Distance(other.transform.position, wellCrankPosition));

            }

            // the animation will pause mid-playback if the bucket has reached the top
            if (wellFullyCranked
                && playerControl.animatorIsPlaying("wellCrank")
                && (playerControl.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime % 1 > .9f
                || playerControl.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime % 1 < .2f))
            {
                playerControl.GetComponent<Animator>().SetFloat("reverseAnimationMultiplier", 0f);
                playerControl.setLockPosition(true);
                //wellRopeClump.GetComponent<Animator>().Play("wellCrank");
            }
            else
            {
                playerControl.setLockPosition(false);
                playerControl.GetComponent<Animator>().SetFloat("reverseAnimationMultiplier", 1f);
            }

        }

        if(other.name == "player")
        {
            if (Input.GetKeyUp(KeyCode.X))
            {
                other.GetComponent<Animator>().runtimeAnimatorController = mainAnimatorController;
                wellCrank.transform.GetChild(0).transform.GetComponent<SpriteRenderer>().enabled = true;

                if (Vector3.Distance(other.transform.position, wellCrankPosition2) > 1f && !crankReleased && !playerSkippedOff)
                {
                    wellCollider1.GetComponent<PolygonCollider2D>().isTrigger = false;
                }
            }
        }

        if (wellCollider1.GetComponent<PolygonCollider2D>().isTrigger)
        {
            if (!Input.GetKey(KeyCode.X))
            {
                innerWellCollider.SetActive(true);
            }
            else innerWellCollider.SetActive(false);
        }


        if (Input.GetKeyDown(KeyCode.X))
        {
            wellCrankOutline.GetComponent<SpriteRenderer>().enabled = false;

            if (crankReleased)
            {
                other.GetComponent<Animator>().runtimeAnimatorController = extraPlayerController;
                playerControl.GetComponent<Animator>().Play("wellCrank");
                crankReleased = false;
            }

            
        }
       
        if (Input.GetKeyUp(KeyCode.X))
        {
            wellCranking = false;

            if(wellRope.transform.GetChild(0).transform.position.y <= -36)
            {
                wellFullyCranked = false;
            }

            if (wellFullyCranked && !crankReleased && !wellCollider1.GetComponent<PolygonCollider2D>().isTrigger)
            {
                wellFullyCranked = false;
            }

        }

    }


    void personGoneSensor(Collider2D other)
    {
        if(other.name == "player")
        {
            wellCrankOutline.GetComponent<SpriteRenderer>().enabled = false;

            other.GetComponent<PlayerControl>().returSpriteToOriginLayer();            

            
            wellCollider1.GetComponent<PolygonCollider2D>().isTrigger = false;

        }
    }

    void setPlayerSkippedOff(bool b)
    {
        playerSkippedOff = b;
    }


    public bool getEndOfDialogue()
    {
        return dialogueUI.getIsEndOfDialogue();
    }

    public bool getWithinTalkingRange()
    {
        return _withinTalkingRange;
    }
}
