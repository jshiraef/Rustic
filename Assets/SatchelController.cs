using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SatchelController : MonoBehaviour {

    private GameObject blurOverlay;
    private GameObject satchel;
    private GameObject vendor;

    public bool satchelSelect;

	// Use this for initialization
	void Start () {
        
        blurOverlay = GameObject.Find("blurInventory").gameObject;
      
        
        satchel = GameObject.Find("Satchel").gameObject;

        if(!satchel.activeSelf)
        {
            satchel = null;
        }

        vendor = GameObject.Find("Vendor");

    }
	
	// Update is called once per frame
	void Update () {

        if(satchel != null)
        {
            if (vendor.GetComponent<VendorController>().getEndOfDialogue() && vendor.GetComponent<VendorController>().getWithinTalkingRange())
            {

                if (!satchel.activeSelf)
                {
                    satchel.SetActive(true);
                }
            }
            else
            {
                if (satchel.activeSelf)
                {
                    satchel.SetActive(false);
                }
            }
        }
           
    }

    public void setSatchel(bool b)
    {
        satchelSelect = b;
    }

    public bool getSatchelSelect()
    {
        return satchelSelect;
    }

}
