using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingWithTransparency : SortingOrderScript {

    bool transparentTrigger;

	// Use this for initialization
	void Start () {
        sprite = GetComponent<SpriteRenderer>();
        player = GameObject.Find("player");

        currentLayerName = sprite.sortingLayerName.ToString();

        if (thresholdPoint1 != null)
        {
            threshold1 = new Vector2(thresholdPoint1.transform.position.x, thresholdPoint1.transform.position.y);

        }

        if (thresholdPoint2 != null)
        {
            threshold2 = new Vector2(thresholdPoint2.transform.position.x, thresholdPoint2.transform.position.y);
        }

        if (thresholdPoint1 != null && thresholdPoint2 != null)
        {
            slope = (thresholdPoint2.transform.position.y - thresholdPoint1.transform.position.y) /
                (thresholdPoint2.transform.position.x - thresholdPoint1.transform.position.x);

            yintercept = thresholdPoint2.transform.position.y - (slope * thresholdPoint2.transform.position.x);
        }
    }
	
	// Update is called once per frame
	void Update () {

        if (player.transform.position.y > threshold)
        {

            if (transparentTrigger)
            {
                FadeOut();
            }
            else
            {
                FadeIn();
            }

            sprite.sortingLayerName = OverlapLayer;
        }
        else if (player.transform.position.y > (slope * player.transform.position.x) + yintercept && player.transform.position.x > (this.transform.position.x - this.sprite.size.x / 2) && player.transform.position.x < (this.transform.position.x + this.sprite.size.x / 2))
        {
            if (player.transform.position.y - this.transform.position.y < sprite.size.y / 2)
            {
                FadeOut();
            }
            else
            {
                FadeIn();
            }

            sprite.sortingLayerName = OverlapLayer;

        }
        else
        {
            FadeIn();

            sprite.sortingLayerName = currentLayerName;

        }


        if (copyParentSortingLayer)
        {
            
            Color tmpColor1 = transform.parent.GetComponent<SpriteRenderer>().color;
            Color tmpColor2 = sprite.color;
            sprite.color = new Color(tmpColor2.r, tmpColor2.g, tmpColor2.b, tmpColor1.a);

            //if (player.transform.position.y - this.transform.position.y < sprite.size.y / 2)
            //{
            //    FadeOut();
            //}
            //else
            //{
            //    FadeIn();
            //}

            sprite.sortingLayerName = transform.parent.GetComponent<SpriteRenderer>().sortingLayerName;
        }

        if(this.name == "willowTrunk" && transparentTrigger)
        {
            Debug.Log("we have communication happening!!");
        }

    }

    void FadeOut()
    {
        Color tmpColor = sprite.color;
        sprite.color = new Color(tmpColor.r, tmpColor.g, tmpColor.b, Mathf.Lerp(tmpColor.a, .4f, Time.deltaTime));
    }

    void FadeIn()
    {
        Color tmpColor = sprite.color;
        sprite.color = new Color(tmpColor.r, tmpColor.g, tmpColor.b, Mathf.Lerp(tmpColor.a, 1f, Time.deltaTime));
    }

    void setTempBool(bool b)
    {
        transparentTrigger = b;
    }

}
