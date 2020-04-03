using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoppableBarrier : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionStay2D(Collision2D coll)
    {
        if(coll.gameObject.name == "player")
        {
            //Debug.Log("the player's action is " + coll.gameObject.GetComponent<PlayerControl>().getCurrentAction());
        }
    }
}
