﻿using UnityEngine;
using System.Collections;

public class SortingOrderScript : MonoBehaviour 
{
	public const string OverlapLayer = "Overlap";
	public string currentLayerName;
	public int sortingOrder = 0;
	private SpriteRenderer sprite;
	private GameObject player;


	// This is the threshhold at which the player's position 
	// will change the nearby objects sorting layer
	public double threshold = 9.5;

    // Here we create two empty gameobjects which represent two points. 
    // A line is therefore created for sorting depth rendering based on the player's position relative to this line
    public GameObject thresholdPoint1;
    public GameObject thresholdPoint2;

    private Vector2 threshold1;
    private Vector2 threshold2;
    private float slope;
    private float yintercept;


	// Use this for initialization
	void Start () 
	{
		sprite = GetComponent<SpriteRenderer> ();
		player = GameObject.Find ("player");

		currentLayerName = sprite.sortingLayerName.ToString ();

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
        if (thresholdPoint1 != null && thresholdPoint2 != null)
        {
            slope = (thresholdPoint2.transform.position.y - thresholdPoint1.transform.position.y) /
                (thresholdPoint2.transform.position.x - thresholdPoint1.transform.position.x);

            yintercept = thresholdPoint2.transform.position.y - (slope * thresholdPoint2.transform.position.x);
        }

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
            sprite.sortingLayerName = currentLayerName;

        if (thresholdPoint1 != null && thresholdPoint2 != null)
        {
            Debug.Log("the slope is " + slope);
            Debug.Log("the thresholdPoint2 is " + thresholdPoint2.transform.position.x + " , " + thresholdPoint2.transform.position.y);
            Debug.Log("the yintercept is " + yintercept);
        }

        //		Debug.Log ("the sprite's current sorting layer is" + sprite.sortingLayerName);
        ////		Debug.Log ("the player's x & y are" + player.transform.position.x + " , " + player.transform.position.y);
        //        Debug.Log ("the threshold is " + threshold);
        //        Debug.Log("the player's y is" + player.transform.position.y);
    }
}
