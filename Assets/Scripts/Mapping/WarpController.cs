using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// this script will warp the player or object to another location in world space
public class WarpController : MonoBehaviour
{

    public GameObject newPosition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        coll.gameObject.transform.position = newPosition.transform.position;

        // this changes the alpha of the Tonic House sprite in the "Position Shift" script
        if(this.transform.parent.transform.name == "Tonic House")
        {
            Debug.Log("this happened");
            if(this.transform.name == "warpToRoof")
            {
                this.transform.parent.gameObject.GetComponent<TonicRoofController>().setRoofTrigger(true);

                // this is a brute force way to change the sprite's sorting layer
                this.transform.parent.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = default;
                this.transform.parent.gameObject.GetComponent<SortingOrderScript>().enabled = false;
            }
            else if(this.transform.name == "warpToGround")
            {
                this.transform.parent.gameObject.GetComponent<TonicRoofController>().setRoofTrigger(false);
                this.transform.parent.gameObject.GetComponent<SortingOrderScript>().enabled = true;
            }
        }
    }

}
