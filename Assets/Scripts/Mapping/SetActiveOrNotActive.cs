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

    public Component[] allWillowWaterBobs;

    // Start is called before the first frame update
    void Start()
    {
        camera = GameObject.Find("Main Camera");
        beginningSwitchCoolDown = 400;

        GetComponentInChildren<waterBob>().enabled = false;


        // for some reason you have to deactivate and then reactive the waterbob script in order for them all to work
        allWillowWaterBobs = GetComponentsInChildren<waterBob>();

        foreach(waterBob wb in allWillowWaterBobs)
        {
            wb.enabled = false;
            wb.enabled = true;
        }
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
            if (!objectToTurnOff.activeSelf)
            {
                objectToTurnOff.SetActive(true);

                foreach (waterBob wb in allWillowWaterBobs)
                {
                    wb.enabled = false;
                    wb.enabled = true;
                }
            }
            

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
