using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockController : Entity {

    private bool breakable;

    private bool pickedUp;
    private float pickedUpTimer;

    private bool hitByRay;
    private int hitByRayCoolDown;


    private bool putDown;
    private int setDownTimer;

    private int fallTimer;

    private bool airBorne;
    public float airSpeed;
    public float rollSpeed;

    private GameObject outline;

    private GameObject player;
    private PlayerControl playerControl;

    public string currentLayerName;
    public int currentLayerOrder;

    private Vector3 originalScale;
    private Vector3 playerDirection;

    private SpriteRenderer rockSprite;


    // Use this for initialization
    void Start ()
    {
        rockSprite = GetComponent<SpriteRenderer>();

        player = GameObject.Find("player");
        playerControl = player.GetComponent<PlayerControl>();

        outline = this.transform.GetChild(0).gameObject;
        currentLayerName = getSpriteLayerName(rockSprite);
        currentLayerOrder = getSpriteSortingOrder(rockSprite);
        originalScale = this.transform.localScale;

        fallSpeed = -5.9f;
        airSpeed = 8f;

        body = GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update () {

        if (playerControl.interact && this.hitByRay)
        {
            outline.SetActive(true);

                //outline.AddComponent<CircleCollider2D>();
                //outline.GetComponent<CircleCollider2D>().isTrigger = true;
                //outline.GetComponent<CircleCollider2D>().radius = 2f;
      
            //grabbed = true;    
        }
        else
        {
            outline.SetActive(false);
        }


        if (pickedUp && playerControl.holdingThrowableItem)
        {       
            if(playerControl.getDirectionAngle360() > 15 && playerControl.getDirectionAngle360() <= 180)
            {
                rockSprite.sortingLayerName = currentLayerName;
                rockSprite.sortingOrder = currentLayerOrder;
                //this.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 1.3f, 0);
            }
            else
            {
                rockSprite.sortingLayerName = getSpriteLayerName(player.GetComponent<SpriteRenderer>());
                rockSprite.sortingOrder = getSpriteSortingOrder(player.GetComponent<SpriteRenderer>()) + 1;
            }

            if (playerControl.getDirectionAngle360() >= 0 && playerControl.getDirectionAngle360() < 16.35)
            {
                this.transform.localPosition = new Vector3(.38f, .91f, 0);
                this.transform.localScale = new Vector3(.55f, .85f, .85f);
                //East
            }
            else if (playerControl.getDirectionAngle360() < 49.1)
            {
                this.transform.localPosition = new Vector3(.21f, .94f, 0);
                this.transform.localScale = new Vector3(.85f, .85f, .85f);
                //NorthEast30
            }
            else if (playerControl.getDirectionAngle360() < 81.85)
            {
                this.transform.localPosition = new Vector3(.09f, 1.05f, 0);
                //NorthEast70
            }
            else if (playerControl.getDirectionAngle360() < 114.6)
            {                          
                 this.transform.localPosition = new Vector3(-.1f, 1.15f, 0);
                //North
            }
            else if (playerControl.getDirectionAngle360() < 147.35)
            {
                this.transform.localPosition = new Vector3(-.067f, 1.154f, 0);
                //NorthWest120
            }
            else if (playerControl.getDirectionAngle360() <= 180)
            {
                this.transform.localScale = new Vector3(.85f, .85f, .85f);
                this.transform.localPosition = new Vector3(-.2f, 1.136f, 0);
                //NorthWest150
            }
            else if (playerControl.getDirectionAngle360() < 212.85)
            {
                this.transform.localPosition = new Vector3(-.243f, 1.145f, 0);
                this.transform.localScale = new Vector3(.55f, .85f, .85f);
                //West
            }
            else if (playerControl.getDirectionAngle360() < 245.6)
            {
                this.transform.localPosition = new Vector3(.013f, 1.111f, 0);
                this.transform.localScale = new Vector3(.75f, .85f, .85f);
                //SouthWest210
            }
            else if (playerControl.getDirectionAngle360() < 278.35)
            {
                this.transform.localPosition = new Vector3(.14f, 1.14f, 0);
                this.transform.localScale = new Vector3(.85f, .85f, .85f);
                //SouthWest240
            }
            else if (playerControl.getDirectionAngle360() < 311.1)
            {
                this.transform.localPosition = new Vector3(.24f, 1.04f, 0);
                //South
            }
            else if (playerControl.getDirectionAngle360() < 343.85)
            {
                this.transform.localPosition = new Vector3(.27f, 1.01f, 0);
                this.transform.localScale = new Vector3(.75f, .85f, .85f);
                //SouthEast300
            }
            else
            {
                this.transform.localPosition = new Vector3(.28f, .97f, 0);
                this.transform.localScale = new Vector3(.65f, .85f, .85f);
                //SouthEast330
            }
        }
        else if (putDown)
        {

            GetComponent<CircleCollider2D>().enabled = false;

            if(playerControl.getDirectionNSEW() == Direction.SOUTH)
            {
                transform.localPosition -= new Vector3(0, 10f * Time.deltaTime, 0);
            }
            else if (playerControl.getDirectionNSEW() == Direction.NORTH)
            {
                transform.localPosition -= new Vector3(0, 4f * Time.deltaTime, 0);
            }
            else if (playerControl.getDirectionNSEW() == Direction.EAST)
            {
                transform.localPosition -= new Vector3(-2f * Time.deltaTime, 8f * Time.deltaTime, 0);
            }
            else if(playerControl.getDirectionNSEW() == Direction.WEST)
            {
                transform.localPosition -= new Vector3(2f * Time.deltaTime, 9f * Time.deltaTime, 0);
            }
        }
        else if (falling)
        {
            if(fallTimer > 45)
            {
                transform.Translate(new Vector3(0, 1f * Time.deltaTime, 0));
            }
            else
            {
                fallSpeed += 1 / fallTimer * 20f;
                transform.Translate(new Vector3(0, fallSpeed * Time.deltaTime, 0));
            }

            body.isKinematic = false;

            if (airBorne)
            {
                if(playerControl.getDirectionAngle360() > 250 && playerControl.getDirectionAngle360() < 290)
                {
                    fallSpeed = -5.9f;
                    airSpeed = 3.5f;
                }
                else if(playerControl.getDirectionAngle360() > 170 && playerControl.getDirectionAngle360() < 190 || playerControl.getDirectionAngle360() < 10 || playerControl.getDirectionAngle360() > 350)
                {
                    fallSpeed = -3f;
                    airSpeed = 8f;
                }
                else if (playerControl.getDirectionAngle360() < 170 && playerControl.getDirectionAngle360() > 10)
                {
                    fallSpeed = -5.9f;
                    airSpeed = 12f;
                }
                else
                {
                    fallSpeed = -5.9f;
                    airSpeed = 6f;
                }

                    Vector3 throwDistance = new Vector3(playerDirection.x * (airSpeed * Time.deltaTime), playerDirection.y * (airSpeed * Time.deltaTime), 0);
                    transform.Translate(throwDistance);              
            }
        }

        // SortingOrderScript takes control only when object is not pickedUp by player
        GetComponent<SortingOrderScript>().enabled = !pickedUp;



        if(fallTimer > 0 && fallTimer < 20)
        {
            GetComponent<CircleCollider2D>().isTrigger = false;
            pickedUp = false;
        }



        if (putDown && setDownTimer <= 0 && transform.parent == null)
        {
            pickedUp = false;
        }


        if (hitByRayCoolDown > 0)
        {
            hitByRayCoolDown -= Mathf.RoundToInt(Time.deltaTime * 100);
        }

        if (hitByRayCoolDown <= 0)
        {
            hitByRay = false;
        }

        if (fallTimer > 0)
        {
            fallTimer -= Mathf.RoundToInt(Time.deltaTime * 100);
        }

        if (fallTimer <= 0)
        {
            fallSpeed = -6f;
            falling = false;
            airBorne = false;
            body.isKinematic = true;
            body.velocity = Vector3.zero;
            body.angularVelocity = 0f;
            this.transform.localScale = originalScale;
        }

        if (setDownTimer > 0)
        {
            setDownTimer -= Mathf.RoundToInt(Time.deltaTime * 100);
        }

        if(pickedUpTimer > 0)
        {
            pickedUpTimer -= Mathf.RoundToInt(Time.deltaTime * 100);
        }

        if (putDown && setDownTimer <= 0)
        {
            this.transform.localScale = originalScale;
            GetComponent<CircleCollider2D>().isTrigger = false;
            GetComponent<CircleCollider2D>().enabled = true;
            putDown = false;
            pickedUp = false;
        }

        //Debug.Log("the playerDirection is " + playerDirection);

        //Debug.Log("the rock's fallSpeed is " + fallSpeed);     
        //Debug.Log("the hitByRay bool is " + hitByRay);
        //Debug.Log("the hitByRayCoolDown is " + hitByRayCoolDown);
        //Debug.Log("the rock's pickedUp bool is " + pickedUp);
    }


    public void noGravity()
    {
        Physics.gravity = Vector3.zero;
    }

    public void setGravity()
    {
        Physics.gravity = new Vector3(0f, 0f, -9.8f);
    }


    public void setPickUp(bool b)
    {
        pickedUp = b;
        pickedUpTimer = 200;
    }

    public void hitByRaycast(bool b)
    {       
        hitByRay = b;
        hitByRayCoolDown = 50;
    }

    public void setDown(bool b)
    {
        setDownTimer = 28;
        putDown = b;
    }

    public void setDropped(bool b)
    {
        this.transform.localScale = originalScale;

        falling = b;
        fallTimer = 45;
        pickedUp = false;
        putDown = false;
    }

    public void setThrown(bool b)
    {
        playerDirection = new Vector3(Mathf.Cos(playerControl.getDirectionAngle360() * Mathf.Deg2Rad), Mathf.Sin(playerControl.getDirectionAngle360() * Mathf.Deg2Rad), 0);
        this.transform.localScale = originalScale;
        this.transform.localRotation = new Quaternion(0, 0, 0, 0);
        airBorne = b;

        //if (playerDirection.y > .5f)
        //{
        //    noGravity();
        //    airSpeed = airSpeed + (playerDirection.y * 3f);
        //}
        //else
        //{
        //    airSpeed = 8f;
        //    setGravity();
        //}

        falling = true;
        fallTimer = 60;
    }
}
