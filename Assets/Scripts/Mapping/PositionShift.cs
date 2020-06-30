using System.Collections;
using System.Collections.Generic;
using UnityEngine;





// this script will can alternate a sprite or gameobject's position. It can be used to shift around an object based off of the camera's location
public class PositionShift : MonoBehaviour
{

    public Vector3 originalPosition;
    public Color originalColor;
    public Vector3 newPosition;
    public int thresholdToInvokePositionChange;
    public int disappearingThreshold;
    public int fadingOutThreshold;
    public bool positionTrigger;
    public bool colorTrigger = false;
    private GameObject player;
    

    private SpriteRenderer sprite;

    

    // Start is called before the first frame update
    void Start()
    {
        originalPosition = this.transform.localPosition;
        sprite = GetComponent<SpriteRenderer>();
        originalColor = sprite.color;
        player = GameObject.Find("player");

    }

    // Update is called once per frame
    void Update()
    {

        //changes the objects position dependent on the player or the camera's position
        if (player.transform.position.y > thresholdToInvokePositionChange && thresholdToInvokePositionChange != 0)
        {
            this.transform.localPosition = newPosition;
        }
        else this.transform.localPosition = originalPosition;


        // changes the sprite's alpha (makes the object disappear) dependent on the player or the camera's position
        if (player.transform.position.y > disappearingThreshold && disappearingThreshold != 0)
        {
            colorTrigger = true;
            Color tmpColor = sprite.color;
            sprite.color = new Color(tmpColor.r, tmpColor.g, tmpColor.b, 0f);
        }
        else if(colorTrigger && player.transform.position.y < disappearingThreshold)
        {
            Color tmpColor = sprite.color;
            sprite.color = originalColor;
            colorTrigger = false;
        }

        //changes the object's alpha dependent on the player or the camera's position
        if(player.transform.position.y > fadingOutThreshold)
        {

            // checks to see if it has the sorting transparency script attached
            if(GetComponent<SortingWithTransparency>() != null)
            {
                if (GetComponent<SortingWithTransparency>().enabled)
                {
                    GetComponent<SortingWithTransparency>().enabled = false;
                }
            }

            Color tmpColor = sprite.color;
            sprite.color = new Color(tmpColor.r, tmpColor.g, tmpColor.b, Mathf.Lerp(sprite.color.a, 0f, Time.deltaTime));
        }
        else if (player.transform.position.y < fadingOutThreshold)
        {

            if(GetComponent<SortingWithTransparency>() != null)
            {
                if (!GetComponent<SortingWithTransparency>().enabled)
                {
                    GetComponent<SortingWithTransparency>().enabled = true;
                }              
            }

            Color tmpColor = sprite.color;
            sprite.color = new Color(tmpColor.r, tmpColor.g, tmpColor.b, Mathf.Lerp(sprite.color.a, 1f, Time.deltaTime));
        }


        
    }

    
}
