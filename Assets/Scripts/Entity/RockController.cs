using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockController : Entity {

    private bool breakable;

    private bool grabbed;
    private float grabbableTimer;

    private bool hitByRay;
    private int hitByRayCoolDown;

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

            if (grabbed && grabbableTimer <= 0)
            {
                grabbableTimer = 1f;
                //outline.AddComponent<CircleCollider2D>();
                //outline.GetComponent<CircleCollider2D>().isTrigger = true;
                //outline.GetComponent<CircleCollider2D>().radius = 2f;
            }
            //grabbed = true;    
        }
        else
        {
            outline.SetActive(false);
        }

        if (grabbed && player.GetComponent<PlayerControl>().holdingThrowableItem)
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
                this.transform.localPosition = new Vector3(-.1f, 1.25f, 0);
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

        
            GetComponent<SortingOrderScript>().enabled = !grabbed;
        
        

        //if(grabbableTimer > 0)
        //{
        //    grabbableTimer -= Time.deltaTime;
        //}

        //if(grabbableTimer < 0)
        //{
        //    grabbed = false;
        //    //Destroy(outline.GetComponent<CircleCollider2D>());
        //}

        if (hitByRayCoolDown > 0)
        {
            hitByRayCoolDown -= Mathf.RoundToInt(Time.deltaTime * 100);
        }

        if (hitByRayCoolDown <= 0)
        {
            hitByRay = false;
            //Destroy(outline.GetComponent<CircleCollider2D>());
        }

        //Debug.Log("the rock's grabbed bool is " + grabbed);
        //Debug.Log("the hitByRay bool is " + hitByRay);
        //Debug.Log("the hitByRayCoolDown is " + hitByRayCoolDown);
    }

    public void setGrabbed(bool b)
    {
        grabbed = b; 
    }

    public void hitByRaycast(bool b)
    {       
        hitByRay = b;
        hitByRayCoolDown = 50;
    }
}
