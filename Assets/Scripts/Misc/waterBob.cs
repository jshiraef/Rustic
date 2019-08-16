using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waterBob : MonoBehaviour
{
    [SerializeField]
    float height = 0.1f;

    [SerializeField]
    float period = 1;

    private Vector3 initialPosition;
    private Vector3 initialRotation;
    private float offset;

    public bool Vertical;

    public bool rotationOnly;

    private void Awake()
    {
        initialPosition = transform.position;
        initialRotation = new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z);

        offset = 1 - (Random.value * 2);
    }

    private void Update()
    {
        if (!rotationOnly)
        {
            if (!Vertical)
            {
                transform.position = initialPosition - Vector3.left * Mathf.Sin((Time.time + offset) * period) * height;
            }
            else
            {
                transform.position = initialPosition - Vector3.up * Mathf.Sin((Time.time + offset) * period) * height;
            }
        }
        else
        {
            transform.Rotate(0,0,Mathf.Sin((Time.time + offset) * period) * height, Space.Self);
        }

    }
}
