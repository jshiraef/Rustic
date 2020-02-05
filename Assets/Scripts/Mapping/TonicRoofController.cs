using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TonicRoofController : MonoBehaviour
{

    public GameObject TonicHouseRoof;

    public GameObject roofContingency1;
    public GameObject roofContingency2;

    private bool roofTrigger;

    private SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (roofTrigger)
        {
            if (!TonicHouseRoof.activeSelf)
            {
                TonicHouseRoof.SetActive(true);
            }

            roofContingency1.GetComponent<SortingOrderScript>().enabled = false;
            roofContingency1.GetComponent<SpriteRenderer>().sortingLayerName = default;

            roofContingency2.GetComponent<SortingOrderScript>().enabled = false;
            roofContingency2.GetComponent<SpriteRenderer>().sortingLayerName = default;

            Color tmpColor = sprite.color;
            sprite.color = new Color(tmpColor.r, tmpColor.g, tmpColor.b, 1f);
        }
        else
        {
            if (TonicHouseRoof.activeSelf)
            {
                TonicHouseRoof.SetActive(false);
            }

            roofContingency1.GetComponent<SortingOrderScript>().enabled = true;
            roofContingency2.GetComponent<SortingOrderScript>().enabled = true;
        }

        Debug.Log("the roof trigger is " + roofTrigger);
    }

    public void setRoofTrigger(bool b)
    {
        roofTrigger = b;
        Debug.Log("this happened");
    }
}
