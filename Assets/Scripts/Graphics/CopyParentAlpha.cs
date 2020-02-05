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

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();

        posShift = GetComponentInParent<PositionShift>();

        parentSprite = transform.parent.GetComponent<SpriteRenderer>();


    }

    // Update is called once per frame
    void Update()
    {
        

        Color tmpColor = sprite.color;
        Color parentTmpColor = parentSprite.color;
        sprite.color = parentSprite.color;

        if (copySortingLayer)
        {
            sprite.sortingLayerName = parentSprite.sortingLayerName;
        }

    }
}
