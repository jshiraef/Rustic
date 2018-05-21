using UnityEngine;
using System.Collections;

public class Barrel : Entity {

	public GameObject[] barrels;
	public bool hit = false;
	public bool broken = false;

    private bool pickedUp;
    private int pickedUpTimer;

	public float barrelDistanceToPlayer;

    private float grabbableTimer;

    private bool hitByRay;
    private int hitByRayCoolDown;

    private bool putDown;
    private int setDownTimer;

    private int fallTimer;

    private bool airBorne;
    public float airSpeed;

    private GameObject player;
    private PlayerControl playerControl;

    private GameObject outline;

	public static int barrelTimer = 50;

    private SpriteRenderer barrelSprite;

    public string currentLayerName;
    public int currentLayerOrder;

    private Vector3 playerDirection;


    // Use this for initialization
    void Start () {

        barrelSprite = GetComponent<SpriteRenderer>();

        currentLayerName = getSpriteLayerName(barrelSprite);
        currentLayerOrder = getSpriteSortingOrder(barrelSprite);

        anim = GetComponent<Animator> ();
        player = GameObject.Find("player");
        playerControl = player.GetComponent<PlayerControl>();

        outline = this.transform.GetChild(0).gameObject;

        fallSpeed = -5.9f;
        airSpeed = 5f;
    }

    // Update is called once per frame
    void Update()
    {

        if (playerControl.interact && this.hitByRay)
        {
            outline.SetActive(true);

            
        }
        else
        {
            outline.SetActive(false);
        }

        if (hit)
        {
            Debug.Log("barrel was hit");
        }

        if (barrelTimer < 0)
        {
            anim.Play("barrelBreak");
        }

        if (pickedUp && playerControl.holdingThrowableItem)
        {
            if (playerControl.getDirectionAngle360() > 15 && playerControl.getDirectionAngle360() <= 180)
            {
                barrelSprite.sortingLayerName = currentLayerName;
                barrelSprite.sortingOrder = currentLayerOrder;
                //this.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 1.3f, 0);
            }
            else
            {
                barrelSprite.sortingLayerName = getSpriteLayerName(player.GetComponent<SpriteRenderer>());
                barrelSprite.sortingOrder = getSpriteSortingOrder(player.GetComponent<SpriteRenderer>()) + 1;
            }

            if (playerControl.getDirectionAngle360() >= 0 && playerControl.getDirectionAngle360() < 16.35)
            {
                this.transform.localPosition = new Vector3(.38f, .91f, 0);
                //East
            }
            else if (playerControl.getDirectionAngle360() < 49.1)
            {
                this.transform.localPosition = new Vector3(.21f, .94f, 0);
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
                this.transform.localPosition = new Vector3(-.2f, 1.136f, 0);
                //NorthWest150
            }
            else if (playerControl.getDirectionAngle360() < 212.85)
            {
                this.transform.localPosition = new Vector3(-.243f, 1.145f, 0);
                //West
            }
            else if (playerControl.getDirectionAngle360() < 245.6)
            {
                this.transform.localPosition = new Vector3(.013f, 1.111f, 0);
                //SouthWest210
            }
            else if (playerControl.getDirectionAngle360() < 278.35)
            {
                this.transform.localPosition = new Vector3(.14f, 1.14f, 0);
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
                //SouthEast300
            }
            else
            {
                this.transform.localPosition = new Vector3(.28f, .97f, 0);
                //SouthEast330
            }

        }
        else if (putDown)
        {
            if (playerControl.getDirectionNSEW() == Direction.SOUTH)
            {
                transform.localPosition -= new Vector3(0, 15f * Time.deltaTime, 0);
            }
            else if (playerControl.getDirectionNSEW() == Direction.NORTH)
            {
                transform.localPosition -= new Vector3(0, 6f * Time.deltaTime, 0);
            }
            else if (playerControl.getDirectionNSEW() == Direction.EAST)
            {
                transform.localPosition -= new Vector3(-2f * Time.deltaTime, 9f * Time.deltaTime, 0);
            }
            else if (playerControl.getDirectionNSEW() == Direction.WEST)
            {
                transform.localPosition -= new Vector3(2f * Time.deltaTime, 9f * Time.deltaTime, 0);
            }
        }
        else if (falling)
        {
            if (fallTimer > 45)
            {
                transform.Translate(new Vector3(0, 1f * Time.deltaTime, 0));
            }
            else
            {
                fallSpeed += 1 / fallTimer * 20f;
                transform.Translate(new Vector3(0, fallSpeed * Time.deltaTime, 0));
            }

            if (airBorne)
            {
                Vector3 throwDistance = new Vector3(playerDirection.x * (airSpeed * Time.deltaTime), playerDirection.y * (airSpeed * Time.deltaTime), 0);
                transform.Translate(throwDistance);
            }
        }

        // SortingOrderScript takes control only when object is not pickedUp by player
        GetComponent<SortingOrderScript>().enabled = !pickedUp;



        if (fallTimer > 0 && fallTimer < 20)
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
        }

        if (setDownTimer > 0)
        {
            setDownTimer -= Mathf.RoundToInt(Time.deltaTime * 100);
        }

        if (pickedUpTimer > 0)
        {
            pickedUpTimer -= Mathf.RoundToInt(Time.deltaTime * 100);
        }

        if (putDown && setDownTimer <= 0)
        {
            GetComponent<CircleCollider2D>().isTrigger = false;
            putDown = false;
            pickedUp = false;
        }

        //	print ("the barrel's distance to player is " + distanceToPlayer);
    }

    void OnCollisionEnter2D(Collision2D coll)
	{
		if (coll.gameObject.tag == "weapon") {
			Debug.Log ("a hit");
//			Destroy(coll.gameObject);
//			Destroy(this.gameObject);

			hit = true;
		}
	}

	public static void barrelCountDown()
	{
		barrelTimer = barrelTimer - 5;
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
        setDownTimer = 30;
        putDown = b;
    }

    public void setDropped(bool b)
    {
        falling = b;
        fallTimer = 55;
        pickedUp = false;
        putDown = false;
    }

    public void setThrown(bool b)
    {
        playerDirection = new Vector3(Mathf.Cos(playerControl.getDirectionAngle360() * Mathf.Deg2Rad), Mathf.Sin(playerControl.getDirectionAngle360() * Mathf.Deg2Rad), 0);
        airBorne = b;
        falling = true;
        fallTimer = 60;
    }
}
