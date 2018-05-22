using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sickleSwipe : Entity {

    

    private float swipeStartAngle;

    private TrailRenderer swipe;

    private GameObject player;
    private PlayerControl playerControl;

	// Use this for initialization
	void Start () {

        swipe = this.GetComponent<TrailRenderer>();

        swipeStartAngle = 35f;

        player = transform.parent.gameObject;
        playerControl = player.GetComponent<PlayerControl>();

    }
	
	// Update is called once per frame
	void Update () {

        if (playerControl.getSwinging())
        {

            if (playerControl.getSwingCoolDown()< .35f && playerControl.getSwingCoolDown() > .05f)
            {
                swipe.enabled = true;

                swipeStartAngle += Mathf.Round(Time.deltaTime * 350);
            }
            else
            {
                swipe.enabled = false;


                if (playerControl.getDirection8() == Direction.NORTHEAST50)
                {
                    swipeStartAngle = 30f;
                }
                else if (playerControl.getDirection8() == Direction.NORTH)
                {
                    swipeStartAngle = 60f;
                }
                else if (playerControl.getDirection8() == Direction.NORTHWEST130)
                {
                    swipeStartAngle = 100f;
                }
                else if (playerControl.getDirection8() == Direction.WEST)
                {
                    swipeStartAngle = 140f;
                }
                else if (playerControl.getDirection8() == Direction.SOUTHWEST230)
                {
                    swipeStartAngle = 190f;
                }
                else if (playerControl.getDirection8() == Direction.SOUTH)
                {
                    swipeStartAngle = 250f;
                }
                else if (playerControl.getDirection8() == Direction.SOUTHEAST310)
                {
                    swipeStartAngle = 290f;
                }
                else if (playerControl.getDirection8() == Direction.EAST)
                {
                    swipeStartAngle = 330f;
                }
            }

            Debug.Log("the swipeStartAngle is " + swipeStartAngle);

            Vector3 sickleSwipePosition = new Vector3(Mathf.Cos(swipeStartAngle * Mathf.Deg2Rad) * 1.65f, Mathf.Sin(swipeStartAngle * Mathf.Deg2Rad) * 1.65f, 0);
            this.transform.localPosition = sickleSwipePosition;
        }

    }
}
