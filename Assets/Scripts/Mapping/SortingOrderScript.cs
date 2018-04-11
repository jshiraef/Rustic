using UnityEngine;
using System.Collections;

public class SortingOrderScript : MonoBehaviour 
{
	public const string OverlapLayer = "Overlap";
	public string currentLayerName;
	public int sortingOrder = 0;
	protected SpriteRenderer sprite;
	protected GameObject player;

    public bool copyParentSortingLayer;


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
            threshold = this.transform.position.y + 1.25f;
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

    }
	
	// Update is called once per frame
	void Update () 
	{

        if (player.transform.position.y > threshold)
            {

                //			sprite.sortingOrder = sortingOrder;
                sprite.sortingLayerName = OverlapLayer;

            }
            else if (player.transform.position.y > (slope * player.transform.position.x) + yintercept)
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
    }

   
}
