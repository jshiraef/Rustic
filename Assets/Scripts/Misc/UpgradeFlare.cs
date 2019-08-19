using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeFlare : MonoBehaviour
{

    public bool flareTrigger;
    public float flareSpeed;
    GameObject upgradeFlare;
    // Start is called before the first frame update
    void Start()
    {
        upgradeFlare = transform.GetChild(0).gameObject;
        flareSpeed = .1f;
        upgradeFlare.transform.localPosition = new Vector3(0, 0, -12f);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            flareTrigger = true;
        }

        if (flareTrigger)
        {
            upgradeFlare.transform.Translate(0, 0, flareSpeed);
        }

        if(upgradeFlare.transform.position.z >= 10)
        {
            flareTrigger = false;
            upgradeFlare.transform.localPosition = new Vector3(0, 0, -12f);
        }

        if (flareTrigger)
        {
            upgradeFlare.SetActive(true);
        }
        else upgradeFlare.SetActive(false);
    }
}
