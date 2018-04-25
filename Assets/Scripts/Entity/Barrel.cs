using UnityEngine;
using System.Collections;

public class Barrel : Entity {

	public GameObject[] barrels;
	public bool hit = false;
	public bool broken = false;
    private bool grabbed;
	public float barrelDistanceToPlayer;

    private float grabbableTimer;

    private bool hitByRay;
    private int hitByRayCoolDown;

    private GameObject player;
    private GameObject outline;

	public static int barrelTimer = 50;

    private SpriteRenderer barrelSprite;

    public string currentLayerName;
    public int currentLayerOrder;


    // Use this for initialization
    void Start () {

        barrelSprite = GetComponent<SpriteRenderer>();

        currentLayerName = getSpriteLayerName(barrelSprite);
        currentLayerOrder = getSpriteSortingOrder(barrelSprite);

        anim = GetComponent<Animator> ();
        player = GameObject.Find("player");
        outline = this.transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {

        if (player.GetComponent<PlayerControl>().interact && this.hitByRay)
        {
            outline.SetActive(true);

            if (grabbed && grabbableTimer <= 0)
            {
                grabbableTimer = 1f;

            }
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

        if (grabbed && player.GetComponent<PlayerControl>().holdingThrowableItem)
        {
            if (player.GetComponent<PlayerControl>().getDirectionAngle360() > 15 && player.GetComponent<PlayerControl>().getDirectionAngle360() <= 180)
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

            if (player.GetComponent<PlayerControl>().getDirectionAngle360() >= 0 && player.GetComponent<PlayerControl>().getDirectionAngle360() < 16.35)
            {
                this.transform.localPosition = new Vector3(.38f, .91f, 0);
                //East
            }
            else if (player.GetComponent<PlayerControl>().getDirectionAngle360() < 49.1)
            {
                this.transform.localPosition = new Vector3(.21f, .94f, 0);
                //NorthEast30
            }
            else if (player.GetComponent<PlayerControl>().getDirectionAngle360() < 81.85)
            {
                this.transform.localPosition = new Vector3(.09f, 1.05f, 0);
                //NorthEast70
            }
            else if (player.GetComponent<PlayerControl>().getDirectionAngle360() < 114.6)
            {
                this.transform.localPosition = new Vector3(.09f, 1.25f, 0);
                //North
            }
            else if (player.GetComponent<PlayerControl>().getDirectionAngle360() < 147.35)
            {
                this.transform.localPosition = new Vector3(-.067f, 1.154f, 0);
                //NorthWest120
            }
            else if (player.GetComponent<PlayerControl>().getDirectionAngle360() <= 180)
            {
                this.transform.localPosition = new Vector3(-.2f, 1.136f, 0);
                //NorthWest150
            }
            else if (player.GetComponent<PlayerControl>().getDirectionAngle360() < 212.85)
            {
                this.transform.localPosition = new Vector3(-.243f, 1.145f, 0);
                //West
            }
            else if (player.GetComponent<PlayerControl>().getDirectionAngle360() < 245.6)
            {
                this.transform.localPosition = new Vector3(.013f, 1.111f, 0);
                //SouthWest210
            }
            else if (player.GetComponent<PlayerControl>().getDirectionAngle360() < 278.35)
            {
                this.transform.localPosition = new Vector3(.14f, 1.14f, 0);
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
                //SouthEast300
            }
            else
            {
                this.transform.localPosition = new Vector3(.28f, .97f, 0);
                //SouthEast330
            }

            //	print ("the barrel's distance to player is " + distanceToPlayer);

        }

        //if (grabbableTimer > 0)
        //{
        //    grabbableTimer -= Time.deltaTime;
        //}

        //if (grabbableTimer < 0)
        //{
        //    grabbed = false;
        //}

        if (hitByRayCoolDown > 0)
        {
            hitByRayCoolDown -= Mathf.RoundToInt(Time.deltaTime * 100);
        }

        if (hitByRayCoolDown <= 0)
        {
            hitByRay = false;
        }
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
