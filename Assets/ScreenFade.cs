using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenFade : MonoBehaviour
{
    SpriteRenderer spriteRend;
    public bool fadeOut;
    public bool fadeToBlack;

    private GameObject player;

    private bool completelyFaded;
    private int completelyFadedTimer;

    private BoxCollider2D fadeTriggerCollider;

    // Start is called before the first frame update
    void Start()
    {
        spriteRend = GetComponent<SpriteRenderer>();
        spriteRend.enabled = false;

        player = GameObject.Find("player");

        fadeTriggerCollider = transform.GetComponentInChildren<BoxCollider2D>();

    }

    // Update is called once per frame
    void Update()
    {
        if(spriteRend.color.a > .1f && fadeOut)
        {
            Color tmpColor = spriteRend.color;
            spriteRend.color = new Color(tmpColor.r, tmpColor.g, tmpColor.b, Mathf.Lerp(tmpColor.a, .1f, Time.deltaTime));
        }

        if(spriteRend.color.a <= .12f && fadeOut)
        {
            spriteRend.enabled = false;
            fadeOut = false;
            fadeTriggerCollider.enabled = true;
        }

        if(fadeToBlack && !spriteRend.enabled)
        {
            spriteRend.enabled = true;
        }

        if(spriteRend.color.a < .99f && fadeToBlack)
        {
            Color tmpColor = spriteRend.color;
            spriteRend.color = new Color(tmpColor.r, tmpColor.g, tmpColor.b, Mathf.Lerp(tmpColor.a, 1f, Time.deltaTime * 2));
            fadeTriggerCollider.enabled = false;
        }

        if(spriteRend.color.a > .98f && fadeToBlack)
        {           
            completelyFaded = true;
            completelyFadedTimer = 100;
        }

        if (completelyFaded && fadeToBlack)
        {
            fadeToBlack = false;
            player.GetComponent<PlayerControl>().setForcePlayer(true);
        }

        // triggers the blackout to subsided once the Timer is almost done
        if(completelyFadedTimer > 0 && completelyFadedTimer < 20)
        {
            fadeOut = true;
        }


        if (completelyFadedTimer > 0)
        {
            completelyFadedTimer -= Mathf.RoundToInt(Time.deltaTime * 100);
        }
        else if (completelyFadedTimer <= 0 && completelyFaded)
        {
            completelyFaded = false;
            completelyFadedTimer = 0;
        }

    }


    public void setFadeOut(bool b)
    {
        fadeOut = b;
    }

    public void setFadeToBlack(bool b)
    {
        fadeToBlack = b;
        fadeOut = false;
    }
}
