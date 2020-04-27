using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// For this class is understood that their is a SpriteOutline2 script/component 
// already attached to the same gameObject

// It also helps if you give the object a child named Outline & drag and drop the Outline object to the inspector "actualOutline"
// in the Inspector increase the size to 18 and blur size to 19

    // *The Sprite Image must be "Read/Write enabled within the unity editor*

public class GlowingOutline : MonoBehaviour
{
    public GameObject objectToOutline;

    public GameObject actualOutline;

    private bool glowUp;


    // Start is called before the first frame update
    void Start()
    {
        if (actualOutline == null)
        {
            if(transform.childCount > 0)
            {
                actualOutline = transform.Find("Outline").gameObject;
            }
        }

        //actualOutline.GetComponent<SpriteRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (actualOutline == null)
        {
            if (transform.childCount > 0)
            {
                actualOutline = transform.Find("Outline").gameObject;
            }
        }

        // this will make the outline around the object glow or appear to be glowing

        if(actualOutline != null)
        {
            if (actualOutline.GetComponent<SpriteRenderer>().enabled)
            {
                if (!glowUp)
                {
                    if (objectToOutline.GetComponent<SpriteOutline2>().blurAlphaMultiplier > .2f)
                    {
                        objectToOutline.GetComponent<SpriteOutline2>().blurAlphaMultiplier -= .05f;
                    }

                    if (objectToOutline.GetComponent<SpriteOutline2>().blurAlphaMultiplier < .2f)
                    {
                        glowUp = true;
                    }

                }

                if (glowUp)
                {
                    objectToOutline.GetComponent<SpriteOutline2>().blurAlphaMultiplier += .05f;

                    if (objectToOutline.GetComponent<SpriteOutline2>().blurAlphaMultiplier > .98f)
                    {
                        glowUp = false;
                    }
                }

            }
        }
       

    }
}
