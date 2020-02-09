using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderTrigger : MonoBehaviour
{
    public GameObject collider1;
   

    public GameObject colliderA;
    public GameObject colliderB;

    public bool colliderSwitch;
    public bool stairsTrigger;

    private GameObject player;
    private PlayerControl playerController;

    public GameObject pointA;
    public GameObject pointB;

    private PolygonCollider2D stairsCollider;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("player");
        playerController = player.GetComponent<PlayerControl>();

        stairsCollider = GetComponent<PolygonCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (player.transform.position.x > pointA.transform.position.x && player.transform.position.y > pointA.transform.position.y &&
        //    player.transform.position.x < pointB.transform.position.x && player.transform.position.y > pointB.transform.position.y &&
        //    player.transform.position.y < this.transform.position.y - 1 &&
        //    !colliderSwitch)
        //{
        //    Debug.Log("this is happening");
        //    if (playerController.getDirectionAngle360() < 180)
        //    {
        //        colliderSwitch = true;
        //        stairsCollider.enabled = true;

        //    }
        //}



        //if (player.transform.position.x > pointA.transform.position.x && player.transform.position.y < pointA.transform.position.y &&
        //    player.transform.position.x < pointB.transform.position.x && player.transform.position.y < pointB.transform.position.y &&
        //    player.transform.position.y < this.transform.position.y - 1 &&
        //    colliderSwitch)
        //{
        //    if (playerController.getDirectionAngle360() > 180)
        //    {
        //        colliderSwitch = false;
        //    }
        //}


        if(colliderSwitch)
        {
            collider1.SetActive(true);
            

            colliderA.SetActive(false);
            colliderB.SetActive(false);
        }
        else
        {
            collider1.SetActive(false);          

            colliderA.SetActive(true);
            colliderB.SetActive(true);
        }

        if(!stairsTrigger)
        {
            collider1.SetActive(false);

            colliderA.SetActive(true);
            colliderB.SetActive(true);
        }

        if(!colliderSwitch)
        {
            stairsCollider.enabled = false;
        }

        //Debug.Log("the y position of this object is " + this.transform.position.y);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.name == "player" && colliderSwitch)
        {
            stairsTrigger = true;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.name == "player" && colliderSwitch)
        {
            stairsTrigger = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.name == "player" && colliderSwitch)
        {
            stairsTrigger = false;
        }

        //stairsCollider.enabled = false;
    }


    void switchExit(Collider2D other)
    {
        if (playerController.getDirectionAngle360() < 180)
        {
            colliderSwitch = true;
            stairsCollider.enabled = true;

            GetComponent<SpriteRenderer>().sortingLayerName = "default";
            GetComponent<SortingOrderScript>().enabled = false;

        }

        if (playerController.getDirectionAngle360() > 180)
        {
            colliderSwitch = false;
            GetComponent<SortingOrderScript>().enabled = true;
        }
    }
}
