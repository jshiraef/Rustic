using UnityEngine;
using System.Collections;

public class RenderMask : MonoBehaviour {

    Animator MovingMask;

    private GameObject renderMask;
    private GameObject renderMaskOutliner;
    private GameObject renderMaskOutliner2;
    public MaskType maskType;

	// Use this for initialization
	void Start ()
    {

        MovingMask = GetComponent<Animator>();
        renderMask = GameObject.Find("renderMask");
        renderMaskOutliner = GameObject.Find("renderMaskOutliner");
        renderMaskOutliner2 = GameObject.Find("renderMaskOutliner2");

        maskType = MaskType.NULL;

    }
	
	// Update is called once per frame
	void Update ()
    {
        renderMaskOutliner.GetComponent<RenderMask>().maskType = renderMask.GetComponent<RenderMask>().maskType;
        renderMaskOutliner2.GetComponent<RenderMask>().maskType = renderMask.GetComponent<RenderMask>().maskType;

        if (GetComponentInParent<PlayerControl>().isIdle)
        {
            //MovingMask.enabled = false;
            MovingMask.speed = 0.0f;
        }
        else if(GetComponentInParent<PlayerControl>().isWalking)
        {
            //MovingMask.enabled = true;   
            MovingMask.speed = .3f;
        }
        else  
        {
            //MovingMask.enabled = true;   
            MovingMask.speed = 1f;
        }

        switch ((int)maskType)
        {
            case 0:
                MovingMask.Play("waterFlowMask");

                break;

            case 1:
                MovingMask.Play("softWind");
                MovingMask.Play("softWindOutline");
                break;
            case 2:
                MovingMask.Play("nullAnimation");
                break;
        }

        if(maskType == MaskType.NULL)
        {
            MovingMask.Play("nullAnimation");
        }


        //Debug.Log("the mask should be displaying " + maskType);

    }

    public void setMaskType(MaskType mask)
    {
        this.maskType = mask;
    }

    public enum MaskType
    {
        WATER, GRASS, NULL
    }

    void onTriggerEnter2D (Collider2D coll)
    {
        //if(coll.name == "grassEdge")
        //{
        //    MovingMask.Play("SoftWind");
        //}
        //else if (coll.name == "waterEdge")
        //{
        //    MovingMask.Play("waterFlowMash");
        //}

        Debug.Log("Hey! This happened");
    }

    void onCollisionEnter2D (Collision coll)
    {
        if(coll.gameObject.name == "grassEdge")
        {
            Debug.Log("the circle collider is working!");
        }
    }
}
