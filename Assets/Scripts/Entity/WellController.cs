using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WellController : MonoBehaviour
{
    private GameObject wellCrank;
    private GameObject wellCrankOutline;
    private bool glowUp;
    private Animator wellCrankAnim;
    public RuntimeAnimatorController mainAnimatorController, extraPlayerController;
    private int wellCrankTimer;
    private bool wellCranking, wellFullyCranked, crankReleased, playerSkippedOff;
    private GameObject wellRope;
    private GameObject wellCollider1, wellCollider2;
    private GameObject wellLogRoll;
    private GameObject wellRopeClump;
    private GameObject innerWellCollider;
    private PlayerControl playerControl;
    private Vector3 wellRopeOriginalPosition, wellCrankPosition, wellCrankPosition2;

    private waterBob waterBob;

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

        wellCrankOutline = wellCrank.transform.Find("Outline").gameObject;
        wellCrankOutline.GetComponent<SpriteRenderer>().enabled = false;
        wellCrankAnim = wellCrank.GetComponent<Animator>();
        wellCrankAnim.enabled = false;

        wellCrankPosition = transform.Find("wellCrankPosition").transform.position;
        wellCrankPosition2 = transform.Find("wellCrankPosition").gameObject.transform.GetChild(0).transform.position;

        playerControl = GameObject.Find("player").GetComponent<PlayerControl>();

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

        if (wellCranking)
        {
            if (wellRope.transform.GetChild(0).transform.position.y <= -35)
            {
                wellCrankTimer += Mathf.RoundToInt(Time.deltaTime * 100);
                wellRope.transform.Translate(-.075f, 0f * Time.deltaTime, 0);

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

        if (wellCrankTimer > 0 && !wellCranking && !wellFullyCranked)
        {
            wellCrankTimer -= Mathf.RoundToInt(Time.deltaTime * 100);


            if (wellRope.transform.position.x < wellRopeOriginalPosition.x)
            {
                wellRope.transform.Translate(+.075f, 0f * Time.deltaTime, 0);

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
        }

        // this detects if the player has chosen to move away from the well crank instead of continuing cranking
        if (crankReleased &&
        !playerControl.GetComponent<BoxCollider2D>().IsTouching(innerWellCollider.GetComponent<PolygonCollider2D>()))
        {
            playerSkippedOff = true;
            crankReleased = false;
            playerControl.GetComponent<Animator>().runtimeAnimatorController = extraPlayerController;
            playerControl.GetComponent<Animator>().Play("wellDismount");
            playerControl.setLockPosition(true);

        }


        // this invokes the player jumpdown animation from the well ledge.
        // It also reinitializes and resumes the player back to its regular Animator Controller.
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
             
                    wellCollider1.GetComponent<PolygonCollider2D>().isTrigger = true;
                    other.transform.position = Vector3.MoveTowards(other.transform.position, wellCrankPosition2, Time.deltaTime * 2.5f);
                    other.GetComponent<Animator>().runtimeAnimatorController = extraPlayerController;
                    playerControl.setLockPosition(true);
                    playerControl.Overlap(this.transform.gameObject, 2);
               
                //}              
                //Debug.Log("the distance between the player and the wellCrankPosition1 is " + Vector3.Distance(other.transform.position, wellCrankPosition));

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
            wellFullyCranked = false;

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
}
