using System.Collections;

using System.Collections.Generic;

using UnityEngine;



public class SatchelController : MonoBehaviour
{



    private GameObject blurOverlay;

    private GameObject satchel;

    private GameObject vendor;


    public bool isPaused;

    public bool satchelSelect;



    // Use this for initialization

    void Start()
    {



        blurOverlay = GameObject.Find("blurInventory").gameObject;



        satchel = GameObject.Find("Satchel").gameObject;



        if (!satchel.activeSelf)

        {

            satchel = null;

        }



        vendor = GameObject.Find("Vendor");



    }



    // Update is called once per frame

    void Update()
    {



        if (satchel != null)

        {

            if (vendor.GetComponent<VendorController>().getEndOfDialogue() && vendor.GetComponent<VendorController>().getWithinTalkingRange())

            {

                // show Vendor Inventory

            }
            else

            {

                // turn off Inventory

            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Debug.Log("you hit pause!");
                if (isPaused)
                {
                    isPaused = false;

                    
                        satchel.SetActive(false);
                        Time.timeScale = 1f;                                    

                }
                else
                {
                    isPaused = true;
                    satchel.SetActive(true);
                    Time.timeScale = 0f;

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