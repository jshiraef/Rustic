using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeFlare : MonoBehaviour
{

    public bool flareTrigger;
    public float flareSpeed;
    // Start is called before the first frame update
    void Start()
    {
        flareSpeed = .1f;
        this.transform.localPosition = new Vector3(0, 0, -12f);
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
            this.transform.Translate(0, 0, flareSpeed);
        }

        if(this.transform.position.z >= 10)
        {
            flareTrigger = false;
            this.transform.localPosition = new Vector3(0, 0, -12f);
        }
    }
}
