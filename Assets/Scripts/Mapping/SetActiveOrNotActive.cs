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

    // Start is called before the first frame update
    void Start()
    {
        camera = GameObject.Find("Main Camera");
    }

    // Update is called once per frame
    void Update()
    {
        float dst = Vector3.Distance(camera.transform.position, objectToTurnOff.transform.position);

        if (dst > distance)
        {
            objectToTurnOff.SetActive(false);
        }
        else
        {
            objectToTurnOff.SetActive(true);
        }

    }
}
