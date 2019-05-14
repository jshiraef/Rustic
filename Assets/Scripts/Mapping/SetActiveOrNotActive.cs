using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveOrNotActive : MonoBehaviour
{
    [SerializeField]
    int distance;

    [SerializeField]
    GameObject objectToTurnOff;

    GameObject camera;

    public bool activeOnlyAtBeginningSwitch;
    public int beginningSwitchCoolDown;

    // Start is called before the first frame update
    void Start()
    {
        camera = GameObject.Find("Main Camera");
        beginningSwitchCoolDown = 400;
    }

    // Update is called once per frame
    void Update()
    {
        float dst = Vector3.Distance(camera.transform.position, objectToTurnOff.transform.position);

        if (dst > distance && beginningSwitchCoolDown <= 0)
        {
            objectToTurnOff.SetActive(false);
        }
        else 
        {
            objectToTurnOff.SetActive(true);
        }




        //if(beginningSwitchCoolDown > 0)
        //{
        //    objectToTurnOff.SetActive(true);
        //}
        //else
        //{
        //    objectToTurnOff.SetActive(false);
        //}

        if(beginningSwitchCoolDown > 0)
        {
            beginningSwitchCoolDown -= Mathf.RoundToInt(Time.deltaTime * 100);
        }

    }
}
