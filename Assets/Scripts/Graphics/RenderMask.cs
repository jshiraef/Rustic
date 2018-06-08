using UnityEngine;
using System.Collections;

public class RenderMask : MonoBehaviour {

    Animator MovingMask;

    private GameObject renderMask;
    private GameObject renderMaskOutliner;
    private GameObject renderMaskOutliner2;
    private SpriteRenderer renderMaskSprite;
    public MaskType playerMaskType;
    public MaskType vendorMaskType;

    // Use this for initialization
    void Start ()
    {

        MovingMask = GetComponent<Animator>();
        renderMask = GameObject.Find("renderMask");
        renderMaskOutliner = GameObject.Find("renderMaskOutliner");
        renderMaskOutliner2 = GameObject.Find("renderMaskOutliner2");
        renderMaskSprite = renderMask.GetComponent<SpriteRenderer>();

        playerMaskType = MaskType.NULL;
        vendorMaskType = MaskType.NULL;


    }

    // Update is called once per frame
    void Update ()
    {
        renderMaskOutliner.GetComponent<RenderMask>().playerMaskType = renderMask.GetComponent<RenderMask>().playerMaskType;
        renderMaskOutliner2.GetComponent<RenderMask>().playerMaskType = renderMask.GetComponent<RenderMask>().playerMaskType;

        if(this.gameObject.transform.parent.name == "player")
        {
            if (GetComponentInParent<PlayerControl>().isIdle)
            {
                //MovingMask.enabled = false;
                MovingMask.speed = 0.0f;
            }
            else if (GetComponentInParent<PlayerControl>().isWalking)
            {
                //MovingMask.enabled = true;   
                MovingMask.speed = .3f;
            }
            else
            {
                //MovingMask.enabled = true;   
                MovingMask.speed = 1f;
            }
        }

        if(transform.root.name == "player")
        {
            switch ((int)playerMaskType)
            {
                case 0:
                    MovingMask.Play("waterFlowMask");
                    //renderMask.GetComponent<RenderMask>().setWaterFlowLayer();
                    break;

                case 1:
                    MovingMask.Play("softWind");
                    //MovingMask.Play("softWindOutline");
                    break;
                case 2:
                    MovingMask.Play("nullAnimation");
                    break;
            }
        }

        
        if(transform.root.name == "Vendor")
        {
            switch ((int)vendorMaskType)
            {
                case 0:
                    MovingMask.Play("waterFlowMask");
                    //renderMask.GetComponent<RenderMask>().setWaterFlowLayer();
                    break;

                case 1:
                    MovingMask.Play("softWind");
                    //MovingMask.Play("softWindOutline");
                    break;
                case 2:
                    MovingMask.Play("nullAnimation");
                    break;
            }
        }
        

        //if (playerMaskType == MaskType.NULL || vendorMaskType == MaskType.NULL)
        //{
        //    MovingMask.Play("nullAnimation");
        //}

        //if(this.gameObject.transform.parent.name == "player" && this.name == "renderMask")
        //{
        //    Debug.Log("the player mask should be displaying " + maskType);
        //}

        //if (this.gameObject.transform.parent.name == "Vendor" && this.name == "renderMask")
        //{
        //    Debug.Log("the Vendor mask should be displaying " + maskType);
        //}

    }

    public void setPlayerMaskType(MaskType mask)
    {
        this.playerMaskType = mask;
    }

    public void setVendorMaskType(MaskType mask)
    {
        this.vendorMaskType = mask;
    }

    public enum MaskType
    {
        WATER, GRASS, NULL
    }


    void onCollisionEnter2D (Collision coll)
    {
        if(coll.gameObject.name == "grassEdge")
        {
            Debug.Log("the circle collider is working!");
        }
    }

    void setWaterFlowLayer()
    {
        renderMaskSprite.sortingLayerName = "Default";
        renderMaskSprite.sortingOrder = -6;
    }
}
