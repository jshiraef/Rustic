using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendTriggerUp : MonoBehaviour
{

    public bool reverse;

    public bool screenFadeToBlack;

    public bool wellTrigger;

    private GameObject player;
    

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

     void OnTriggerEnter2D(Collider2D other)
    {
        //transform.parent.SendMessage("OnTriggerEnter2D", other);
    }

    

    void OnTriggerStay2D(Collider2D other)
    {
        if (wellTrigger)
        {
            transform.parent.SendMessage("personSensor", other);
            return;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (screenFadeToBlack)
        {
            transform.parent.SendMessage("setFadeToBlack", true);
            return;
        }

        if (wellTrigger)
        {
            transform.parent.SendMessage("personGoneSensor", other);
            return;

        }


        if (reverse)
        {
            transform.parent.SendMessage("ExitSouthSwitch", other);
        }

        if (!reverse)
        {
            transform.parent.SendMessage("ExitNorthSwitch", other);
        }
    }
}
