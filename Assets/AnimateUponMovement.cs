using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateUponMovement : MonoBehaviour
{
    Animator Movement;

    // Start is called before the first frame update
    void Start()
    {
        Movement = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.root.name == "player")
        {
            if (GetComponentInParent<PlayerControl>().isIdle)
            {
                //MovingMask.enabled = false;
                Movement.speed = 0.0f;
            }
            else if (GetComponentInParent<PlayerControl>().isWalking)
            {
                //MovingMask.enabled = true;
                Movement.speed = .3f;
            }
            else
            {
                //MovingMask.enabled = true;   
                Movement.speed = 1f;
            }
        }
    }
}
