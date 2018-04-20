using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockController : MonoBehaviour {

    private bool breakable;

    private bool grabbable;
    private float grabbableTimer;

    public float airSpeed;
    public float rollSpeed;

    private GameObject outline;

    private GameObject player;

	// Use this for initialization
	void Start () {
        player = GameObject.Find("player");
        outline = this.transform.GetChild(0).gameObject;
	}
	
	// Update is called once per frame
	void Update () {

        if (player.GetComponent<PlayerControl>().interact)
        {
            outline.SetActive(true);

            if (!grabbable && grabbableTimer <= 0)
            {
                grabbableTimer = 1f;
                //outline.AddComponent<CircleCollider2D>();
                //outline.GetComponent<CircleCollider2D>().isTrigger = true;
                //outline.GetComponent<CircleCollider2D>().radius = 2f;
            }
            grabbable = true;    
        }
        else
        {
            outline.SetActive(false);
        }

        if(grabbableTimer > 0)
        {
            
        }


        if(grabbableTimer > 0)
        {
            grabbableTimer -= Time.deltaTime;
        }

        if(grabbableTimer <= 0)
        {
            grabbable = false;
            //Destroy(outline.GetComponent<CircleCollider2D>());
        }
	}
}
