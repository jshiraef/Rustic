using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sickleSwipe : Entity {

    

    private float swipeStartAngle;
    private float swipeReach;

    private TrailRenderer swipe;

    private GameObject player;
    private GameObject swipeReference;
    private PlayerControl playerControl;
    private Vector3 swipeOriginalPosition;

	// Use this for initialization
	void Start () {

        swipe = this.GetComponent<TrailRenderer>();

        swipeStartAngle = 35f;
        swipeReach = 1.65f;

        player = transform.parent.transform.parent.gameObject;
        playerControl = player.GetComponent<PlayerControl>();
        swipeReference = transform.parent.gameObject;
        swipeOriginalPosition = swipeReference.transform.localPosition;

    }
	
	// Update is called once per frame
	void Update () {

        if (playerControl.getSwinging())
        {

            if (playerControl.getSwingCoolDown()< .3f && playerControl.getSwingCoolDown() > .1f)
            {
                swipe.enabled = true;

                swipeStartAngle += Mathf.Round(Time.deltaTime * 650);          
            }
            else
            {
                swipe.enabled = false;

                if (playerControl.getDirection8() == Direction.NORTHEAST50)
                {
                    swipeStartAngle = 15f;
                    swipeReference.transform.localPosition = Vector3.Lerp(swipeReference.transform.localPosition, new Vector3(-.25f, -1.75f, 0f), Time.deltaTime * 5); 
                    //swipeReach = 1.25f;

                    Debug.Log("the is happening");
                }
                else if (playerControl.getDirection8() == Direction.NORTH)
                {
                    swipeStartAngle = 50f;
                    swipeReference.transform.localPosition = Vector3.Lerp(swipeReference.transform.localPosition, new Vector3(0f, -1.75f, 0f), Time.deltaTime * 5);
                    //swipeReach = 1f;
                }
                else if (playerControl.getDirection8() == Direction.NORTHWEST130)
                {
                    swipeStartAngle = 100f;
                    swipeReference.transform.localPosition = swipeOriginalPosition;
                    //swipeReach = 1.5f;
                }
                else if (playerControl.getDirection8() == Direction.WEST)
                {
                    swipeStartAngle = 140f;
                    swipeReference.transform.localPosition = swipeOriginalPosition;
                    //swipeReach = 1.5f;
                }
                else if (playerControl.getDirection8() == Direction.SOUTHWEST230)
                {
                    swipeStartAngle = 190f;
                    swipeReference.transform.localPosition = swipeOriginalPosition;
                    //swipeReach = 1.65f;
                }
                else if (playerControl.getDirection8() == Direction.SOUTH)
                {
                    swipeStartAngle = 250f;
                    swipeReference.transform.localPosition = swipeOriginalPosition;
                    //swipeReach = 1.65f;
                }
                else if (playerControl.getDirection8() == Direction.SOUTHEAST310)
                {
                    swipeStartAngle = 290f;
                    swipeReference.transform.localPosition = swipeOriginalPosition;
                    //swipeReach = 1.65f;
                }
                else if (playerControl.getDirection8() == Direction.EAST)
                {
                    swipeStartAngle = 350f;
                    swipeReference.transform.localPosition = Vector3.Lerp(swipeReference.transform.localPosition, new Vector3(-.25f, -2f, 0f), Time.deltaTime * 5); ;
                    //swipeReach = 1.25f;
                }
            }

            

            //Debug.Log("the swipeStartAngle is " + swipeStartAngle);

            Vector3 sickleSwipePosition = new Vector3(Mathf.Cos(swipeStartAngle * Mathf.Deg2Rad) * swipeReach, Mathf.Sin(swipeStartAngle * Mathf.Deg2Rad) * swipeReach, 0);
            this.transform.localPosition = sickleSwipePosition;
        }

    }
}
