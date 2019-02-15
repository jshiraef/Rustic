using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacelessController : Entity {

    private GameObject AlertIcon;
    private GameObject player;

    private bool _withingTalkingRange;

    private bool triggered;
    private bool spinAttack;
    private bool roaming;

    public TextAsset dialogue;
    private Dialogue dialogueUI;
    private int changeDirectionCoolDown;
    private int spinAttackCoolDown;
    private int spinAttackTimer;
    private float distanceToPlayer;
    private float angleToPlayer;
    private float angleToPlayer360;

    private CircleCollider2D dialogueCollider;
    private GameObject playerSickleSwipe;

    private float analogAxesAngle;
    private float analogAxesAngle360;

    private float knockBack;
    private float knockBackCoolDown;




	// Use this for initialization
	void Start () {

        player = GameObject.Find("player");
        playerSickleSwipe = player.transform.Find("sickleSwipeReference").Find("sickleSwipe").gameObject;

        dialogueUI = GameObject.Find("Dialogue Text").GetComponent<Dialogue>();

        anim = GetComponent<Animator>();

        changeDirectionCoolDown = 500;
        spinAttackCoolDown = 100;
        spinAttackTimer = 100;

        roaming = true;

        moveSpeed = 1f;
        knockBack = 10f;


    }

    // Update is called once per frame
    void Update () {

        if (roaming)
        {
            moveSpeed = 1f;

            if (changeDirectionCoolDown > 4000)
            {
                getNextPosition(-1f, 0);
                down = true;
                up = false;
            }
            else if (changeDirectionCoolDown > 3000)
            {
                getNextPosition(0, -1f);
                left = true;
                right = false; ;
            }
            else if (changeDirectionCoolDown > 2000)
            {
                getNextPosition(1f, 0);
                up = true;
                down = false;
            }
            else if (changeDirectionCoolDown > 50)
            {
                getNextPosition(0, 1f);
                right = true;
                left = false;
            }
        }
        else if (triggered)
        {
            moveSpeed = 2f;

            if(this.transform.position.x < player.transform.position.x)
            {
                getNextPosition(0, 1f);               
                right = true;
                left = false;
            }
            else if(this.transform.position.x > player.transform.position.x)
            {
                getNextPosition(0, -1f);
                left = true;
                right = false;
            }

            if(this.transform.position.y < player.transform.position.y)
            {
                getNextPosition(1f, 0);
                up = true;
                down = false;
            }
            else if (this.transform.position.y > player.transform.position.y)
            {
                getNextPosition(-1f, 0);
                down = true;
                up = false;
            }
        }


        // send the character in the opposite direction of the player
        if(knockBackCoolDown > 0)
        {
            getNextPosition( (Mathf.Sin(angleToPlayer * Mathf.PI / 180) * -1) * knockBack, (Mathf.Cos(angleToPlayer * Mathf.PI / 180) * -1) * knockBack);
        }

        if(spinAttackCoolDown > 0)
        {
            spinAttack = false;
        }
        

        // stagger the spinAttack so it isn't spammed
        if(triggered)
        {
            if(spinAttackCoolDown <= 0 && !spinAttack)
            {
                spinAttack = true;
                spinAttackTimer = 100;
            }
            
        }

        Movement();
        setDirectionBasedOnPlayer();
        setDirection8();

        if (changeDirectionCoolDown < 50)
        {
            changeDirectionCoolDown = 5000;
        }

        if (changeDirectionCoolDown > 0)
        {
            changeDirectionCoolDown -= Mathf.RoundToInt(100 * Time.deltaTime);
        }

        if (spinAttackTimer > 0)
        {
            spinAttackTimer -= Mathf.RoundToInt(100 * Time.deltaTime);
        }

        if(spinAttackCoolDown > 0)
        {
            spinAttackCoolDown -= Mathf.RoundToInt(100 * Time.deltaTime);
        }

        if (spinAttack && spinAttackTimer <= 0)
        {
            if(triggered && spinAttackCoolDown <= 0)
            spinAttackCoolDown = 100;

            spinAttack = false;
        }

        if (knockBackCoolDown > 0)
        {
            knockBackCoolDown -= Mathf.RoundToInt(100 * Time.deltaTime);
        }


        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);


        // gets the angle between player and faceless
        Vector3 targetDir = player.transform.position - this.transform.position;
        angleToPlayer = Vector3.Angle(targetDir, player.transform.right);


        if(this.transform.position.y > player.transform.position.y)
        {
            angleToPlayer = angleToPlayer * -1;
        }

        

        if(this.GetComponent<CapsuleCollider2D>().IsTouching(playerSickleSwipe.GetComponent<CircleCollider2D>()))
            {
            Debug.Log("the Faceless was hit by the sickle");
            triggered = true;
            roaming = false;

            knockBackCoolDown = 10;
        }

        //Debug.Log("the knockback cooldown is " + knockBackCoolDown);
        //Debug.Log("the angle to the player is " + angleToPlayer);  
        //Debug.Log("the faceless spin attack cooldown is " + spinAttackCoolDown);
        //Debug.Log("the timer is " + spinAttackTimer);
        //Debug.Log("the faceless direction 8 is " + getDirection());

    }

    void Movement()
    {
        if(roaming)
        {
            if (down)
            {              
                anim.Play("MovingSouth");
            }
            else if (left)
            {              
                anim.Play("MovingWest");
            }
            else if (up)
            {
                anim.Play("MovingNorth");
            }
            else if (right)
            {               
                anim.Play("MovingEast");
            }
        }
        else if (triggered)
        {

            if (getDirection8() == Direction.SOUTH)
            {               

                if(spinAttack)
                {
                    anim.Play("SpinAttackSouth");
                }
                else anim.Play("MovingSouth");

            }
            else if (getDirection8() == Direction.WEST)
            {               

                if (spinAttack)
                {
                    anim.Play("SpinAttackWest");
                }
                else anim.Play("MovingWest");
            }
            else if (getDirection8() == Direction.NORTH)
            {              

                if (spinAttack)
                {
                    anim.Play("SpinAttackNorth");
                }
                else anim.Play("MovingNorth");
                
            }
            else if (getDirection8() == Direction.EAST)
            {               

                if (spinAttack)
                {
                    anim.Play("SpinAttackEast");
                }
                else anim.Play("MovingEast");
            }
            else if (getDirection8() == Direction.NORTHEAST50)
            {

                if (spinAttack)
                {
                    anim.Play("SpinAttackNorthEast");
                }
                else anim.Play("MovingNorthEast");
            }
            else if (getDirection8() == Direction.NORTHWEST130)
            {

                if (spinAttack)
                {
                    anim.Play("SpinAttackNorthWest");
                }
                else anim.Play("MovingNorthWest");
            }
            else if(getDirection8() == Direction.SOUTHEAST310)
            {

                if (spinAttack)
                {
                    anim.Play("SpinAttackSouthEast");
                }
                else anim.Play("MovingSouthEast");
            }
            else if(getDirection8() == Direction.SOUTHWEST230)
            {

                if (spinAttack)
                {
                    anim.Play("SpinAttackSouthWest");
                }
                else anim.Play("MovingSouthWest");
            }
        }
       
    }

    void setDirectionBasedOnPlayer()
    {
        if(angleToPlayer > -20 && angleToPlayer <= 20)
        {
            this.direction = Direction.EAST;
        }
        else if(angleToPlayer > 20 && angleToPlayer < 70)
        {
            this.direction = Direction.NORTHEAST50;
        }
        else if(angleToPlayer > 70 && angleToPlayer <= 110)
        {
            this.direction = Direction.NORTH;
        }
        else if(angleToPlayer > 110 && angleToPlayer < 160 )
        {
            this.direction = Direction.NORTHWEST130;
        }
        else if(angleToPlayer > 160 || angleToPlayer < -160)
        {
            this.direction = Direction.WEST;
        }
        else if(angleToPlayer > -160 && angleToPlayer < -110)
        {
            this.direction = Direction.SOUTHWEST230;
        }
        else if (angleToPlayer > -110 && angleToPlayer < -70)
        {
            this.direction = Direction.SOUTH;
        }
        else if (angleToPlayer > -70 && angleToPlayer < - 20)
        {
            this.direction = Direction.SOUTHEAST310;
        }
    }

     void OnCollisionEnter2D(Collision2D coll)
    {
        if(coll.gameObject.tag == "weapon")
        {
            triggered = true;         
        }
    }

    public int getSpinAttackCoolDown()
    {
        return spinAttackCoolDown;
    }

    public bool getSpinAttack()
    {
        return spinAttack;
    }
}
