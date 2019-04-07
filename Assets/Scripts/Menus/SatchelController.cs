using System.Collections;

using System.Collections.Generic;

using UnityEngine;



public class SatchelController : MonoBehaviour
{



    private GameObject blurOverlay;

    private GameObject satchel;

    private GameObject vendor;

    private GameObject herbsHighlight;
    private GameObject vialsHighlight;
    private GameObject stoneBagHighlight;
    private GameObject selectorArrow;

    private bool vialSelected, herbSelected, stoneSelected;
    private Vector3 herbPosition, vialPosition, stoneBagPosition;

    private Animator stoneBagAnim;


    public bool isPaused;

    public bool satchelSelect;

    [SerializeField] AnimatorSound animatorSound;
    public int index;
    [SerializeField] bool keyDown;
    [SerializeField] int maxIndex;
    public AudioSource audioSource;



    // Use this for initialization

    void Start()
    {

        herbsHighlight = GameObject.Find("herbsOutline");
        vialsHighlight = GameObject.Find("vialsOutline");
        stoneBagHighlight = GameObject.Find("stoneBagOutline");
        selectorArrow = GameObject.Find("favArrow");


        blurOverlay = GameObject.Find("blurInventory").gameObject;

        vialPosition = new Vector3(-1.41f, -.1f, 0);
        herbPosition = new Vector3(-9f, 4f, 0);
        stoneBagPosition = new Vector3(9f, .06f, 0);

        selectorArrow.transform.localPosition = vialPosition;

        satchel = GameObject.Find("Satchel").gameObject;

        stoneBagAnim = GetComponentInChildren<Animator>();



        if (!satchel.activeSelf)

        {

            satchel = null;

        }



        vendor = GameObject.Find("Vendor");

        audioSource = GetComponent<AudioSource>();

        stoneBagAnim.updateMode = AnimatorUpdateMode.UnscaledTime;



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
                    stoneBagAnim.Play("stoneBagJostle");
                    //animatorSound.disableOnce = true;

                    


                }

            }

            if (index == 0)
            {
                selectorArrow.transform.localPosition = Vector3.Lerp(selectorArrow.transform.localPosition, vialPosition, .2f);
            }
            else if (index == 1)
            {
                selectorArrow.transform.localPosition = Vector3.Lerp(selectorArrow.transform.localPosition, stoneBagPosition, .2f);
            }
            else if (index == 2)
            {
                selectorArrow.transform.localPosition = Vector3.Lerp(selectorArrow.transform.localPosition, herbPosition, .2f);
            }



        }

        if (Input.GetAxis("Horizontal") != 0)
        {
            if (!keyDown)
            {
                if (Input.GetAxis("Horizontal") > 0)
                {
                    if (index < maxIndex)
                    {
                        index++;
                    }
                    else
                    {
                        index = 0;
                    }
                }
                else if (Input.GetAxis("Horizontal") < 0)
                {
                    if (index > 0)
                    {
                        index--;
                    }
                    else
                    {
                        index = maxIndex;
                    }
                }
                keyDown = true;
            }
        }
        else
        {
            keyDown = false;
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