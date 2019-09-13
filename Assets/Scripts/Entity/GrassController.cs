using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassController : MonoBehaviour {

    private GameObject player;
    private Animator anim;

    private bool chopped;

	// Use this for initialization
	void Start ()
    {
        player = GameObject.Find("player");
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {


        if (Mathf.Abs(player.transform.position.x - this.transform.position.x) < (GetComponent<SpriteRenderer>().size.x * this.transform.localScale.x)/2 && Mathf.Abs(player.transform.position.y - this.transform.position.y) < (GetComponent<SpriteRenderer>().size.y * this.transform.localScale.y)/2)
        {
            if (player.GetComponent<PlayerControl>().isIdle)
            {
                anim.speed = 0f;
            }
            else if (player.GetComponent<PlayerControl>().isWalking)
            {
                anim.speed = 1.5f;
            }
            else if (player.GetComponent<PlayerControl>().isRunning)
            {
                anim.speed = 2f;
            }
            else
            {
                anim.speed = 0f;
            }       
        }
        else
        {
            anim.speed = 0f;
        }

        if(chopped)
        {
            anim.Play("smallWindyGoldWheatGrass");
            Destroy(this.transform.gameObject);
        }

        //Debug.Log("the speed of the animator is " + anim.speed);
        
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.gameObject.tag == "weapon")
        {
            //Debug.Log("the grass collided with a weapon");
            chopped = true;
        }
    }
}
