using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// this simple class will outline any object that has a child named "Outline" along with a SpriteRenderer component and a Sprite/Outline material on it
public class ObjectOutline : MonoBehaviour
{
    private GameObject objectOutline;
    private PlayerControl playerControl;
    public bool useColliderForOutlineTrigger;
    private GameObject objectCollider;

    // Start is called before the first frame update
    void Start()
    {
        playerControl = GameObject.Find("player").GetComponent<PlayerControl>();
        objectOutline = transform.Find("Outline").gameObject;
        objectCollider = transform.Find("outlineCollider").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        // if the player is close to the object the outline will appear
        if(Vector3.Distance(playerControl.transform.position, this.transform.position) < 1)
        {
            objectOutline.SetActive(true);

            Debug.Log("this should be happening!");
            // if the object itself is not being rendered, the outline shouldn't be either
            if (!GetComponent<SpriteRenderer>().enabled)
            {
                objectOutline.GetComponent<SpriteRenderer>().enabled = false;
            }

        }
        else
        {
            objectOutline.SetActive(false);
        }

        // if the object is an awkward shape then it might be useful to use collision to trigger the outline 
        if (useColliderForOutlineTrigger)
        {
            if (playerControl.transform.GetComponent<BoxCollider2D>().IsTouching(objectCollider.GetComponent<CircleCollider2D>()))
            {
                objectOutline.SetActive(true);
            }
            else
            {
                objectOutline.SetActive(false);
            }
        }
    }

}
