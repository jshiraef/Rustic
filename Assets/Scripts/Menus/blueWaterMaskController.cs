using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blueWaterMaskController : MonoBehaviour
{
    private GameObject blueMaskAnchor, blueMask1, blueMask2, blueMask3;
    private PlayerControl player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("player").GetComponent<PlayerControl>();

        blueMaskAnchor = transform.parent.gameObject;
        blueMask1 = transform.GetChild(0).gameObject;
        blueMask2 = blueMask1.transform.GetChild(0).gameObject;
        blueMask3 = blueMask1.transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
