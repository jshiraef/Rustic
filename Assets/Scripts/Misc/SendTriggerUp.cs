using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendTriggerUp : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

     void OnTriggerEnter2D(Collider2D other)
    {
        //transform.parent.SendMessage("OnTriggerEnter2D", other);
    }

     void OnTriggerExit2D(Collider2D other)
    {
        transform.parent.SendMessage("switchExit", other);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        //transform.parent.SendMessage("OnTriggerStay2D", other);
    }
}
