using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class VendorController : Entity {

    private GameObject AlertIcon;
    private GameObject player;

    private bool _withinTalkingRange;

    public TextAsset dialogue;
    private Dialogue dialogueUI;

    private int changeDirectionCoolDown;
    private float distanceToPlayer;

    private GameObject renderMask;

    public bool inWater = false;

    public bool isIdle;
    public bool isWalking;


    public PathCreator pathCreator;
    public float speed = 1;
    float distanceTravelled;
    float movementAngle;
    Vector3 oldPosition;

    public bool dialogueIsShowing;

    private SatchelController inventory;

    private CircleCollider2D dialogueCollider;

    // Use this for initialization
    void Start () {

        dialogueUI = GameObject.Find("Dialogue Text").GetComponent<Dialogue>();
        body = GetComponent<Rigidbody2D>();
        AlertIcon = GameObject.Find("AlertIcon");

        player = GameObject.Find("player");

        renderMask = transform.Find("renderMask").gameObject;

        dialogueCollider = GetComponent<CircleCollider2D>();

        inventory = GameObject.Find("inventory").GetComponent<SatchelController>();


        anim = GetComponent<Animator>();

        changeDirectionCoolDown = 500;

        isWalking = true;

        oldPosition = transform.position;
    }
	
	// Update is called once per frame
	void Update () {

        if (_withinTalkingRange && !dialogueUI.getIsDialoguePlaying())
        {
            AlertIcon.SetActive(true);
        }
        else
        {
            AlertIcon.SetActive(false);
        }

        if (!_withinTalkingRange)
        {
            isWalking = true;
            dialogueUI.transform.parent.transform.parent.GetComponent<CanvasGroup>().alpha = 0;
        }
        else isWalking = false;

        if (!dialogueUI.getIsDialoguePlaying())
        {
            //if (changeDirectionCoolDown > 4000)
            //{
            //    transform.Translate(0, -1f * Time.deltaTime, 0);
            //    anim.Play("walkingSouth");
            //}
            //else if (changeDirectionCoolDown > 3000)
            //{
            //    transform.Translate(-1f * Time.deltaTime, 0, 0);
            //    anim.Play("walkingWest");
            //}
            //else if (changeDirectionCoolDown > 2000)
            //{
            //    transform.Translate(0, 1f * Time.deltaTime, 0);
            //    anim.Play("walkingNorth");
            //}
            //else if (changeDirectionCoolDown > 50)
            //{
            //    transform.Translate(1f * Time.deltaTime, 0, 0);
            //    anim.Play("walkingEast");
            //}
        }
        else if (dialogueUI.getIsDialoguePlaying())
        {
            anim.Play("showInventorySouth");
        }


        if (changeDirectionCoolDown < 50)
        {
            changeDirectionCoolDown = 5000;
        }

        if(changeDirectionCoolDown > 0)
        {
            changeDirectionCoolDown -= Mathf.RoundToInt(100 * Time.deltaTime);
        }

        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        //Debug.Log("the distance from the vendor to the player is " + distanceToPlayer);


        // dialogue text box display
        if(distanceToPlayer < 3)
        {
            if(!dialogueIsShowing)
            {
                dialogueUI.LoadDialogueAsset(dialogue);
                dialogueUI.setIsDialoguePlaying(false);

                _withinTalkingRange = true;
                dialogueIsShowing = true;

            }         
        }
        else
        {
            if(dialogueIsShowing)
            {

                dialogueUI.UnloadDialogueAsset();

                dialogueIsShowing = false;
                _withinTalkingRange = false;

            }
            
        }

        if(isWalking)
        {
            distanceTravelled += speed * Time.deltaTime;
            transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled);
        }

        getDirectionFromMovement();
        setAnimationFromPathDirection();


        Debug.Log("the vendor is moving in the direction of" + getDirectionFromMovement());

        //Debug.Log("the Vendor environment count is " + environmentCount);


    }

    void setAnimationFromPathDirection()
    {
        if(getDirectionFromMovement() != 90)
        {
            if (getDirectionFromMovement() > 45 && getDirectionFromMovement() < 135)
            {
                anim.Play("walkingNorth");
            }
            else if (getDirectionFromMovement() > 135 || getDirectionFromMovement() < -135)
            {
                anim.Play("walkingWest");
            }
            else if (getDirectionFromMovement() < -45 && getDirectionFromMovement() > -135)
            {
                anim.Play("walkingSouth");
            }
            else if (getDirectionFromMovement() > -45 && getDirectionFromMovement() < 45)
            {
                anim.Play("walkingEast");
            }
        }
        
    }


    void OnTriggerEnter2D(Collider2D collider)
    {
        //if (collider.name == "player")
        //{
        //    dialogueUI.LoadDialogueAsset(dialogue);
        //}

        //dialogueUI.setIsDialoguePlaying(false);

        if (collider.tag == "environment")
        {
            if (collisionCount == 0)
            {
                environmentCount++;
            }
        }

        if (collider.name == "waterEdge")
        {
            renderMask.transform.localScale = new Vector3(.75f, .75f, .75f);
            renderMask.transform.localPosition = new Vector3(.05f, -1.44f, 0f);
        }

        
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.name == "player")
        {
            
        }

        if (collider.tag == "environment")
        {

            environmentCount--;

            if (environmentCount <= 0)
            {
                renderMask.GetComponent<RenderMask>().setVendorMaskType(RenderMask.MaskType.NULL);
            }
        }

        if (collider.name == "waterEdge")
        {
            inWater = false;

            renderMask.transform.localScale = new Vector3(1.225f, 1.225f, 1.225f);
            renderMask.transform.localPosition = new Vector3(.05f, -2f, 0f);
        }

        
    }

     void OnTriggerStay2D(Collider2D other)
    {
        if (other.name == "waterEdge")
        {

            inWater = true;

            renderMask.GetComponent<RenderMask>().setVendorMaskType(RenderMask.MaskType.WATER);
            
        }

        if (other.name == "grassEdge")
        {
            renderMask.GetComponent<RenderMask>().setVendorMaskType(RenderMask.MaskType.GRASS);
            
        }
    }

    // this will detect the angle of movement when the NPC is on a fixed path
    public float getDirectionFromMovement()
    {
        if(changeDirectionCoolDown % 100 < 10)
        {
            oldPosition = transform.position;
        }

        Vector3 targetDir = transform.position - oldPosition;
        movementAngle = Vector3.Angle(targetDir, transform.right);

        if(oldPosition.y > transform.position.y)
        {
            movementAngle = movementAngle * -1;
        }

        return movementAngle ;

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
