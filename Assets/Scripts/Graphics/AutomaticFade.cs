using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticFade : MonoBehaviour
{

    private GameObject player;
    public float fadeInTriggerDistance;

    private bool faded;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("player");
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(player.transform.position, this.transform.position) < fadeInTriggerDistance)
        {
            fadeIn();
        }


        fadeOut();
    }

    void fadeOut()
    {

        if (faded)
            return;

        if (!faded)
        {
            GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, .005f * Time.deltaTime * 60);
        }

        if (GetComponent<SpriteRenderer>().color.a <= .005f)
        {
            faded = true;
        }

    }

    void fadeIn()
    {
        GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, .01f * Time.deltaTime * 60);
        faded = false;
    }

    void showSpriteObject()
    {
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        faded = false;
    }
}
