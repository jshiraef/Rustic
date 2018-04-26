using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockController : Entity {

    private bool breakable;

    private bool pickedUp;
    private float pickedUpTimer;

    private bool hitByRay;
    private int hitByRayCoolDown;

    private bool dropped;
    private int droppedCoolDown;

    private bool putDown;
    private int setDownTimer;

    public float airSpeed;
    public float rollSpeed;

    private GameObject outline;

    private GameObject player;

    public string currentLayerName;
    public int currentLayerOrder;

    private Vector3 originalScale;

    private SpriteRenderer rockSprite;
    // Use this for initialization
    void Start () {
        rockSprite = GetComponent<SpriteRenderer>();
        player = GameObject.Find("player");
        outline = this.transform.GetChild(0).gameObject;
        currentLayerName = getSpriteLayerName(rockSprite);
        currentLayerOrder = getSpriteSortingOrder(rockSprite);
        originalScale = this.transform.localScale;
    }
	
	// Update is called once per frame
	void Update () {

        if (player.GetComponent<PlayerControl>().interact && this.hitByRay)
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


        if (pickedUp && player.GetComponent<PlayerControl>().holdingThrowableItem)
        {       
            if(player.GetComponent<PlayerControl>().getDirectionAngle360() > 15 && player.GetComponent<PlayerControl>().getDirectionAngle360() <= 180)
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

            if (player.GetComponent<PlayerControl>().getDirectionAngle360() >= 0 && player.GetComponent<PlayerControl>().getDirectionAngle360() < 16.35)
            {
                this.transform.localPosition = new Vector3(.38f, .91f, 0);
                this.transform.localScale = new Vector3(.55f, .85f, .85f);
                //East
            }
            else if (player.GetComponent<PlayerControl>().getDirectionAngle360() < 49.1)
            {
                this.transform.localPosition = new Vector3(.21f, .94f, 0);
                this.transform.localScale = new Vector3(.85f, .85f, .85f);
                //NorthEast30
            }
            else if (player.GetComponent<PlayerControl>().getDirectionAngle360() < 81.85)
            {
                this.transform.localPosition = new Vector3(.09f, 1.05f, 0);
                //NorthEast70
            }
            else if (player.GetComponent<PlayerControl>().getDirectionAngle360() < 114.6)
            {                          
                 this.transform.localPosition = new Vector3(-.1f, 1.15f, 0);
                //North
            }
            else if (player.GetComponent<PlayerControl>().getDirectionAngle360() < 147.35)
            {
                this.transform.localPosition = new Vector3(-.067f, 1.154f, 0);
                //NorthWest120
            }
            else if (player.GetComponent<PlayerControl>().getDirectionAngle360() <= 180)
            {
                this.transform.localScale = new Vector3(.85f, .85f, .85f);
                this.transform.localPosition = new Vector3(-.2f, 1.136f, 0);
                //NorthWest150
            }
            else if (player.GetComponent<PlayerControl>().getDirectionAngle360() < 212.85)
            {
                this.transform.localPosition = new Vector3(-.243f, 1.145f, 0);
                this.transform.localScale = new Vector3(.55f, .85f, .85f);
                //West
            }
            else if (player.GetComponent<PlayerControl>().getDirectionAngle360() < 245.6)
            {
                this.transform.localPosition = new Vector3(.013f, 1.111f, 0);
                this.transform.localScale = new Vector3(.75f, .85f, .85f);
                //SouthWest210
            }
            else if (player.GetComponent<PlayerControl>().getDirectionAngle360() < 278.35)
            {
                this.transform.localPosition = new Vector3(.14f, 1.14f, 0);
                this.transform.localScale = new Vector3(.85f, .85f, .85f);
                //SouthWest240
            }
            else if (player.GetComponent<PlayerControl>().getDirectionAngle360() < 311.1)
            {
                this.transform.localPosition = new Vector3(.24f, 1.04f, 0);
                //South
            }
            else if (player.GetComponent<PlayerControl>().getDirectionAngle360() < 343.85)
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
            if(player.GetComponent<PlayerControl>().getDirectionNSEW() == Direction.SOUTH)
            {
                transform.localPosition -= new Vector3(0, 11f * Time.deltaTime, 0);
            }
            else if (player.GetComponent<PlayerControl>().getDirectionNSEW() == Direction.NORTH)
            {
                transform.localPosition -= new Vector3(0, 6f * Time.deltaTime, 0);
            }
            else if (player.GetComponent<PlayerControl>().getDirectionNSEW() == Direction.EAST)
            {
                transform.localPosition -= new Vector3(-2f * Time.deltaTime, 8f * Time.deltaTime, 0);
            }
            else if(player.GetComponent<PlayerControl>().getDirectionNSEW() == Direction.WEST)
            {
                transform.localPosition -= new Vector3(2f * Time.deltaTime, 9f * Time.deltaTime, 0);
            }
        }


        GetComponent<SortingOrderScript>().enabled = !pickedUp;



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

        if (droppedCoolDown > 0)
        {
            droppedCoolDown -= Mathf.RoundToInt(Time.deltaTime * 100);
        }

        if (droppedCoolDown <= 0)
        {
            dropped = false;
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
            putDown = false;
            pickedUp = false;
        }

        Debug.Log("the rock's pickedUp timer is " + pickedUpTimer);
        Debug.Log("the rock's pickedUp bool is " + pickedUp);
        //Debug.Log("the hitByRay bool is " + hitByRay);
        //Debug.Log("the hitByRayCoolDown is " + hitByRayCoolDown);
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

    public void setDown(int setDownTime)
    {
        setDownTimer = setDownTime;
        putDown = true;
    }

    public void setDropped(bool b)
    {
        dropped = b;
        droppedCoolDown = 100;
    }
}
