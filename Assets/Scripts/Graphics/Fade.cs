using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{
    SpriteRenderer spriteRend;
    public bool fadeOut;

    // Start is called before the first frame update
    void Start()
    {
        spriteRend = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(spriteRend.color.a > .1f && fadeOut)
        {
            Color tmpColor = spriteRend.color;
            spriteRend.color = new Color(tmpColor.r, tmpColor.g, tmpColor.b, Mathf.Lerp(tmpColor.a, .1f, Time.deltaTime));
        }

        if(spriteRend.color.a <= .12f)
        {
            Destroy(this.transform.gameObject, 1f);
        }

    }

    public void setFadeOut(bool b)
    {
        fadeOut = b; 
    }
}
