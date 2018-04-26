using UnityEngine;
using System.Collections;

public class SortingOrderScript : MonoBehaviour 
{
	public const string OverlapLayer = "Overlap";
	public string currentLayerName;
	public int sortingOrder = 0;
    public GameObject neverRenderBehindThisObject;
	protected SpriteRenderer sprite;
	protected GameObject player;

    public bool copyParentSortingLayer;

    protected bool bruteForceRender;


	// This is the threshhold at which the player's position 
	// will change the nearby objects sorting layer
	public double threshold = 0;

    // Here we create two empty gameobjects which represent two points. 
    // A line is therefore created for sorting depth rendering based on the player's position relative to this line
    public GameObject thresholdPoint1;
    public GameObject thresholdPoint2;

    protected Vector2 threshold1;
    protected Vector2 threshold2;
    protected float slope;
    protected float yintercept;


	// Use this for initialization
	void Start () 
	{
		sprite = GetComponent<SpriteRenderer> ();
		player = GameObject.Find ("player");

		currentLayerName = sprite.sortingLayerName.ToString ();

        if(threshold == 0 && thresholdPoint1 == null)
        {
            threshold = this.transform.position.y;
        }
        

        if (thresholdPoint1 != null)
        {
            threshold1 = new Vector2(thresholdPoint1.transform.position.x, thresholdPoint1.transform.position.y);

        }

        if (thresholdPoint2 != null)
        {
            threshold2 = new Vector2(thresholdPoint2.transform.position.x, thresholdPoint2.transform.position.y);
        }

        if (thresholdPoint1 != null && thresholdPoint2 != null)
        {
            slope = (thresholdPoint2.transform.position.y - thresholdPoint1.transform.position.y) /
                (thresholdPoint2.transform.position.x - thresholdPoint1.transform.position.x);

            yintercept = thresholdPoint2.transform.position.y - (slope * thresholdPoint2.transform.position.x);
        }

        if(neverRenderBehindThisObject != null)
        {
            bruteForceRender = true;
        }

    }
	
	// Update is called once per frame
	void Update () 
	{

        if (threshold == 0 && thresholdPoint1 == null)
        {
            threshold = this.transform.position.y;
        }
        if(this.name == "simpleRock")
        {
            threshold = this.transform.position.y + 1;
        }

        if (player.transform.position.y > threshold && player.transform.position.x > (this.transform.position.x - this.sprite.size.x/2) && player.transform.position.x < (this.transform.position.x + this.sprite.size.x/2))
            {

                //			sprite.sortingOrder = sortingOrder;
                sprite.sortingLayerName = OverlapLayer;

            }
            else if (player.transform.position.y > (slope * player.transform.position.x) + yintercept && player.transform.position.x > (this.transform.position.x - this.sprite.size.x / 2) && player.transform.position.x < (this.transform.position.x + this.sprite.size.x / 2))
            {

                sprite.sortingLayerName = OverlapLayer;
            }
            else
            {
                sprite.sortingLayerName = currentLayerName;
            }      
            

        if (copyParentSortingLayer)
        {
            sprite.sortingLayerName = transform.parent.GetComponent<SpriteRenderer>().sortingLayerName;

        }

        if(bruteForceRender)
        {
            if(neverRenderBehindThisObject.GetComponent<SpriteRenderer>().sortingLayerID != 0)
            {
                sprite.sortingLayerID = neverRenderBehindThisObject.GetComponent<SpriteRenderer>().sortingLayerID;
                sprite.sortingOrder = neverRenderBehindThisObject.GetComponent<SpriteRenderer>().sortingOrder + 1;
            }
        }

        if(Mathf.Abs((float) threshold - this.transform.position.y) > sprite.size.y)
        {
            threshold = this.transform.position.y + 1;
        }


    }

   
}
