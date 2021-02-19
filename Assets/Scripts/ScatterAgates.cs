using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScatterAgates : MonoBehaviour
{
    public GameObject agatePrefab;

    public int numberOfAgates;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    void Scatter()
    {
        Vector3 sp = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 dir = (Input.mousePosition - sp).normalized;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
        float spread = Random.Range(-10, 10);
        Quaternion agateRotation = Quaternion.Euler(new Vector3(0, 0, angle + spread));


        for(int i = 0; i < numberOfAgates; i++)
        {
            GameObject agate = (GameObject)GameObject.Instantiate(agatePrefab, transform.position, agateRotation);
            agate.GetComponent<SpriteRenderer>().sprite = transform.GetChild((int)Random.Range(1f, 5f)).GetComponent<SpriteRenderer>().sprite;
            agate.transform.Translate(Random.Range(-3f, 3f), Random.Range(-3f, 3f), 0);
        }

        // Instantiate the agates using our new rotation
        //GameObject agate = (GameObject)GameObject.Instantiate(agatePrefab, transform.position, agateRotation);

        //agate.transform.Translate(Random.Range(-3f, 3f), Random.Range(-3f, 3f), 0);

        // Instantiate the agate using our new rotation
        //GameObject agate2 = (GameObject)GameObject.Instantiate(agatePrefab, transform.position, agateRotation);

        //agate2.transform.Translate(Random.Range(-3f, 3f), Random.Range(-3f, 3f), 0);
    }
}
