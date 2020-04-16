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

    //private PolygonCollider2D stairsCollider;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("player");
        playerController = player.GetComponent<PlayerControl>();

        //stairsCollider = GetComponent<PolygonCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {

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

        //Debug.Log("the Collider Switch is " + colliderSwitch);
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
    }

    // these next two methods are called from child object trigger events and sent up the hierarchy via "sendMessage"
    void ExitNorthSwitch(Collider2D other)
    {

        //Debug.Log("the north switch works");
        
            if (playerController.getDirectionAngle360() < 180)
            {
                colliderSwitch = true;
                //stairsCollider.enabled = true;

                GetComponent<SpriteRenderer>().sortingLayerName = "default";
                GetComponent<SortingOrderScript>().enabled = false;

            }

            if (playerController.getDirectionAngle360() > 180)
            {
                colliderSwitch = false;
                GetComponent<SortingOrderScript>().enabled = true;
            }
            
    }

    void ExitSouthSwitch(Collider2D other)
    {
        //Debug.Log("the south switch works");

        if (playerController.getDirectionAngle360() > 180)
            {
                colliderSwitch = true;
                //stairsCollider.enabled = true;

                GetComponent<SpriteRenderer>().sortingLayerName = "default";
                GetComponent<SortingOrderScript>().enabled = false;

            }

            if (playerController.getDirectionAngle360() < 180)
            {
                colliderSwitch = false;
                GetComponent<SortingOrderScript>().enabled = true;
            }
    }


}
