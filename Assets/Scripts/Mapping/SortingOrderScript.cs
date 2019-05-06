using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class SortingOrderScript : MonoBehaviour 
{
	public const string OverlapLayer = "Overlap";

    // these are the variables that can determine sorting Order
    public float zPosOriginal;
	public string currentLayerName;
    public int currentSortingOrderNumber;
	public int sortingOrder = 0;


    public GameObject neverRenderBehindThisObject;
    public GameObject checkObjectForOverlap;
	protected SpriteRenderer sprite;
	protected GameObject player;

    public bool copyParentSortingLayer;

    protected bool bruteForceRender;
    protected bool bruteForceOverlap;


	// This is the threshhold at which the player's position 
	// will change the nearby objects sorting layer
	public double threshold = 0;

    // Here we create two empty gameobjects which represent two points. 
    // A line is therefore created for sorting depth rendering based on the player's position relative to this line
    public GameObject thresholdPoint1;
    public GameObject thresholdPoint2;
    public GameObject thresholdPoint3;
    public GameObject thresholdPoint4;

    protected Vector2 threshold1;
    protected Vector2 threshold2;
    protected Vector2 threshold3;
    protected Vector2 threshold4;
    protected float slope, slope2;
    protected float yintercept, yintercept2;


    protected GameObject[] allMovableObjects;
    protected List<GameObject> allMovableObjectsWithinProximity;

    protected List<Entity> allMovableEntities;


	// Use this for initialization
	void Start () 
	{
		sprite = GetComponent<SpriteRenderer> ();
		player = GameObject.Find ("player");

		currentLayerName = sprite.sortingLayerName.ToString ();
        currentSortingOrderNumber = sprite.sortingOrder;
        zPosOriginal = this.transform.position.z;

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

        if (thresholdPoint3 != null)
        {
            threshold3 = new Vector2(thresholdPoint3.transform.position.x, thresholdPoint3.transform.position.y);
        }

        if (thresholdPoint4 != null)
        {
            threshold4 = new Vector2(thresholdPoint4.transform.position.x, thresholdPoint4.transform.position.y);
        }

        if (thresholdPoint1 != null && thresholdPoint2 != null)
        {
            slope = (thresholdPoint2.transform.position.y - thresholdPoint1.transform.position.y) /
                (thresholdPoint2.transform.position.x - thresholdPoint1.transform.position.x);

            yintercept = thresholdPoint2.transform.position.y - (slope * thresholdPoint2.transform.position.x);
        }

        if (thresholdPoint4 != null && thresholdPoint3 != null)
        {
            slope2 = (thresholdPoint4.transform.position.y - thresholdPoint3.transform.position.y) /
                (thresholdPoint4.transform.position.x - thresholdPoint3.transform.position.x);

            yintercept2 = thresholdPoint4.transform.position.y - (slope2 * thresholdPoint4.transform.position.x);
        }

        if (neverRenderBehindThisObject != null)
        {
            bruteForceRender = true;
        }

        if(checkObjectForOverlap != null) 
        {
            bruteForceOverlap = true;
        }


        allMovableObjects = GameObject.FindGameObjectsWithTag("movable");
        allMovableObjectsWithinProximity = new List<GameObject>();


        //foreach (GameObject movableObject in allMovableObjects)
        //{
        //    allMovableEntities.Add(movableObject.GetComponent<Entity>());
        //}

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


        if(checkObjectForOverlap != null)
        {
            if (checkObjectForOverlap.GetComponent<SpriteRenderer>().sortingLayerName == "Overlap")
            {
                bruteForceOverlap = true;
            }
            else bruteForceOverlap = false;
        }

        //checks to see if moving object are within a 100 unit radius
        //foreach (GameObject movableObject in allMovableObjects)
        //{
        //    float dst = Vector3.Distance(movableObject.transform.position, this.transform.position);

        //    if (dst < 10 && !allMovableObjectsWithinProximity.Contains(movableObject))
        //    {
        //        allMovableObjectsWithinProximity.Add(movableObject);
        //    }
        //    else
        //    {
        //        if (allMovableObjectsWithinProximity.Contains(movableObject) && dst > 10)
        //        {
        //            allMovableObjectsWithinProximity.Remove(movableObject);
        //        }
        //    }
        //}


        //foreach (GameObject movableObject in allMovableObjectsWithinProximity)
        //{
        //    if (movableObject.transform.position.y > threshold)// && movableObject.transform.position.x > (this.transform.position.x - this.sprite.size.x / 2) && movableObject.transform.position.x < (this.transform.position.x + this.sprite.size.x / 2))
        //    {
        //        //			sprite.sortingOrder = sortingOrder;

        //        Overlap();

        //    }
        //    else if (movableObject.transform.position.y > (slope * movableObject.transform.position.x) + yintercept || 
        //                                                   movableObject.transform.position.y > (slope2 * movableObject.transform.position.x) + yintercept2)// && movableObject.transform.position.x > (this.transform.position.x - this.sprite.size.x / 2) && movableObject.transform.position.x < (this.transform.position.x + this.sprite.size.x / 2))
        //    {

        //        Overlap();            
        //    }
        //    else
        //    {
        //        sprite.sortingLayerName = currentLayerName;
        //        sprite.sortingOrder = currentSortingOrderNumber;
        //    }
        //}

        if(player.transform.position.y > threshold)
        {
            Overlap();
        }
        else if(player.transform.position.y > (slope * player.transform.position.x) + yintercept ||
                                                           player.transform.position.y > (slope2 * player.transform.position.x) + yintercept2)
        {
            Overlap();
        }
        else
        {
            sprite.sortingLayerName = currentLayerName;
            sprite.sortingOrder = currentSortingOrderNumber;
        }

        //Debug.Log("the number of object with the list is " + allMovableObjectsWithinProximity.Count);


        if (copyParentSortingLayer)
        {
            sprite.sortingLayerName = transform.parent.GetComponent<SpriteRenderer>().sortingLayerName;

        }

        if(bruteForceRender)
        {
         //  if(neverRenderBehindThisObject.GetComponent<SpriteRenderer>().sortingLayerID != 0)
         //   {
              
                sprite.sortingLayerID = neverRenderBehindThisObject.GetComponent<SpriteRenderer>().sortingLayerID;
                sprite.sortingOrder = neverRenderBehindThisObject.GetComponent<SpriteRenderer>().sortingOrder + 1;
         //   }

        }

        if(bruteForceOverlap)
        {
            sprite.sortingLayerID = checkObjectForOverlap.GetComponent<SpriteRenderer>().sortingLayerID;
            sprite.sortingOrder = checkObjectForOverlap.GetComponent<SpriteRenderer>().sortingOrder + 1;
        }

       

        //if(Mathf.Abs((float) threshold - this.transform.position.y) > sprite.size.y)
        //{
        //    threshold = this.transform.position.y + 1;
        //}

        if(this.name == "gazebo back")
        {

            //Debug.Log("the second slope is" + slope2);
            //Debug.Log("the first yintercept is " + yintercept2);
        }

        
    }

    void Overlap()
    {
        sprite.sortingLayerName = OverlapLayer;
    }

   
}
