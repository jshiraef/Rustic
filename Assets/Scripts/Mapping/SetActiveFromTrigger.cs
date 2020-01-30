using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveFromTrigger : MonoBehaviour
{
    private GameObject landMask;
    private Vector3 originalPosition;
    private Vector3 newPosition;

    public GameObject positionChanger1;
    public GameObject positionChanger2;

    public GameObject positionChangerA;
    public GameObject positionChangerB;

    private GameObject player;
    private PlayerControl playerController;
    private PolygonCollider2D landMaskCollider;

    private bool maskSwitch;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("player");
        playerController = player.GetComponent<PlayerControl>();
        
        originalPosition = this.transform.position;
        landMask = GameObject.Find("landMask").transform.GetChild(0).gameObject;
        landMaskCollider = landMask.GetComponentInParent<PolygonCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //these next four conditional statements check to see if the player is crossing a particular threshold
        // on both the east and west sides of the elevated veggie house and yard in order to alternate colliders and rendermasks
        if(player.transform.position.x > 82 && player.transform.position.y > -13 && !maskSwitch)
        {
            if (playerController.getDirectionAngle360() < 180)
            {
                maskSwitch = true;
            }
        }

        if (player.transform.position.x < 42 && player.transform.position.y > -13 && !maskSwitch)
        {
            if (playerController.getDirectionAngle360() < 180)
            {
                maskSwitch = true;
            }
        }

        if (player.transform.position.x > 82 && player.transform.position.y < -13 && maskSwitch)
        {
            if(playerController.getDirectionAngle360() > 180)
            {
                maskSwitch = false;
            }
        }

        if (player.transform.position.x < 42 && player.transform.position.y < -13 && maskSwitch)
        {
            if (playerController.getDirectionAngle360() > 180)
            {
                maskSwitch = false;
            }
        }

        if (maskSwitch)
        {
            landMaskCollider.enabled = false;
        }
        else landMaskCollider.enabled = true;

        //Debug.Log("the player's angle is " + playerController.getDirectionAngle360());
        //Debug.Log("the maskSwitch is " + maskSwitch);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if(other.tag == "movable")
        {
            landMask.SetActive(false);
            //Debug.Log("this is happening");

            positionChanger1.SetActive(true);
            positionChanger2.SetActive(false);

            positionChangerA.SetActive(true);
            positionChangerB.SetActive(false);
        }
        
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "movable")
        {
            landMask.SetActive(true);

            positionChanger1.SetActive(false);
            positionChanger2.SetActive(true);

            positionChangerA.SetActive(false);
            positionChangerB.SetActive(true);
        }
    }
}
