using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// this is a simple script that will automatically make a gameobject's spriterenderer's alpha be the same as its parent's spriterenderer
public class CopyParentAlpha : MonoBehaviour
{
    SpriteRenderer sprite;
    SpriteRenderer parentSprite;
    //public Color parentColor;
    private PositionShift posShift;

    public bool copySortingLayer;
    public bool copyObjectSpriteAlpha;

    public GameObject objectToCopyAlphaFrom;

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();

        if (transform.parent.transform.gameObject.GetComponent<PositionShift>() != null)
        {
            posShift = GetComponentInParent<PositionShift>();
        }

        if (transform.parent.GetComponent<SpriteRenderer>() != null)
        {
            parentSprite = transform.parent.GetComponent<SpriteRenderer>();
        }
        else parentSprite = sprite;

    }

    // Update is called once per frame
    void Update()
    {       

        Color tmpColor = sprite.color;
        Color parentTmpColor = parentSprite.color;
        sprite.color = parentSprite.color;

        if (objectToCopyAlphaFrom != null)
        {
            Color tmpColor2 = sprite.color;
            Color objectColor2 = objectToCopyAlphaFrom.GetComponent<SpriteRenderer>().color;
            sprite.color = objectToCopyAlphaFrom.GetComponent<SpriteRenderer>().color;
        }

        if (copySortingLayer)
        {
            sprite.sortingLayerName = parentSprite.sortingLayerName;
        }

    }
}
