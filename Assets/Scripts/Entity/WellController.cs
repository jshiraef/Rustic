using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WellController : MonoBehaviour
{
    private GameObject wellCrank;
    private GameObject wellCrankOutline;
    private bool glowUp;
    private Animator wellCrankAnim;
    private int wellCrankTimer;
    private bool wellCranking, wellFullyCranked;
    private GameObject wellRope;
    private GameObject wellCollider1, wellCollider2;
    private GameObject wellLogRoll;
    private GameObject wellRopeClump;
    private Vector3 wellRopeOriginalPosition;

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
        wellCrankOutline = wellCrank.transform.Find("Outline").gameObject;
        wellCrankOutline.GetComponent<SpriteRenderer>().enabled = false;
        wellCrankAnim = wellCrank.GetComponent<Animator>();
        wellCrankAnim.enabled = false;

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

                if(wellCrank.GetComponent<SpriteOutline2>().blurAlphaMultiplier < .2f)
                {
                    glowUp = true;
                }
            }


            if (glowUp)
            {
                wellCrank.GetComponent<SpriteOutline2>().blurAlphaMultiplier += .05f;

                if(wellCrank.GetComponent<SpriteOutline2>().blurAlphaMultiplier > .98f)
                {
                    glowUp = false;
                }

            }
        }

        if (wellCranking)
        {
            if(wellRope.transform.GetChild(0).transform.position.y <= -35)
            {
                wellCrankTimer += Mathf.RoundToInt(Time.deltaTime * 100);
                wellRope.transform.Translate(-.1f, 0f * Time.deltaTime, 0);

                wellRopeClump.GetComponent<Animator>().SetFloat("reverseAnimationMultiplier", 1f);
                wellRopeClump.GetComponent<Animator>().Play("wellRopeClump");

            } 
            
            if(wellRope.transform.GetChild(0).transform.position.y > -35)
            {
                //Debug.Log("the rope reached the choke point");
                wellFullyCranked = true;
                wellCranking = false;
            }

            wellRopeClump.GetComponent<Animator>().enabled = true;
        }

        if(wellCrankTimer > 0)
        {
            wellCollider1.SetActive(false);
            wellCollider2.SetActive(true);

            if (!wellFullyCranked)
            {
                wellLogRoll.SetActive(true);
            }
            else wellLogRoll.SetActive(false);
        }

        if(wellCrankTimer > 0 && !wellCranking && !wellFullyCranked)
        {
            wellCrankTimer -= Mathf.RoundToInt(Time.deltaTime * 100);

            

            if(wellRope.transform.position.x < wellRopeOriginalPosition.x)
            {
                wellRope.transform.Translate(+.1f, 0f * Time.deltaTime, 0);

                wellRopeClump.GetComponent<Animator>().SetFloat("reverseAnimationMultiplier", -1);

                if(wellRopeClump.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 1f)
                {
                    wellRopeClump.GetComponent<Animator>().Play("wellRopeClump", 0, 1f);
                }

                wellRopeClump.GetComponent<Animator>().Play("wellRopeClump");
            }
        }

        if (wellCrankTimer <= 0)
        {
            wellCrankTimer = 0;

            wellCollider1.SetActive(true);
            wellCollider2.SetActive(false);

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

        if (other.gameObject.name == "player")
        {
            //other.GetComponent<PlayerControl>().setForcePlayer(true, .2f, .2f, 200);

            //Vector3 distanceToPlayer
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            wellCrankAnim.enabled = true;
            wellCranking = true;
            wellCrankOutline.GetComponent<SpriteRenderer>().enabled = false;           

        }
       
        if (Input.GetKeyUp(KeyCode.X))
        {
            wellCrankAnim.enabled = false;
            wellCranking = false;
            wellFullyCranked = false;
        }

    }

    void personGoneSensor(Collider2D other)
    {
        wellCrankOutline.GetComponent<SpriteRenderer>().enabled = false;

        wellCrankAnim.enabled = false;
    }
}
