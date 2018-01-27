using UnityEngine;
using System.Collections;

public class RenderMask : MonoBehaviour {

    Animator MovingMask;
    private bool grassTrigger;
    private bool waterTrigger;

    private GameObject renderMask;

	// Use this for initialization
	void Start ()
    {

        MovingMask = GetComponent<Animator>();
        renderMask = GameObject.Find("RenderMask");

    }
	
	// Update is called once per frame
	void Update ()
    {   
        if(waterTrigger)
        {
            MovingMask.Play("waterFlowMask");
        }

        if(grassTrigger)
        {
            
        }

        if (GetComponentInParent<PlayerControl>().isIdle)
        {
            MovingMask.enabled = false;
        }
        else MovingMask.enabled = true;


    }
}
